// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.ErrorLogging;
using Resources;
using NLog;
using POS_Core.Resources;
using System.Collections;
////using POS.Resources;

namespace POS_Core.DataAccess
{
    // Provides data access methods for DeptCode
    public class TransDetailSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

        public void Persist(TransDetailData updates, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData)
        {
            try
            {
                //this.Delete(updates, tx);
                this.Insert(updates, tx, TransID, dsRX, oTDTaxData, oItemMonitorTransDetailData, oTransSignLogData);
                //this.Update(updates, tx);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(TransDetailData updates, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(TransDetailData updates, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(TransDetailData updates, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData)");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }

        public void PutOnHold(TransDetailData updates, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData)
        {
            try
            {
                TransDetailRXSvr oSvr = new TransDetailRXSvr();
                TransDetailTaxSvr oTDTaxSrv = new TransDetailTaxSvr();
                oSvr.DeleteOnHold(TransID, tx);

                this.DeleteOnHold(TransID, tx);
                this.InsertOnHold(updates, tx, TransID, dsRX, oTDTaxData);  //Sprint-22 - 05-Oct-2015 JY Added oTDTaxData
                //this.UpdateOnHold(updates, tx,TransID);

                //TransDetailRXSvr oSvr = new TransDetailRXSvr();
                oSvr.PutOnHold(dsRX, tx);
                oTDTaxSrv.PutOnHold(oTDTaxData, tx, TransID);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PutOnHold(TransDetailData updates, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PutOnHold(TransDetailData updates, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PutOnHold(TransDetailData updates, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData)");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion Persist Methods

        #region Insert, Update, and Delete Methods
        public static string strFormatedRXItemID = "";//Added by Krishna to access this information from ItemSvr to verify Item ID for Price change in AddItemPriceHistory(..) method.
        public void Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData)
        {
            TransDetailTable addedTable = ds.TransDetail;
            //.GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;
            int Row = 0;
            List<int> lstTaxTableRowStatus = new List<int>();
            List<int> lstValidRx = new List<int>();   //Sprint-22 02-Nov-2015 JY Added to ignore invalid rx entries from POSTransactionRXDetail table

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (TransDetailRow row in addedTable.Rows)
                {
                    try
                    {
                        Row = row.TransDetailID;
                        row.TransID = TransID;
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.TransDetail_tbl, insParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        int TransDetailID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));

                        ItemSvr oISvr = new ItemSvr();
                        oISvr.LessStock(row.ItemID, row.QTY, tx);

                        //                        if (row.ItemID.ToUpper() != "RX" && row.IsPriceChanged == true)//Commented by Krishna on 5 October 2011 to show RX items in PriceOverride Report
                        if (row.IsPriceChanged == true)
                        {
                            if (row.ItemID.ToUpper() == "RX" && dsRX.TransDetailRX.Rows.Count > 0)
                            {
                                foreach (TransDetailRXRow rxRow in dsRX.TransDetailRX.Rows)
                                {
                                    if (rxRow.TransDetailID == row.TransDetailID)
                                    {
                                        strFormatedRXItemID = rxRow.RXNo + "-" + rxRow.NRefill + "RX";    //PRIMEPOS-3015 26-Oct-2021 JY Modified
                                        row.ItemID = strFormatedRXItemID;
                                        break;
                                    }
                                }
                            }//End of Added by Krishna on 5 October 2011
                            Configuration.UpdatedBy = "T";
                            oISvr.AddItemPriceHistory(row.ItemID, row.Price, row.ItemCost, row.UserID, "T", Configuration.UpdatedBy, tx, row.TransID, 0, row.IsPriceChangedByOverride, row.OverrideRemark);    //Sprint-25 - PRIMEPOS-294 04-May-2017 JY sending promotional price 0 as it will not change in transaction    //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added row.IsPriceChangedByOverride  //PRIMEPOS-3015 26-Oct-2021 JY Added OverrideRemark
                            if (row.ItemID == strFormatedRXItemID)
                            {
                                row.ItemID = "RX";
                                strFormatedRXItemID = "";
                            }
                        }

                        //int TransDetailID=Convert.ToInt32(DataHelper.ExecuteScalar(tx,CommandType.Text,"select @@identity"));

                        if (row.ItemID.ToUpper() == "RX")
                        {
                            foreach (TransDetailRXRow rxRow in dsRX.TransDetailRX.Rows)
                            {
                                if (rxRow.TransDetailID == row.TransDetailID)
                                {
                                    rxRow.TransDetailID = TransDetailID;
                                    lstValidRx.Add(rxRow.RXDetailID);    //Sprint-22 02-Nov-2015 JY Added to ignore invalid rx entries from POSTransactionRXDetail table
                                    break;
                                }
                            }
                        }

                        //Added By Shitaljit for Multiple tacx to items.
                        //if (row.TaxAmount > 0)    //Sprint-18 - 2142 10-Dec-2014 JY Commented because TransDetailId not updated in taxdetail table in case of return trans
                        if (row.TaxAmount != 0) //Sprint-18 - 2142 10-Dec-2014 JY Added corrected code
                        {
                            int nPrevRow = 0, cnt = 0;   //Sprint-21 18-Sep-2015 JY Added
                            //Fix Tax issue 6/11/2015
                            foreach (TransDetailTaxRow oTaxRow in oTDTaxData.TransDetailTax.Rows)
                            {
                                //if (oTaxRow.ItemID == row.ItemID && row.TransDetailID == Row)
                                //if(oTaxRow.ItemRow == Row)    //Sprint-21 18-Sep-2015 JY Commented
                                if (oTaxRow.ItemID == row.ItemID && lstTaxTableRowStatus.Contains(oTaxRow.ItemRow) == false)    //Sprint-21 18-Sep-2015 JY Added to resolve tax issue
                                {
                                    #region Sprint-21 18-Sep-2015 JY Added to resolve tax issue
                                    if (cnt == 0)
                                    {
                                        nPrevRow = oTaxRow.ItemRow;
                                        cnt++;
                                    }
                                    if (nPrevRow != oTaxRow.ItemRow)
                                    {
                                        lstTaxTableRowStatus.Add(nPrevRow);
                                        break;
                                    }
                                    #endregion
                                    oTaxRow.TransDetailID = TransDetailID;
                                    //Added logic to handle the scenario where we have multiple ItemId-TaxId record in "POSTransactionDetailTax" table. In CalculateTax method it was computed wrong and we can't correct it over there. Hence updating it before saving the record
                                    //if (MultipleItemTaxPairInTaxDetailTable(oTDTaxData, oTaxRow.ItemID.Trim().ToUpper(), oTaxRow.TaxID))
                                    //{
                                    //    oTaxRow.TaxAmount = row.TaxAmount;
                                    //}
                                }
                            }
                            if (nPrevRow != 0) lstTaxTableRowStatus.Add(nPrevRow); //Sprint-21 18-Sep-2015 JY Added 
                        }
                        else
                        {
                            foreach (TransDetailTaxRow oTaxRow in oTDTaxData.TransDetailTax.Rows)
                            {
                                //if(oTaxRow.TaxAmount == 0 && oTaxRow.ItemRow == Row)  //Sprint-21 18-Sep-2015 JY Commented
                                if (oTaxRow.TaxAmount == 0 && oTaxRow.ItemID == row.ItemID && lstTaxTableRowStatus.Contains(oTaxRow.ItemRow) == false)    //Sprint-21 18-Sep-2015 JY Added to resolve tax issue
                                {
                                    oTaxRow.TransDetailID = TransDetailID;
                                    lstTaxTableRowStatus.Add(oTaxRow.ItemRow);  //Sprint-21 18-Sep-2015 JY Added to resolve tax issue
                                    break;  //Sprint-21 18-Sep-2015 JY Added to resolve tax issue
                                }
                            }
                        }
                        //END
                        /*if (row.ItemID.ToUpper()=="RX" && row.ItemDescription.IndexOf("-")>0 && Configuration.CPOSSet.UsePrimeRX==true)
                        {
                            PharmBL oPBL=new PharmBL();
                            oPBL.MarkDelivery(row.ItemDescription.Substring(0,row.ItemDescription.IndexOf("-")),DateTime.Now,"Y");
                        }*/

                        #region Sprint-23 - PRIMEPOS-2029 20-Apr-2016 JY Added to update transdetailId in ItemMonitorTransDetailData
                        if (oItemMonitorTransDetailData.Tables.Count > 0 && oItemMonitorTransDetailData.ItemMonitorTransDetail.Rows.Count > 0)
                        {
                            foreach (ItemMonitorTransDetailRow rowIMTD in oItemMonitorTransDetailData.ItemMonitorTransDetail.Rows)
                            {
                                if (rowIMTD.TransDetailID == row.TransDetailID)
                                {
                                    rowIMTD.TransDetailID = TransDetailID;
                                    //break not applied as there might be multiple monitoring categories present against one TransDetail record (one item)
                                }
                            }
                        }
                        #endregion

                        #region Sprint-23 - PRIMEPOS-2029 29-Apr-2016 JY Added to update transdetailId in oTransSignLogData
                        if (oTransSignLogData.Tables.Count > 0 && oTransSignLogData.POSTransSignLog.Rows.Count > 0)
                        {
                            foreach (POSTransSignLogRow rowTSL in oTransSignLogData.POSTransSignLog.Rows)
                            {
                                if (rowTSL.TransDetailID == row.TransDetailID)
                                {
                                    rowTSL.TransDetailID = TransDetailID;
                                    break;
                                }
                            }
                        }
                        #endregion

                        #region PRIMEPOS-2402 08-Jul-2021 JY Added
                        try
                        {
                            if (row.DiscountOverrideUser != "" && row.Discount != row.OldDiscountAmt)
                            {
                                InsertOverrideDetails(1, row.TransID, TransDetailID, row.OldDiscountAmt.ToString(), row.Discount.ToString(), row.DiscountOverrideUser, tx);
                            }
                        }
                        catch (Exception Exp)
                        {
                            logger.Fatal(Exp, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData) - Update discount override");
                        }

                        #region PRIMEPOS-3166N
                        try
                        {
                            if (!String.IsNullOrWhiteSpace(row.MonitorItemOverrideUser))
                            {
                                InsertOverrideDetails(17, row.TransID, TransDetailID, "", "Item has been overridden", row.MonitorItemOverrideUser, tx);
                            }
                            //POSTransaction.isOverrideMonitorItem = false; //PRIMEPOS-3230N
                        }
                        catch (Exception Exp)
                        {
                            logger.Fatal(Exp, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData) - Update Monitor Item Override");
                        }
                        #endregion

                        try
                        {
                            if (row.QuantityOverrideUser != "" && row.QTY > 1)
                            {
                                InsertOverrideDetails(7, row.TransID, TransDetailID, "1", row.QTY.ToString(), row.QuantityOverrideUser, tx);
                            }
                        }
                        catch (Exception Exp)
                        {
                            logger.Fatal(Exp, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData) - Update quantity override");
                        }

                        try
                        {
                            if (row.TaxOverrideUser != "")
                            {
                                bool bTaxOverride = false;
                                if (row.OldTaxCodesWithPercentage == "" && row.TaxAmount > 0)
                                    bTaxOverride = true;
                                else
                                {
                                    string[] arrTaxIdsWithAmt = row.OldTaxCodesWithPercentage.Split(',');
                                    ArrayList al = new ArrayList();
                                    for (int i = 0; i < arrTaxIdsWithAmt.Length; i++)
                                    {
                                        string[] arr = arrTaxIdsWithAmt[i].Split('~');
                                        al.Add(arr[0]);
                                    }

                                    foreach (TransDetailTaxRow tdTaxRow in oTDTaxData.TransDetailTax.Rows)
                                    {
                                        if (TransDetailID == tdTaxRow.TransDetailID)
                                        {
                                            if (!al.Contains(tdTaxRow.TaxID.ToString()))
                                            {
                                                bTaxOverride = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (bTaxOverride)
                                {
                                    if (row.OldTaxCodesWithPercentage != "")
                                    {
                                        string[] arrTaxIdsWithAmt = row.OldTaxCodesWithPercentage.Split(',');
                                        for (int i = 0; i < arrTaxIdsWithAmt.Length; i++)
                                        {
                                            string[] arr = arrTaxIdsWithAmt[i].Split('~');
                                            if (row.ItemID.Trim().ToUpper() == "RX") //Tax Override For Rx
                                                InsertOverrideDetails(11, row.TransID, TransDetailID, arr[0], arr[1], row.TaxOverrideUser, tx);
                                            else //Tax Override For OTC
                                                InsertOverrideDetails(10, row.TransID, TransDetailID, arr[0], arr[1], row.TaxOverrideUser, tx);
                                        }
                                    }
                                    else
                                    {
                                        if (row.ItemID.Trim().ToUpper() == "RX") //Tax Override For Rx
                                            InsertOverrideDetails(11, row.TransID, TransDetailID, row.OldTaxCodesWithPercentage, "", row.TaxOverrideUser, tx);
                                        else //Tax Override For OTC
                                            InsertOverrideDetails(10, row.TransID, TransDetailID, row.OldTaxCodesWithPercentage, "", row.TaxOverrideUser, tx);
                                    }
                                }
                            }
                        }
                        catch (Exception Exp)
                        {
                            logger.Fatal(Exp, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData) - Update Tax Override For OTC and Rx");
                        }

                        try
                        {
                            if (row.TaxOverrideAllOTCUser != "" || row.TaxOverrideAllRxUser != "")
                            {
                                if (row.OldTaxCodesWithPercentage != "")
                                {
                                    string[] arrTaxIdsWithAmt = row.OldTaxCodesWithPercentage.Split(',');
                                    for (int i = 0; i < arrTaxIdsWithAmt.Length; i++)
                                    {
                                        string[] arr = arrTaxIdsWithAmt[i].Split('~');
                                        if (row.ItemID.Trim().ToUpper() == "RX") //9	Tax Override All For Rx
                                            InsertOverrideDetails(9, row.TransID, TransDetailID, arr[0], arr[1], row.TaxOverrideAllRxUser, tx);
                                        else //8	Tax Override All For OTC
                                            InsertOverrideDetails(8, row.TransID, TransDetailID, arr[0], arr[1], row.TaxOverrideAllOTCUser, tx);
                                    }
                                }
                            }
                        }
                        catch (Exception Exp)
                        {
                            logger.Fatal(Exp, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData) - Update Tax Override All For OTC and Rx");
                        }

                        try
                        {
                            if (row.MaxDiscountLimitUser != "") //12	Max. Discount Limit
                            {
                                object MaxDiscountLimitForOverriddenUser = DataHelper.ExecuteScalar("SELECT MaxDiscountLimit FROM Users  WHERE UserID = '" + row.MaxDiscountLimitUser + "'");
                                InsertOverrideDetails(12, row.TransID, TransDetailID, Configuration.UserMaxDiscountLimit.ToString(), Configuration.convertNullToString(MaxDiscountLimitForOverriddenUser), row.MaxDiscountLimitUser, tx);
                            }
                        }
                        catch (Exception Exp)
                        {
                            logger.Fatal(Exp, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData) - Update discount override");
                        }
                        #endregion
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                #region Sprint-21 24-Sep-2015 JY Added  - if TransDetailId is not updated in tax table then set TaxAmount = 0
                foreach (TransDetailTaxRow oTaxRow in oTDTaxData.TransDetailTax.Rows)
                {
                    if (lstTaxTableRowStatus.Contains(oTaxRow.ItemRow) == false)
                    {
                        //oTaxRow.TaxAmount = 0;    //Sprint-23 - PRIMEPOS-N TaxType 27-Jun-2016 JY Commented
                        oTaxRow.TransDetailID = 0;  //Sprint-23 - PRIMEPOS-N TaxType 27-Jun-2016 JY Added 
                    }
                }
                #endregion

                #region Sprint-22 02-Nov-2015 JY Added to ignore invalid rx entries from POSTransactionRXDetail table
                try
                {
                    for (int i = dsRX.TransDetailRX.Rows.Count; i > 0; i--)
                    {
                        if (lstValidRx.Contains(Configuration.convertNullToInt(dsRX.TransDetailRX.Rows[i - 1][clsPOSDBConstants.TransDetailRX_Fld_RXDetailID])) == false)
                        {
                            //Logs.Logger("TransDetailSvr.cs", "Insert(...)", "Delete the invalid Rx record");
                            logger.Trace("Insert() - Delete the invalid Rx record");
                            dsRX.TransDetailRX.Rows.RemoveAt(i - 1);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    logger.Fatal(Ex, "Insert(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData, ItemMonitorTransDetailData oItemMonitorTransDetailData, POSTransSignLogData oTransSignLogData)");
                }
                #endregion

                addedTable.AcceptChanges();
            }
        }

        #region Sprint-21 24-Sep-2015 JY Added to resolve tax issue
        private bool MultipleItemTaxPairInTaxDetailTable(TransDetailTaxData oTDTaxData1, string ItemID, int TaxID)
        {
            try
            {
                int cnt = 0;
                foreach (TransDetailTaxRow oTaxRow1 in oTDTaxData1.TransDetailTax.Rows)
                {
                    if (ItemID == oTaxRow1.ItemID.Trim().ToUpper() && TaxID == oTaxRow1.TaxID)
                        cnt++;
                    if (cnt > 1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "MultipleItemTaxPairInTaxDetailTable(TransDetailTaxData oTDTaxData1, string ItemID, int TaxID))");
                return false;
            }
        }
        #endregion

        public void InsertOnHold(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData)
        {
            TransDetailTable addedTable = (TransDetailTable)ds.TransDetail;
            //.GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;
            List<int> lstTaxTableRowStatus = new List<int>();   //Sprint-22 - 05-Oct-2015 JY Added

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (TransDetailRow row in addedTable.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    try
                    {
                        row.TransID = TransID;
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.TransDetail_OnHold_tbl, insParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);

                        int TransDetailID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));

                        if (row.ItemID.ToUpper() == "RX")
                        {
                            foreach (TransDetailRXRow rxRow in dsRX.TransDetailRX.Rows)
                            {
                                if (rxRow.TransDetailID == row.TransDetailID)
                                {
                                    rxRow.TransDetailID = TransDetailID;
                                    break;
                                }
                            }
                        }

                        #region Sprint-22 - 05-Oct-2015 JY Added
                        if (row.TaxAmount != 0)
                        {
                            int nPrevRow = 0, cnt = 0;
                            foreach (TransDetailTaxRow oTaxRow in oTDTaxData.TransDetailTax.Rows)
                            {
                                if (oTaxRow.ItemID == row.ItemID && lstTaxTableRowStatus.Contains(oTaxRow.ItemRow) == false)
                                {
                                    if (cnt == 0)
                                    {
                                        nPrevRow = oTaxRow.ItemRow;
                                        cnt++;
                                    }
                                    if (nPrevRow != oTaxRow.ItemRow)
                                    {
                                        lstTaxTableRowStatus.Add(nPrevRow);
                                        break;
                                    }
                                    oTaxRow.TransDetailID = TransDetailID;
                                    //Added logic to handle the scenario where we have multiple ItemId-TaxId record in "POSTransactionDetailTax_OnHold" table. In CalculateTax method it was computed wrong and we can't correct it over there. Hence updating it before saving the record
                                    //if (MultipleItemTaxPairInTaxDetailTable(oTDTaxData, oTaxRow.ItemID.Trim().ToUpper(), oTaxRow.TaxID))
                                    //{
                                    //    oTaxRow.TaxAmount = row.TaxAmount;
                                    //}
                                }
                            }
                            if (nPrevRow != 0) lstTaxTableRowStatus.Add(nPrevRow);
                        }
                        else
                        {
                            foreach (TransDetailTaxRow oTaxRow in oTDTaxData.TransDetailTax.Rows)
                            {
                                if (oTaxRow.TaxAmount == 0 && oTaxRow.ItemID == row.ItemID && lstTaxTableRowStatus.Contains(oTaxRow.ItemRow) == false)
                                {
                                    oTaxRow.TransDetailID = TransDetailID;
                                    lstTaxTableRowStatus.Add(oTaxRow.ItemRow);
                                    break;
                                }
                            }
                        }
                        #endregion
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "InsertOnHold(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "InsertOnHold(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "InsertOnHold(TransDetailData ds, IDbTransaction tx, System.Int32 TransID, TransDetailRXData dsRX, TransDetailTaxData oTDTaxData)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                #region Sprint-21 24-Sep-2015 JY Added  - if TransDetailId is not updated in tax table then set TaxAmount = 0
                foreach (TransDetailTaxRow oTaxRow in oTDTaxData.TransDetailTax.Rows)
                {
                    if (lstTaxTableRowStatus.Contains(oTaxRow.ItemRow) == false)
                    {
                        oTaxRow.TransDetailID = 0;
                    }
                }
                #endregion
                addedTable.AcceptChanges();
            }
        }

        private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "INSERT INTO " + tableName + " ( ";
            // build where clause
            sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn;
            }
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }

        public void DeleteOnHold(Int32 TransID, IDbTransaction tx)
        {
            string sSQL;
            try
            {
                //delParam = PKParameters(row);

                sSQL = BuildDeleteOnHoldSQL(clsPOSDBConstants.TransDetail_OnHold_tbl, TransID);
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "DeleteOnHold(Int32 TransID, IDbTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "DeleteOnHold(Int32 TransID, IDbTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteOnHold(Int32 TransID, IDbTransaction tx)");
                ErrorHandler.throwException(ex, "", "");
            }
        }

        private string BuildDeleteSQL(string tableName, TransDetailRow row)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            sDeleteSQL += clsPOSDBConstants.TransDetail_Fld_TransDetailID + " = " + row.TransDetailID.ToString();
            return sDeleteSQL;
        }

        private string BuildDeleteOnHoldSQL(string tableName, Int32 TransID)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            sDeleteSQL += clsPOSDBConstants.TransDetail_Fld_TransID + " = " + TransID.ToString();
            return sDeleteSQL;
        }

        public void UpdateOnHold(TransDetailData ds, IDbTransaction tx, System.Int32 OrderID)
        {
            TransDetailTable modifiedTable = (TransDetailTable)ds.TransDetail.GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (TransDetailRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.TransDetail_OnHold_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "UpdateOnHold(TransDetailData ds, IDbTransaction tx, System.Int32 OrderID)");
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "UpdateOnHold(TransDetailData ds, IDbTransaction tx, System.Int32 OrderID)");
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "UpdateOnHold(TransDetailData ds, IDbTransaction tx, System.Int32 OrderID)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "update " + tableName + " set ";
            // build where clause
            sInsertSQL = sInsertSQL + delParam[0].SourceColumn + "=" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length - 1; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn + "=" + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " where " + delParam[delParam.Length - 1].SourceColumn + " = " + delParam[delParam.Length - 1].ParameterName;

            return sInsertSQL;
        }

        #region PRIMEPOS-2402 08-Jul-2021 JY Added
        public void InsertOverrideDetails(Int32 OverrideFieldId, Int32 TransID, Int32 TransDetailID, string OldValue, string NewValue, string OverrideBy, IDbTransaction tx)
        {
            try
            {
                string strSQL = "INSERT INTO OverrideDetails(OverrideFieldId,TransID,TransDetailID,OldValue,NewValue,OverrideBy) VALUES(" +
                    OverrideFieldId + "," + TransID + "," + TransDetailID + ",'" + OldValue + "','" + NewValue + "','" + OverrideBy + "')";
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL, null);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "InsertOverrideDetails(Int32 OverrideFieldId, Int32 TransID, Int32 TransDetailID, string OldValue, string NewValue, string OverrideBy, IDbTransaction tx)");
            }
        }
        #endregion

        #endregion Insert, Update, and Delete Methods

        #region IDBDataParameter Generator Methods

        private IDbDataParameter[] whereParameters(string swhere)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
            sqlParams[0] = DataFactory.CreateParameter();

            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Size = 2000;
            sqlParams[0].ParameterName = "@whereClause";

            sqlParams[0].Value = swhere;
            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(System.Int32 TransDetailID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@TransDetailID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = TransDetailID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(TransDetailRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@TransDetailID";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.TransDetailID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TransDetailID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(TransDetailRow row)
        {
            //IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(12);//Orignal commented by Krishna on 15 July 2011
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(24); //  replace 18 to 22 - Solutran - PRIMEPOS-2663 - NileshJ  //PRIMEPOS-2768 02-Jan-2020 JY changed from 22 to 23    //PRIMEPOS-2907 13-Oct-2020 JY 23 to 24

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TransID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_QTY, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_Price, System.Data.DbType.Currency);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_Discount, System.Data.DbType.Currency);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ItemID, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ItemDescription, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TaxID, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TaxAmount, System.Data.DbType.Currency);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice, System.Data.DbType.Currency);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ItemCost, System.Data.DbType.Currency);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_IsIIAS, System.Data.DbType.Boolean);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_IsEBT, System.Data.DbType.Boolean);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId, System.Data.DbType.Int32);//Added by Krishna on 15 July 2011
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_IsComboItem, System.Data.DbType.Boolean);
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_OrignalPrice, System.Data.DbType.Currency);
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints, System.Data.DbType.Int32);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_CouponID, System.Data.DbType.Int64);    //PRIMEPOS-2034 05-Mar-2018 JY Added
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_IsNonRefundable, System.Data.DbType.Boolean);   //PRIMEPOS-2592 02-Nov-2018 JY Added 

            #region Added for Solutran - PRIMEPOS-2663 - NileshJ
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_S3TransID, System.Data.DbType.String);
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_S3PurAmount, System.Data.DbType.String);
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount, System.Data.DbType.String);
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_S3TaxAmount, System.Data.DbType.String);
            #endregion

            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount, System.Data.DbType.Currency);  //PRIMEPOS-2768 02-Jan-2020 JY Added
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_IsOnSale, System.Data.DbType.Boolean);  //PRIMEPOS-2907 13-Oct-2020 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TransID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.TransDetail_Fld_QTY;
            sqlParams[2].SourceColumn = clsPOSDBConstants.TransDetail_Fld_Price;
            sqlParams[3].SourceColumn = clsPOSDBConstants.TransDetail_Fld_Discount;
            sqlParams[4].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ItemID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ItemDescription;
            sqlParams[6].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TaxID;
            sqlParams[7].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TaxAmount;
            sqlParams[8].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ExtendedPrice;
            sqlParams[9].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ItemCost;
            sqlParams[10].SourceColumn = clsPOSDBConstants.TransDetail_Fld_IsIIAS;
            sqlParams[11].SourceColumn = clsPOSDBConstants.TransDetail_Fld_IsEBT;
            sqlParams[12].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId;//Added by Krishna on 15 July 2011
            sqlParams[13].SourceColumn = clsPOSDBConstants.TransDetail_Fld_IsComboItem;
            sqlParams[14].SourceColumn = clsPOSDBConstants.TransDetail_Fld_OrignalPrice;
            sqlParams[15].SourceColumn = clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints;
            sqlParams[16].SourceColumn = clsPOSDBConstants.TransDetail_Fld_CouponID;    //PRIMEPOS-2034 05-Mar-2018 JY Added
            sqlParams[17].SourceColumn = clsPOSDBConstants.TransDetail_Fld_IsNonRefundable; //PRIMEPOS-2592 02-Nov-2018 JY Added 

            #region Added for Solutran - PRIMEPOS-2663 - NileshJ
            sqlParams[18].SourceColumn = clsPOSDBConstants.TransDetail_Fld_S3TransID;
            sqlParams[19].SourceColumn = clsPOSDBConstants.TransDetail_Fld_S3PurAmount;
            sqlParams[20].SourceColumn = clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount;
            sqlParams[21].SourceColumn = clsPOSDBConstants.TransDetail_Fld_S3TaxAmount;
            #endregion

            sqlParams[22].SourceColumn = clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount; //PRIMEPOS-2768 02-Jan-2020 JY Added 
            sqlParams[23].SourceColumn = clsPOSDBConstants.TransDetail_Fld_IsOnSale;    //PRIMEPOS-2907 13-Oct-2020 JY Added

            if (row.TransID.ToString() != System.String.Empty)
                sqlParams[0].Value = row.TransID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.QTY.ToString() != System.String.Empty)
                sqlParams[1].Value = row.QTY;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Price.ToString() != System.String.Empty)
                sqlParams[2].Value = Math.Round(Configuration.convertNullToDecimal(row.Price), 2, MidpointRounding.AwayFromZero); //Sprint-27 - PRIMEPOS-2413 06-Nov-2017 JY round the price value as in case of override sale item we used average price and to get more aqccurage we used the calculated price which is with multiple decimals
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.Discount.ToString() != System.String.Empty)
                sqlParams[3].Value = row.Discount;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ItemID.ToString() != System.String.Empty)
                sqlParams[4].Value = row.ItemID;
            else
                sqlParams[4].Value = DBNull.Value;
            //Made changes on 13-No-08 by SRT based on email communication by Naim
            if (row.ItemDescription != System.String.Empty)
            {
                if (row.ItemDescription.Length > 100)
                {
                    sqlParams[5].Value = row.ItemDescription.Substring(0, 100);
                }
                else
                {
                    sqlParams[5].Value = row.ItemDescription;
                }
            }
            else
            {
                sqlParams[5].Value = "";
            }
            //Till herer.

            if (row.ItemDescription != System.String.Empty)
            {
                if (row.ItemDescription.Length > 100)
                {
                    sqlParams[5].Value = row.ItemDescription.Substring(0, 100);
                }
                else
                {
                    sqlParams[5].Value = row.ItemDescription;
                }
            }
            else
            {
                sqlParams[5].Value = "";
            }

            if (row.TaxID != 0)
                sqlParams[6].Value = row.TaxID;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.TaxAmount.ToString() != System.String.Empty)
                sqlParams[7].Value = row.TaxAmount;
            else
                sqlParams[7].Value = DBNull.Value;

            if (row.ExtendedPrice.ToString() != System.String.Empty)
                sqlParams[8].Value = row.ExtendedPrice;
            else
                sqlParams[8].Value = DBNull.Value;

            if (row.ItemCost.ToString() != System.String.Empty)
                sqlParams[9].Value = row.ItemCost;
            else
                sqlParams[9].Value = DBNull.Value;

            if (row.IsIIAS.ToString() != System.String.Empty)
                sqlParams[10].Value = row.IsIIAS;
            else
                sqlParams[10].Value = false;

            sqlParams[11].Value = row.IsEBTItem;
            //Follwing Added by Krishna on 15 July 2011
            if (row.ReturnTransDetailId.ToString() != System.String.Empty)
                sqlParams[12].Value = row.ReturnTransDetailId;
            else
                sqlParams[12].Value = DBNull.Value;
            //Till here Added by Krishna on 15 July 2011
            sqlParams[13].Value = row.IsComboItem;
            sqlParams[14].Value = row.OrignalPrice;
            sqlParams[15].Value = row.LoyaltyPoints;
            sqlParams[16].Value = row.CouponID; //PRIMEPOS-2034 05-Mar-2018 JY Added
            sqlParams[17].Value = row.IsNonRefundable;  //PRIMEPOS-2592 02-Nov-2018 JY Added 

            #region Added for Solutran - PRIMEPOS-2663 - NileshJ
            sqlParams[18].Value = row.S3TransID;
            sqlParams[19].Value = row.S3PurAmount;
            sqlParams[20].Value = row.S3DiscountAmount;
            sqlParams[21].Value = row.S3TaxAmount;
            #endregion

            sqlParams[22].Value = row.InvoiceDiscount;  //PRIMEPOS-2768 02-Jan-2020 JY Added
            sqlParams[23].Value = row.IsOnSale; //PRIMEPOS-2907 13-Oct-2020 JY Added
            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(TransDetailRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(22); // Replace 16 to 20 add new parameter for Solutran - PRIMEPOS-2663 - NileshJ   //PRIMEPOS-2768 02-Jan-2020 JY changed from 22 to 23    //PRIMEPOS-2907 13-Oct-2020 JY Changed to 22

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TransID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_QTY, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_Price, System.Data.DbType.Currency);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_Discount, System.Data.DbType.Currency);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ItemID, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ItemDescription, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TaxID, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TaxAmount, System.Data.DbType.Currency);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice, System.Data.DbType.Currency);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_TransDetailID, System.Data.DbType.Int32);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_ItemCost, System.Data.DbType.Currency);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_IsIIAS, System.Data.DbType.Boolean);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_IsEBT, System.Data.DbType.Boolean);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints, System.Data.DbType.Int32);
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_CouponID, System.Data.DbType.Int64);    //PRIMEPOS-2034 05-Mar-2018 JY Added
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_IsNonRefundable, System.Data.DbType.Boolean);   //PRIMEPOS-2592 02-Nov-2018 JY Added 

            #region Added for Solutran - PRIMEPOS-2663 - NileshJ
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_S3TransID, System.Data.DbType.String);
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_S3PurAmount, System.Data.DbType.String);
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount, System.Data.DbType.String);
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_S3TaxAmount, System.Data.DbType.String);
            #endregion

            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount, System.Data.DbType.Currency);  //PRIMEPOS-2768 02-Jan-2020 JY Added
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransDetail_Fld_IsOnSale, System.Data.DbType.Boolean);  //PRIMEPOS-2907 13-Oct-2020 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TransID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.TransDetail_Fld_QTY;
            sqlParams[2].SourceColumn = clsPOSDBConstants.TransDetail_Fld_Price;
            sqlParams[3].SourceColumn = clsPOSDBConstants.TransDetail_Fld_Discount;
            sqlParams[4].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ItemID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ItemDescription;
            sqlParams[6].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TaxID;
            sqlParams[7].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TaxAmount;
            sqlParams[8].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ExtendedPrice;
            sqlParams[9].SourceColumn = clsPOSDBConstants.TransDetail_Fld_TransDetailID;
            sqlParams[10].SourceColumn = clsPOSDBConstants.TransDetail_Fld_ItemCost;
            sqlParams[11].SourceColumn = clsPOSDBConstants.TransDetail_Fld_IsIIAS;
            sqlParams[12].SourceColumn = clsPOSDBConstants.TransDetail_Fld_IsEBT;
            sqlParams[13].SourceColumn = clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints;
            sqlParams[14].SourceColumn = clsPOSDBConstants.TransDetail_Fld_CouponID;   //PRIMEPOS-2034 05-Mar-2018 JY Added
            sqlParams[15].SourceColumn = clsPOSDBConstants.TransDetail_Fld_IsNonRefundable; //PRIMEPOS-2592 02-Nov-2018 JY Added 

            #region Added for Solutran - PRIMEPOS-2663 - NileshJ
            sqlParams[16].SourceColumn = clsPOSDBConstants.TransDetail_Fld_S3TransID;
            sqlParams[17].SourceColumn = clsPOSDBConstants.TransDetail_Fld_S3PurAmount;
            sqlParams[18].SourceColumn = clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount;
            sqlParams[19].SourceColumn = clsPOSDBConstants.TransDetail_Fld_S3TaxAmount;
            #endregion

            sqlParams[20].SourceColumn = clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount; //PRIMEPOS-2768 02-Jan-2020 JY Added
            sqlParams[21].SourceColumn = clsPOSDBConstants.TransDetail_Fld_IsOnSale;    //PRIMEPOS-2907 13-Oct-2020 JY Added

            if (row.TransID.ToString() != System.String.Empty)
                sqlParams[0].Value = row.TransID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.QTY.ToString() != System.String.Empty)
                sqlParams[1].Value = row.QTY;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Price.ToString() != System.String.Empty)
                sqlParams[2].Value = row.Price;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.Discount.ToString() != System.String.Empty)
                sqlParams[3].Value = row.Discount;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ItemID.ToString() != System.String.Empty)
                sqlParams[4].Value = row.ItemID;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.ItemDescription != System.String.Empty)
                sqlParams[5].Value = row.ItemDescription;
            else
                sqlParams[5].Value = "";

            if (row.TaxID != 0)
                sqlParams[6].Value = row.TaxID;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.TaxAmount.ToString() != System.String.Empty)
                sqlParams[7].Value = row.TaxAmount;
            else
                sqlParams[7].Value = DBNull.Value;

            if (row.ExtendedPrice.ToString() != System.String.Empty)
                sqlParams[8].Value = row.ExtendedPrice;
            else
                sqlParams[8].Value = DBNull.Value;

            sqlParams[9].Value = row.TransDetailID;

            if (row.ItemCost.ToString() != System.String.Empty)
                sqlParams[10].Value = row.ItemCost;
            else
                sqlParams[10].Value = DBNull.Value;

            sqlParams[11].Value = row.IsIIAS;

            sqlParams[12].Value = row.IsEBTItem;
            sqlParams[13].Value = row.LoyaltyPoints;
            sqlParams[14].Value = row.CouponID; //PRIMEPOS-2034 05-Mar-2018 JY Added
            sqlParams[15].Value = row.IsNonRefundable;  //PRIMEPOS-2592 02-Nov-2018 JY Added 

            #region Added for Solutran - PRIMEPOS-2663 - NileshJ
            sqlParams[16].Value = row.S3TransID;
            sqlParams[17].Value = row.S3PurAmount;
            sqlParams[18].Value = row.S3DiscountAmount;
            sqlParams[19].Value = row.S3TaxAmount;
            #endregion

            sqlParams[20].Value = row.InvoiceDiscount;  //PRIMEPOS-2768 02-Jan-2020 JY Added
            sqlParams[21].Value = row.IsOnSale;  //PRIMEPOS-2768 02-Jan-2020 JY Added

            return (sqlParams);
        }
        #endregion IDBDataParameter Generator Methods

        #region Get Methods
        // Looks up a ItemVendor based on its primary-key:System.String VendorItemID
        public static bool isCallofRetTrans = false;//Added by Krishna for checking if the call is made from return trans
        public DataSet Populate(System.Int32 TransId, IDbConnection conn)
        {
            string sSQL = "";
            try
            {
                if (isCallofRetTrans == false)
                {
                    sSQL = "SELECT " 
                        + "td." + clsPOSDBConstants.TransDetail_Fld_ItemID
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_ItemDescription
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_QTY
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_Price
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                        + ",tx." + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_TaxAmount
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_Discount
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_TransID
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_TransDetailID
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_ItemCost
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_IsEBT
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_IsComboItem
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_OrignalPrice
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_CouponID  //PRIMEPOS-2034 05-Mar-2018 JY Added
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_IsNonRefundable   //PRIMEPOS-2592 02-Nov-2018 JY Added 
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_S3TransID //PRIMEPOS-2663 - Solutran - NileshJ Added
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_S3PurAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_S3TaxAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount    //PRIMEPOS-2768 24-Dec-2019 JY Added
                        + ",td." + clsPOSDBConstants.TransDetail_Fld_IsOnSale //PRIMEPOS-2907 13-Oct-2020 JY Added
                        + " FROM POSTransactionDetail td LEFT JOIN TaxCodes tx ON tx.TaxID = td.TaxID"
                        + " WHERE td.TransID = " + TransId;
                }
                else   //Following Query Added by Krishna on 17 May 2011
                {
                    # region OLD QUERY
                    //sSQL = "select"
                    //    + " td." + clsPOSDBConstants.TransDetail_Fld_ItemID
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_ItemDescription
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_QTY
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_Price
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                    //    + " , tx." + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_TaxAmount
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_Discount
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_TransID
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_TransDetailID
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_ItemCost
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                    //    + " , td." + clsPOSDBConstants.TransDetail_Fld_IsEBT
                    //    + " FROM POSTransactionDetail td left join TaxCodes tx on ( tx.TaxID=td.TaxID) where ItemId " +
                    //       " NOT IN ( select pod.Itemid from POSTransactionDetail as pod where transid in  (Select trans.transid " +
                    //       " FROM POSTransaction as Trans WHERE  trans.returntransid='" + TransId + "')  ) and transid='" + TransId + "' AND td.QTY>0";

                    # endregion

                    //sSQL = "SELECT * FROM (SELECT ROW_NUMBER() OVER(PARTITION BY c.RxNo, c.NRefill, c.PartialFillNo ORDER BY c.RxDetailID DESC) as rNum, T.TransType,"
                    //    + "td." + clsPOSDBConstants.TransDetail_Fld_ItemID
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_ItemDescription
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_QTY
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_Price
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                    //    + ",tx." + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_TaxAmount
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_Discount
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_TransID
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_TransDetailID
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_ItemCost
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_IsEBT
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_IsComboItem
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_OrignalPrice
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_CouponID  //PRIMEPOS-2034 05-Mar-2018 JY Added
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_IsNonRefundable //PRIMEPOS-2592 02-Nov-2018 JY Added 
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_S3TransID //PRIMEPOS-2663 - Solutran - NileshJ Added
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_S3PurAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_S3TaxAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount    //PRIMEPOS-2768 24-Dec-2019 JY Added
                    //    + ",td." + clsPOSDBConstants.TransDetail_Fld_IsOnSale //PRIMEPOS-2907 13-Oct-2020 JY Added
                    //    + " FROM POSTransactionDetail TD LEFT JOIN TaxCodes tx ON tx.TaxID = td.TaxID INNER JOIN POSTransaction T ON TD.TransId = T.TransId"
                    //    + " INNER JOIN POSTransactionRXDetail c on TD.TransDetailID = c.TransDetailID"
                    //    + " INNER JOIN(SELECT bb.RXNo, bb.NRefill, bb.PartialFillNo FROM POSTransactionDetail aa INNER JOIN POSTransactionRXDetail bb ON aa.TransDetailID = bb.TransDetailID WHERE aa.TransID = " + TransId + ") d ON c.RXNo = d.RXNo and c.NRefill = d.NRefill and c.PartialFillNo=d.PartialFillNo "
                    //    + ") X WHERE rNum = 1 AND TransType = 1";

                    sSQL = "Select "
                        + " td." + clsPOSDBConstants.TransDetail_Fld_ItemID
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_ItemDescription
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_QTY
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_Price
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                        + " , tx." + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_TaxAmount
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_Discount
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_TransID
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_TransDetailID
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_ItemCost
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_IsEBT
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_IsComboItem
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_OrignalPrice
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_CouponID  //PRIMEPOS-2034 05-Mar-2018 JY Added
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_IsNonRefundable //PRIMEPOS-2592 02-Nov-2018 JY Added 
                         + " , td." + clsPOSDBConstants.TransDetail_Fld_S3TransID //PRIMEPOS-2663 - Solutran - NileshJ Added
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_S3PurAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_S3TaxAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_InvoiceDiscount    //PRIMEPOS-2768 24-Dec-2019 JY Added
                        + " , td." + clsPOSDBConstants.TransDetail_Fld_IsOnSale //PRIMEPOS-2907 13-Oct-2020 JY Added
                        + " FROM POSTransactionDetail TD left join TaxCodes tx on ( tx.TaxID=td.TaxID) INNER JOIN POSTransaction T ON (TD.TransId =T.TransId) AND "
                        + " TD.TransDetailID NOT IN (Select TD.ReturnTransDetailId from POSTransactionDetail TD where ReturnTransDetailID "
                        + " in (Select TransDetailId from POSTRansactionDetail WHERE TransId = '" + TransId + "')) AND T.TransId='" + TransId + "' AND td.QTY>0";

                    isCallofRetTrans = false;
                }
                //till here added by Krishna

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(TransId));
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet PopulateTransDetailId(System.Int32 TransDetailId, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + " td." + clsPOSDBConstants.TransDetail_Fld_ItemID
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_ItemDescription
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_QTY
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_Price
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                    + " , tx." + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_TaxAmount
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_Discount
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_TransID
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_TransDetailID
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_ItemCost
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_IsEBT
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_IsComboItem
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_OrignalPrice
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_ReturnTransDetailId
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_CouponID //PRIMEPOS-2034 05-Mar-2018 JY Added
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_IsNonRefundable  //PRIMEPOS-2592 02-Nov-2018 JY Added 
                      + " , td." + clsPOSDBConstants.TransDetail_Fld_S3TransID //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_S3PurAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_S3TaxAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , td." + clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " FROM "
                    + clsPOSDBConstants.TransDetail_tbl + " td left join "
                    + clsPOSDBConstants.TaxCodes_tbl + " tx"
                    + " on ( "
                    + "tx." + clsPOSDBConstants.TaxCodes_Fld_TaxID + "=td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                    + ") where " + clsPOSDBConstants.TransDetail_Fld_TransDetailID + " = " + TransDetailId;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(TransDetailId));
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateTransDetailId(System.Int32 TransDetailId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }



        public DataSet Populate(System.Int32 TransId, bool isCallofRetTrans)
        {
            TransDetailSvr.isCallofRetTrans = isCallofRetTrans;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate(TransId, conn));
        }

        public DataSet Populate(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (Populate(TransId, conn));
        }

        public DataSet PopulateTransDetailId(System.Int32 TransDetailId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (PopulateTransDetailId(TransDetailId, conn));
        }

        public TransDetailData PopulateData(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = Populate(TransId, conn);

            TransDetailData oTD = new TransDetailData();
            oTD.TransDetail.MergeTable(ds.Tables[0]);
            return oTD;
        }

        public TransDetailRow GetRowById(System.Int32 transDetailId)
        {
            string sSQL = "Select td.*, tx." + clsPOSDBConstants.TaxCodes_Fld_TaxCode
            + " FROM "
            + clsPOSDBConstants.TransDetail_tbl + " td left join "
            + clsPOSDBConstants.TaxCodes_tbl + " tx"
            + " on ( "
            + "tx." + clsPOSDBConstants.TaxCodes_Fld_TaxID + "=td." + clsPOSDBConstants.TransDetail_Fld_TaxID
            + ") where " + clsPOSDBConstants.TransDetail_Fld_TransDetailID + " = " + transDetailId;

            TransDetailData otd = new TransDetailData();
            otd.TransDetail.MergeTable(DataHelper.ExecuteDataset(sSQL).Tables[0]);
            if (otd.TransDetail.Rows.Count > 0)
                return otd.TransDetail[0];
            else
                return null;
        }

        public DataSet GetReturnSaleDetail(List<int> TransDetailID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            string where = string.Empty;
            int count = 1;
            foreach (var Id in TransDetailID)
            {
                if (TransDetailID.Count > count)
                    where += " ReturnTransDetailID = " + Id + " AND ";
                else
                    where += " ReturnTransDetailID = " + Id;
                count++;
            }
            string sSQL = "Select TransDetailID, TransID, ItemID, ItemDescription, Qty, Price, ReturnTransDetailID from POSTRANSACTIONDETAIL Where " + where;

            DataSet ds = new DataSet();
            ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
            ds.AcceptChanges();
            return ds;
        }

        public DataSet GetOnHoldTransDetail(System.Int32 TransId, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + " td.*"
                    + " , tx." + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                    + " FROM "
                    + clsPOSDBConstants.TransDetail_OnHold_tbl + " td left join "
                    + clsPOSDBConstants.TaxCodes_tbl + " tx"
                    + " on ( "
                    + "tx." + clsPOSDBConstants.TaxCodes_Fld_TaxID + "=td." + clsPOSDBConstants.TransDetail_Fld_TaxID
                    + ") where " + clsPOSDBConstants.TransDetail_Fld_TransID + " = " + TransId;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                ds.AcceptChanges();
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetOnHoldTransDetail(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetOnHoldTransDetail(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetOnHoldTransDetail(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public TransDetailData GetOnHoldTransDetail(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = GetOnHoldTransDetail(TransId, conn);

            TransDetailData oTD = new TransDetailData();
            oTD.TransDetail.MergeTable(ds.Tables[0]);
            return oTD;
        }

        // Fills a ItemVendorData with all ItemVendor

        //Added by Krishna on 2 June 2011
        public DataSet PopulatePOSTrans(string sWhereClause)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            return (PopulatePOSTrans(sWhereClause, conn));
        }

        public DataSet PopulatePOSTrans(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL1 = "Select Count(pdt.ItemId) as ItemCount,pt.TransId,pt.UserID,pt.StationID,pt.TransactionStartDate" +
                                ",pt.TransDate as TransactionEndDate,DATEDIFF(S,pt.TransactionStartDate,pt.TransDate) as TotalTime " +
                                ",p.PayTypeDesc,pt.TotalPaid" +
                                " from POSTransactionDetail pdt,POSTransaction pt,POSTransPayment ppay,PayType p";
                sWhereClause = sWhereClause.Replace("WHERE", "AND");
                string sSQL = String.Concat(sSQL1, sWhereClause);
                sSQL += " AND pt.Transid=ppay.TransId AND ppay.TransTypeCode=p.PaytypeId Group  by pt.TransId,pt.UserID,pt.TransactionStartDate,pt.TransDate,p.PayTypeDesc,pt.TotalPaid,pt.StationID";
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause));
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulatePOSTrans(string sWhereClause, IDbConnection conn)");
                throw (ex);
            }
        }

        //Till here Added by Krishna on 2 June 2011

        public DataSet PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL1 = "Select "
                    + clsPOSDBConstants.TransDetail_Fld_ItemID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_ItemDescription
                    + " , " + clsPOSDBConstants.TransDetail_Fld_QTY
                    + " , " + clsPOSDBConstants.TransDetail_Fld_Price
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TaxID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TaxAmount
                    + " , " + clsPOSDBConstants.TransDetail_Fld_Discount
                    + " , " + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TransID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TransDetailID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_ItemCost
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsEBT
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsComboItem
                    + " , " + clsPOSDBConstants.TransDetail_Fld_OrignalPrice
                    + " , " + clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints
                    + " , " + clsPOSDBConstants.TransDetail_Fld_CouponID    //PRIMEPOS-2034 05-Mar-2018 JY Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsNonRefundable //PRIMEPOS-2592 02-Nov-2018 JY Added 
                     + " , " + clsPOSDBConstants.TransDetail_Fld_S3TransID //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_S3PurAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_S3TaxAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " FROM "
                    + clsPOSDBConstants.TransDetail_tbl;

                sWhereClause = sWhereClause.Replace("WHERE", "AND");
                string sSQL = String.Concat(sSQL1, sWhereClause);
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause));
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        //Added By SRT(Gaurav) Date : 17-Jul-2009
        public DataSet PopulateList(string sWhereClause, IDbConnection conn, out bool queryResult)
        {
            queryResult = true;
            try
            {
                string sSQL1 = "Select "
                    + clsPOSDBConstants.TransDetail_Fld_ItemID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_ItemDescription
                    + " , " + clsPOSDBConstants.TransDetail_Fld_QTY
                    + " , " + clsPOSDBConstants.TransDetail_Fld_Price
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TaxID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TaxAmount
                    + " , " + clsPOSDBConstants.TransDetail_Fld_Discount
                    + " , " + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TransID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TransDetailID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_ItemCost
                    //Modified by Amit Date 27 june 2011
                    //+ " , td." + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                    //+ " , td." + clsPOSDBConstants.TransDetail_Fld_IsEBT
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsEBT
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsComboItem
                    + " , " + clsPOSDBConstants.TransDetail_Fld_OrignalPrice
                    + " , " + clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints
                    + " , " + clsPOSDBConstants.TransDetail_Fld_CouponID    //PRIMEPOS-2034 05-Mar-2018 JY Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsNonRefundable //PRIMEPOS-2592 02-Nov-2018 JY Added 
                     + " , " + clsPOSDBConstants.TransDetail_Fld_S3TransID //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_S3PurAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_S3TaxAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " FROM "
                    + clsPOSDBConstants.TransDetail_tbl;

                sWhereClause = sWhereClause.Replace("WHERE", "AND");
                string sSQL = String.Concat(sSQL1, sWhereClause);
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause));
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, out bool queryResult)");
                queryResult = false;
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, out bool queryResult)");
                queryResult = false;
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, out bool queryResult)");
                queryResult = false;
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet PopulateList(string whereClause)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            return (PopulateList(whereClause, conn));
        }

        //Added By SRT(Gaurav) Date : 17-Jul-2009
        public DataSet PopulateList(string whereClause, out bool queryResult)
        {
            queryResult = true;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            return (PopulateList(whereClause, conn, out queryResult));
        }

        public DataSet PopulateList(string whereClause, out bool queryResult, out string strDistItems)
        {
            queryResult = true;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            return (PopulateList(whereClause, conn, out queryResult, out strDistItems));
        }

        public DataSet PopulateList(string sWhereClause, IDbConnection conn, out bool queryResult, out string strDistItems)
        {
            queryResult = true;
            strDistItems = "";
            try
            {
                strDistItems = "Select DISTINCT " + clsPOSDBConstants.TransDetail_Fld_ItemID + " FROM " + clsPOSDBConstants.TransDetail_tbl;

                string sSQL1 = "Select "
                    + clsPOSDBConstants.TransDetail_Fld_ItemID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_ItemDescription
                    + " , " + clsPOSDBConstants.TransDetail_Fld_QTY
                    + " , " + clsPOSDBConstants.TransDetail_Fld_Price
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TaxID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TaxAmount
                    + " , " + clsPOSDBConstants.TransDetail_Fld_Discount
                    + " , " + clsPOSDBConstants.TransDetail_Fld_ExtendedPrice
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TransID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_TransDetailID
                    + " , " + clsPOSDBConstants.TransDetail_Fld_ItemCost
                    //Modified by Amit Date 27 june 2011
                    //+ " , td." + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                    //+ " , td." + clsPOSDBConstants.TransDetail_Fld_IsEBT
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsIIAS
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsEBT
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsComboItem
                    + " , " + clsPOSDBConstants.TransDetail_Fld_OrignalPrice
                    + " , " + clsPOSDBConstants.TransDetail_Fld_LoyaltyPoints
                    + " , " + clsPOSDBConstants.TransDetail_Fld_CouponID    //PRIMEPOS-2034 05-Mar-2018 JY Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_IsNonRefundable //PRIMEPOS-2592 02-Nov-2018 JY Added 
                     + " , " + clsPOSDBConstants.TransDetail_Fld_S3TransID //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_S3PurAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_S3TaxAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " , " + clsPOSDBConstants.TransDetail_Fld_S3DiscountAmount //PRIMEPOS-2663 - Solutran - NileshJ Added
                    + " FROM "
                    + clsPOSDBConstants.TransDetail_tbl;

                sWhereClause = sWhereClause.Replace("WHERE", "AND");
                strDistItems = String.Concat(strDistItems, sWhereClause);
                string sSQL = String.Concat(sSQL1, sWhereClause);
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause));
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, out bool queryResult, out string strDistItems)");
                queryResult = false;
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, out bool queryResult, out string strDistItems)");
                queryResult = false;
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, out bool queryResult, out string strDistItems)");
                queryResult = false;
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        public int GetTotalQtySold(string sItemCode, int customerID, DateTime fromDate, DateTime toDate)
        {
            int retVal = 0;
            try
            {
                string SaleTransSQL = " Select IsNull(sum(qty),0) from POSTransactionDetail ptd, POSTransaction pt "
                + " Where pt.TransID=ptd.TransID "
                + " And ItemID='" + sItemCode.Replace("'", "''") + "'"
                + " And pt.CustomerID=" + customerID.ToString()
                + " And pt.TransType=" + 1
                + " And pt.TransDate between Cast('" + fromDate.ToShortDateString() + " 00:00' as datetime) and Cast('" + toDate.ToShortDateString() + " 23:59' as datetime) ";

                string RetTrnasSQL = " Select IsNull(sum(qty),0) from POSTransactionDetail ptd, POSTransaction pt "
                + " Where pt.TransID=ptd.TransID "
                + " And ItemID='" + sItemCode.Replace("'", "''") + "'"
                + " And pt.CustomerID=" + customerID.ToString()
                + " And pt.TransType=" + 2
                + " And pt.TransDate between Cast('" + fromDate.ToShortDateString() + " 00:00' as datetime) and Cast('" + toDate.ToShortDateString() + " 23:59' as datetime) ";

                object SaleTransValue = DataHelper.ExecuteScalar(SaleTransSQL);
                object RetTransValue = DataHelper.ExecuteScalar(RetTrnasSQL);

                if (SaleTransValue != null)
                {
                    retVal = Configuration.convertNullToInt(SaleTransValue);
                }
                if (RetTransValue != null)
                {
                    retVal = retVal - Math.Abs(Configuration.convertNullToInt(RetTransValue));
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetTotalQtySold(string sItemCode, int customerID, DateTime fromDate, DateTime toDate)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetTotalQtySold(string sItemCode, int customerID, DateTime fromDate, DateTime toDate)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTotalQtySold(string sItemCode, int customerID, DateTime fromDate, DateTime toDate)");
                ErrorHandler.throwException(ex, "", "");
            }
            return retVal;
        }

        #region Sprint-18 - 2133 13-Nov-2014 JY Added to check sudafed item present in the list
        public Boolean? IsSudaFedItem(string sItemCode)
        {
            Boolean? retVal = null;
            try
            {
                string strSQL = "SELECT ISNULL(b.ePSE,0) AS ePSE FROM ItemMonitorCategoryDetail a INNER JOIN ItemMonitorCategory b on a.ItemMonCatID = b.ID " +
                                " WHERE b.LimitPeriodDays > 0 AND b.LimitPeriodQty > 0 AND a.ItemId = '" + sItemCode + "'";
                object oReturn = DataHelper.ExecuteScalar(strSQL);
                if (oReturn != null)
                    retVal = (Boolean)oReturn;
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "IsSudaFedItem(string sItemCode)");
            }
            return retVal;
        }
        #endregion Sprint-18 - 2133 13-Nov-2014 JY Added to check sudafed item present in the list

        #region Sprint-18 - 2133 11-Nov-2014 JY Added

        public decimal GetTotalQtySold(int ItemMonitorCategoryId, int customerID, DateTime fromDate, DateTime toDate)
        {
            decimal retVal = 0;
            try
            {
                string SaleTransSQL = " SELECT ISNULL(SUM(a.Qty * (case WHEN c.UnitsPerPackage IS NULL OR c.UnitsPerPackage = 0 then  1 when c.UnitsPerPackage < 0 THEN 1 ELSE c.UnitsPerPackage END)),0) FROM POSTransactionDetail a " +
                    " inner join POSTransaction b on a.TransID = b.TransID " +
                    " inner join ItemMonitorCategoryDetail  c on a.ItemID = c.ItemID " +
                    " INNER JOIN ITEM d on c.ItemID = d.ItemID " +
                    " Where d.ISOTCITEM = 1 AND c.ItemMonCatID = " + ItemMonitorCategoryId + " and b.CustomerID= " + customerID.ToString() + " AND  b.TransType=1 " +
                    " And b.TransDate between Cast('" + fromDate.ToShortDateString() + " 00:00' as datetime) and Cast('" + toDate.ToShortDateString() + " 23:59' as datetime) " +
                    " and  c.ItemMonCatID = " + ItemMonitorCategoryId;

                string RetTrnasSQL = " SELECT ISNULL(SUM(a.Qty * (case WHEN c.UnitsPerPackage IS NULL OR c.UnitsPerPackage = 0 then  1 when c.UnitsPerPackage < 0 THEN 1 ELSE c.UnitsPerPackage END)),0) FROM POSTransactionDetail a " +
                    " inner join POSTransaction b on a.TransID = b.TransID " +
                    " inner join ItemMonitorCategoryDetail  c on a.ItemID = c.ItemID " +
                    " INNER JOIN ITEM d on c.ItemID = d.ItemID " +
                    " Where d.ISOTCITEM = 1 AND c.ItemMonCatID = " + ItemMonitorCategoryId + " and b.CustomerID= " + customerID.ToString() + " AND  b.TransType=2 " +
                    " And b.TransDate between Cast('" + fromDate.ToShortDateString() + " 00:00' as datetime) and Cast('" + toDate.ToShortDateString() + " 23:59' as datetime) " +
                    " and  c.ItemMonCatID = " + ItemMonitorCategoryId;

                object SaleTransValue = DataHelper.ExecuteScalar(SaleTransSQL);
                object RetTransValue = DataHelper.ExecuteScalar(RetTrnasSQL);

                if (SaleTransValue != null)
                {
                    retVal = Configuration.convertNullToDecimal(SaleTransValue);
                }
                if (RetTransValue != null)
                {
                    retVal = retVal - Math.Abs(Configuration.convertNullToDecimal(RetTransValue));
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetTotalQtySold(int ItemMonitorCategoryId, int customerID, DateTime fromDate, DateTime toDate)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetTotalQtySold(int ItemMonitorCategoryId, int customerID, DateTime fromDate, DateTime toDate)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTotalQtySold(int ItemMonitorCategoryId, int customerID, DateTime fromDate, DateTime toDate)");
                ErrorHandler.throwException(ex, "", "");
            }
            return retVal;
        }
        #endregion Sprint-18 - 2133 11-Nov-2014 JY Added

        #region Sprint-18 - 2133 13-Nov-2014 JY Added to get individual item quantity
        public decimal GetCurrentItemQty(string sItemCode, int Qty, int ItemMonitorCategoryId)
        {
            decimal retVal = 0;
            try
            {
                string strSQL = " SELECT ISNULL(" + Qty + " * (case WHEN a.UnitsPerPackage IS NULL OR a.UnitsPerPackage = 0 then  1 when a.UnitsPerPackage < 0 THEN 1 ELSE a.UnitsPerPackage END),0) " +
                                "  FROM ItemMonitorCategoryDetail a INNER JOIN ItemMonitorCategory b on a.ItemMonCatID = b.ID " +
                                " WHERE b.LimitPeriodDays > 0 AND b.LimitPeriodQty > 0 AND a.ItemId = '" + sItemCode + "' AND a.ItemMonCatID = " + ItemMonitorCategoryId;

                object ItemQty = DataHelper.ExecuteScalar(strSQL);

                if (ItemQty != null)
                {
                    retVal = Configuration.convertNullToDecimal(ItemQty);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetCurrentItemQty(string sItemCode, int Qty, int ItemMonitorCategoryId)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetCurrentItemQty(string sItemCode, int Qty, int ItemMonitorCategoryId)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetCurrentItemQty(string sItemCode, int Qty, int ItemMonitorCategoryId)");
                ErrorHandler.throwException(ex, "", "");
            }
            return retVal;
        }
        #endregion Sprint-18 - 2133 13-Nov-2014 JY Added to get individual item quantity

        public void CalculateLoyaltyPointsForReturnItem(TransDetailRow oRow, int customerID)
        {
            try
            {
                string SQL = "Select ptd.TransDetailId,pt.TransDate "
                + " from POSTransactionDetail ptd, POSTransaction pt "
                + " Where pt.TransID=ptd.TransID "
                + " And TransDetailID = "
                + " (Select Max(TransDetailID) from POSTransactionDetail ptd, POSTransaction pt "
                + " Where pt.TransID=ptd.TransID "
                + " And ItemID='" + oRow.ItemID.Replace("'", "''") + "'"
                + " And pt.CustomerID=" + customerID.ToString()
                + " And pt.TransType=1 )";

                int ptdID = 0;
                DateTime transDate = DateTime.MinValue;
                using (IDataReader reader = DataHelper.ExecuteReader(SQL))
                {
                    if (reader.Read())
                    {
                        ptdID = reader.GetInt32(reader.GetOrdinal("TransDetailId"));
                        transDate = reader.GetDateTime(reader.GetOrdinal("transdate"));
                    }
                }

                if (ptdID > 0)
                {
                    TransDetailSvr tdSvr = new TransDetailSvr();
                    TransDetailRow orignalRow = tdSvr.GetRowById(ptdID);
                    if (orignalRow == null)
                        return;

                    SQL = " select * from cl_setup where id=(Select max(id) from cl_setup where createdon<@createdOn)";
                    IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
                    sqlParams[0] = DataFactory.CreateParameter();

                    sqlParams[0].DbType = System.Data.DbType.DateTime;
                    sqlParams[0].ParameterName = "@createdOn";
                    sqlParams[0].Value = transDate;

                    String CLPointsCalcMethod = Configuration.CLoyaltyInfo.PointsCalcMethod;
                    decimal CLRedeemValue = Configuration.CLoyaltyInfo.RedeemValue;

                    using (IDataReader reader = DataHelper.ExecuteReader(SQL, sqlParams))
                    {
                        if (reader.Read())
                        {
                            CLPointsCalcMethod = Configuration.convertNullToString(reader.GetValue(reader.GetOrdinal("PointsCalcMethod")));
                            CLRedeemValue = Configuration.convertNullToDecimal(reader.GetValue(reader.GetOrdinal("RedeemValue")));
                        }
                    }

                    if (CLPointsCalcMethod == "A")
                    {
                        decimal lineNetTotal = (oRow.ExtendedPrice - oRow.Discount);
                        //Fix by Manoj: Was rounding the CL points.  Vivek and Fahad reported  9/22/2014
                        oRow.LoyaltyPoints = Math.Round(lineNetTotal * CLRedeemValue, 2); //Keep the points as a decimal
                    }
                    else if (CLPointsCalcMethod == "L")
                    {
                        //Fix by Manoj: Was rounding the CL points.  Vivek and Fahad reported  9/22/2014
                        oRow.LoyaltyPoints = Math.Round(orignalRow.LoyaltyPoints, 2);
                    }
                    else if (CLPointsCalcMethod == "Q")
                    {
                        // oRow.LoyaltyPoints = (int)(((decimal)orignalRow.LoyaltyPoints / (decimal)orignalRow.QTY) * oRow.QTY);
                        //Fix by Manoj: Was rounding the CL points.  Vivek and Fahad reported  9/22/2014
                        oRow.LoyaltyPoints = Math.Round(((orignalRow.LoyaltyPoints / orignalRow.QTY) * oRow.QTY), 2);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Fatal(e, "CalculateLoyaltyPointsForReturnItem(TransDetailRow oRow, int customerID)");
            }
            finally { }
            if (oRow.LoyaltyPoints > 0)
            {
                oRow.LoyaltyPoints = oRow.LoyaltyPoints * -1;
            }
        }

        //Sprint-23 - PRIMEPOS-2029 25-Mar-2016 JY Added to get unis of sudafed in grams
        public decimal GetSudafedUnits(string sItemCode)
        {
            decimal UnitsInGrams = 0;
            try
            {
                string strSQL = "SELECT CASE WHEN b.UOM = 1 THEN a.UnitsPerPackage/1000 ELSE a.UnitsPerPackage END AS UnitsInGrams FROM ItemMonitorCategoryDetail a INNER JOIN ItemMonitorCategory b on a.ItemMonCatID = b.ID " +
                                " WHERE b.LimitPeriodDays > 0 AND b.LimitPeriodQty > 0 AND a.ItemId = '" + sItemCode + "'";
                object obj = DataHelper.ExecuteScalar(strSQL);
                if (obj != null)
                    UnitsInGrams = Configuration.convertNullToDecimal(obj);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetSudafedUnits(string sItemCode)");
            }
            return UnitsInGrams;
        }

        //Sprint-23 - PRIMEPOS-2029 20-Apr-2016 JY Added to get item monitoring details
        public DataSet GetItemMonitoringdetails(System.String ItemId)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "SELECT b.ItemMonCatID, a.UOM, b.UnitsPerPackage, a.ePSE FROM ItemMonitorCategory a " +
                                " INNER JOIN ItemMonitorCategoryDetail b ON a.ID = b.ItemMonCatID WHERE b.ItemID = '" + ItemId + "'";

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetItemMonitoringdetails(System.String ItemId)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        #region Sprint-26 - PRIMEPOS-2418 01-Aug-2017 JY Added
        public DataSet GetTransDetail(System.Int32 TransDetailID)
        {
            string sSQL = string.Empty;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection();
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                sSQL = "SELECT a.TransID, a.TransDate, a.UserID, a.StationID, b.Price FROM POSTransaction a" +
                        " INNER JOIN POSTransactionDetail b ON a.TransID = b.TransID " +
                        " WHERE b.TransDetailID = " + TransDetailID;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(TransDetailID));
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTransDetail(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #endregion Get Methods

        #region PRIMEPOS-2761
        public DataSet GetCcTransmissionLog(DateTime dtFrom, DateTime dtTo, string status = "", string stationID = "", string user = "")
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "";
                    sSQL = "select TransNo,TransDateTime,TransAmount,TransDataStr,RecDataStr,PaymentProcessor, UserID ,  StationID ,TicketNo , TransmissionStatus, HostTransID ,POSTransID,POSPaymentID ,TransType,IsReversed, AmtApproved , TerminalRefNumber , ResponseMessage , last4  from CCTransmission_Log "
                    //+ " where  (TransType like '%Sale%' or TransType like '%Return%' or TransType like '%Purchase%') "; //IsReversed = 0  and  POSTransID is null and //PRIMEPOS-3182
                    + " where  TransNo is not null  "; //IsReversed = 0  and  POSTransID is null and
                    if (dtFrom != null && dtTo != null)
                    {
                        sSQL = sSQL + " and Convert(datetime, TransDateTime, 109) between convert(datetime, cast('" + dtFrom + "' as datetime) ,113) and convert(datetime, cast('" + dtTo + "' as datetime) ,113)";
                    }
                    if (status != "All")
                    {
                        sSQL += " and ResponseMessage = '" + status + "'";
                    }
                    if (stationID != "All")
                    {
                        sSQL += " and StationID = " + stationID;
                    }
                    if (user != "All")
                    {
                        sSQL += " and UserID = '" + user + "'";
                    }
                    sSQL += " order by TransNo desc";

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetCcTransmissionLog(DateTime dtFrom , DateTime dtTo , string status  , string stationID , string user)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetStatus()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "";
                    sSQL = " select ' All' as ResponseMessage union select distinct ResponseMessage   from CCTransmission_Log " +
                    " where  (TransType like '%Sale%' or TransType like '%Return%' or TransType like '%Purchase%') and ISNULL(ResponseMessage,'') <>''"; //  IsReversed = 0  and POSTransID is null and

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetStatus()");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetUser()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "";
                    sSQL = " select ' All' as UserID union select distinct UserID  from CCTransmission_Log " +
                    " where (TransType like '%Sale%' or TransType like '%Return%' or TransType like '%Purchase%') and ISNULL(UserID,'') <>'' "; //  IsReversed = 0  and POSTransID is null

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetStatus()");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetStation()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "";
                    sSQL = " select ' All' as StationID union  select distinct StationID  from CCTransmission_Log " +
                    " where  IsReversed = 0  and POSTransID is null and (TransType like '%Sale%' or TransType like '%Return%' or TransType like '%Purchase%') and ISNULL(StationID,'') <>''";

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetStation()");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion
        public void Dispose() { }
    }
}