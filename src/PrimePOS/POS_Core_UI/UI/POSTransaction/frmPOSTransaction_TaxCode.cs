using Infragistics.Win.UltraWinGrid;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Core_UI.UI
{
    public partial class frmPOSTransaction
    {

        #region Taxcode
        //completed by sandeep
        private void ValidateItemTax(object sender, System.ComponentModel.CancelEventArgs e)
        {
            #region Sprint-19 - 2146 23-Dec-2014 JY Added to assign tax code to combo-check boxes
            if (sMode == "S" || strActionButton == "T") //Sprint-23 - PRIMEPOS-2302 23-May-2016 JY Added strActionButton == "T"
            {
                string tempTax = cmbTaxCode.Text;
                cmbTaxCode.Text = ValidateTaxCodes();
                //Sprint-26 - PRIMEPOS-566 26-Jul-2017 JY Added logic to pop up message when Tax Code(s) is invalid
                var firstOrdered = tempTax.Split(',').Select(t => t.Trim()).OrderBy(t => t);
                var secondOrdered = cmbTaxCode.Text.Split(',').Select(t => t.Trim()).OrderBy(t => t);
                bool stringsAreEqual = firstOrdered.SequenceEqual(secondOrdered, StringComparer.OrdinalIgnoreCase);
                if (!stringsAreEqual)   //PRIMEPOS-2500 30-Mar-2018 JY Added logic to compare override tax codes
                {
                    cmbTaxCode.ResetText();
                    Resources.Message.Display("Tax Code(s) not found", "PrimePOS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearItemRow();
                    grdDetail.Focus();
                    return;
                }
            }
            #endregion Sprint-19 - 2146 23-Dec-2014 JY Added to assign tax code to combo-check boxes
            try
            {
                #region tax codes

                List<int> ListTaxIds = new List<int>();
                TaxCodesData oTaxCodesData = new TaxCodesData();
                if (taxoverflag == 0)
                {
                    ListTaxIds = cmbTaxCode.CheckedItems.Select(checkedItem => int.Parse(checkedItem.DataValue.ToString())).ToList();
                    oPOSTrans.UpdateTransTaxCode(ListTaxIds, out oTaxCodesData, oPOSTrans.oTDRow.TransDetailID);    //PRIMEPOS-2500 02-Apr-2018 JY Added "TransDetailID" parameter 
                }
                else
                {
                    #region PRIMEPOS-2402 15-Jul-2021 JY Added
                    try
                    {
                        if (oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count > 0)
                        {
                            string strOldTaxCodesWithPercentage = string.Empty;
                            for (int i = 0; i < oPOSTrans.oTransDData.TransDetail.Rows.Count; i++)
                            {
                                strOldTaxCodesWithPercentage = "";
                                for (int j = oPOSTrans.oTDTaxData.TransDetailTax.Rows.Count - 1; j >= 0; j--)
                                {
                                    TransDetailTaxRow oTransDetailTaxRow = oPOSTrans.oTDTaxData.TransDetailTax[j];
                                    if (oTransDetailTaxRow.TransDetailID == Configuration.convertNullToInt(oPOSTrans.oTransDData.TransDetail.Rows[i][clsPOSDBConstants.TransDetail_Fld_TransDetailID]))
                                    {
                                        if (strOldTaxCodesWithPercentage == "")
                                            strOldTaxCodesWithPercentage = oTransDetailTaxRow.TaxID.ToString() + "~" + oTransDetailTaxRow.TaxPercent.ToString();
                                        else
                                            strOldTaxCodesWithPercentage += "," + oTransDetailTaxRow.TaxID.ToString() + "~" + oTransDetailTaxRow.TaxPercent.ToString();
                                    }
                                }
                                oPOSTrans.oTransDData.TransDetail.Rows[i][clsPOSDBConstants.TransDetail_Fld_OldTaxCodesWithPercentage] = strOldTaxCodesWithPercentage;
                            }
                        }
                    }
                    catch (Exception ex)
                    { 
                    }
                    #endregion

                    oPOSTrans.oTDTaxData.Clear();
                    oPOSTrans.oTDTaxData.AcceptChanges();
                }
                if (this.cmbTaxCode.Text.ToString().Trim() == "")
                {
                    this.oPOSTrans.oTDRow.TaxCode = "";
                    this.oPOSTrans.oTDRow.TaxID = 0;
                    this.oPOSTrans.oTDRow.TaxAmount = 0;
                }
                else
                {
                    oPOSTrans.ApplyTaxCalculation(oPOSTrans.oTDRow, oTaxCodesData, oPOSTrans.oTDTaxData);
                }
                if (this.isAddRow == true)
                    ValidateRow(sender, e);
                else if (this.isEditRow == true)
                {
                    if (isAddRow == false)
                    {
                        if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, -998))
                        {
                            //Following  added by shitaljit(QuicSolv) on 14 April 2011
                            if (taxoverflag == 1)
                            {
                                this.ValidateRow(sender, e);
                                this.ClearItemRow();
                                this.grdDetail.Focus();
                                return;
                            }
                            //till here Added by shitaljit(QuicSolv)
                            else
                            {
                                #region Sprint-23 - PRIMEPOS-2302 23-May-2016 JY Added 
                                if (cmbTaxCode.Text.Trim() != "")
                                {
                                    if (!Configuration.CPOSSet.SelectMultipleTaxes && cmbTaxCode.CheckedItems.Count > 1)
                                    {
                                        clsUIHelper.ShowErrorMsg("\nYou can not select multiple Tax Codes as the respective settings is off.");
                                        cmbTaxCode.Focus();
                                        return;
                                    }
                                }
                                #endregion

                                if (Resources.Message.Display("Do You Want To Update Tax Setting In Item File.", "Tax Override", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                                {
                                    try
                                    {
                                        ItemData odata = oPOSTrans.PopulateItem(oPOSTrans.oTDRow.ItemID);
                                        if (ListTaxIds.Count == 0)
                                        {
                                            odata.Item[0].isTaxable = false;
                                        }
                                        else
                                        {
                                            odata.Item[0].isTaxable = true;
                                        }
                                        ItemPriceValidation oItemPriceValid = new ItemPriceValidation();

                                        if (oItemPriceValid.ValidateItem(odata.Item[0], odata.Item[0].SellingPrice) == false)
                                        {
                                            clsUIHelper.ShowErrorMsg("Current values in item conflicts with validation settings.");
                                        }
                                        else
                                        {
                                            oPOSTrans.PersistItem(odata);
                                            TaxCodeHelper.PersistItemTaxCodes(ListTaxIds, oPOSTrans.oTDRow.ItemID);
                                        }
                                    }
                                    catch (Exception) { }
                                }
                                else  // Need to add here for AuditTrail-NileshJ - PRIMEPOS-2808
                                {
                                    if (oAuditTrail.oAuditDataSet.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = oAuditTrail.oAuditDataSet.Tables[0].Rows.Count - 1; i >= 0; i--)
                                        {
                                            DataRow dr = oAuditTrail.oAuditDataSet.Tables[0].Rows[i];
                                            if (dr["EntityKey"] == oPOSTrans.oTDRow.ItemDescription.Trim() && dr["EntityName"] == "ItemTax" && dr["FieldChanged"] == "TaxID")
                                            {
                                                dr.Delete();
                                            }
                                        }
                                        oAuditTrail.oAuditDataSet.Tables[0].AcceptChanges();
                                    }
                                    ItemTaxData oItemTaxData = new ItemTaxData();
                                    oItemTaxData = TaxCodeHelper.FetchItemTaxInfo(oPOSTrans.oTDRow.ItemID);

                                    DataTable dtTax = new DataTable();
                                    dtTax = oAuditTrail.CreateAuditLogDatatable();
                                    DataRow row = dtTax.NewRow();
                                    row["EntityName"] = "ItemTax";
                                    row["EntityKey"] = oPOSTrans.oTDRow.ItemDescription;
                                    row["FieldChanged"] = "TaxID";
                                    if (oItemTaxData.Tables[0].Rows.Count > 0)
                                    {
                                        row["OldValue"] = oItemTaxData.Tables[0].Rows[0]["TaxID"].ToString();
                                    }
                                    else
                                    {
                                        row["OldValue"] = "";
                                    }
                                    foreach (int selectedTax in ListTaxIds)
                                    {
                                        row["NewValue"] = selectedTax;
                                    }

                                    row["DateChanged"] = DateTime.Now;
                                    row["ActionBy"] = Configuration.UserName;
                                    row["Operation"] = "I";
                                    row["ApplicationName"] = "PrimePOS";

                                    dtTax.Rows.Add(row);
                                    oAuditTrail.oAuditDataSet.Tables[0].Merge(dtTax);
                                }
                            }
                        }
                    }
                    this.ValidateRow(sender, e);
                    this.ClearItemRow();
                    this.grdDetail.Focus();
                }

                #endregion tax codes
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "ValidateItemTax()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        //completed by sandeep
        private void cmbTaxCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                if (this.txtItemCode.Text.Trim() != "")
                    SearchTaxCode();
                else
                    this.txtItemCode.Focus();
            }
            catch (Exception) { }
        }
        //completed by sandeep
        private void cmbTaxCode_AfterCloseUp(object sender, EventArgs e)
        {
            if (this.txtDepartmentCode.Visible == false && isItemadding == false)
            {
                this.cmbTaxCode.Width -= 250;
            }
            else
            {
                this.cmbTaxCode.Width -= 250;
                this.txtDepartmentCode.Visible = true;
                isItemadding = false;
            }
            this.cmbTaxCode.Text = TaxCodeHelper.GetTrimmedTaxCodes(cmbTaxCode.CheckedItems);

            #region Sprint-19 - 2146 29-Dec-2014 JY Added multiple tax selection validation
            if (!Configuration.CPOSSet.SelectMultipleTaxes && cmbTaxCode.CheckedItems.Count > 1)
            {
                clsUIHelper.ShowErrorMsg("\nYou can not select multiple Tax Codes as the respective settings is off.");
            }

            #endregion Sprint-19 - 2146 29-Dec-2014 JY Added multiple tax selection validation
        }
        //completed by sandeep
        private void cmbTaxCode_BeforeDropDown(object sender, CancelEventArgs e)
        {
            if (this.txtDepartmentCode.Visible == false)
            {
                this.cmbTaxCode.Width += 250;
            }
            else
            {
                this.txtDepartmentCode.Visible = false;
                isItemadding = true;
                this.cmbTaxCode.Width += 250;
            }
        }
        //completed by sandeep
        private void cmbTaxCode_TextChanged(object sender, EventArgs e)
        {
            if (sMode != "S" && strActionButton != "T") //Sprint-19 - 2146 23-Dec-2014 JY Added if condition - In simple mode tax code should be assigned directly by typing into combo box //Sprint-23 - PRIMEPOS-2302 23-May-2016 JY Added logic to bypass the "T"-tax override means user can enter the taxcode manually in case of tax override
            {
                this.cmbTaxCode.Text = TaxCodeHelper.GetTrimmedTaxCodes(cmbTaxCode.CheckedItems);
            }
        }
        //completed by sandeep
        private void cmbTaxCode_Click(object sender, EventArgs e)
        {
            return;
        }
        #endregion

        #region cmbTaxCode Method

        //completed by sandeep
        private string ValidateTaxCodes()
        {
            string strTaxCodes = string.Empty;
            try
            {
                Dictionary<int, string> dict = new Dictionary<int, string>();
                var taxCodesData = oPOSTrans.PopulateTaxCodeList(" WHERE 1=1");
                for (int j = 0; j < cmbTaxCode.Items.Count; j++)
                    cmbTaxCode.Items[j].CheckState = CheckState.Unchecked;

                string[] arrTaxCodes = cmbTaxCode.Text.Trim().Split(',');

                for (int i = 0; i < arrTaxCodes.Length; i++)
                {
                    for (int j = 0; j < cmbTaxCode.Items.Count; j++)
                    {
                        if (arrTaxCodes[i].Trim().ToUpper() == taxCodesData.TaxCodes.Rows[j]["TaxCode"].ToString().Trim().ToUpper())
                        {
                            try
                            {
                                dict.Add(Configuration.convertNullToInt((taxCodesData.TaxCodes.Rows[j]["TaxID"].ToString())), taxCodesData.TaxCodes.Rows[j]["TaxCode"].ToString().Trim().ToUpper());
                            }
                            catch
                            {
                            }
                            break;
                        }
                    }
                }
                bool bStatus = false;
                foreach (var item in this.cmbTaxCode.Items)
                {
                    string str = string.Empty;
                    bStatus = dict.TryGetValue(Configuration.convertNullToInt(item.DataValue), out str);
                    if (bStatus)
                    {
                        while (item.CheckState == CheckState.Unchecked) //PRIMEPOS-2302 05-Aug-2016 JY Resolved one issue with Tax override, when we remove tax and then add tax then it is not applied
                        {
                            item.CheckState = CheckState.Checked;
                        }
                        if (strTaxCodes == string.Empty)
                            strTaxCodes = str;
                        else
                            strTaxCodes += "," + str;
                    }
                }
            }
            catch
            {
                strTaxCodes = string.Empty;
            }
            return strTaxCodes;
        }

        //completed by sandeep
        private void SearchTaxCode()
        {
            try
            {

                logger.Trace("SearchTaxCode() - " + clsPOSDBConstants.Log_Entering);
                //Modified by shitaljit for JIRA PRIMEPOS-1359 Tax filter on the transaction screen displays all the Tax records when searched for particular Tax code
                frmSearch oSearch = new frmSearch(clsPOSDBConstants.TaxCodes_tbl, this.cmbTaxCode.Text.Trim(), "");
                oSearch.searchInConstructor = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strCode = oSearch.SelectedRowID();
                    if (strCode == "")
                        return;

                    EditTax(strCode, clsPOSDBConstants.TaxCodes_tbl);
                    this.setItemValues(oPOSTrans.oTDRow);
                }
                logger.Trace("SearchTaxCode() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchTaxCode()");
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        //completed by sandeep
        private void RecalculateTax(string strCoupon)
        {
            logger.Trace("RecalculateTax() - Single parameter - " + clsPOSDBConstants.Log_Entering);

            System.Decimal CouponAmt = 0;
            string sTaxIDs = string.Empty;
            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                oPOSTrans.GetTransDetailRow(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGRow.ListIndex);
                if (Configuration.CouponItemCode.ToUpper() == Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value).Trim().ToUpper())
                {
                    CouponAmt = Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString());
                    break;
                }
            }

            decimal totalPrice = 0;
            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                if (Configuration.CouponItemCode.ToUpper() == Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value).Trim().ToUpper())
                    continue;

                totalPrice += Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value) - Convert.ToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value);
            }

            decimal discPerc = Convert.ToDecimal((Convert.ToDecimal(CouponAmt) / totalPrice * 100) * -1);

            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                oPOSTrans.GetTransDetailRow(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TransDetailID].Value.ToString(), oGRow.ListIndex);
                string sItemID = Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value);
                if ((Configuration.CouponItemCode.ToUpper() == Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value).Trim().ToUpper()) || (!oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, sItemID, out sTaxIDs, oPOSTrans.oTDRow.TransDetailID)))
                    continue;

                Decimal TempExtPrice = oPOSTrans.oTDRow.ExtendedPrice;
                TaxCodesData oTCD = oPOSTrans.PopulateTaxCodeList(" WHERE TaxID IN " + sTaxIDs);
                System.Decimal LIDisc = 0;
                LIDisc = (discPerc / 100 * (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_Discount].Value.ToString())));
                oPOSTrans.oTDRow.ExtendedPrice = (Configuration.convertNullToDecimal(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ExtendedPrice].Value.ToString()) - LIDisc);
                oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_TaxAmount].Value = 0;
                oPOSTrans.CalculateTax(oPOSTrans.oTDRow, oTCD, oPOSTrans.oTDTaxData);
                oPOSTrans.oTDRow.ExtendedPrice = TempExtPrice;
            }
            Application.DoEvents();

            logger.Trace("RecalculateTax() - Single parameter - " + clsPOSDBConstants.Log_Exiting);
        }

        //done by sandeep
        private void LoadTaxCodes(string sItemCode, out TaxCodesData oTaxCodesData, string strKey = "") //PRIMEPOS-2924 08-Dec-2020 JY Added strKey parameter
        {
            oTaxCodesData = new TaxCodesData();
            string sTaxIds = string.Empty;

            try
            {
                if (oPOSTrans.IsItemTaxableForTrasaction(oPOSTrans.oTDTaxData, oPOSTrans.oTDRow.ItemID, out sTaxIds, oPOSTrans.oTDRow.TransDetailID))
                {
                    oTaxCodesData = oPOSTrans.PopulateTaxCodeList(" WHERE TaxID IN " + sTaxIds);
                }

                this.cmbTaxCode.DataSource = TaxCodeHelper.GetTaxCodeDataTable();
                this.cmbTaxCode.ValueMember = clsPOSDBConstants.TaxCodes_Fld_TaxCode;
                this.cmbTaxCode.DisplayMember = clsPOSDBConstants.TaxCodes_Fld_Description;
                this.cmbTaxCode.DataBind();
                this.cmbTaxCode.CheckedListSettings.CheckBoxStyle = Infragistics.Win.CheckStyle.CheckBox;
                this.cmbTaxCode.CheckedListSettings.CheckBoxAlignment = ContentAlignment.MiddleLeft;
                this.cmbTaxCode.CheckedListSettings.EditorValueSource = Infragistics.Win.EditorWithComboValueSource.SelectedItem;
                this.cmbTaxCode.CheckedListSettings.ListSeparator = ", ";
                this.cmbTaxCode.CheckedListSettings.ItemCheckArea = Infragistics.Win.ItemCheckArea.Item;

                if (strKey == "removeindTax") return;   //PRIMEPOS-2924 08-Dec-2020 JY Added                

                if (Configuration.isNullOrEmptyDataSet(oTaxCodesData) == false)
                {
                    foreach (var item in this.cmbTaxCode.Items)
                    {
                        foreach (TaxCodesRow oRow in oTaxCodesData.TaxCodes.Rows)
                        {
                            if (Configuration.convertNullToInt(item.DataValue) == oRow.TaxID)
                            {
                                item.CheckState = CheckState.Checked;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "LoadTaxCodes()");
                throw Ex;
            }
        }
        //done by sandeep
        private void EditTax(string code, string senderName)
        {
            try
            {
                if (senderName == clsPOSDBConstants.TaxCodes_tbl)
                {
                    #region TaxCodes

                    try
                    {
                        TaxCodesData oTaxCodesData = new TaxCodesData();
                        LoadTaxCodes(oPOSTrans.oTDRow.ItemID, out oTaxCodesData);
                        oPOSTrans.CalculateTax(oPOSTrans.oTDRow, oTaxCodesData, oPOSTrans.oTDTaxData);
                    }
                    catch (POS_Core.ErrorLogging.POSExceptions posExp)
                    {
                        logger.Fatal(posExp, "EditTax()");
                        throw (posExp);
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        this.oPOSTrans.oTDRow.TaxCode = String.Empty;
                        this.oPOSTrans.oTDRow.TaxID = 0;
                        this.oPOSTrans.oTDRow.TaxAmount = 0;
                        SearchTaxCode();
                    }
                    catch (Exception exp)
                    {
                        logger.Fatal(exp, "EditTax()");
                        clsUIHelper.ShowErrorMsg(exp.Message);
                        this.oPOSTrans.oTDRow.TaxCode = String.Empty;
                        this.oPOSTrans.oTDRow.TaxID = 0;
                        this.oPOSTrans.oTDRow.TaxAmount = 0;
                        SearchTaxCode();
                    }
                    #endregion TaxCodes
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "EditTax()");
                clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        //completed by sandeep
        public void taxOverrideAll()
        {
            //if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.TaxOverrideAll.ID, UserPriviliges.Permissions.TaxOverrideAll.Name)) {   //PRIMEPOS-2510 26-Apr-2018 JY Commented
            //    if (Resources.Message.Display("Are you Sure you want to Override All Tax?", "Override Tax", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes) {   //PRIMEPOS-2510 26-Apr-2018 JY Commented
            //PRIMEPOS-2510 26-Apr-2018 JY Added user level permisson to control tax override for Rx item
            bool bIsRx = false, bIsOTC = false;
            CheckItemStatus(ref bIsRx, ref bIsOTC);
            bool isEditable = false;
            string sRxUserID = string.Empty, sPOSUserID = string.Empty; //PRIMEPOS-2402 14-Jul-2021 JY Added
            if (bIsRx)
            {
                if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.TaxOverrideAllForRx.ID, UserPriviliges.Permissions.TaxOverrideAllForRx.Name, out sRxUserID))    //PRIMEPOS-2402 14-Jul-2021 JY Added sRxUserID
                    isEditable = true;
                else
                    return;
            }
            if (bIsOTC)
            {
                if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID, UserPriviliges.Permissions.TaxOverrideAll.ID, UserPriviliges.Permissions.TaxOverrideAll.Name, out sPOSUserID))   //PRIMEPOS-2402 14-Jul-2021 JY Added sPOSUserID
                    isEditable = true;
                else
                    return;
            }
            if (isEditable)
            {
                try
                {
                    int i = 0;
                    taxoverflag = 1;
                    while (i < grdDetail.Rows.Count)
                    {
                        grdDetail.Rows[i].Activate();
                        tbEditItemActions("removeindTax", sPOSUserID, sRxUserID);   //PRIMEPOS-2402 14-Jul-2021 JY modified
                        i++;
                    }
                    taxoverflag = 0;
                    this.txtItemCode.Focus();
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "taxOverrideAll()");
                    clsUIHelper.ShowErrorMsg(ex.Message);
                }
            }
            //    }
            //}
        }
        #endregion

        #region PRIMEPOS-2510 26-Apr-2018 JY Added logic to check OTC/Rx item exists in line items
        private void CheckItemStatus(ref bool bIsRx, ref bool bIsOTC)
        {
            int rxCnt = 0, OTCCnt = 0;
            foreach (UltraGridRow oGRow in this.grdDetail.Rows)
            {
                if (Configuration.convertNullToString(oGRow.Cells[clsPOSDBConstants.TransDetail_Fld_ItemID].Value).Trim().ToUpper() == "RX")
                {
                    bIsRx = true;
                    rxCnt += 1;
                }
                else
                {
                    bIsOTC = true;
                    OTCCnt += 1;
                }
                if (rxCnt > 0 && OTCCnt > 0) break;
            }
        }
        #endregion
    }
}
