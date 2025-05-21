using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
namespace POS_Core.DataAccess
{
    

    // Provides data access methods for DeptCode

    public class PODetailSvr : IDisposable
    {

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

        public bool Persist(DataSet updates, IDbTransaction tx, System.Int32 OrderID)
        {
            bool bStatus = true;   //PRIMEPOS-3030 26-Nov-2021 JY Added
            try
            {
                this.Delete(updates, tx);
                bStatus = this.Insert(updates, tx, OrderID);
                this.Update(updates, tx, OrderID);

                updates.AcceptChanges();
                bStatus = true;
            }
            catch (POSExceptions ex)
            {
                bStatus = false;
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                bStatus = false;
                throw (ex);
            }
            catch (Exception ex)
            {
                bStatus = false;
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
            return bStatus;
        }

        public void PutOnHold(DataSet updates, IDbTransaction tx, System.Int32 OrderID)
        {
            try
            {
                this.InsertOnHold(updates, tx, OrderID);

                updates.AcceptChanges();
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        // Inserts, updates or deletes rows in a DataSet.

        /*		public  void Persist(DataSet updates) 
                {

                    IDbTransaction tx=null;
                    try 
                    {
                        IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                        tx = conn.BeginTransaction();
                        this.Persist(updates, tx);
                        tx.Commit();
                    } 
                    catch(POSExceptions ex) 
                    {
                        throw(ex);
                    }

                    catch(OtherExceptions ex) 
                    {
                        throw(ex);
                    }

                    catch(Exception ex) 
                    {
                        tx.Rollback();
                        ErrorHandler.throwException(ex,"","");
                    }

                }
        */
        #endregion

        #region Get Methods

        // Looks up a DeptCode based on its primary-key:System.Int32 DeptCode

        /*
        public PODetailData Populate(System.Int32 DeptCode, IDbConnection conn) 
        {
            try 
            {
                string sSQL = "Select " 
                                    + clsPOSDBConstants.PODetail_Fld_DeptID 
                                    + " , " + clsPOSDBConstants.PODetail_Fld_DeptCode 
                                    + " , " +  clsPOSDBConstants.PODetail_Fld_DeptName 
                                    + " , " +  clsPOSDBConstants.PODetail_Fld_Discount 
                                    + " , " +  clsPOSDBConstants.PODetail_Fld_IsTaxable 
                                    + " , " +  clsPOSDBConstants.PODetail_Fld_SaleStartDate 
                                    + " , " +  clsPOSDBConstants.PODetail_Fld_SaleEndDate 
                                    + " , dept." +   clsPOSDBConstants.PODetail_Fld_TaxID + " as " + clsPOSDBConstants.PODetail_Fld_TaxID
                                    + " , " +  clsPOSDBConstants.PODetail_Fld_SalePrice
                                    + " , taxcodes." + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " as " + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                                    + " , " +  clsPOSDBConstants.TaxCodes_Fld_Description
                                    + " , dept." +  clsPOSDBConstants.PODetail_Fld_UserID + " as " + clsPOSDBConstants.PODetail_Fld_UserID
                                + " FROM " 
                                    + clsPOSDBConstants.PODetail_tbl + " As Dept "
                                    + " , " + clsPOSDBConstants.TaxCodes_tbl + " As TaxCodes "
                                + " WHERE " 
                                    + " Dept." + clsPOSDBConstants.PODetail_Fld_TaxID + " *= TaxCodes." + clsPOSDBConstants.TaxCodes_Fld_TaxID
                                    + " AND " + clsPOSDBConstants.PODetail_Fld_DeptCode + " ='" + DeptCode + "'";


                PODetailData ds = new PODetailData();
                ds.PODetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                            ,  sSQL
                                            , PKParameters(DeptCode)).Tables[0]);
                return ds;
            } 
            catch(POSExceptions ex) 
            {
                throw(ex);
            }

            catch(OtherExceptions ex) 
            {
                throw(ex);
            }

            catch(Exception ex) 
            {
                ErrorHandler.throwException(ex,"","");
                return null;
            }
        }
*/

        public PODetailData Populate(System.Int32 PODetailID, Boolean bSkipIncompleteItems = false)   //Sprint-27 - PRIMEPOS-2026 13-Oct-2017 JY Added optional parameter
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(PODetailID, conn, bSkipIncompleteItems));  //Sprint-27 - PRIMEPOS-2026 13-Oct-2017 JY Added optional parameter  
            }
        }

