using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//using POS.UI;
using POS_Core.CommonData;
using POS_Core_UI.Reports.Reports;
using POS_Core.BusinessRules;
using Infragistics.Win.UltraWinGrid;    //Sprint-18 - 2144 05-Dec-2014 JY added

namespace POS_Core_UI.Reports.ReportsUI
{
    public partial class frmRptPriceOverridden : Form
    {
        public frmRptPriceOverridden()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception Ex)
            {
            }
        }

        private void txtItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchItem(txtItemCode);   //Sprint-18 - 2144 05-Dec-2014 JY Added to select multiple items
            #region Sprint-18 - 2144 05-Dec-2014 JY commented
            //try
            //{
            //    frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
            //    oSearch.ShowDialog();
            //    if (oSearch.IsCanceled) return;
            //    txtItemCode.Text = oSearch.SelectedRowID();
            //}
            //catch (Exception Ex)
            //{
            //}
            #endregion
        }

        private void txtUserId_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Users_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Users_tbl;  //20-Dec-2017 JY Added 
                oSearch.ShowDialog();
                if (oSearch.IsCanceled) return;
                txtUserId.Text = oSearch.SelectedRowID();
            }
            catch (Exception Ex)
            {
            }
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                        dtpFromDate.Value = DateTime.Now.Date;
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private bool validateFields(out string fieldName)
        {
            bool isValid = true;
            string field = string.Empty;
            try
            {
                if ((DateTime)dtpFromDate.Value > (DateTime)dtpToDate.Value)
                {
                    isValid = false;
                    fieldName = "DATE";
                    return isValid;
                }
            }
            catch (Exception ex)
            {
                clsUIHelper.ShowErrorMsg(ex.Message);
            }
            fieldName = field;
            return isValid;
        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            string fieldName = string.Empty;
            try
            {
                if (!validateFields(out fieldName))
                {
                    if (fieldName == "DATE")
                    {
                        clsUIHelper.ShowErrorMsg("From Date can not be greater than To Date.");
                        dtpToDate.Value = DateTime.Now.Date;
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void txtItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    this.txtItemCode_EditorButtonClick(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void dtpFromDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void dtpToDate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void txtUserId_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    this.txtUserId_EditorButtonClick(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void rdSelectTransWise_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            try
            {
                Preview(false);
            }
            catch (Exception Ex)
            {
            }
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                Preview(true);
            }
            catch (Exception Ex)
            {
            }
        }

        private void Preview(bool PrintId)
        {
            try
            {
                string rptTitle = string.Empty;
                string sSQL = string.Empty;
                Item oItem = new Item();
                rptPriceOverridden oRpt = new rptPriceOverridden();
                DataTable dtItemPriceHist = new DataTable();

                sSQL = GetSelectQuery();

                if (rdbItem.Checked)
                    rptTitle = "By Item";
                if (rdbUser.Checked)
                    rptTitle = "By User";
                if (rdbTrans.Checked)
                    rptTitle = "By Transaction";
                clsReports.setCRTextObjectText("txtFromDate", "From :" + this.dtpFromDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtToDate", "To :" + dtpToDate.Text, oRpt);
                clsReports.setCRTextObjectText("txtRptTitle", rptTitle, oRpt);
                oRpt.Database.Tables[0].SetDataSource(clsReports.GetReportSource(sSQL).Tables[0]);
                clsReports.SetRepParam(oRpt, "Group", rptTitle);
                clsReports.DStoExport = clsReports.GetReportSource(sSQL); //PRIMEPOS-2471 16-Feb-2021 JY Added
                clsReports.Preview(PrintId, oRpt);
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private string buildCriteria()
        {
            string sCriteria = "";
            try
            {
                if (dtpFromDate.Value.ToString() != "")
                    sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,a.AddedOn,107)) >= '" + dtpFromDate.Text + "'";
                if (dtpToDate.Value.ToString() != "")
                    sCriteria = sCriteria + " AND Convert(smalldatetime,Convert(Varchar,a.AddedOn,107)) <= '" + dtpToDate.Text + "'";
                if (txtItemCode.Text.Trim() != "")
                {
                    //sCriteria = sCriteria + " AND ItemID = '" + txtItemCode.Text + "'";   //Sprint-18 - 2144 05-Dec-2014 JY commented
                    sCriteria += " AND a.ItemID IN (" + "'" + txtItemCode.Text.Replace(",", "','") + "'" +")";    //Sprint-18 - 2144 05-Dec-2014 JY Added
                }
                if (txtExcludeItemCode.Text.Trim() != "")
                    sCriteria += " AND a.ItemID NOT IN (" + "'" + txtExcludeItemCode.Text.Replace(",", "','") + "'" + ")";    //Sprint-18 - 2144 05-Dec-2014 JY Added
                if (txtUserId.Text.Trim().Replace("'", "''") != "")
                    sCriteria = sCriteria + " AND a.USerID = '" + txtUserId.Text + "'";
                if (txtTransID.Text.Trim().Replace("'", "''") != "")
                    sCriteria = sCriteria + " AND a.TransID = '" + txtTransID.Text + "'";
                
                #region PRIMEPOS-2294 22-Aug-2016 JY Added
                if (rdbOnlyRxItem.Checked)
                {
                    sCriteria += " AND a.ItemID LIKE '%RX'";
                }
                else if (rdbOnlyOTCItem.Checked)
                {
                    sCriteria += " AND a.ItemID NOT LIKE '%RX'";
                }
                #endregion

                #region Sprint-26 - PRIMEPOS-2294 28-Jul-2017 JY Added
                if (chkIsPriceChangedByOverride.Checked)
                    sCriteria += " AND IsPriceChangedByOverride = 1 ";
                #endregion
            }
            catch (Exception Ex)
            {
            }
            return sCriteria;
        }

        private string ConcatItemCodes(string strItemCodes)
        {
            string retItemCodes = string.Empty;
            retItemCodes = "'" + strItemCodes.Replace(",","','") + "'";
            return retItemCodes;
        }

        private string GetSelectQuery()
        {
            string sSQL = string.Empty;
            try
            {
                #region PRIMEPOS-2294 22-Aug-2016 JY Commented
                //#region OLD CODE
                ///*Date : 16/01/2014 
                // * Added By : Shitaljit    Ticket : 1727  
                // * This code is added for searching with filtering by Rx Items , Non Rx Items(OTC Items) and All Item */

                ///* Old Code */
                ////sSQL = "WITH PriceHist AS" +
                ////    " ( SELECT ID,ItemID,AddedOn,ChangedIn,UpdatedBy,saleprice,UserID,TransID,OrgSellingPrice, ROW_NUMBER() OVER( ORDER BY ID DESC) As [rowNum]" +
                ////    " FROM ItemPriceHistory where changedin='T'" + buildCriteria() + ")" +
                ////    " select PriceHist1.ID,PriceHist1.ItemID,PriceHist1.AddedOn,PriceHist1.ChangedIn,PriceHist1.UpdatedBy,PriceHist1.TransID,isnull(PriceHist1.SalePrice,0.00) as [Price AfterOverride]," +
                ////    " Isnull(PriceHist1.OrgSellingPrice,0.00) as [Price BeforeOverride],PriceHist1.UserID,isnull((PriceHist1.SalePrice - PriceHist1.OrgSellingPrice),0.00) as [Difference] " +
                ////    " from PriceHist PriceHist1 " +
                ////    " left outer JOIN PriceHist PriceHist2 ON PriceHist2.rowNum = PriceHist1.rowNum + 1 and PriceHist1.ItemID = PriceHist2.ItemID " +
                ////    " group by PriceHist1.id,PriceHist1.itemid,PriceHist1.AddedOn,PriceHist1.updatedby,PriceHist1.changedin,PriceHist1.SalePrice,PriceHist2.SalePrice,PriceHist1.userid,PriceHist1.TransID,PriceHist1.OrgSellingPrice order by PriceHist1.itemid,PriceHist1.ID desc";
                //#endregion

                ///*New Code*/
                //string ItemType = " ";
                //if (rdbOnlyRxItem.Checked)
                //{
                //    ItemType = " where a.ItemID LIKE '%RX'";
                //}
                //else if (rdbOnlyOTCItem.Checked)
                //{
                //    ItemType = " where a.ItemID NOT LIKE '%RX'";
                //}

                //sSQL = "WITH PriceHist AS" +
                //   " ( SELECT ID,ItemID,AddedOn,ChangedIn,UpdatedBy,saleprice,UserID,TransID,OrgSellingPrice, ROW_NUMBER() OVER( ORDER BY ID DESC) As [rowNum]" +
                //   " FROM ItemPriceHistory where 1=1 " + buildCriteria() + " )" +
                //   "  select distinct a.*  from ( "+
                //   "select PriceHist1.ID,PriceHist1.ItemID,PriceHist1.AddedOn,PriceHist1.ChangedIn,PriceHist1.UpdatedBy,PriceHist1.TransID,isnull(PriceHist1.SalePrice,0.00) as [Price AfterOverride]," +
                //   " Isnull(PriceHist1.OrgSellingPrice,0.00) as [Price BeforeOverride],PriceHist1.UserID,isnull((PriceHist1.SalePrice - PriceHist1.OrgSellingPrice),0.00) as [Difference] " +
                //   " from PriceHist PriceHist1 " +
                //   " left outer JOIN PriceHist PriceHist2 ON PriceHist2.rowNum = PriceHist1.rowNum + 1 and PriceHist1.ItemID = PriceHist2.ItemID " +
                //   " group by PriceHist1.id,PriceHist1.itemid,PriceHist1.AddedOn,PriceHist1.updatedby,PriceHist1.changedin,PriceHist1.SalePrice,PriceHist2.SalePrice,PriceHist1.userid,PriceHist1.TransID,PriceHist1.OrgSellingPrice) a, POSTransactionDetail " +
                //   " " + ItemType + "" +
                //   " order by a.itemid,a.ID desc";
                #endregion

                //PRIMEPOS-2294 22-Aug-2016 JY Added as the old SQL was wrong, also showing item price sale price or cost price changed record. Ideally it should populate only price overridden records through transaction
                //PRIMEPOS-2628 15-Jan-2019 JY Added Item Description
                sSQL = "SELECT a.ID, a.ItemID, ISNULL(b.Description,'') AS ItemDesc, a.AddedOn, a.ChangedIn, a.UpdatedBy, a.TransID, ISNULL(a.SalePrice,0.00) AS [Price AfterOverride], Isnull(a.OrgSellingPrice,0.00) AS [Price BeforeOverride], a.UserID, ISNULL((a.SalePrice - a.OrgSellingPrice),0.00) AS [Difference], a.IsPriceChangedByOverride, a.Remarks " +
                      " FROM ItemPriceHistory a LEFT JOIN Item b ON a.ItemID = b.ItemID " +
                      " WHERE a.TRANSID IS NOT NULL AND a.SalePrice<> a.ORGSELLINGPRICE " + buildCriteria() + " ORDER BY a.ItemID, a.ID DESC";
            }
            catch (Exception Ex)
            {
            }
            return sSQL;   
        }

        private void txtTransID_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.TransHeader_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.TransHeader_tbl;    //20-Dec-2017 JY Added
                oSearch.ShowDialog();
                if (oSearch.IsCanceled) return;
                txtTransID.Text = oSearch.SelectedRowID();
            }
            catch (Exception Ex)
            {
            }
        }

        private void txtTransID_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    this.txtTransID_EditorButtonClick(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        private void frmRptPriceOverridden_Shown(object sender, EventArgs e)
        {
            try
            {
                clsUIHelper.setColorSchecme(this);
                dtpToDate.Value = DateTime.Now.Date;
                dtpFromDate.Value = DateTime.Now.Date;
            }
            catch (Exception Ex)
            {
            }
        }

        private void frmRptPriceOverridden_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                }
            }
            catch (Exception Ex)
            {
            }
        }

        private void frmRptPriceOverridden_KeyUp(object sender, KeyEventArgs e)
        {
            try         
            {
                if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    if (txtItemCode.ContainsFocus)
                        txtItemCode_EditorButtonClick(null, null);
                    else if (txtTransID.ContainsFocus)
                        txtTransID_EditorButtonClick(null, null);
                    else if (txtUserId.ContainsFocus)
                        txtUserId_EditorButtonClick(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }

        #region Sprint-18 - 2144 05-Dec-2014 JY Added to select multiple items
        private void SearchItem(Infragistics.Win.UltraWinEditors.UltraTextEditor TextBox)
        {
            try
            {
                //frmSearch oSearch = new frmSearch(clsPOSDBConstants.Item_tbl);
                frmSearchMain oSearch = new frmSearchMain(true);    //20-Dec-2017 JY Added new reference
                oSearch.SearchTable = clsPOSDBConstants.Item_tbl;  //20-Dec-2017 JY Added 
                oSearch.AllowMultiRowSelect = true;
                oSearch.ShowDialog(this);
                if (!oSearch.IsCanceled)
                {
                    string strItemCode = "";
                    string strItemDesc = "";
                    if (TextBox.Text.Trim() != "")  TextBox.Text = "," + TextBox.Text + ",";
                    foreach (UltraGridRow oRow in oSearch.grdSearch.Rows)
                    {
                        if ((bool)oRow.Cells["check"].Value == true)
                        {
                            if (TextBox.Text.Trim() != "")
                            {
                                if (TextBox.Text.Contains("," + oRow.Cells["Item Code"].Text + ",") == false)
                                {
                                    strItemCode += "," + oRow.Cells["Item Code"].Text;
                                    strItemDesc += "," + oRow.Cells["item Description"].Text;
                                }
                            }
                            else
                            {
                                strItemCode += "," + oRow.Cells["Item Code"].Text;
                                strItemDesc += "," + oRow.Cells["item Description"].Text;
                            }
                        }
                    }

                    if (TextBox.Text.Trim() != "")
                    {
                        TextBox.Text = TextBox.Text.Substring(1, TextBox.Text.Length-2) + strItemCode;
                        TextBox.Tag = TextBox.Tag.ToString() + strItemDesc;
                    }
                    else
                    {
                        if (strItemCode.Trim() != "")
                        {
                            TextBox.Text = strItemCode.Substring(1);
                            TextBox.Tag = strItemDesc.Substring(1);
                        }
                    }
                }
                else
                {
                    TextBox.Text = string.Empty;
                    TextBox.Tag = string.Empty;
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion

        #region Sprint-18 - 2144 05-Dec-2014 JY added to exclude items
        private void txtExcludeItemCode_EditorButtonClick(object sender, Infragistics.Win.UltraWinEditors.EditorButtonEventArgs e)
        {
            SearchItem(txtExcludeItemCode);   
        }

        private void txtExcludeItemCode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == System.Windows.Forms.Keys.Enter)
                {
                    this.SelectNextControl(this.ActiveControl, true, true, true, true);
                }
                else if (e.KeyData == System.Windows.Forms.Keys.F4)
                {
                    this.txtExcludeItemCode_EditorButtonClick(null, null);
                }
            }
            catch (Exception exp)
            {
                clsUIHelper.ShowErrorMsg(exp.Message);
            }
        }
        #endregion
    }
}