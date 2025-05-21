//Added By Shitaljit(QuicSolv)---------------------
//To Calculate selling price by adding fixed and percentage  based on current cost price and current selling price.
//Started working on 19 April 2011-----------------

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using POS_Core_UI.Reports.Reports;
//using POS.UI;
using POS_Core.CommonData;
using System.Data;
using POS_Core.BusinessRules;
using POS_Core.DataAccess;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;
//using POS_Core.DataAccess;
using System.Data.SqlClient;
using System.Text;
using System.Collections.Generic;
using POS_Core.Resources;

namespace POS_Core_UI
{
    public partial class frmSetSellingPrice : Form
    {        
        private ItemData oItemData = new ItemData();
        private ItemRow oItemRow;
        private DataSet dsDept;
        private DataSet dsItem;
        private Department oDept;
        private IDataAdapter da;
        private POSSET oPOSSet;
        public String labelText;
        frmSellingPriceCalculate oSelPriceCal;
        private double addAmt;
        private bool fixCostFlag = false;
        private bool fixSellFlag = false;
        private bool perSellFlag = false;
        private bool perCostFlag = false;
        private bool listCountFlag = false;
        List<string> listDept = new List<string>();


        public frmSetSellingPrice()
        {
            InitializeComponent();
            SetNew();
        }


        private void optYesNo_ValueChanged(object sender, EventArgs e)
        {
            if (optYesNo.CheckedIndex == 0)
            {
                gbpBoxDept.Enabled = false;
                numfromCostPrice.Enabled = false;
                numtoCostPrice.Enabled = false;
                numfromSellPrice.Enabled = false;
                numtoSellPrice.Enabled = false;
            }
            else if (optYesNo.CheckedIndex == 1)
            {
                gbpBoxDept.Enabled = true;
            }
        }