        /// <summary>
        /// Author: Shitaljit 
        /// To get recodrs from OnHold Detail table
        /// </summary>
        /// <param name="PODetailID"></param>
        /// <returns></returns>
        public PODetailData PopulateDetailFromHold(System.Int32 PODetailID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateFromHold(PODetailID, conn));
            }
        }
        //Added By SRT(Gaurav) Date: 15-Jul-2009
        public PODetailData Populate(System.Collections.Generic.List<string> PODetailID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(PODetailID, conn));
            }
        }
        //End Of Added By SRT(Gaurav)
        //Added By Gaurav
        public PODetailData Populate(System.String dataQuery)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(conn, dataQuery));
            }
        }
        public PODetailData Populate(System.Int32 PODetailID, IDbConnection conn, Boolean bSkipIncompleteItems = false)   //Sprint-27 - PRIMEPOS-2026 13-Oct-2017 JY Added optional parameter
        {
            try
            {
                //string sSQL = "Select "
                //    + "   pod." + clsPOSDBConstants.PODetail_Fld_PODetailID
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " as " + clsPOSDBConstants.PODetail_Fld_ItemID
                //    + " , item."+ clsPOSDBConstants.Item_Fld_Description + " as " + clsPOSDBConstants.Item_Fld_Description
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_QTY + " as " + clsPOSDBConstants.PODetail_Fld_QTY
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_Cost + " as " + clsPOSDBConstants.PODetail_Fld_Cost
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_Comments + " as " + clsPOSDBConstants.PODetail_Fld_Comments
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckQTY + " as " + clsPOSDBConstants.PODetail_Fld_AckQTY
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckStatus + " as " + clsPOSDBConstants.PODetail_Fld_AckStatus
                //    + " , po."  + clsPOSDBConstants.POHeader_Fld_VendorID + " as " + clsPOSDBConstants.POHeader_Fld_VendorID
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_VendorItemCode + " as " + clsPOSDBConstants.PODetail_Fld_VendorItemCode
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductID + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductID
                //    + " , (select case when pod." + clsPOSDBConstants.PODetail_Fld_QTY + " <> 0 THEN(pod." + clsPOSDBConstants.PODetail_Fld_Cost + "/ pod." + clsPOSDBConstants.PODetail_Fld_QTY + ") ELSE 0 END)AS "+clsPOSDBConstants.PODetail_Fld_Price
                //    + " FROM "
                //    + clsPOSDBConstants.PODetail_tbl + " As pod"
                //    + " , " + clsPOSDBConstants.Item_tbl + " As item"
                //    + " , " + clsPOSDBConstants.POHeader_tbl  + " As po"
                //    + " WHERE "
                //    + " pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " = item." + clsPOSDBConstants.Item_Fld_ItemID
                //    + " AND po." + clsPOSDBConstants.PODetail_Fld_OrderID + " = pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                //    + " AND pod." + clsPOSDBConstants.PODetail_Fld_OrderID + " = " + PODetailID;

                string sSQL = string.Empty;
                if (bSkipIncompleteItems == true)   //Sprint-27 - PRIMEPOS-2026 13-Oct-2017 JY Added IF CONDITON TI SKIP BLANK DESCRIPTION ITEMS
                {
                    sSQL = "Select "
                    + "   pod." + clsPOSDBConstants.PODetail_Fld_PODetailID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " as " + clsPOSDBConstants.PODetail_Fld_ItemID
                    + " , item." + clsPOSDBConstants.Item_Fld_Description + " as " + clsPOSDBConstants.Item_Fld_Description
                    + " , item." + clsPOSDBConstants.ItemVendor_Fld_PckSize + " as [" + clsPOSDBConstants.PODetail_Fld_PackSize + "]"
                    + " , item." + clsPOSDBConstants.ItemVendor_Fld_PckQty + " as  [" + clsPOSDBConstants.PODetail_Fld_PackQuant + "]"
                    + " , item." + clsPOSDBConstants.ItemVendor_Fld_PckUnit + " as [" + clsPOSDBConstants.PODetail_Fld_PackUnit + "]"
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_QTY + " as " + clsPOSDBConstants.PODetail_Fld_QTY
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_Cost + " as " + clsPOSDBConstants.PODetail_Fld_Cost
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_LastCostPrice + " as " + clsPOSDBConstants.PODetail_Fld_LastCostPrice
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_Comments + " as " + clsPOSDBConstants.PODetail_Fld_Comments
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckQTY + " as " + clsPOSDBConstants.PODetail_Fld_AckQTY
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckStatus + " as " + clsPOSDBConstants.PODetail_Fld_AckStatus
                    + " , po." + clsPOSDBConstants.POHeader_Fld_VendorID + " as " + clsPOSDBConstants.POHeader_Fld_VendorID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_VendorItemCode + " as " + clsPOSDBConstants.PODetail_Fld_VendorItemCode
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ProcessedQty + " as " + clsPOSDBConstants.PODetail_Fld_ProcessedQty 
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductID + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductID
                    + " , ISNULL(item." + clsPOSDBConstants.Item_Fld_QtyInStock + ",0) AS " + clsPOSDBConstants.PODetail_Fld_QtyInStock   //PRIMEPOS-2396 12-Jun-2018 JY Added
                    + " , ISNULL(item." + clsPOSDBConstants.Item_Fld_SellingPrice + ",0) AS " + clsPOSDBConstants.PODetail_Fld_Price    //PRIMEPOS-3124 19-Sep-2022 JY Added
                    + " FROM " + clsPOSDBConstants.PODetail_tbl + " As pod"
                    + " INNER JOIN " + clsPOSDBConstants.Item_tbl + " As item ON " + " pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " = item." + clsPOSDBConstants.Item_Fld_ItemID
                    + " INNER JOIN " + clsPOSDBConstants.POHeader_tbl + " As po ON " + " po." + clsPOSDBConstants.PODetail_Fld_OrderID + " = pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                    + " WHERE ISNULL(item.Description,'') <> '' " + " AND pod." + clsPOSDBConstants.PODetail_Fld_OrderID + " = " + PODetailID;
                }
                else
                {
                    sSQL = "Select "
                    + "   pod." + clsPOSDBConstants.PODetail_Fld_PODetailID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " as " + clsPOSDBConstants.PODetail_Fld_ItemID
                    + " , item." + clsPOSDBConstants.Item_Fld_Description + " as " + clsPOSDBConstants.Item_Fld_Description
                    //Added by SRT(Abhishek)
                    + " , item." + clsPOSDBConstants.ItemVendor_Fld_PckSize + " as [" + clsPOSDBConstants.PODetail_Fld_PackSize + "]"
                    + " , item." + clsPOSDBConstants.ItemVendor_Fld_PckQty + " as  [" + clsPOSDBConstants.PODetail_Fld_PackQuant + "]"
                    + " , item." + clsPOSDBConstants.ItemVendor_Fld_PckUnit + " as [" + clsPOSDBConstants.PODetail_Fld_PackUnit + "]"
                    //End Of Added by SRT(Abhishek) 
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_QTY + " as " + clsPOSDBConstants.PODetail_Fld_QTY
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_Cost + " as " + clsPOSDBConstants.PODetail_Fld_Cost
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_LastCostPrice + " as " + clsPOSDBConstants.PODetail_Fld_LastCostPrice//added by Ravindra PRIMEPOS-1043
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_Comments + " as " + clsPOSDBConstants.PODetail_Fld_Comments
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckQTY + " as " + clsPOSDBConstants.PODetail_Fld_AckQTY
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckStatus + " as " + clsPOSDBConstants.PODetail_Fld_AckStatus
                    + " , po." + clsPOSDBConstants.POHeader_Fld_VendorID + " as " + clsPOSDBConstants.POHeader_Fld_VendorID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_VendorItemCode + " as " + clsPOSDBConstants.PODetail_Fld_VendorItemCode
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ProcessedQty + " as " + clsPOSDBConstants.PODetail_Fld_ProcessedQty //added by RAVINDRA(Quicsolv) 3 April 2013
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductID + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductID
                    + " , ISNULL(item." + clsPOSDBConstants.Item_Fld_QtyInStock + ",0) AS " + clsPOSDBConstants.PODetail_Fld_QtyInStock   //PRIMEPOS-2396 12-Jun-2018 JY Added
                    + " , ISNULL(item." + clsPOSDBConstants.Item_Fld_SellingPrice + ",0) AS " + clsPOSDBConstants.PODetail_Fld_Price    //PRIMEPOS-3124 19-Sep-2022 JY Added
                    + " FROM "
                    + clsPOSDBConstants.PODetail_tbl + " As pod"
                    + " , " + clsPOSDBConstants.Item_tbl + " As item"
                    + " , " + clsPOSDBConstants.POHeader_tbl + " As po"
                    + " WHERE "
                    + " pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " = item." + clsPOSDBConstants.Item_Fld_ItemID
                    + " AND po." + clsPOSDBConstants.PODetail_Fld_OrderID + " = pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                    + " AND pod." + clsPOSDBConstants.PODetail_Fld_OrderID + " = " + PODetailID;
                }

                PODetailData ds = new PODetailData();
                ds.PODetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                    , sSQL
                    , PKParameters(PODetailID)).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        /// <summary>
        /// Author: Shitaljit
        /// Added On: 5/27/2014
        /// Added to get PODetaisl records from Onhold table.
        /// </summary>
        /// <param name="PODetailID"></param>
        /// <param name="conn"></param>
        /// <returns></returns>
        public PODetailData PopulateFromHold(System.Int32 PODetailID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + "   pod." + clsPOSDBConstants.PODetail_Fld_PODetailID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " as " + clsPOSDBConstants.PODetail_Fld_ItemID
                    + " , item." + clsPOSDBConstants.Item_Fld_Description + " as " + clsPOSDBConstants.Item_Fld_Description
                    + " , item." + clsPOSDBConstants.ItemVendor_Fld_PckSize + " as [" + clsPOSDBConstants.PODetail_Fld_PackSize + "]"
                    + " , item." + clsPOSDBConstants.ItemVendor_Fld_PckQty + " as  [" + clsPOSDBConstants.PODetail_Fld_PackQuant + "]"
                    + " , item." + clsPOSDBConstants.ItemVendor_Fld_PckUnit + " as [" + clsPOSDBConstants.PODetail_Fld_PackUnit + "]"
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_QTY + " as " + clsPOSDBConstants.PODetail_Fld_QTY
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_Cost + " as " + clsPOSDBConstants.PODetail_Fld_Cost
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_LastCostPrice + " as " + clsPOSDBConstants.PODetail_Fld_LastCostPrice//added by Ravindra PRIMEPOS-1043
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_Comments + " as " + clsPOSDBConstants.PODetail_Fld_Comments
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckQTY + " as " + clsPOSDBConstants.PODetail_Fld_AckQTY
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckStatus + " as " + clsPOSDBConstants.PODetail_Fld_AckStatus
                    + " , po." + clsPOSDBConstants.POHeader_Fld_VendorID + " as " + clsPOSDBConstants.POHeader_Fld_VendorID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_VendorItemCode + " as " + clsPOSDBConstants.PODetail_Fld_VendorItemCode
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ProcessedQty + " as " + clsPOSDBConstants.PODetail_Fld_ProcessedQty //added by RAVINDRA(Quicsolv) 3 April 2013
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductID + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductID
                    + " FROM "
                    + clsPOSDBConstants.PODetailOnHold_tbl + " As pod"
                    + " , " + clsPOSDBConstants.Item_tbl + " As item"
                    + " , " + clsPOSDBConstants.POHeaderOnHold_tbl + " As po"
                    + " WHERE "
                    + " pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " = item." + clsPOSDBConstants.Item_Fld_ItemID
                    + " AND po." + clsPOSDBConstants.PODetail_Fld_OrderID + " = pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                    + " AND pod." + clsPOSDBConstants.PODetail_Fld_OrderID + " = " + PODetailID;

                PODetailData ds = new PODetailData();
                ds.PODetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                    , sSQL
                    , PKParameters(PODetailID)).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        //Added By SRT(Gaurav) Date : 15-Jul-2009
        public PODetailData Populate(System.Collections.Generic.List<string> PODetailID, IDbConnection conn)
        {
            try
            {
                //string sSQL = "Select "
                //    + "   pod." + clsPOSDBConstants.PODetail_Fld_PODetailID
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " as " + clsPOSDBConstants.PODetail_Fld_ItemID
                //    + " , item."+ clsPOSDBConstants.Item_Fld_Description + " as " + clsPOSDBConstants.Item_Fld_Description
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_QTY + " as " + clsPOSDBConstants.PODetail_Fld_QTY
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_Cost + " as " + clsPOSDBConstants.PODetail_Fld_Cost
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_Comments + " as " + clsPOSDBConstants.PODetail_Fld_Comments
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckQTY + " as " + clsPOSDBConstants.PODetail_Fld_AckQTY
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckStatus + " as " + clsPOSDBConstants.PODetail_Fld_AckStatus
                //    + " , po."  + clsPOSDBConstants.POHeader_Fld_VendorID + " as " + clsPOSDBConstants.POHeader_Fld_VendorID
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_VendorItemCode + " as " + clsPOSDBConstants.PODetail_Fld_VendorItemCode
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier
                //    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductID + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductID
                //    + " , (select case when pod." + clsPOSDBConstants.PODetail_Fld_QTY + " <> 0 THEN(pod." + clsPOSDBConstants.PODetail_Fld_Cost + "/ pod." + clsPOSDBConstants.PODetail_Fld_QTY + ") ELSE 0 END)AS "+clsPOSDBConstants.PODetail_Fld_Price
                //    + " FROM "
                //    + clsPOSDBConstants.PODetail_tbl + " As pod"
                //    + " , " + clsPOSDBConstants.Item_tbl + " As item"
                //    + " , " + clsPOSDBConstants.POHeader_tbl  + " As po"
                //    + " WHERE "
                //    + " pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " = item." + clsPOSDBConstants.Item_Fld_ItemID
                //    + " AND po." + clsPOSDBConstants.PODetail_Fld_OrderID + " = pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                //    + " AND pod." + clsPOSDBConstants.PODetail_Fld_OrderID + " = " + PODetailID;

                string sSQL = "Select "
                    + "   pod." + clsPOSDBConstants.PODetail_Fld_PODetailID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " as " + clsPOSDBConstants.PODetail_Fld_ItemID
                    + " , item." + clsPOSDBConstants.Item_Fld_Description + " as " + clsPOSDBConstants.Item_Fld_Description
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_QTY + " as " + clsPOSDBConstants.PODetail_Fld_QTY
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_Cost + " as " + clsPOSDBConstants.PODetail_Fld_Cost
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_LastCostPrice + " as " + clsPOSDBConstants.PODetail_Fld_LastCostPrice
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_Comments + " as " + clsPOSDBConstants.PODetail_Fld_Comments
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckQTY + " as " + clsPOSDBConstants.PODetail_Fld_AckQTY          
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_AckStatus + " as " + clsPOSDBConstants.PODetail_Fld_AckStatus
                     + " , pod." + clsPOSDBConstants.PODetail_Fld_ProcessedQty + " as " + clsPOSDBConstants.PODetail_Fld_ProcessedQty // Adedd by Ravindra (Quicsolv) 3 April
                    + " , po." + clsPOSDBConstants.POHeader_Fld_VendorID + " as " + clsPOSDBConstants.POHeader_Fld_VendorID
                    + " , vendor."+ clsPOSDBConstants.Vendor_Fld_VendorCode + " as VendorCode"
                    + " , vendor."+ clsPOSDBConstants.Vendor_Fld_VendorName +" as VendorName"
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_VendorItemCode + " as " + clsPOSDBConstants.PODetail_Fld_VendorItemCode
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_ChangedProductID + " as " + clsPOSDBConstants.PODetail_Fld_ChangedProductID
                    //+ " , (CASE WHEN pod." + clsPOSDBConstants.PODetail_Fld_QTY + ">0 THEN pod." + clsPOSDBConstants.PODetail_Fld_Cost + " / pod." + clsPOSDBConstants.PODetail_Fld_QTY + " ELSE 0 END) as Price" //PRIMEPOS-3155 12-Oct-2022 JY Commented
                    + " , pod." + clsPOSDBConstants.PODetail_Fld_Cost + " as Price"     //PRIMEPOS-3155 12-Oct-2022 JY Added
                    + " FROM "
                    + clsPOSDBConstants.PODetail_tbl + " As pod"
                    + " , " + clsPOSDBConstants.Item_tbl + " As item "
                    + " , " + clsPOSDBConstants.POHeader_tbl + " As po"
                    + " , " + clsPOSDBConstants.Vendor_tbl +" As vendor"
                    + " WHERE "
                    + " pod." + clsPOSDBConstants.PODetail_Fld_ItemID + " = item." + clsPOSDBConstants.Item_Fld_ItemID
                    + " AND po." + clsPOSDBConstants.PODetail_Fld_OrderID + " = pod." + clsPOSDBConstants.PODetail_Fld_OrderID
                    + " AND vendor."+ clsPOSDBConstants.Vendor_Fld_VendorId +" = po." + clsPOSDBConstants.POHeader_Fld_VendorID                   
                    + " AND pod." + clsPOSDBConstants.PODetail_Fld_PODetailID + " in ( " + string.Join(",", PODetailID.ToArray()) +")";

                PODetailData ds = new PODetailData();
                ds.PODetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                    , sSQL
                    , null).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        //End Of Added By SRT(Gaurav)
        //Added By Gaurav
        public PODetailData Populate(IDbConnection conn, System.String sqlQuery)
        {
            try
            {

                PODetailData ds = new PODetailData();
                ds.PODetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                    , sqlQuery
                    , null).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        // Fills a PODetailData with all DeptCode
        /*
                public PODetailData PopulateList(string sWhereClause, IDbConnection conn) 
                {
                    try 
                    { 
                        string sSQL = String.Concat("Select " 
                                                + clsPOSDBConstants.PODetail_Fld_DeptID 
                                                + " , " + clsPOSDBConstants.PODetail_Fld_DeptCode 
                                                + " , " +  clsPOSDBConstants.PODetail_Fld_DeptName 
                                                + " , " +  clsPOSDBConstants.PODetail_Fld_Discount 
                                                + " , " +  clsPOSDBConstants.PODetail_Fld_IsTaxable 
                                                + " , " +  clsPOSDBConstants.PODetail_Fld_SaleStartDate 
                                                + " , " +  clsPOSDBConstants.PODetail_Fld_SaleEndDate 
                                                + " , " +  clsPOSDBConstants.PODetail_Fld_TaxID 
                                                + " , " +  clsPOSDBConstants.PODetail_Fld_SalePrice
                                            + " FROM " 
                                                + clsPOSDBConstants.PODetail_tbl 
                                            ,sWhereClause);

                        PODetailData ds = new PODetailData();
                        ds.PODetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
                        return ds;
                    } 
                    catch(POSExceptions ex) 
                    {
                        throw(ex);
                    }

                    catch(OtherExceptions ex) 
                    {
                        throw(ex);
                    }

                    catch (Exception ex) 
                    {    
                        ErrorHandler.throwException(ex,"",""); 
                        return null;
                    } 
                }

                public PODetailData Populate(System.String DeptCode) 
                {
                    using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
                    {
                        return(Populate(DeptCode, conn));
                    }
                }

                // Fills a PODetailData with all DeptCode

            // Fills a PODetailData with all DeptCode

		
        */
        public String GetNextPODID()
        {
            IDbConnection conn = null;
            string sSQL = string.Empty;
            string response = string.Empty;
            int podID = 1;
            try
            {
                conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                sSQL = "Select MAX(PODetailID) from " + clsPOSDBConstants.PODetail_tbl;
                response = DataHelper.ExecuteScalar(conn, CommandType.Text, sSQL).ToString();
                if (response != null)
                {
                    podID = Convert.ToInt32(response);
                    podID++;
                }
            }
            catch (Exception ex)
            {
                return podID.ToString();
            }
            return podID.ToString();
        }
        public PODetailData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(whereClause, conn));
            }
        }
        //Added By SRT(Gaurav)
        public PODetailData PopulateList(string strQuery, out bool Result)
        {
            Result = false;
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                try
                {
                    PODetailData ds = new PODetailData();
                    ds.PODetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strQuery).Tables[0]);
                    return ds;
                    Result = true;

                }
                catch (Exception ex)
                {
                    Result = false;
                    return null;
                }
            }
        }
        //End Of Added By SRT(Gaurav)
        private PODetailData PopulateList(string whereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select "
                                         + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_PODetailID
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_OrderID
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_ItemID
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_QTY
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_Cost
                                          + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_LastCostPrice //Added BY Ravindra PRIMEPOS-1043
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_AckQTY
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_ProcessedQty //Added BY Ravindra 
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_AckStatus
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_Comments
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_VendorItemCode
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_ChangedProductID
                                         + " , " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier
                                         + " ,itm." + clsPOSDBConstants.Item_Fld_Description
                                         + " ,PO." + clsPOSDBConstants.POHeader_Fld_Status
                                         + " ,PO." + clsPOSDBConstants.POHeader_Fld_OrderNo
                                     + " FROM "
                                         + clsPOSDBConstants.PODetail_tbl + " As " + clsPOSDBConstants.PODetail_tbl
                                         + " , " + clsPOSDBConstants.POHeader_tbl + " As PO"
                                         + " , " + clsPOSDBConstants.Item_tbl + " As itm" +
                                     " WHERE "
                                     + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_OrderID + " = " + "PO." + clsPOSDBConstants.POHeader_Fld_OrderID
                                     + " AND " + clsPOSDBConstants.PODetail_tbl + "." + clsPOSDBConstants.PODetail_Fld_ItemID + " = " + "itm." + clsPOSDBConstants.Item_Fld_ItemID
                                     + " AND PO." + clsPOSDBConstants.POHeader_Fld_Status + " IN (0,1,2,3)"
                                     , whereClause);

                PODetailData ds = new PODetailData();
                ds.PODetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(whereClause)).Tables[0]);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Insert, Update, and Delete Methods
        public bool Insert(DataSet ds, IDbTransaction tx, System.Int32 OrderID)
        {
            bool bStatus = true;   //PRIMEPOS-3030 26-Nov-2021 JY Added
            PODetailTable addedTable = (PODetailTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (PODetailRow row in addedTable.Rows)
                {
                    try
                    {
                        row.OrderID = OrderID;
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.PODetail_tbl, insParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        bStatus = true;
                    }
                    catch (POSExceptions ex)
                    {
                        bStatus = false;
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        bStatus = false;
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        bStatus = false;
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();                
            }
            return bStatus;
        }

        public void InsertOnHold(DataSet ds, IDbTransaction tx, System.Int32 OrderID)
        {

            PODetailTable addedTable = (PODetailTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (PODetailRow row in addedTable.Rows)
                {
                    try
                    {
                        row.OrderID = OrderID;
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.PODetailOnHold_tbl, insParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }
        // Update all rows in a DeptCodes DataSet, within a given database transaction.

        public void Update(DataSet ds, IDbTransaction tx, System.Int32 OrderID)
        {
            PODetailTable modifiedTable = (PODetailTable)ds.Tables[0].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (PODetailRow row in modifiedTable.Rows)
                {
                    try
                    {
                        row.OrderID = OrderID;
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.PODetail_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
            OrderID = 0;
        }

        // Delete all rows within a DeptCodes DataSet, within a given database transaction.
        public void Delete(DataSet ds, IDbTransaction tx)
        {
            PODetailTable table = (PODetailTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
            string sSQL;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (PODetailRow row in table.Rows)
                {
                    try
                    {
                        sSQL = BuildDeleteSQL(clsPOSDBConstants.PODetail_tbl, row);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
            }
        }
        //Added by Prashant(SRT) Date:18-7-09
        public int DeletePODetails(String poDetailID)
        {
            IDbConnection conn = null;
            string sSQL = string.Empty;
            int response = 0;
            try
            {
                conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                sSQL = "delete from  " + clsPOSDBConstants.PODetail_tbl + " where PODetailID = '" + poDetailID + "'";
                response = (int)DataHelper.ExecuteNonQuery(conn, CommandType.Text, sSQL);
            }
            catch (Exception ex)
            {
                return response;
            }
            return response;
        }
        //End Added by Prashant(SRT) Date:18-7-09
        private string BuildDeleteSQL(string tableName, PODetailRow row)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            // build where clause
            //for(int i = 0;i<delParam.Length;i++)
            //{
            //	sDeleteSQL = sDeleteSQL + delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
            //}
            sDeleteSQL += clsPOSDBConstants.PODetail_Fld_PODetailID + " = " + row[0].ToString();
            return sDeleteSQL;
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


        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].Value;
            sUpdateSQL = sUpdateSQL + " , " + updParam[2].SourceColumn + "  = " + updParam[2].Value;
            sUpdateSQL = sUpdateSQL + " , " + updParam[3].SourceColumn + "  = " + updParam[3].Value;
            sUpdateSQL = sUpdateSQL + " , " + updParam[4].SourceColumn + "  = '" + updParam[4].Value + "' ";
            sUpdateSQL = sUpdateSQL + " , " + updParam[5].SourceColumn + "  = '" + updParam[5].Value + "' ";
            sUpdateSQL = sUpdateSQL + " , " + updParam[6].SourceColumn + "  = '" + updParam[6].Value + "' ";
            sUpdateSQL = sUpdateSQL + " , " + updParam[7].SourceColumn + "  = '" + updParam[7].Value + "' ";
            sUpdateSQL = sUpdateSQL + " , " + updParam[8].SourceColumn + "  = '" + updParam[8].Value + "' ";
            sUpdateSQL = sUpdateSQL + " , " + updParam[9].SourceColumn + "  = '" + updParam[9].Value + "' ";
            sUpdateSQL = sUpdateSQL + " , " + updParam[10].SourceColumn + "  = '" + updParam[10].Value + "' ";
            //added by atul 22-oct -2010
            sUpdateSQL = sUpdateSQL + " , " + updParam[11].SourceColumn + "  = '" + updParam[11].Value + "' ";
            sUpdateSQL = sUpdateSQL + " , " + updParam[12].SourceColumn + "  = '" + updParam[12].Value + "' ";
            //End of added by atul 22-oct -2010//Added by Rqavindra(Quicsolv
            sUpdateSQL = sUpdateSQL + " , " + updParam[13].SourceColumn + "  = '" + updParam[13].Value + "' ";
            sUpdateSQL = sUpdateSQL + " , " + updParam[14].SourceColumn + "  = '" + updParam[14].Value + "' ";//Added by Ravindra PRIMEPOS-1043

            //for(int i = 2;i<updParam.Length-1;i++)
            //{
            //	sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn +"  = " + updParam[i].ParameterName ;
            //}

            //sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'" ;

            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].Value;
            return sUpdateSQL;
        }

        #endregion
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
        private IDbDataParameter[] PKParameters(System.Int32 PODetailID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@PODetailID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = PODetailID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(PODetailRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@PODetailID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.PODetailID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.PODetail_Fld_PODetailID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(PODetailRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(14);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_OrderID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_QTY, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_Cost, System.Data.DbType.Currency);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ItemID, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_Comments, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_AckQTY, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_AckStatus, System.Data.DbType.String);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_VendorItemCode, System.Data.DbType.String);

            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier, System.Data.DbType.String);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ChangedProductID, System.Data.DbType.String);
            //added by atul 22-oct-2010
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ItemDescType, System.Data.DbType.String);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_Idescription, System.Data.DbType.String);
            //End of added by atul 22-oct-2010
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ProcessedQty, System.Data.DbType.Int32 );//Added BY Ravindra 
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_LastCostPrice, System.Data.DbType.Currency);//Added BY Ravindra PRIMEPOS-1043 

            sqlParams[0].SourceColumn = clsPOSDBConstants.PODetail_Fld_OrderID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.PODetail_Fld_QTY;
            sqlParams[2].SourceColumn = clsPOSDBConstants.PODetail_Fld_Cost;
            sqlParams[3].SourceColumn = clsPOSDBConstants.PODetail_Fld_ItemID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.PODetail_Fld_Comments;
            sqlParams[5].SourceColumn = clsPOSDBConstants.PODetail_Fld_AckQTY;
            sqlParams[6].SourceColumn = clsPOSDBConstants.PODetail_Fld_AckStatus;
            sqlParams[7].SourceColumn = clsPOSDBConstants.PODetail_Fld_VendorItemCode;

            sqlParams[8].SourceColumn = clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier;
            sqlParams[9].SourceColumn = clsPOSDBConstants.PODetail_Fld_ChangedProductID;

            //added by atul 22-oct-2010
            sqlParams[10].SourceColumn = clsPOSDBConstants.PODetail_Fld_ItemDescType;
            sqlParams[11].SourceColumn = clsPOSDBConstants.PODetail_Fld_Idescription;
            //End of added by atul 22-oct-2010
            sqlParams[12].SourceColumn = clsPOSDBConstants.PODetail_Fld_ProcessedQty;
            sqlParams[13].SourceColumn = clsPOSDBConstants.PODetail_Fld_LastCostPrice;//Added BY Ravindra PRIMEPOS-1043 

            if (row.OrderID.ToString() != System.String.Empty)
                sqlParams[0].Value = row.OrderID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.QTY.ToString() != System.String.Empty)
                sqlParams[1].Value = row.QTY;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Cost.ToString() != System.String.Empty)
                sqlParams[2].Value = row.Cost;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.ItemID.ToString() != System.String.Empty)
                sqlParams[3].Value = row.ItemID;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.Comments != System.String.Empty)
                sqlParams[4].Value = row.Comments;
            else
                sqlParams[4].Value = "";

            if (row.QTY.ToString() != System.String.Empty)
                sqlParams[5].Value = row.AckQTY;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.AckStatus != System.String.Empty)
                sqlParams[6].Value = row.AckStatus;
            else
                sqlParams[6].Value = "";

            if (row.VendorItemCode != System.String.Empty)
                sqlParams[7].Value = row.VendorItemCode;
            else
                sqlParams[7].Value = "";

            if (row.ChangedProductQualifier != System.String.Empty)
                sqlParams[8].Value = row.ChangedProductQualifier;
            else
                sqlParams[8].Value = "";

            if (row.ChangedProductID != System.String.Empty)
                sqlParams[9].Value = row.ChangedProductID;
            else
                sqlParams[9].Value = "";

            //Added by atul 22-oct-2010
            if (row.ItemDescType != System.String.Empty)
                sqlParams[10].Value = row.ItemDescType;
            else
                sqlParams[10].Value = "";

            if (row.Idescription != System.String.Empty)    //PRIMEPO-159 02-May-2018 JY Correct the condition
                sqlParams[11].Value = row.Idescription.Replace("'", "''");   //PRIMEPOS-2597 09-Oct-2018 JY modified
            else
                sqlParams[11].Value = "";
            //End of Added by atul 22-oct-2010

            //Added by Ravindra
            if (row.ProcessedQTY.ToString() != System.String.Empty)
                sqlParams[12].Value = row.ProcessedQTY;
            else
                sqlParams[12].Value = DBNull.Value;

            sqlParams[13].Value = row.LastCostPrice;//Added BY Ravindra PRIMEPOS-1043 

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(PODetailRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(15);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_PODetailID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_QTY, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_OrderID, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_Cost, System.Data.DbType.Currency);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ItemID, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_Comments, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_AckQTY, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_AckStatus, System.Data.DbType.String);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_VendorItemCode, System.Data.DbType.String);

            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier, System.Data.DbType.String);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ChangedProductID, System.Data.DbType.String);
            //added by atul 22-oct-2010
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ItemDescType, System.Data.DbType.String);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_Idescription, System.Data.DbType.String);
            //End of added by atul 22-oct-2010
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_ProcessedQty, System.Data.DbType.Int32);//Added by Ravindra (Quicsolv) 3 April 2013
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PODetail_Fld_LastCostPrice, System.Data.DbType.Currency);//Added by Ravindra (Quicsolv) PRIMEPOS-1043
           

            sqlParams[0].SourceColumn = clsPOSDBConstants.PODetail_Fld_PODetailID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.PODetail_Fld_QTY;
            sqlParams[2].SourceColumn = clsPOSDBConstants.PODetail_Fld_OrderID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.PODetail_Fld_Cost;
            sqlParams[4].SourceColumn = clsPOSDBConstants.PODetail_Fld_ItemID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.PODetail_Fld_Comments;
            sqlParams[6].SourceColumn = clsPOSDBConstants.PODetail_Fld_AckQTY;
            sqlParams[7].SourceColumn = clsPOSDBConstants.PODetail_Fld_AckStatus;
            sqlParams[8].SourceColumn = clsPOSDBConstants.PODetail_Fld_VendorItemCode;

            sqlParams[9].SourceColumn = clsPOSDBConstants.PODetail_Fld_ChangedProductQualifier;
            sqlParams[10].SourceColumn = clsPOSDBConstants.PODetail_Fld_ChangedProductID;
            //added by atul 22-oct-2010
            sqlParams[11].SourceColumn = clsPOSDBConstants.PODetail_Fld_ItemDescType;
            sqlParams[12].SourceColumn = clsPOSDBConstants.PODetail_Fld_Idescription;

            //End of added by atul 22-oct-2010
            sqlParams[13].SourceColumn = clsPOSDBConstants.PODetail_Fld_ProcessedQty; //Added by Ravindra (Quicsolv) 3 April 2013
            sqlParams[14].SourceColumn = clsPOSDBConstants.PODetail_Fld_LastCostPrice;//Added BY Ravindra PRIMEPOS-1043 

            if (Convert.ToInt32(row[0].ToString()) != 0)
                sqlParams[0].Value = Convert.ToInt32(row[0].ToString());
            else
                sqlParams[0].Value = 0;

            sqlParams[1].Value = row.QTY;
            sqlParams[2].Value = row.OrderID;

            sqlParams[3].Value = row.Cost;

            if (row.ItemID != System.String.Empty)
                sqlParams[4].Value = row.ItemID;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.Comments != System.String.Empty)
                sqlParams[5].Value = row.Comments;
            else
                sqlParams[5].Value = DBNull.Value;

            sqlParams[6].Value = row.AckQTY;
            sqlParams[7].Value = row.AckStatus;
            sqlParams[8].Value = row.VendorItemCode;


            if (row.ChangedProductQualifier != System.String.Empty)
                sqlParams[9].Value = row.ChangedProductQualifier;
            else
                sqlParams[9].Value = "";

            if (row.ChangedProductID != System.String.Empty)
                sqlParams[10].Value = row.ChangedProductID;
            else
                sqlParams[10].Value = "";
            //Added by atul 22-oct-2010
            if (row.ItemDescType != System.String.Empty)
                sqlParams[11].Value = row.ItemDescType;
            else
                sqlParams[11].Value = "";

            if (row.Idescription != System.String.Empty)    //PRIMEPO-159 02-May-2018 JY correct the condition
                sqlParams[12].Value = row.Idescription.Replace("'","''");   //PRIMEPOS-2597 09-Oct-2018 JY modified
            else
                sqlParams[12].Value = "";
            //End of Added by atul 22-oct-2010
            sqlParams[13].Value = row.ProcessedQTY;//Added by Ravindra(QuicSolv) 3 April 2013
            sqlParams[14].Value = row.LastCostPrice;//Added BY Ravindra PRIMEPOS-1043 
            return (sqlParams);
        }

        #endregion


        public void Dispose() { }
    }
}
