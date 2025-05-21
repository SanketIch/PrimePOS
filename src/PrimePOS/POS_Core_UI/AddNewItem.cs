using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POS_Core.CommonData;
//using POS_Core.DataAccess;
using POS_Core.BusinessRules;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.DataAccess;
using Resources;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmAddNewItem : Form
    {
        #region Private Declearation
        ItemVendor oItemVendor = new ItemVendor();
        ItemVendorData oItemVendorData = new ItemVendorData();
        ItemData oItemData = new ItemData();
        Item oItem = new Item();
        POS_Core.BusinessRules.Vendor vendor = null;
        VendorData vendorData = null;
        //Added By SRT(Gaurav) Date: 10 Jul 2009
        private ItemRow oItemRow = null;
        private bool isCanceled = true;
        string VendorCode = string.Empty;
        //End Of Added By SRT(Gaurav)        
        List<string> oItemsList = new List<string>();
        #endregion


        #region Public Properties
        //Added By SRT(Gaurav) Date: 10 Jul 2009
        public bool IsCanceled
        {
            get
            {
                return (isCanceled);
            }
        }
        public ItemRow CurrentItem
        {
            get
            {
                return (oItemRow);
            }
        }

        public string UPCCODE
        {
            get
            {
                string tempItemId = string.Empty;
                if (txtItemCode.Visible == true)
                {
                    tempItemId = txtItemCode.Text;
                }
                else
                {
                    tempItemId = cmbItemCode.Text;
                }
                return (tempItemId);
            }
        }
        //End Of Added By SRT(Gaurav)
        #endregion

        public frmAddNewItem()
        {
            InitializeComponent();
            FillVendors();
        }

        private void frmAddNewItem_Load(object sender, EventArgs e)
        {
            try
            {
                clsUIHelper.setColorSchecme(this);
                
                this.cmbVendorCode.Text = POS_Core.Resources.Configuration.CPrimeEDISetting.DefaultVendor;  //PRIMEPOS-3167 07-Nov-2022 JY Modified
            }
            catch (Exception ex)
            {
            }
            
        }
        private void FillVendors()
        {
            
            try
            {
                vendor = new POS_Core.BusinessRules.Vendor();
                vendorData = vendor.PopulateList("");

                foreach (DataRow vendorRow in vendorData.Tables[0].Rows)
                {
                    this.cmbVendorCode.Items.Add(vendorRow["VendorID"].ToString(), vendorRow["VendorCode"].ToString());
                }

            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
        }
        public void FillItemDescription(string ItemData,string SearchCriteria)
        {
            try
            {
                FillVendors();
                if (SearchCriteria == "DESCRIPTION")
                {
                    txtItemDescription.Text = ItemData;
                    txtItemDescription.Enabled = false;
                    //FIND ITEMS
                    oItemData = oItem.PopulateList(" WHERE DESCRIPTION LIKE '" + txtItemDescription.Text + "%'");

                    if (oItemData != null && oItemData.Tables[0].Rows.Count > 0)
                    {

                        if (oItemData.Tables[0].Rows.Count > 1)
                        {
                            //IF MULTIPLE
                            cmbItemCode.Enabled = true;
                            foreach (ItemRow oRow in oItemData.Tables[0].Rows)
                            {
                                cmbItemCode.Items.Add(oRow.ItemID.ToString());
                                oItemsList.Add(oRow.Description);
                            }
                            txtItemCode.Visible = false;
                            cmbItemCode.Visible = true;
                            lblMessage.Text = "Please Select Item Id From List.";
                            cmbItemCode.Focus();
                        }
                        else
                        {
                            //IF SINGLE ITEM
                            txtItemCode.Enabled = true;
                            txtItemCode.Visible = true;
                            ItemRow oRow = (ItemRow)oItemData.Tables[0].Rows[0];
                            txtItemCode.Text = oRow.ItemID;
                            txtItemDescription.Text = oRow.Description;
                            txtItemCode.Focus();
                        }
                    }
                    else
                    {
                        //IF NOT ITEMS
                        txtItemCode.Enabled = true;
                        txtItemCode.Visible = true;
                        cmbItemCode.Visible = false;
                        txtItemCode.Focus();
                    }

                }
                else if (SearchCriteria == "VENDOR_ITEM_CODE")
                {
                    txtVendItemCode.Text = ItemData;
                    txtVendItemCode.Enabled = false;
                    cmbItemCode.Visible = false;
                }
                else
                {
                    txtItemCode.Text = ItemData;
                    cmbItemCode.Text = ItemData;
                    txtItemCode.Enabled = false;
                    cmbItemCode.Enabled = false;
                    cmbItemCode.Visible = false;
                    oItemData = oItem.Populate(ItemData);
                    if (oItemData != null && oItemData.Tables[0].Rows.Count > 0)
                    {
                        ItemRow oRow = (ItemRow)oItemData.Tables[0].Rows[0];
                        txtItemDescription.Text = oRow.Description;
                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }

        public void FillItemDescription(string ItemId, string ItemDescription, string VendorCode)
        {
            try
            {
                ItemRow oRow = null;
                FillVendors();
                txtItemCode.Text = ItemId;
                txtItemCode.Visible = true;
                txtItemCode.Enabled = false;
                txtItemDescription.Enabled = false;
                txtSellingPrice.Enabled = false;
                cmbItemCode.Visible = false;
                oItemData = oItem.Populate(ItemId.Trim());
                if (oItemData != null && oItemData.Tables[0].Rows.Count > 0)
                {
                    oRow = (ItemRow)oItemData.Tables[0].Rows[0];
                    //txtItemDescription.Text = oRow.Description;
                }
                txtItemDescription.Text = ItemDescription;
                //Added By SRT(Gaurav) Date : 16-Jul-2009
                //This code line added to set selling price of existing item.
                txtSellingPrice.Value = oRow.SellingPrice;
                //End Of Added By SRT(Gaurav)
                cmbVendorCode.Enabled = true;
                this.VendorCode = VendorCode;
            }
            catch (Exception Ex)
            {
            }
            
        }
        private void btnNextToEditItem_Click(object sender, EventArgs e)
        {
            bool isEDIVendor = false;
            try
            {
                string tempItemId = string.Empty;
                if (txtItemCode.Visible == true)
                {
                    tempItemId = txtItemCode.Text;
                }
                else
                {
                    tempItemId = cmbItemCode.Text;
                }
                if (cmbVendorCode.Text.Trim().Length == 0)
                {
                    clsUIHelper.ShowErrorMsg("Please Endter Vendor Code");
                    cmbVendorCode.Focus();
                    return;
                }
                else//Added By shitaljit to allow user to alow users to exclude item vendor code in case the selected vendor it Non-EDI 
                {

                    vendor = new POS_Core.BusinessRules.Vendor();
                    vendorData = vendor.Populate(this.cmbVendorCode.Text);
                    if (vendorData != null)
                    {
                        if (vendorData.Vendor.Rows.Count > 0)
                        {
                            VendorRow oRow = vendorData.Vendor[0];
                            isEDIVendor = oRow.USEVICForEPO;
                        }
                    }
                }
                if (txtVendItemCode.Text.Trim().Length == 0 && isEDIVendor == true)
                {
                    clsUIHelper.ShowErrorMsg("Please Enter Vendor Item Code");
                    txtVendItemCode.Focus();
                    return;
                }
                decimal cost = 0;
                string strCostPrice, strSellingPrice;   //Sprint-25 - PRIMEPOS-660 03-Feb-2017 JY renamed these two variables, previously it was Cost1,Cost2 which was confusing results to commit interchanged values 
                strCostPrice = txtCostPrice.Text.Replace("_", "");
                strSellingPrice = txtSellingPrice.Text.Replace("_", "");
                if (!decimal.TryParse(strCostPrice, out cost) || cost == 0)
                {
                    clsUIHelper.ShowErrorMsg("Please Enter Vendor Item Cost Price.");
                    txtCostPrice.Focus();
                    return;
                }
                cost = 0;
                if (!decimal.TryParse(strSellingPrice, out cost))// || cost == 0) //Commented By Ravindra
                {
                    clsUIHelper.ShowErrorMsg("Please Enter Vendor Item Selling Price.");
                    txtSellingPrice.Focus();
                    return;
                }

                vendorData = vendor.Populate(cmbVendorCode.Text);
                string tempVendId = string.Empty;
                if (vendorData != null && vendorData.Tables.Count > 0 && vendorData.Tables[0].Rows.Count > 0)
                {
                    tempVendId = vendorData.Tables[0].Rows[0][0].ToString();
                    oItemVendorData = oItemVendor.PopulateList(" WHERE ItemId='" + tempItemId + "' and VendorItemId = '" + txtVendItemCode.Text.Trim() + "' and ItemVendor.vendorid=" + tempVendId);
                }
                else
                {
                    oItemVendorData = oItemVendor.PopulateList(" WHERE ItemId='" + tempItemId + "' and VendorItemId = '" + txtVendItemCode.Text.Trim() + "'");
                }


                if (oItemVendorData != null && oItemVendorData.Tables.Count > 0 && oItemVendorData.Tables[0].Rows.Count > 0)
                {
                    clsUIHelper.ShowErrorMsg("Item Code #" + tempItemId + " For Vendor " + cmbVendorCode.Text + " Allready Exists.");
                    if (txtItemCode.Visible)
                    {
                        txtItemCode.Focus();
                    }
                    else
                    {
                        cmbItemCode.Focus();
                    }
                    return;
                }
                frmItems editItems = new frmItems();
                editItems.txtItemCode.Text = this.txtItemCode.Text;
                editItems.txtDescription.Text = this.txtItemDescription.Text;
                editItems.numSellingPrice.Text = strSellingPrice;  //Sprint-25 - PRIMEPOS-660 03-Feb-2017 JY Corrected the assignment
                editItems.numLastCostPrice.Text = strCostPrice; //Sprint-25 - PRIMEPOS-660 03-Feb-2017 JY Corrected the assignment
                editItems.combEditorPrefVendor.Text = this.cmbVendorCode.Text;
                editItems.IsCanceled = false;

                if (oItemData == null || oItemData.Item.Rows.Count == 0)
                {
                    editItems.ShowDialog();
                    this.txtItemCode.Text = editItems.txtItemCode.Text;
                    this.txtItemDescription.Text = editItems.txtDescription.Text;
                    this.txtSellingPrice.Text = editItems.numSellingPrice.Text.Trim().Replace("_", "");
                    this.txtCostPrice.Text = editItems.numLastCostPrice.Text.Trim().Replace("_", "");
                    this.cmbVendorCode.Text = editItems.combEditorPrefVendor.Text;
                }
                else
                {
                    this.txtItemCode.Text = oItemData.Item.Rows[0]["ItemId"].ToString().Trim().Length > 0 ? oItemData.Item.Rows[0]["ItemId"].ToString() : editItems.txtItemCode.Text;
                    tempItemId = txtItemCode.Text;
                    this.txtItemDescription.Text = oItemData.Item.Rows[0]["Description"].ToString();
                    this.txtSellingPrice.Text = editItems.numSellingPrice.Text.Trim().Replace("_", "");
                    this.txtCostPrice.Text = editItems.numLastCostPrice.Text.Trim().Replace("_", "");
                    this.cmbVendorCode.Text = editItems.combEditorPrefVendor.Text;
                }

                if (!editItems.IsCanceled)
                {
                    try
                    {
                        oItemRow = editItems.CurrentItem;


                        POS_Core.BusinessRules.Vendor oVendor = new POS_Core.BusinessRules.Vendor();
                        VendorData oVendorData = new VendorData();
                        oVendorData = oVendor.Populate(cmbVendorCode.Text);

                        IDbTransaction tr = null;
                        IDbConnection conn = DataFactory.CreateConnection();
                        IDbCommand cmd = DataFactory.CreateCommand();

                        string sSQL = "INSERT INTO ITEMVENDOR (ITEMID,VENDORITEMID,VENDORID,VENDORCOSTPRICE,USERID) VALUES"
                            + "('" + tempItemId + "','" + txtVendItemCode.Text + "'," + oVendorData.Vendor[0].VendorId
                            + "," + Configuration.convertNullToDecimal(txtCostPrice.Text.Replace("_", "")) + ",'" + Configuration.UserName + "')";
                        conn.ConnectionString = Configuration.ConnectionString;

                        conn.Open();
                        tr = conn.BeginTransaction();

                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = sSQL;
                        cmd.Transaction = tr;
                        cmd.Connection = conn;
                        cmd.ExecuteNonQuery();
                        tr.Commit();
                        tr = null;
                        conn.Close();
                        isCanceled = false;
                        POS_Core_UI.Resources.Message.Display("Item Code #" + tempItemId + " With Vendor Item Code #" + txtVendItemCode.Text + " Added Successfully.");
                    }
                    catch (Exception Ex)
                    {
                        isCanceled = true;
                    }
                }
                else
                {
                    isCanceled = editItems.IsCanceled;
                }
                this.Close();
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.ToString());
            }
        }
        private bool Save()
        {
            try
            {
                oItemVendor.Persist(oItemVendorData);
                return true;
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
                return false;
            }
        }
        public void FormShown()
        {
            try
            {
                if (txtItemCode.Visible && txtItemCode.Enabled)
                {
                    txtItemCode.Focus();
                }
                else if (cmbItemCode.Visible && cmbItemCode.Enabled)
                {

                    cmbItemCode.Enabled = true;
                    cmbItemCode.Focus();
                    if (cmbItemCode.Items.Count > 0)
                    {
                        cmbItemCode.SelectedIndex = 0;
                    }
                    Infragistics.Win.ValueListItemsCollection oColl = cmbVendorCode.Items;
                    foreach (Infragistics.Win.ValueListItem oListItem in oColl)
                    {
                        if (oListItem.DisplayText.Trim().ToUpper() == Configuration.CPrimeEDISetting.DefaultVendor.Trim().ToUpper())    //PRIMEPOS-3167 07-Nov-2022 JY Modified
                        {
                            cmbVendorCode.SelectedItem = oListItem;                            
                            break;
                        }
                        
                    }
                }
                else if (txtItemDescription.Enabled)
                {
                    txtItemDescription.Focus();
                }
                else if (txtVendItemCode.Enabled)
                {
                    if (VendorCode != string.Empty)
                    {
                        Infragistics.Win.ValueListItemsCollection oColl = cmbVendorCode.Items;
                        foreach (Infragistics.Win.ValueListItem oListItem in oColl)
                        {
                            if (oListItem.DisplayText == VendorCode)
                            {
                                cmbVendorCode.SelectedItem = oListItem;
                                cmbVendorCode.Enabled = false;
                                break;
                            }
                        }
                    }
                    txtVendItemCode.Focus();
                }
            }
            catch (Exception Ex)
            {
                //clsUIHelper.ShowErrorMsg(Ex.Message);
            }
            
        }
        private void frmAddNewItem_Shown(object sender, EventArgs e)
        {
            try
            {
                FormShown();
            }
            catch (Exception Ex)
            {
            }            
        }

        private void frmAddNewItem_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.Escape)
                {
                    isCanceled = true;
                    this.Close();
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                isCanceled = true;
                this.Close();
            }
            catch (Exception Ex)
            {
            }
        }

        private void txtItemCode_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtItemCode.Text.Trim().Length > 0)
                {
                    oItemData = oItem.Populate(txtItemCode.Text);
                    if (oItemData != null && oItemData.Tables[0].Rows.Count > 0)
                    {
                        ItemRow oRow = (ItemRow)oItemData.Tables[0].Rows[0];
                        if (oRow != null && oRow.Description.Trim().Length > 0)
                        {
                            txtItemDescription.Text = oRow.Description;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private void cmbItemCode_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                txtItemDescription.Text = oItemsList[cmbItemCode.SelectedIndex];
            }
            catch (Exception Ex)
            {
            }
        }

        private void cmbItemCode_Enter(object sender, EventArgs e)
        {
            try
            {
                cmbItemCode.SelectedText = cmbItemCode.Text;
            }
            catch(Exception Ex)
            {
            }
        }

        private void txtCostPrice_Enter(object sender, EventArgs e)
        {
            try
            {
                string str = txtCostPrice.Value.ToString();
                txtCostPrice.SelectionLength = str.Length;
            }
            catch (Exception Ex)
            {
            }
        }

        private void txtSellingPrice_Enter(object sender, EventArgs e)
        {
            try
            {
                string str = txtSellingPrice.Value.ToString();
                txtSellingPrice.SelectionLength = str.Length;
            }
            catch (Exception Ex)
            {
            }
        }

        private void txtItemCode_Enter(object sender, EventArgs e)
        {
            try
            {
                string str = txtItemCode.Value.ToString();
                txtItemCode.SelectionLength = str.Length;
            }
            catch (Exception Ex)
            {
            }
        }

        private void txtItemDescription_Enter(object sender, EventArgs e)
        {
            try
            {
                string str = txtItemDescription.Value.ToString();
                txtItemDescription.SelectionLength = str.Length;
            }
            catch (Exception Ex)
            {
            }
        }

       
    }
}