        private void ApplyGrigFormat()
        {
            clsUIHelper.SetAppearance(this.grdSellingPrice);

            this.grdSellingPrice.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_ItemID].Hidden = false;
            this.grdSellingPrice.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].MaxLength = 50;
            this.grdSellingPrice.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Header.Caption = "Item Description";
            this.grdSellingPrice.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].CellActivation = Activation.Disabled;
            this.grdSellingPrice.DisplayLayout.Bands[0].Columns[clsPOSDBConstants.Item_Fld_Description].Width = 250;
        }

        private void frmSetSellingPrice_Load(object sender, EventArgs e)
        {
            clsUIHelper.setColorSchecme(this);

            clsUIHelper.SetKeyActionMappings(this.grdSellingPrice);

            oPOSSet = Configuration.CPOSSet;
            numfromSellPrice.Enabled = false;
            numfromCostPrice.Enabled = false;
            numtoCostPrice.Enabled = false;
            numtoSellPrice.Enabled = false;
            cmbSellingPriceCalCriteria.SelectedIndex = 0;
            numAddedAmount.Enabled = false;
            this.numfromCostPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numfromCostPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.numfromSellPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numfromSellPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.numtoCostPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numtoCostPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);
            this.numtoSellPrice.AfterEnterEditMode += new System.EventHandler(clsUIHelper.AfterEnterEditMode);
            this.numtoSellPrice.AfterExitEditMode += new System.EventHandler(clsUIHelper.AfterExitEditMode);

            //////UI.clsUIHelper.SetKeyActionMappings(this.grdSellingPrice);
            //////grdSellingPrice.DisplayLayout.Bands[0].Columns["Check Items"].Header.VisiblePosition = 0;
            ShowDepartment();

        }


        private void ShowDepartment()
        {
            try
            {
                dsDept = new DataSet();
                oDept = new Department();
                dsDept = oDept.PopulateList();
                clsUIHelper.SetKeyActionMappings(this.grdDept);
                grdDept.DataSource = dsDept;
                grdDept.DisplayLayout.Bands[0].Columns["Dept"].Header.SetVisiblePosition(0,false);
                grdDept.DisplayLayout.Bands[0].Columns["DeptID"].Hidden = true;
                
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
        }



        private void ShowData()
        {
            try
            {
                bool clearFlag = false;
                dsItem = new DataSet();
                string sSQL = "";
                try
                {
                    string InQuery = string.Join(",", listDept.ToArray());
                    grdDept.Refresh();

                    if (optYesNo.CheckedIndex == 0)
                    {
                        if (fixCostFlag)
                        {
                            sSQL = "SELECT ItemID, DepartmentID, Description, LastCostPrice as CostPrice, SellingPrice" +
                            ", ROUND((LastCostPrice  +'" + addAmt + "'),2) as NewSellingPrice " +
                            "FROM Item WHERE SellingPrice > 0 AND LastCostPrice > 0  ";
                        }
                        else if (fixSellFlag)
                        {
                            sSQL = "SELECT ItemID, DepartmentID, Description, SellingPrice" +
                            ", ROUND((SellingPrice + '" + addAmt + "'),2) as NewSellingPrice " +
                            "FROM Item WHERE SellingPrice > 0 AND LastCostPrice > 0 ";
                        }
                        else if (perCostFlag)
                        {
                            sSQL = "SELECT ItemID, DepartmentID, Description, LastCostPrice as CostPrice, SellingPrice" +
                                ", ROUND((LastCostPrice + (LastCostPrice * '" + addAmt + "' /100)),2) as NewSellingPrice " +
                                "FROM Item WHERE SellingPrice > 0 AND LastCostPrice > 0  ";
                        }
                        else if (perSellFlag)
                        {
                            sSQL = "SELECT ItemID, DepartmentID, Description, SellingPrice" +
                                ", ROUND((SellingPrice + (SellingPrice * '" + addAmt + "' /100)),2) as NewSellingPrice " +
                                "FROM Item WHERE SellingPrice > 0 AND LastCostPrice > 0 ";
                        }
                    }
                    else
                        if (optYesNo.CheckedIndex == 1)
                        {
                            if (fixCostFlag)
                            {
                                sSQL = "SELECT ItemID, DepartmentID, Description, LastCostPrice as CostPrice, SellingPrice" +
                                 ", ROUND((LastCostPrice + '" + addAmt + "'),2) as NewSellingPrice " +
                                 "FROM Item WHERE SellingPrice > 0 AND LastCostPrice > 0 " +
                                 "AND DepartmentID IN (SELECT DeptID from Department WHERE DeptID IN (" + InQuery + "))";
                            }
                            else if (perSellFlag)
                            {
                                sSQL = "SELECT ItemID, DepartmentID, Description, SellingPrice" +
                                    ", ROUND((SellingPrice + (SellingPrice * '" + addAmt + "' /100)),2) as NewSellingPrice  " +
                                    "FROM Item WHERE SellingPrice > 0 AND LastCostPrice > 0 " +
                                    "AND DepartmentID IN (SELECT DeptID from Department WHERE DeptID IN (" + InQuery + "))";

                            }
                            else if (fixSellFlag)
                            {
                                sSQL = "SELECT ItemID, DepartmentID, Description, SellingPrice" +
                                ", ROUND((SellingPrice + '" + addAmt + "'),2) as NewSellingPrice " +
                                "FROM Item WHERE SellingPrice > 0 AND LastCostPrice > 0 " +
                                "AND DepartmentID IN (SELECT DeptID from Department WHERE DeptID IN (" + InQuery + "))";
                            }
                            else if (perCostFlag)
                            {
                                sSQL = "SELECT ItemID, DepartmentID, Description, LastCostPrice as CostPrice, SellingPrice" +
                                    ", ROUND((LastCostPrice +(LastCostPrice * '" + addAmt + "' /100)),2) as NewSellingPrice " +
                                    "FROM Item WHERE SellingPrice > 0 AND LastCostPrice > 0 " +
                                    "AND DepartmentID IN (SELECT DeptID from Department WHERE DeptID IN (" + InQuery + "))";
                            }
                        }

                    sSQL = sSQL + buildCriteria();
                    Search oSearch = new Search();
                    dsItem = oSearch.SearchData(sSQL);
                    grdSellingPrice.DataSource = dsItem;
                    listDept.Clear();
                }
                
                catch (Exception exp)
                {
                    throw (exp);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }


        private string buildCriteria()
        {
            string sCriteria = "";
            if (Convert.ToDouble(numfromCostPrice.Value) >= 0 && Convert.ToDouble(numtoCostPrice.Value) > 0)
            {
                sCriteria = sCriteria + ((sCriteria == "") ? " AND " : " AND ") + " LastCostPrice  BETWEEN '" + numfromCostPrice.Value + "' AND '" + numtoCostPrice.Value + "'";
            }

            if (Convert.ToDouble(numfromSellPrice.Value) >= 0 && Convert.ToDouble(numtoSellPrice.Value) > 0)
            {
                sCriteria = sCriteria + ((sCriteria == "") ? " AND " : " AND  ") + " SellingPrice BETWEEN '" + numfromSellPrice.Value + "' AND '" + numtoSellPrice.Value + "'";

            }
            return sCriteria;

        }

        private bool validateField()
        {
            string errorMsg = "";
            bool validateFlag = false;
            try
            {

                for (int GridRowNo = 0; GridRowNo < grdDept.Rows.Count; GridRowNo++)
                {
                    if (Convert.ToBoolean(this.grdDept.Rows[GridRowNo].Cells["Dept"].Value))
                    {
                        listDept.Add(this.grdDept.Rows[GridRowNo].Cells[0].Value.ToString());
                    }
                }

                if (listDept.Count > 0)
                {
                    listCountFlag = true;

                }
                else if (!(listDept.Count > 0) && optYesNo.CheckedIndex == 1)
                {
                    listCountFlag = false;
                    errorMsg = "Please Select A Department";
                    validateFlag = true;

                }
                if (cmbSellingPriceCalCriteria.SelectedIndex != 1 && cmbSellingPriceCalCriteria.SelectedIndex != 2 && cmbSellingPriceCalCriteria.SelectedIndex != 3 && cmbSellingPriceCalCriteria.SelectedIndex != 4)
                {

                    errorMsg += "\nSelect One Of The Option For Calculation";

                    validateFlag = true;
                }
            }
            catch (Exception exp)
            {

                clsUIHelper.ShowErrorMsg(exp.Message);
            }
            if (validateFlag)
            {
                clsUIHelper.ShowWarningMsg(errorMsg);
            }

            return validateFlag;
        }


        private void btnShowItem_Click(object sender, EventArgs e)
        {
            int rowIndex = 0;

            if (!validateField())
            {
                grdSellingPrice.Refresh();

                ShowData();

                if (grdSellingPrice.Rows.Count > 0)
                {

                    foreach (DataRow oRow in dsItem.Tables[0].Rows)
                    {
                        this.grdSellingPrice.Rows[rowIndex].Cells["Check Items"].Value = true;
                        rowIndex++;
                    }


                }
            }

            txtEditorNoOfItems.Text = Convert.ToString(grdSellingPrice.Rows.Count);
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (grdSellingPrice.Rows.Count > 0)
            {
                if (Resources.Message.Display("Abandon Selection, Are you Sure you want to close this form?", "Abandon Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    this.Close();

                }
            }
            else 
            {
                this.Close();
            }

        }

        private void bttnClear_Click(object sender, EventArgs e)
        {
            if (Resources.Message.Display("Abandon Selection, Are you Sure?", "Abandon Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                DataSet dsTemp = new DataSet();
                grdDept.ClearUndoHistory();
                listDept.Clear();
                dsTemp = dsItem.Copy();
                if (dsTemp != null)
                {
                    if (dsItem.Tables.Count != 0)
                    {
                        if (dsItem.Tables[0].Rows.Count > 0)
                        {
                            dsItem.Tables[0].Clear();
                        }
                    }
                }

            }
            txtEditorNoOfItems.Text = "";
            this.chckselectAlDept.Checked = false;
            this.chckSelectAllItem.Checked = false;
            ShowDepartment();
        }

        private void bttnClear_Click(object sender, EventArgs e, bool cleaFlag)
        {
            if (grdSellingPrice.Rows.Count > 0 && cleaFlag == true)
            {
                DataSet dsTemp = new DataSet();
                dsTemp = dsItem.Copy();
                if (dsTemp != null)
                {
                    if (dsItem.Tables[0].Rows.Count > 0)
                    {
                        dsItem.Tables[0].Clear();
                    }

                }

            }
            txtEditorNoOfItems.Text = "";
        }


        private void setComboBoxState(string strlabelText)
        {
            try
            {
                bool OkCalcel = false;
                oSelPriceCal = new frmSellingPriceCalculate(strlabelText);

                oSelPriceCal.ShowDialog(this);
                if (cmbSellingPriceCalCriteria.SelectedIndex == 2)
                {
                    OkCalcel = oSelPriceCal.RETVAl;
                    addAmt = oSelPriceCal.ADDAMT;
                    lblSelectedCriteria.Text = "Fixed Amount Added On Current Cost Price";
                    numAddedAmount.Value = addAmt;
                }

                if (cmbSellingPriceCalCriteria.SelectedIndex == 1)
                {
                    OkCalcel = oSelPriceCal.RETVAl;
                    addAmt = oSelPriceCal.ADDAMT;
                    lblSelectedCriteria.Text = "Fixed Amount Added On Current Selling Price";
                    numAddedAmount.Value = oSelPriceCal.ADDAMT; ;
                }

                if (cmbSellingPriceCalCriteria.SelectedIndex == 3)
                {
                    OkCalcel = oSelPriceCal.RETVAl;
                    addAmt = oSelPriceCal.ADDAMT;
                    lblSelectedCriteria.Text = "Amount In Percentage Added On Current Selling Price";
                    numAddedAmount.Value = oSelPriceCal.ADDAMT; ;
                }
                if (cmbSellingPriceCalCriteria.SelectedIndex == 4)
                {

                    OkCalcel = oSelPriceCal.RETVAl;
                    addAmt = oSelPriceCal.ADDAMT;
                    lblSelectedCriteria.Text = "Amount In Percentage Added On Current Cost Price";
                    numAddedAmount.Value = oSelPriceCal.ADDAMT;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);

            }
        }

        private void chckSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
            try
            {
                if (chckSelectAllItem.Checked)
                {
                    chckUnselectAllItem.Checked = false;
                    if (grdSellingPrice.Rows.Count > 0)
                    {

                        foreach (DataRow oRow in dsItem.Tables[0].Rows)
                        {
                            this.grdSellingPrice.Rows[rowIndex].Cells["Check Items"].Value = true;
                            rowIndex++;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void chckClearAll_CheckedChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
            try
            {
                if (chckUnselectAllItem.Checked)
                {
                    chckSelectAllItem.Checked = false;
                    if (grdSellingPrice.Rows.Count > 0)
                    {
                        foreach (DataRow oRow in dsItem.Tables[0].Rows)
                        {
                            this.grdSellingPrice.Rows[rowIndex].Cells["Check Items"].Value = false;
                            rowIndex++;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmSetSellingPrice_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter || e.KeyData == System.Windows.Forms.Keys.Tab)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }

            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }


        private void SetNew()
        {            
            oItemData = new ItemData();

            //Added By Shitaljit(QuicSolv) on 1 July 2011
            //Added last "false" value for isEBTItem field to set it false By default
            oItemRow = oItemData.Item.AddRow("", 0, "", "", "", "", "", "", 0, 0, 0, 0, false, 0, false, 0, DBNull.Value
                , DBNull.Value, 0, 0, "", 0, 0, 0, null
                , "", "", System.DateTime.MinValue, System.DateTime.MinValue, "", false, false, false, false, true, 0, false, 0, "", true, true, false, 0
                , 0, 0, 0, 0 ); //Sprint-21 - 2173 06-Jul-2015 JY Added "True" parameter for IsActive   //PRIMEPOS-2592 01-Nov-2018 JY Added "false" for IsNonRefundable // Added for Solutran: 0,0,0,0 - PRIMEPOS-2663 - NileshJ - 05-July-2019
        }

        private void bttnApplyChanges_Click(object sender, EventArgs e)
        {
            if (UserPriviliges.getPermission(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.POSTransaction.ID,Configuration.convertNullToInt(UserPriviliges.Permissions.InvInventoryReceived), UserPriviliges.Permissions.InvInventoryReceived.Name))
            {
                //List<string> sellingCal = new List<string>();
                //ItemSvr oItemSvr = new ItemSvr();
                int selectedRowCount = 0;

                #region PRIMEPOS-3125 22-Dec-2022 JY Added
                DataTable dtItemSellingPriceType = new DataTable();
                dtItemSellingPriceType.Columns.Add("ItemID", Type.GetType("System.String"));
                dtItemSellingPriceType.Columns.Add("NewSellingPrice", Type.GetType("System.Decimal"));
                dtItemSellingPriceType.Columns.Add("OldSellingPrice", Type.GetType("System.Decimal"));
                #endregion

                try
                {
                    StringBuilder strBuildItem = new StringBuilder();
                    for (int GridRowNo = 0; GridRowNo < grdSellingPrice.Rows.Count; GridRowNo++)
                    {
                        if (Convert.ToBoolean(grdSellingPrice.Rows[GridRowNo].Cells["Check Items"].Value))
                        {
                            #region PRIMEPOS-3125 22-Dec-2022 JY Commented
                            //if (strBuildItem.ToString() != "")
                            //    strBuildItem.Append(",");
                            //strBuildItem.Append("'" + grdSellingPrice.Rows[GridRowNo].Cells["ItemID"].Value.ToString().Replace("'", "''") + "'");
                            //sellingCal.Add(grdSellingPrice.Rows[GridRowNo].Cells["NewSellingPrice"].Value.ToString());                            
                            #endregion

                            selectedRowCount++;

                            #region PRIMEPOS-3125 22-Dec-2022 JY Added
                            DataRow row = dtItemSellingPriceType.NewRow();
                            row[0] = grdSellingPrice.Rows[GridRowNo].Cells["ItemID"].Value.ToString();
                            row[1] = Configuration.convertNullToDecimal(grdSellingPrice.Rows[GridRowNo].Cells["NewSellingPrice"].Value.ToString());
                            row[2] = Configuration.convertNullToDecimal(grdSellingPrice.Rows[GridRowNo].Cells["SellingPrice"].Value.ToString());
                            dtItemSellingPriceType.Rows.Add(row);
                            #endregion
                        }
                    }

                    if (!(selectedRowCount > 0))
                    {
                        clsUIHelper.ShowWarningMsg("Please Select A Row For Calculation");
                        return;
                    }

                    if (Resources.Message.Display("Are you sure you want to Apply Changes To Item Table?", "Set Selling Price", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        #region PRIMEPOS-3125 22-Dec-2022 JY Added
                        Item oItem = new Item();
                        oItem.UpdateBulkSellingPrice(dtItemSellingPriceType);                        
                        #endregion

                        #region PRIMEPOS-3125 22-Dec-2022 JY Commented
                        //string sSQL = "";
                        //sSQL = "SELECT * FROM ITEM WHERE ItemID IN (" + strBuildItem.ToString() + ")";
                        //DataSet dsItem = new DataSet();
                        //Search oSearch = new Search();
                        //oItemData.Item.Clear();
                        //dsItem = oSearch.SearchData(sSQL);
                        //oItemData.Item.MergeTable(dsItem.Tables[0]);
                        //Configuration.UpdatedBy = "M";
                        //int rowIndex = 0;
                        //foreach (ItemRow oRow in oItemData.Item.Rows)
                        //{
                        //    oRow.SellingPrice =Configuration.convertNullToDecimal(sellingCal[rowIndex]);
                        //    rowIndex++;
                        //}
                        //Item oBRItem = new Item();
                        //oBRItem.Persist(oItemData);
                        //rowIndex = 0;
                        //foreach (DataRow oRow in dsItem.Tables[0].Rows)
                        //{
                        //    this.grdSellingPrice.Rows[rowIndex].Cells["Check Items"].Value = false;
                        //    rowIndex++;
                        //}
                        #endregion

                        Application.DoEvents();
                    }
                    clsUIHelper.ShowOKMsg(selectedRowCount + "  Items Affected On Database.");
                }
                catch (Exception exp)
                {
                    clsUIHelper.ShowErrorMsg(exp.Message);
                }
            }
        }

        private void cmbSellingPriceCalCriteria_SelectionChanged(object sender, EventArgs e)
        {
            #region PRIMEPOS-2464 06-Mar-2020 JY Added
            if (UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.ItemFile.ID, UserPriviliges.Permissions.DisplayItemCost.ID) == false)
            {
                if (cmbSellingPriceCalCriteria.SelectedIndex == 2 || cmbSellingPriceCalCriteria.SelectedIndex == 4)
                {
                    string strMsg = "We couldn't change the Item Cost Price as \"Display Item Cost\" user-level settings turned off";
                    clsUIHelper.ShowWarningMsg(strMsg);
                    cmbSellingPriceCalCriteria.SelectedIndex = 0;
                    return;
                }
            }
            #endregion

            if (cmbSellingPriceCalCriteria.SelectedIndex == 2)
            {
                numtoCostPrice.Enabled = true;
                numfromCostPrice.Enabled = true;
                numfromCostPrice.Focus();
                fixCostFlag = true;
                labelText = "Enter The Fixed Amount To Be Added On Cost Price: ";
                setComboBoxState(labelText);
            }
            else
            {
                fixCostFlag = false;

            }

            if (cmbSellingPriceCalCriteria.SelectedIndex == 4)
            {
                numtoCostPrice.Enabled = true;
                numfromCostPrice.Enabled = true;
                numfromCostPrice.Focus();
                perCostFlag = true;
                labelText = "Enter The Amount in Percentage To Be Added On Current Cost Price: ";
                setComboBoxState(labelText);
            }
            else
            {
                perCostFlag = false;
            }
            if (cmbSellingPriceCalCriteria.SelectedIndex == 1)
            {
                numfromSellPrice.Enabled = true;
                numtoSellPrice.Enabled = true;
                numfromSellPrice.Focus();
                fixSellFlag = true;
                labelText = "Enter The Fixed Amount To Be Added On Current Selling Price: ";
                setComboBoxState(labelText);
            }
            else
            {
                fixSellFlag = false;
            }
            if (cmbSellingPriceCalCriteria.SelectedIndex == 3)
            {
                numfromSellPrice.Enabled = true;
                numtoSellPrice.Enabled = true;
                perSellFlag = true;
                numfromSellPrice.Focus();
                labelText = "Enter The Amount in Percentage To Be Added On Current Selling Price: ";
                setComboBoxState(labelText);
            }
            else
            {
                perSellFlag = false;
            }

            if (cmbSellingPriceCalCriteria.SelectedIndex == 1 || cmbSellingPriceCalCriteria.SelectedIndex == 3)
            {
                numfromCostPrice.Enabled = false;
                numtoCostPrice.Enabled = false;
                numtoCostPrice.Value = 0;
                numfromCostPrice.Value = 0;

            }
            if (cmbSellingPriceCalCriteria.SelectedIndex == 2 || cmbSellingPriceCalCriteria.SelectedIndex == 4)
            {
                numtoSellPrice.Enabled = false;
                numfromSellPrice.Enabled = false;
                numfromSellPrice.Value = 0;
                numtoSellPrice.Value = 0;
            }
        }

        private void chcksellectAllDept_CheckedChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
            if (chckselectAlDept.Checked)
            {
                chckUnselectAllDept.Checked = false;
                if (grdDept.Rows.Count > 0)
                {

                    foreach (DataRow oRow in dsDept.Tables[0].Rows)
                    {
                        this.grdDept.Rows[rowIndex].Cells["Dept"].Value = true;
                        rowIndex++;
                    }

                }
            }
        }


        private void chckUnsellectAllDept_CheckedChanged(object sender, EventArgs e)
        {
            int rowIndex = 0;
            if (chckUnselectAllDept.Checked)
            {
                chckselectAlDept.Checked = false;
                if (grdDept.Rows.Count > 0)
                {

                    foreach (DataRow oRow in dsDept.Tables[0].Rows)
                    {
                        this.grdDept.Rows[rowIndex].Cells["Dept"].Value = false;
                        rowIndex++;

                    }

                }
            }
        }
    }
}