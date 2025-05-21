using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
//using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
////using POS.Resources;
using POS_Core.Resources;
using Resources;
using NLog;

namespace POS_Core.DataAccess
{

    // Provides data access methods for Item

    public class ItemSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        private const string strIsEBTSubQuery = ",IsNull((select top 1 isEBTItem from iias_items as iias where iias.UPCCode=item.itemid),0) as IsEBTItem ";

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

        public void Persist(DataSet updates, IDbTransaction tx)
        {
            try
            {
                this.Delete(updates, tx);
                this.Insert(updates, tx);
                this.Update(updates, tx);

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
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }


        // Inserts, updates or deletes rows in a DataSet.

        public void Persist(DataSet updates)
        {
            try
            {

                using (IDbConnection conn = DataFactory.CreateConnection()) //Sprint-22 05-Nov-2015 JY Added using clause
                {
                    conn.ConnectionString = DBConfig.ConnectionString;
                    conn.Open();
                    IDbTransaction tx = conn.BeginTransaction();
                    this.Persist(updates, tx);
                }
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

        #region PRIMEPOS-3125 22-Dec-2022 JY Modified
        public void UpdateBulkSellingPrice(DataTable dtItemSellingPriceType, IDbTransaction tx)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(DBConfig.ConnectionString))
                {
                    SqlCommand oSqlCommand = new SqlCommand();
                    oSqlCommand.CommandTimeout = 1200; 
                    oSqlCommand.Connection = cn;
                    oSqlCommand.CommandType = CommandType.StoredProcedure;
                    oSqlCommand.CommandText = "usp_UpdateBulkSellingPrice";
                    SqlParameter tvpParam = oSqlCommand.Parameters.AddWithValue("@ItemSellingPriceType", dtItemSellingPriceType);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam = oSqlCommand.Parameters.AddWithValue("@UserID", Configuration.UserName);
                    tvpParam.SqlDbType = SqlDbType.VarChar;
                    cn.Open();
                    int nAffectedRows = oSqlCommand.ExecuteNonQuery();
                }
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
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #endregion

        #region Get Methods

        // Looks up a Item based on its primary-key:System.String ItemId

        public ItemData Populate(System.String ItemId, IDbConnection conn)
        {
            try
            {
                //Updated By SRT(Ritesh Parekh) Date : 27-Jul-2009
                //Updated for triming database field.
                ItemData ds = new ItemData();
                if (ItemId.Length == 11)
                {
                    //cmt by SRT (Sachin) Date : 14 Nov 2009
                    ds.Item.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ItemID.Trim() + " like '" + ItemId.Trim().Replace("'", "''") + "%'", PKParameters(ItemId)).Tables[0]);
                }
                else
                {
                    //cmt by SRT (Sachin) Date : 14 Nov 2009
                    ds.Item.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ItemID.Trim() + " = '" + ItemId.Trim().Replace("'", "''") + "'", PKParameters(ItemId)).Tables[0]);
                    if (ItemId.Trim() != "" && (ds.Item == null || ds.Item.Count == 0)) //02-Sep-2015 JY Added condition "ItemId.Trim() != """ to resolve the issue on inventory received screen, also handeled the same issue on inventory received screen 
                    {
                        //Added by Manoj  to search by Product code (SKU) 7/17/2015
                        ds.Item.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ProductCode.Trim() + " = '" + ItemId.Trim().Replace("'", "''") + "'", PKParameters(ItemId)).Tables[0]);
                    }
                }
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


        //added to populate data for Auto update price 
        public ItemData Populate(System.String ItemId, System.String action, System.Boolean isPrimePO, IDbConnection conn)
        {
            DataTable dtOfItem = null;
            bool isExist = false;
            try
            {
                using (ItemData oItemData = new ItemData())
                {
                    if (action == clsPOSDBConstants.UpdatePrice)
                    {
                        //if (ItemId.Length == 12)
                        //{
                        //    dtOfItem = DataHelper.ExecuteDataset(conn, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ItemID + " ='" + ItemId + "'", PKParameters(ItemId)).Tables[0];
                        //    isExist = IsRecordsExist(dtOfItem);
                        //    ds.Item.MergeTable(dtOfItem);
                        //}
                        if (ItemId.Length == 12)
                        {
                            dtOfItem = DataHelper.ExecuteDataset(conn, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ItemID + " ='" + ItemId + "'", PKParameters(ItemId)).Tables[0];
                            isExist = IsRecordsExist(dtOfItem);
                            if (isExist == false)
                            {
                                ItemId = ItemId.Substring(0, ItemId.Length - 1);
                                dtOfItem = DataHelper.ExecuteDataset(conn, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ItemID + " ='" + ItemId + "'", PKParameters(ItemId)).Tables[0];
                                //isExist = IsRecordsExist(dtOfItem);
                            }
                            oItemData.Item.MergeTable(dtOfItem);
                        }
                        else
                        {
                            dtOfItem = DataHelper.ExecuteDataset(conn, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ItemID + " like '" + ItemId + "%'", PKParameters(ItemId)).Tables[0];
                            //isExist = IsRecordsExist(dtOfItem);
                            oItemData.Item.MergeTable(dtOfItem);
                        }
                    }
                    return oItemData;
                }
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

        private bool IsRecordsExist(DataTable dtOfItem)
        {
            bool flag = false;
            try
            {
                if (dtOfItem.Rows.Count > 0)
                    flag = true;
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
            return flag;
        }

        //Populate by vendor item Code 
        public ItemData Populate(System.String vendorItemCode, System.String VendorId, IDbConnection conn)
        {
            try
            {
                String sql = String.Empty;
                ItemData ds = new ItemData();
                sql = "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " +
                clsPOSDBConstants.ItemVendor_Fld_ItemID + " IN (SELECT ItemID FROM " + clsPOSDBConstants.ItemVendor_tbl +
                " WHERE " + clsPOSDBConstants.ItemVendor_Fld_VendorItemID + " ='" + vendorItemCode + "' and " + clsPOSDBConstants.POHeader_Fld_VendorID + " ='" + VendorId + "')";
                ds.Item.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sql).Tables[0]);
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

        public ItemData Populate(System.String ItemId, IDbTransaction oTrans)
        {
            try
            {
                ItemData ds = new ItemData();
                if (ItemId.Length == 11)
                    ds.Item.MergeTable(DataHelper.ExecuteDataset(oTrans, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ItemID + " like '" + ItemId + "%'", PKParameters(ItemId)).Tables[0]);
                else
                    ds.Item.MergeTable(DataHelper.ExecuteDataset(oTrans, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ItemID + " ='" + ItemId + "'", PKParameters(ItemId)).Tables[0]);
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

        //added to populate data for Auto update price 
        public ItemData Populate(System.String ItemId, System.String action, System.Boolean isPrimePO)
        {
            using (IDbConnection conn = DataFactory.CreateConnection()) //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                return (Populate(ItemId, action, isPrimePO, conn));
            }
        }
        public ItemData Populate(System.String ItemId)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())  //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                return (Populate(ItemId, conn));
            }
        }
        //Populate by vendor item id 
        public ItemData Populate(System.String vendorItemCode, System.String vendorId)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                return (Populate(vendorItemCode, vendorId, conn));
            }
        }
        public ItemData FindItemBySKUCode(System.String SKUCode)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                return (FindItemBySKUCode(SKUCode, conn));
            }
        }

        public ItemData FindItemBySKUCode(System.String SKUCode, IDbConnection conn)
        {
            try
            {
                ItemData ds = new ItemData();
                ds.Item.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ProductCode + " ='" + SKUCode.Replace("'", "''") + "'", PKParameters(SKUCode)).Tables[0]);
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

        public ItemData FindItemByID(System.String itemID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                return (FindItemByID(itemID, conn));
            }
        }

        public ItemData FindItemByID(System.String itemID, IDbConnection conn)
        {
            try
            {
                ItemData ds = new ItemData();
                ds.Item.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select *" + strIsEBTSubQuery + " FROM " + clsPOSDBConstants.Item_tbl + " WHERE " + clsPOSDBConstants.Item_Fld_ItemID + " ='" + itemID + "'", PKParameters(itemID)).Tables[0]);
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

        // Fills a ItemData with all Item

        public ItemData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("SELECT *" + strIsEBTSubQuery + " FROM ", clsPOSDBConstants.Item_tbl, sWhereClause);

                ItemData ds = new ItemData();
                ds.Item.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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

        // Fills the DataSet with All the itemname and itemid
        public DataSet PopulateListWithIdName(string condition)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection(DBConfig.ConnectionString))   //Sprint-22 05-Nov-2015 JY Added using clause
                {
                    DataSet ds = new DataSet();
                    string sSQL = "";
                    sSQL = "SELECT  "
                                        + clsPOSDBConstants.Item_Fld_ItemID
                                        + " , " + clsPOSDBConstants.Item_Fld_Description
                                        + " , " + clsPOSDBConstants.Item_Fld_EXCLUDEFROMCLCouponPay
                                        + " FROM "
                                        + clsPOSDBConstants.Item_tbl;
                    if (!String.IsNullOrEmpty(condition))
                    {
                        sSQL += condition;
                    }

                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        // Fills a ItemData with all Item

        public ItemData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = DBConfig.ConnectionString;

                return (PopulateList(whereClause, conn));
            }
        }
        //Following Code Added by Krishna on 29 November 2011
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereClause">Adv search is  </param>
        /// <param name="isAdvSearch"></param>
        /// <returns></returns>
        public DataSet PopulateAdvSearch(string whereClause, ref bool isVendorRequired, ref bool isItemVendorRequired)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                /// <returns></returns>
                /// <returns></returns>
                return (PopulateAdvSearch(whereClause, ref isVendorRequired, ref isItemVendorRequired, conn));
            }
        }

        #region 19-Jun-2015 JY Added overload for whereClause1
        public DataSet PopulateAdvSearch(string whereClause, ref bool isVendorRequired, ref bool isItemVendorRequired, string WhereClause1)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                /// <returns></returns>
                /// <returns></returns>
                return (PopulateAdvSearch(whereClause, ref isVendorRequired, ref isItemVendorRequired, conn, WhereClause1));
            }
        }
        #endregion

        public DataSet PopulateAdvSearch(string sWhereClause, ref bool isVendorRequired, ref bool isItemVendorRequired, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Empty;
                //18-Jun-2015 JY Added table alias to all columns to avoid column ambiguous error
                string strDisplayFields = "I." + clsPOSDBConstants.Item_Fld_ItemID + " as [Item Code]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Description + " as [Item Description]," +
                                            "I." + clsPOSDBConstants.Item_Fld_SellingPrice + " as [Unit Price]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Unit + " as Unit," +
                                            "I." + clsPOSDBConstants.Item_Fld_Location + " as Location," +
                                            "I." + clsPOSDBConstants.Item_Fld_QtyInStock + " as [Qty In Stock]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Discount + " as Discount," +
                                            "I." + clsPOSDBConstants.Item_Fld_ExptDeliveryDate + " as [Delivery Date]," +
                                            "I." + clsPOSDBConstants.Item_Fld_ReOrderLevel + " as [Reorder Level]," +
                                            "I." + clsPOSDBConstants.Item_Fld_SaleEndDate + " as [Sale End Date]," +
                                            "I." + clsPOSDBConstants.Item_Fld_SaleStartDate + " as [Sale Start Date]," +
                                            "I." + clsPOSDBConstants.Item_Fld_ProductCode + " as [SKU Code]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Remarks + " as [Remarks]," +
                                            "ISNULL(I." + clsPOSDBConstants.Item_Fld_IsNonRefundable + ",0)" + " as [Non-Refundable]," +   //PRIMEPOS-2592 06-Nov-2018 JY Added 
                                            "I." + clsPOSDBConstants.Item_Fld_ExpDate + " AS [Exp. Date]," +  //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Added
                                            "I." + clsPOSDBConstants.Item_Fld_PreferredVendor + " AS [Preferred Vendor]," +
                                            "I." + clsPOSDBConstants.Item_Fld_LastVendor + " AS [Last Vendor]";

                if (isVendorRequired == true && isItemVendorRequired == false)
                {
                    sSQL = "Select " + strDisplayFields + " FROM Item I LEFT OUTER JOIN Vendor V on I.LastVendor = V.VendorCode OR I.PREFERREDVENDOR = V.VendorCode ";
                }
                else if (isItemVendorRequired == true)
                {
                    sSQL = "Select " + strDisplayFields + " from (" + clsPOSDBConstants.Item_tbl + " I LEFT OUTER JOIN " + clsPOSDBConstants.ItemVendor_tbl + " IV on " +
                                   " (I.ItemID=IV.ItemID)) LEFT OUTER JOIN " + clsPOSDBConstants.Vendor_tbl + " V on(IV." + clsPOSDBConstants.ItemVendor_Fld_VendorID + "=V." + clsPOSDBConstants.Vendor_Fld_VendorId + ") ";
                }
                else
                {
                    sSQL = "Select " + strDisplayFields + " from " + clsPOSDBConstants.Item_tbl + " I";
                }

                sSQL = String.Concat(sSQL, sWhereClause);

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
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
        //Till here Added by Krishna on 29 November 2011

        #region 19-Jun-2015 JY Added
        public DataSet PopulateAdvSearch(string sWhereClause, ref bool isVendorRequired, ref bool isItemVendorRequired, IDbConnection conn, string sWhereClause1)
        {
            try
            {
                string sSQL = String.Empty, sSQL1 = string.Empty;
                string strDisplayFields = " 1 as rnum, I." + clsPOSDBConstants.Item_Fld_ItemID + " as [Item Code]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Description + " as [Item Description]," +
                                            "I." + clsPOSDBConstants.Item_Fld_SellingPrice + " as [Unit Price]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Unit + " as Unit," +
                                            "I." + clsPOSDBConstants.Item_Fld_Location + " as Location," +
                                            "I." + clsPOSDBConstants.Item_Fld_QtyInStock + " as [Qty In Stock]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Discount + " as Discount," +
                                            "I." + clsPOSDBConstants.Item_Fld_ExptDeliveryDate + " as [Delivery Date]," +
                                            "I." + clsPOSDBConstants.Item_Fld_ReOrderLevel + " as [Reorder Level]," +
                                            "I." + clsPOSDBConstants.Item_Fld_SaleEndDate + " as [Sale End Date]," +
                                            "I." + clsPOSDBConstants.Item_Fld_SaleStartDate + " as [Sale Start Date]," +
                                            "I." + clsPOSDBConstants.Item_Fld_ProductCode + " as [SKU Code]," +
                                            "I." + clsPOSDBConstants.Item_Fld_Remarks + " as [Remarks]," +
                                            "ISNULL(I." + clsPOSDBConstants.Item_Fld_IsNonRefundable + ",0)" + " as [Non-Refundable]," +   //PRIMEPOS-2592 06-Nov-2018 JY Added 
                                            "I." + clsPOSDBConstants.Item_Fld_ExpDate + " AS [Exp. Date]," +  //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Added
                                            "I." + clsPOSDBConstants.Item_Fld_PreferredVendor + " AS [Preferred Vendor]," +
                                            "I." + clsPOSDBConstants.Item_Fld_LastVendor + " AS [Last Vendor]";

                if (isVendorRequired == true && isItemVendorRequired == false)
                {
                    sSQL = "Select " + strDisplayFields + " FROM Item I LEFT OUTER JOIN Vendor V on I.LastVendor = V.VendorCode OR I.PREFERREDVENDOR = V.VendorCode ";
                }
                else if (isItemVendorRequired == true)
                {
                    sSQL = "Select " + strDisplayFields + " from (" + clsPOSDBConstants.Item_tbl + " I LEFT OUTER JOIN " + clsPOSDBConstants.ItemVendor_tbl + " IV on " +
                                   " (I.ItemID=IV.ItemID)) LEFT OUTER JOIN " + clsPOSDBConstants.Vendor_tbl + " V on(IV." + clsPOSDBConstants.ItemVendor_Fld_VendorID + "=V." + clsPOSDBConstants.Vendor_Fld_VendorId + ") ";
                }
                else
                {
                    sSQL = "Select " + strDisplayFields + " from " + clsPOSDBConstants.Item_tbl + " I";
                }

                sSQL = String.Concat(sSQL, sWhereClause);

                if (sWhereClause1 != string.Empty)
                {
                    string strDisplayFields1 = " 2 as rnum, I." + clsPOSDBConstants.Item_Fld_ItemID + " as [Item Code]," +
                            "I." + clsPOSDBConstants.Item_Fld_Description + " as [Item Description]," +
                            "I." + clsPOSDBConstants.Item_Fld_SellingPrice + " as [Unit Price]," +
                            "I." + clsPOSDBConstants.Item_Fld_Unit + " as Unit," +
                            "I." + clsPOSDBConstants.Item_Fld_Location + " as Location," +
                            "I." + clsPOSDBConstants.Item_Fld_QtyInStock + " as [Qty In Stock]," +
                            "I." + clsPOSDBConstants.Item_Fld_Discount + " as Discount," +
                            "I." + clsPOSDBConstants.Item_Fld_ExptDeliveryDate + " as [Delivery Date]," +
                            "I." + clsPOSDBConstants.Item_Fld_ReOrderLevel + " as [Reorder Level]," +
                            "I." + clsPOSDBConstants.Item_Fld_SaleEndDate + " as [Sale End Date]," +
                            "I." + clsPOSDBConstants.Item_Fld_SaleStartDate + " as [Sale Start Date]," +
                            "I." + clsPOSDBConstants.Item_Fld_ProductCode + " as [SKU Code]," +
                            "I." + clsPOSDBConstants.Item_Fld_Remarks + " as [Remarks]," +
                            "ISNULL(I." + clsPOSDBConstants.Item_Fld_IsNonRefundable + ",0)" + " as [Non-Refundable]," +   //PRIMEPOS-2592 06-Nov-2018 JY Added 
                            "I." + clsPOSDBConstants.Item_Fld_ExpDate + " AS [Exp. Date]," +  //Sprint-21 - PRIMEPOS-2206 07-Mar-2016 JY Added
                            "I." + clsPOSDBConstants.Item_Fld_PreferredVendor + " AS [Preferred Vendor]," +
                            "I." + clsPOSDBConstants.Item_Fld_LastVendor + " AS [Last Vendor]";

                    if (isVendorRequired == true && isItemVendorRequired == false)
                    {
                        sSQL1 = "Select " + strDisplayFields1 + " FROM Item I LEFT OUTER JOIN Vendor V on I.LastVendor = V.VendorCode OR I.PREFERREDVENDOR = V.VendorCode ";
                    }
                    else if (isItemVendorRequired == true)
                    {
                        sSQL1 = "Select " + strDisplayFields1 + " from (" + clsPOSDBConstants.Item_tbl + " I LEFT OUTER JOIN " + clsPOSDBConstants.ItemVendor_tbl + " IV on " +
                                       " (I.ItemID=IV.ItemID)) LEFT OUTER JOIN " + clsPOSDBConstants.Vendor_tbl + " V on(IV." + clsPOSDBConstants.ItemVendor_Fld_VendorID + "=V." + clsPOSDBConstants.Vendor_Fld_VendorId + ") ";
                    }
                    else
                    {
                        sSQL1 = "Select " + strDisplayFields1 + " from " + clsPOSDBConstants.Item_tbl + " I";
                    }

                    sSQL1 = String.Concat(sSQL1, sWhereClause1);

                    sSQL += " UNION " + sSQL1 + " ORDER BY 1, 3 ";
                }

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                ds.Tables[0].Columns.RemoveAt(0);
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
        #endregion

        public DataSet GetItemPriceLog(string sItemCode)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = " SELECT " +
                                        " IPH.* " +
                                        " ,Item.Description " +
                                    " FROM  " +
                                        " Itempricehistory IPH " +
                                        " ,Item " +
                                    " WHERE " +
                                        "IPH.ItemID=Item.ItemID ";

                    if (sItemCode.Trim() != "")
                    {
                        sSQL += " and Item.ItemID='" + sItemCode.Replace("'", "''") + "'";
                    }

                    sSQL += " Order By Item.Description,IPH.AddedON Desc";


                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
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

        public DataSet GetPacketInfo(string sItemCode)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = " SELECT " +
                                        "  Item.ItemID " +
                                        " ,Item.Description " +
                                        " ,Item.PckSize as [Packet Size] " +
                                        " ,Item.PckUnit as [Packet Unit] " +
                                        " ,Item.PckQty  as [Packet Quantity] " +
                                        " ,Item.QtyInStock  as [Qty In Hand] " +
                                    " FROM  " +
                                        " Item ";

                    if (sItemCode.Trim() != "")
                    {
                        sSQL += " Where Item.ItemID='" + sItemCode.Replace("'", "''") + "'";
                    }
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
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

        public DataTable GetIIAS_Category_Descriptor()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = " Select Distinct CategoryDescriptor From IIAS_Items " +
                    " Where IsNull(CategoryDescriptor,'')<>'' Order By CategoryDescriptor ";


                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds.Tables[0];
                }
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

        public DataTable GetIIAS_SubCategory_Descriptor()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())   //Sprint-22 05-Nov-2015 JY Added using clause
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = " Select Distinct SubCategoryDescriptor From IIAS_Items " +
                    " Where IsNull(SubCategoryDescriptor,'')<>'' Order By SubCategoryDescriptor ";


                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds.Tables[0];
                }
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

        public bool IsIIASItem(string sItemCode)
        {
            bool retVal = false;
            IDbConnection conn = null;
            try
            {
                //if itemcode is of 12 digit means with check digit then remove last check digit s
                if (sItemCode.Length == 12)
                {
                    sItemCode = sItemCode.Substring(0, 11);
                }
                conn = DataFactory.CreateConnection();
                conn.ConnectionString = DBConfig.ConnectionString;

                string sSQL = " SELECT " +
                                    " 1 " +
                                " FROM  " +
                                    " IIAS_Items " +
                                 " WHERE UPCCode='" + sItemCode.Replace("'", "''") + "' and isActive=1 and IsNull(changeIndicator,'')<>'D'";

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                conn.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = true;
                }

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
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                conn = null;
            }
            return retVal;
        }

        //End of Added By shitaljit on 23 August 2011
        /// <summary>
        /// Author: Shitaljit
        /// Check itemID lenght if 12 make it 11 and search again if 11 look for 12 digit item.
        /// </summary>
        /// <param name="sItemID"></param>
        public string LookForMatchingItem(string sItemID, out bool IsItemExist)
        {
            POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "LookForMatchingItem() ", clsPOSDBConstants.Log_Entering);
            ItemData oItemData;
            ItemRow oItemRow = null;
            IsItemExist = false;
            string sTempItemID = string.Empty;
            string sNewItemID = string.Empty;
            try
            {
                if (sItemID.Length == 12)
                {
                    sTempItemID = sItemID.Substring(0, 11);
                    oItemData = Populate(sTempItemID);
                    if (Configuration.isNullOrEmptyDataSet(oItemData) == false)
                    {
                        oItemRow = oItemData.Item[0];
                        sNewItemID = oItemRow.ItemID;
                        IsItemExist = true;
                    }
                }
                else if (sItemID.Length == 11)
                {
                    oItemData = PopulateList(" WHERE ItemID LIKE('" + sItemID.Replace("'", "''") + "%')");
                    if (Configuration.isNullOrEmptyDataSet(oItemData) == false)
                    {
                        oItemRow = oItemData.Item[0];
                        sNewItemID = oItemRow.ItemID;
                        IsItemExist = true;
                    }
                }
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "LookForMatchingItem() ", clsPOSDBConstants.Log_Entering);
                return sNewItemID;
            }
            catch (Exception Ex)
            {
                POS_Core.ErrorLogging.Logs.Logger(clsPOSDBConstants.Log_Module_Transaction, "LookForMatchingItem() ", clsPOSDBConstants.Log_Exception_Occured);
                return sNewItemID;
                throw Ex;
            }
        }

        public bool IsEBTItem(string sItemCode)
        {
            bool retVal = false;
            try
            {
                #region Commected Code for JIRA-PRIMEPOS-1187 Ability to maintain separate list for EBT Items

                //if itemcode is of 12 digit means with check digit then remove last check digit s
                //if (sItemCode.Length == 12)
                //{
                //    sItemCode = sItemCode.Substring(0, 11);
                //}
                //Commedted by shitajit as we need to seperate out the logic of EBT item from IIAS item list 
                //JIRA-PRIMEPOS-1187 Ability to maintain separate list for EBT Items
                //string sSQL = " SELECT " +
                //                    " IsNull(IsEBTItem,0) " +
                //                " FROM  " +
                //                    " IIAS_Items " +
                //                 " WHERE UPCCode='" + sItemCode.Replace("'", "''") + "'";
                #endregion
                if (sItemCode.Length == 12)
                {
                    bool IsItemExist = false;
                    sItemCode = LookForMatchingItem(sItemCode, out IsItemExist);
                }
                string sSQL = " SELECT " +
                                    " IsNull(IsEBTItem,0) " +
                                " FROM  " +
                                    clsPOSDBConstants.Item_tbl +
                                 " WHERE " + clsPOSDBConstants.Item_Fld_ItemID + " ='" + sItemCode.Replace("'", "''") + "'";

                object value = DataHelper.ExecuteScalar(sSQL);

                if (value != null)
                {
                    retVal = Convert.ToBoolean(value);
                }

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
            return retVal;
        }

        public bool IsOTCItem(string sItemCode)
        {
            bool retVal = false;
            IDbConnection conn = null;
            try
            {
                conn = DataFactory.CreateConnection();
                conn.ConnectionString = DBConfig.ConnectionString;

                string sSQL = " SELECT " +
                                    " IsOtcItem " +
                                " FROM  " +
                                    " Item " +
                                 " WHERE itemid='" + sItemCode.Replace("'", "''") + "' ";

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                conn.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Configuration.convertNullToBoolean(ds.Tables[0].Rows[0][0].ToString());
                }

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
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                conn = null;
            }
            return retVal;
        }

        public bool IsDiscountable(string sItemCode)
        {
            bool fllag = false;
            using (IDbConnection conn = DataFactory.CreateConnection()) //Sprint-22 05-Nov-2015 JY Added using clause
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                string sSQL = "select isDiscountable from item where ItemID='" + sItemCode + "'";
                DataSet dset = new DataSet();
                dset = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                conn.Close();
                if (dset.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dset.Tables[0].Rows[0][0]) == false)
                        fllag = false;
                    else
                        fllag = true;
                }
                else
                {
                    fllag = false;
                }
                return fllag;
            }
        }

        public int GetFineLineCode(string sItemCode)
        {
            int retVal = 0;
            IDbConnection conn = null;
            try
            {
                conn = DataFactory.CreateConnection();
                conn.ConnectionString = DBConfig.ConnectionString;

                string sSQL = " SELECT " +
                                    " SubSubDepartmentID " +
                                " FROM  " +
                                    " SubSubDeptItems " +
                                 " WHERE UPCCode='" + sItemCode.Replace("'", "''") + "'";

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                conn.Close();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = Configuration.convertNullToInt(ds.Tables[0].Rows[0][0].ToString());
                }

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
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                conn = null;
            }
            return retVal;
        }

        //Sprint-18 - 2133 11-Nov-2014 JY this function returns incorrect calculation hence added its overload
        public bool IsQtyValid(string sItemCode, int customerID, int lineItemQty)
        {
            bool retValue = true;

            if (IsOTCItem(sItemCode))
            {
                ItemMonitorCategoryData oItemMonitorCatData;
                ItemMonitorCategorySvr oItemMonitorCatSvr = new ItemMonitorCategorySvr();
                oItemMonitorCatData = oItemMonitorCatSvr.GetByItemID(sItemCode);
                TransDetailSvr oTransDetailSvr = new TransDetailSvr();

                DateTime monthStartDate = Configuration.GetMonthStartDate(DateTime.Now);
                DateTime monthEndDate = Configuration.GetMonthEndDate(DateTime.Now);

                ItemMonitorCategoryDetailData oIMCDData = null;
                ItemMonitorCategoryDetailRow oIMCDRow = null;
                decimal unitPerPackage = 0;
                ItemMonitorCategoryDetailSvr oIMCDetailsvr = new ItemMonitorCategoryDetailSvr();

                foreach (ItemMonitorCategoryRow row in oItemMonitorCatData.ItemMonitorCategory.Rows)
                {
                    #region commented by shitaljit Previous Item Monitoring Validation logic
                    //if (row.DailyLimit > 0 && oTransDetailSvr.GetTotalQtySold(sItemCode, customerID, DateTime.Now, DateTime.Now) + lineItemQty > row.DailyLimit)
                    //{
                    //    retValue = false;
                    //    throw new Exception("Daily limit exceded for item " + sItemCode + ".");
                    //}
                    //else if (row.ThirtyDaysLimit > 0 && oTransDetailSvr.GetTotalQtySold(sItemCode, customerID, monthStartDate, monthEndDate) + lineItemQty > row.ThirtyDaysLimit)
                    //{
                    //    retValue = false;
                    //    throw new Exception("30 days limit exceded for item " + sItemCode + ".");
                    //}
                    #endregion

                    //New Item Monitoring Validation logic added by shitaljit on 8 May 2012
                    oIMCDData = oIMCDetailsvr.Populate(sItemCode);
                    if (oIMCDData != null && oIMCDData.Tables[0].Rows.Count > 0)
                    {
                        oIMCDRow = (ItemMonitorCategoryDetailRow)oIMCDData.Tables[0].Rows[0];
                        unitPerPackage = oIMCDRow.UnitsPerPackage;
                    }
                    if (unitPerPackage <= 0)
                    {
                        unitPerPackage = 1;
                    }
                    if (row.LimitPeriodDays > 0 && row.LimitPeriodQty > 0 && (unitPerPackage * (oTransDetailSvr.GetTotalQtySold(sItemCode, customerID, DateTime.Now.AddDays(-row.LimitPeriodDays), DateTime.Now) + lineItemQty)) > row.LimitPeriodQty)
                    {
                        retValue = false;
                        throw new Exception("Maximum quantity limit " + row.LimitPeriodQty + " for " + row.LimitPeriodDays + " days exceded for item " + sItemCode + ".");
                    }
                }
            }

            return retValue;
        }

        #region Sprint-18 - 2133 13-Nov-2014 JY Added corrected logic for sudafed 
        public Boolean IsQtyValid(int customerID, DataTable dt)
        {
            Boolean retValue = true;

            ItemMonitorCategoryData oItemMonitorCatData;
            ItemMonitorCategorySvr oItemMonitorCatSvr = new ItemMonitorCategorySvr();
            oItemMonitorCatData = oItemMonitorCatSvr.GetByItemID();
            TransDetailSvr oTransDetailSvr = new TransDetailSvr();

            DateTime monthStartDate = Configuration.GetMonthStartDate(DateTime.Now);
            DateTime monthEndDate = Configuration.GetMonthEndDate(DateTime.Now);

            ItemMonitorCategoryDetailSvr oIMCDetailsvr = new ItemMonitorCategoryDetailSvr();

            foreach (ItemMonitorCategoryRow row in oItemMonitorCatData.ItemMonitorCategory.Rows)
            {
                if (row.LimitPeriodDays > 0 && row.LimitPeriodQty > 0)
                {
                    decimal TotalQtySold = oTransDetailSvr.GetTotalQtySold(row.ID, customerID, DateTime.Now.AddDays(-row.LimitPeriodDays), DateTime.Now);
                    //calculate current item quantity and add into total quantity
                    foreach (TransDetailRow oRow in dt.Rows)
                    {
                        TotalQtySold += oTransDetailSvr.GetCurrentItemQty(oRow.ItemID, oRow.QTY, row.ID);
                    }

                    if (row.LimitPeriodDays > 0 && row.LimitPeriodQty > 0 && TotalQtySold > row.LimitPeriodQty)
                    {
                        retValue = false;
                        throw new Exception("Maximum quantity limit " + row.LimitPeriodQty + " for " + row.LimitPeriodDays + " days exceded for item.");
                    }
                }
            }
            return retValue;
        }
        #endregion

        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx)
        {
            ItemTable addedTable = (ItemTable)ds.Tables[clsPOSDBConstants.Item_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (ItemRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.Item_tbl, insParam);
                        for (int i = 0; i < insParam.Length; i++)
                        {
                            Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
                        }
                        AddItemPriceHistory(row.ItemID, row.SellingPrice, row.LastCostPrice, Configuration.UserName, "I", Configuration.UpdatedBy, tx, 0, row.OnSalePrice, false);  //Sprint-25 - PRIMEPOS-294 04-May-2017 JY Added additional column for logging 
                        try
                        {
                            if (Configuration.convertNullToInt(row.QtyInStock) > 0 && UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.InventoryMgmt.ID, UserPriviliges.Screens.PhysicalInventory.ID))  //PRIMEPOS-3121 12-Sep-2022 JY Added
                            {
                                using (PhysicalInvData oPhysicalInvData = new PhysicalInvData())
                                {
                                    using (BusinessRules.PhysicalInv oPhysicalInv = new BusinessRules.PhysicalInv())
                                    {
                                        PhysicalInvRow oPhysicalInvRow = oPhysicalInvData.PhysicalInv.AddRow(1, Configuration.convertNullToString(row.ItemID), 0, Configuration.convertNullToInt(row.QtyInStock), 0, Configuration.convertNullToDecimal(row.SellingPrice), 0, Configuration.convertNullToDecimal(row.LastCostPrice));
                                        oPhysicalInvRow.UserID = Configuration.UserName;
                                        oPhysicalInvRow.TransDate = System.DateTime.Now;
                                        oPhysicalInvRow.isProcessed = true;
                                        oPhysicalInvRow.PTransDate = System.DateTime.Now;
                                        oPhysicalInvRow.PUserID = Configuration.UserName;
                                        oPhysicalInvRow.NewExpDate = row.ExpDate;
                                        oPhysicalInv.Persist(oPhysicalInvData);                                        
                                    }
                                }
                            }
                        }
                        catch(Exception Ex)
                        {
                            logger.Fatal(Ex, "Insert(DataSet ds, IDbTransaction tx) - update Physical Inventory");
                        }

                        DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL, insParam);
                        #region Commected Code for JIRA-PRIMEPOS-1187 Ability to maintain separate list for EBT Items
                        //this.UpdateEBTStatus(tx, row);
                        #endregion
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.Item_DuplicateCode);
                        else
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

        // Update all rows in a Item DataSet, within a given database transaction.

        public void Update(DataSet ds, IDbTransaction tx)
        {
            ItemTable modifiedTable = (ItemTable)ds.Tables[clsPOSDBConstants.Item_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (ItemRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.Item_tbl, updParam);
                        AddItemPriceHistory(row.ItemID, row.SellingPrice, row.LastCostPrice, Configuration.UserName, "I", Configuration.UpdatedBy, tx, 0, row.OnSalePrice, false);  //Sprint-25 - PRIMEPOS-294 04-May-2017 JY Added additional column for logging
                        DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL, updParam);
                        #region Commected Code for JIRA-PRIMEPOS-1187 Ability to maintain separate list for EBT Items
                        //this.UpdateEBTStatus(tx, row);
                        #endregion
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.Item_DuplicateCode);
                        else
                            throw (ex);
                    }

                    catch (Exception ex)
                    {
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        // Delete all rows within a Item DataSet, within a given database transaction.
        public void Delete(DataSet ds, IDbTransaction tx)
        {

            ItemTable table = (ItemTable)ds.Tables[clsPOSDBConstants.Item_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (ItemRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.Item_tbl, delParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, delParam);
                        this.UpdateEBTStatus(tx, row);
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

        private string BuildDeleteSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            // build where clause
            for (int i = 0; i < delParam.Length; i++)
            {
                sDeleteSQL = sDeleteSQL + delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
            }
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
            sInsertSQL = sInsertSQL + " , UserId ";
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " , '" + Configuration.UserName + "'";
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

            for (int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }
            sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'";
            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            return sUpdateSQL;
        }

        public bool DeleteRow(string CurrentID)
        {
            string sSQL;
            try
            {
                DataTable dt = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, "SELECT TOP 1 ItemID FROM PurchaseOrderDetail WHERE ItemID = '" + CurrentID.Replace("'", "''") + "'").Tables[0];
                if (dt != null && dt.Rows.Count == 0)
                {
                    dt = DataHelper.ExecuteDataset(Configuration.ConnectionString, CommandType.Text, "SELECT TOP 1 ItemID FROM POSTransactionDetail WHERE ItemID = '" + CurrentID.Replace("'", "''") + "'").Tables[0];  //PRIMEPOS-2937 25-Jan-2021 JY Added
                    if (dt != null && dt.Rows.Count == 0)
                    {
                        sSQL = " DELETE FROM ITEM WHERE ItemID = '" + CurrentID.Replace("'", "''") + "'";
                        DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

        private void UpdateEBTStatus(IDbTransaction oTrans, ItemRow oItem)
        {
            string strQuery;

            strQuery = " Select ID From IIAS_Items where UPCCOde='" + oItem.ItemID.Replace("'", "''") + "'";
            if (DataHelper.ExecuteScalar(strQuery) == null)
            {
                if (oItem.IsEBTItem == true)
                {
                    strQuery = "INSERT INTO IIAS_Items  " +
                    " ([UPCCode], [Description],  " +
                    " [ChangeIndicator], [CreateDate],  " +
                    "[IsActive], [InActivateDate],[IsEBTItem])  " +
                    " Values( '" + oItem.ItemID.Replace("'", "''") + "','" + oItem.Description.Replace("'", "''") + "','D',getdate() ,1 ,null," +
                    Configuration.convertBoolToInt(oItem.IsEBTItem) + ")";
                    DataHelper.ExecuteNonQuery(strQuery);

                }
            }
            else
            {
                strQuery = "Update IIAS_Items Set " +
                " [ISEBTItem]= " + Configuration.convertBoolToInt(oItem.IsEBTItem) +
                " Where[UPCCode]= '" + oItem.ItemID.Replace("'", "''") + "'";
                DataHelper.ExecuteNonQuery(strQuery);

            }
        }

        #region PRIMEPOS-2464 05-Mar-2020 JY Added
        public void UpdateDisplayItemCost(int ModuleID, int ScreenID, int PermissionID, bool isAllowed)
        {
            string strQuery = "UPDATE Util_UserRights SET isAllowed = " + Configuration.convertBoolToInt(isAllowed) + " WHERE UserID = '" + Configuration.UserName + "' AND ModuleID = " + ModuleID + " AND ScreenID = " + ScreenID + " AND PermissionID = " + PermissionID;
            DataHelper.ExecuteNonQuery(strQuery);
        }
        #endregion

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
        private IDbDataParameter[] PKParameters(System.String ItemId)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ItemId";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = ItemId;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(ItemRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ItemId";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.ItemID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.Item_Fld_ItemID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(ItemRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(50);//Change by Shitaljit for PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL //Sprint-18 - 2041 28-Oct-2014 JY  changed index to 47  //Sprint-21 - 2206,2173 03-Jul-2015 JY Changed from 47 to 49

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ItemID, System.Data.SqlDbType.VarChar);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_DepartmentID, System.Data.SqlDbType.Int);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Description, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Itemtype, System.Data.SqlDbType.VarChar);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ProductCode, System.Data.SqlDbType.VarChar);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SaleTypeCode, System.Data.SqlDbType.VarChar);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SeasonCode, System.Data.SqlDbType.VarChar);
            sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Unit, System.Data.SqlDbType.VarChar);
            sqlParams[8] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Freight, System.Data.DbType.Currency);
            sqlParams[9] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SellingPrice, System.Data.DbType.Currency);
            sqlParams[10] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_AvgPrice, System.Data.DbType.Currency);
            sqlParams[11] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LastCostPrice, System.Data.DbType.Currency);
            sqlParams[12] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_isTaxable, System.Data.SqlDbType.Bit);
            sqlParams[13] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_TaxID, System.Data.SqlDbType.Int);
            sqlParams[14] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_isDiscountable, System.Data.SqlDbType.Bit);
            sqlParams[15] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Discount, System.Data.SqlDbType.Float);
            sqlParams[16] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SaleStartDate, System.Data.SqlDbType.DateTime);
            sqlParams[17] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SaleEndDate, System.Data.SqlDbType.DateTime);
            sqlParams[18] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_OnSalePrice, System.Data.DbType.Currency);
            sqlParams[19] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_QtyInStock, System.Data.SqlDbType.Int);
            sqlParams[20] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Location, System.Data.SqlDbType.VarChar);
            sqlParams[21] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_MinOrdQty, System.Data.SqlDbType.Int);
            sqlParams[22] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ReOrderLevel, System.Data.SqlDbType.Int);
            sqlParams[23] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_QtyOnOrder, System.Data.SqlDbType.Int);
            sqlParams[24] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExptDeliveryDate, System.Data.SqlDbType.DateTime);
            sqlParams[25] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LastVendor, System.Data.SqlDbType.VarChar);
            sqlParams[26] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LastRecievDate, System.Data.SqlDbType.DateTime);
            sqlParams[27] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LastSellingDate, System.Data.SqlDbType.DateTime);
            sqlParams[28] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Remarks, System.Data.SqlDbType.VarChar);
            sqlParams[29] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_isOnSale, System.Data.SqlDbType.Bit);
            sqlParams[30] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExclFromAutoPO, System.Data.SqlDbType.Bit);
            sqlParams[31] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExclFromRecpt, System.Data.SqlDbType.Bit);
            sqlParams[32] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_isOTCItem, System.Data.SqlDbType.Bit);
            sqlParams[33] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_UpdatePrice, System.Data.SqlDbType.Bit);
            sqlParams[34] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_PreferredVendor, System.Data.SqlDbType.VarChar);
            sqlParams[35] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SubDepartmentID, System.Data.SqlDbType.Int);

            sqlParams[36] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckSize, System.Data.SqlDbType.VarChar);
            sqlParams[37] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckQty, System.Data.SqlDbType.VarChar);
            sqlParams[38] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckUnit, System.Data.SqlDbType.VarChar);
            sqlParams[39] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_TaxPolicy, System.Data.SqlDbType.Char);//Added By Shitaljit(QuicSolv) on 18 August
            //sqlParams[40] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExpDate, System.Data.SqlDbType.SmallDateTime);//Added by Krishna on 5 October 2011
            //sqlParams[41] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LotNumber, System.Data.SqlDbType.VarChar);//Added by Krishna on 5 October 2011
            sqlParams[40] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ManufacturerName, System.Data.SqlDbType.VarChar);
            //Add by Ravindra for sale Limit 22 March 2013
            sqlParams[41] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SaleLimitQty, System.Data.SqlDbType.Int);
            sqlParams[42] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_DiscountPolicy, System.Data.SqlDbType.Char);//Added By Shitaljit(QuicSolv) on 3 April 2013
            sqlParams[43] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_IsEBTItem, System.Data.SqlDbType.Bit);
            //Addedby shitaljit on 2/6/2014 for PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
            sqlParams[44] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_IsDefaultCLPoint, System.Data.SqlDbType.Bit);
            sqlParams[45] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_PointsPerDollar, System.Data.SqlDbType.Int);
            //END
            sqlParams[46] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_CLPointPolicy, System.Data.SqlDbType.Char);   //Sprint-18 - 2041 28-Oct-2014 JY Added
            sqlParams[47] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExpDate, System.Data.SqlDbType.SmallDateTime);    //Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
            sqlParams[48] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_IsActive, System.Data.SqlDbType.Bit); //Sprint-21 - 2173 06-Jul-2015 JY Added
            sqlParams[49] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_IsNonRefundable, System.Data.SqlDbType.Bit);  //PRIMEPOS-2592 01-Nov-2018 JY Added 

            sqlParams[0].SourceColumn = clsPOSDBConstants.Item_Fld_ItemID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Item_Fld_DepartmentID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Item_Fld_Description;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Item_Fld_Itemtype;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Item_Fld_ProductCode;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Item_Fld_SaleTypeCode;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Item_Fld_SeasonCode;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Item_Fld_Unit;
            sqlParams[8].SourceColumn = clsPOSDBConstants.Item_Fld_Freight;
            sqlParams[9].SourceColumn = clsPOSDBConstants.Item_Fld_SellingPrice;
            sqlParams[10].SourceColumn = clsPOSDBConstants.Item_Fld_AvgPrice;
            sqlParams[11].SourceColumn = clsPOSDBConstants.Item_Fld_LastCostPrice;
            sqlParams[12].SourceColumn = clsPOSDBConstants.Item_Fld_isTaxable;
            sqlParams[13].SourceColumn = clsPOSDBConstants.Item_Fld_TaxID;
            sqlParams[14].SourceColumn = clsPOSDBConstants.Item_Fld_isDiscountable;
            sqlParams[15].SourceColumn = clsPOSDBConstants.Item_Fld_Discount;
            sqlParams[16].SourceColumn = clsPOSDBConstants.Item_Fld_SaleStartDate;
            sqlParams[17].SourceColumn = clsPOSDBConstants.Item_Fld_SaleEndDate;
            sqlParams[18].SourceColumn = clsPOSDBConstants.Item_Fld_OnSalePrice;
            sqlParams[19].SourceColumn = clsPOSDBConstants.Item_Fld_QtyInStock;
            sqlParams[20].SourceColumn = clsPOSDBConstants.Item_Fld_Location;
            sqlParams[21].SourceColumn = clsPOSDBConstants.Item_Fld_MinOrdQty;
            sqlParams[22].SourceColumn = clsPOSDBConstants.Item_Fld_ReOrderLevel;
            sqlParams[23].SourceColumn = clsPOSDBConstants.Item_Fld_QtyOnOrder;
            sqlParams[24].SourceColumn = clsPOSDBConstants.Item_Fld_ExptDeliveryDate;
            sqlParams[25].SourceColumn = clsPOSDBConstants.Item_Fld_LastVendor;
            sqlParams[26].SourceColumn = clsPOSDBConstants.Item_Fld_LastRecievDate;
            sqlParams[27].SourceColumn = clsPOSDBConstants.Item_Fld_LastSellingDate;
            sqlParams[28].SourceColumn = clsPOSDBConstants.Item_Fld_Remarks;
            sqlParams[29].SourceColumn = clsPOSDBConstants.Item_Fld_isOnSale;
            sqlParams[30].SourceColumn = clsPOSDBConstants.Item_Fld_ExclFromAutoPO;
            sqlParams[31].SourceColumn = clsPOSDBConstants.Item_Fld_ExclFromRecpt;
            sqlParams[32].SourceColumn = clsPOSDBConstants.Item_Fld_isOTCItem;
            sqlParams[33].SourceColumn = clsPOSDBConstants.Item_Fld_UpdatePrice;
            sqlParams[34].SourceColumn = clsPOSDBConstants.Item_Fld_PreferredVendor;
            sqlParams[35].SourceColumn = clsPOSDBConstants.Item_Fld_SubDepartmentID;

            sqlParams[36].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckSize;
            sqlParams[37].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckQty;
            sqlParams[38].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckUnit;
            sqlParams[39].SourceColumn = clsPOSDBConstants.Item_Fld_TaxPolicy;//Added By Shitaljit(QuicSolv) on 18 August
            //sqlParams[40].SourceColumn = clsPOSDBConstants.Item_Fld_ExpDate;//Added by Krishna on 5 October 2011
            //sqlParams[41].SourceColumn = clsPOSDBConstants.Item_Fld_LotNumber;//Added by Krishna on 5 October 2011
            sqlParams[40].SourceColumn = clsPOSDBConstants.Item_Fld_ManufacturerName;
            //Add by Ravindra for Sale Limit 22 MArch 2013
            sqlParams[41].SourceColumn = clsPOSDBConstants.Item_Fld_SaleLimitQty;
            sqlParams[42].SourceColumn = clsPOSDBConstants.Item_Fld_DiscountPolicy;
            sqlParams[43].SourceColumn = clsPOSDBConstants.Item_Fld_IsEBTItem;
            //Addedby shitaljit on 2/6/2014 for PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
            sqlParams[44].SourceColumn = clsPOSDBConstants.Item_Fld_IsDefaultCLPoint;
            sqlParams[45].SourceColumn = clsPOSDBConstants.Item_Fld_PointsPerDollar;
            //END
            sqlParams[46].SourceColumn = clsPOSDBConstants.Item_Fld_CLPointPolicy;  //Sprint-18 - 2041 28-Oct-2014 JY Added
            sqlParams[47].SourceColumn = clsPOSDBConstants.Item_Fld_ExpDate;    //Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
            sqlParams[48].SourceColumn = clsPOSDBConstants.Item_Fld_IsActive;   //Sprint-21 - 2173 06-Jul-2015 JY Added
            sqlParams[49].SourceColumn = clsPOSDBConstants.Item_Fld_IsNonRefundable;    //PRIMEPOS-2592 01-Nov-2018 JY Added 

            if (row.ItemID != System.String.Empty)
                sqlParams[0].Value = row.ItemID.Trim(); //PRIMEPOS-2582 05-Sep-2018 JY added trim
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.DepartmentID != 0)
                sqlParams[1].Value = row.DepartmentID;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Description != System.String.Empty)
                sqlParams[2].Value = row.Description;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.Itemtype != System.String.Empty)
                sqlParams[3].Value = row.Itemtype;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ProductCode != System.String.Empty)
                sqlParams[4].Value = row.ProductCode;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.SaleTypeCode != System.String.Empty)
                sqlParams[5].Value = row.SaleTypeCode;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.SeasonCode != System.String.Empty)
                sqlParams[6].Value = row.SeasonCode;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.Unit != System.String.Empty)
                sqlParams[7].Value = row.Unit;
            else
                sqlParams[7].Value = DBNull.Value;

            if (row.Freight != 0)
                sqlParams[8].Value = row.Freight;
            else
                sqlParams[8].Value = 0;

            if (row.SellingPrice != 0)
                sqlParams[9].Value = row.SellingPrice;
            else
                sqlParams[9].Value = 0;

            if (row.AvgPrice != 0)
                sqlParams[10].Value = row.AvgPrice;
            else
                sqlParams[10].Value = 0;

            if (row.LastCostPrice != 0)
                sqlParams[11].Value = row.LastCostPrice;
            else
                sqlParams[11].Value = 0;

            sqlParams[12].Value = row.isTaxable;

            if (row.TaxID != 0)
                sqlParams[13].Value = row.TaxID;
            else
                sqlParams[13].Value = DBNull.Value;

            sqlParams[14].Value = row.isDiscountable;

            if (row.Discount != 0)
                sqlParams[15].Value = row.Discount;
            else
                sqlParams[15].Value = 0;

            if (row.SaleStartDate != DBNull.Value)
                sqlParams[16].Value = row.SaleStartDate;
            else
                sqlParams[16].Value = DBNull.Value;

            if (row.SaleEndDate != DBNull.Value)
                sqlParams[17].Value = row.SaleEndDate;
            else
                sqlParams[17].Value = DBNull.Value;

            if (row.OnSalePrice != 0)
                sqlParams[18].Value = row.OnSalePrice;
            else
                sqlParams[18].Value = 0;

            if (row.QtyInStock != 0)
                sqlParams[19].Value = row.QtyInStock;
            else
                sqlParams[19].Value = 0;

            if (row.Location != System.String.Empty)
                sqlParams[20].Value = row.Location;
            else
                sqlParams[20].Value = DBNull.Value;

            if (row.MinOrdQty != 0)
                sqlParams[21].Value = row.MinOrdQty;
            else
                sqlParams[21].Value = 0;

            if (row.ReOrderLevel != 0)
                sqlParams[22].Value = row.ReOrderLevel;
            else
                sqlParams[22].Value = 0;

            if (row.QtyOnOrder != 0)
                sqlParams[23].Value = row.QtyOnOrder;
            else
                sqlParams[23].Value = 0;

            if (row.ExptDeliveryDate != null)
                sqlParams[24].Value = row.ExptDeliveryDate;
            else
                sqlParams[24].Value = DBNull.Value;

            if (row.LastVendor != System.String.Empty)
                sqlParams[25].Value = row.LastVendor;
            else
                sqlParams[25].Value = DBNull.Value;

            if (row.LastRecievDate != System.DateTime.MinValue)
                sqlParams[26].Value = row.LastRecievDate;
            else
                sqlParams[26].Value = DBNull.Value;

            if (row.LastSellingDate != System.DateTime.MinValue)
                sqlParams[27].Value = row.LastSellingDate;
            else
                sqlParams[27].Value = DBNull.Value;

            if (row.Remarks != System.String.Empty)
                sqlParams[28].Value = row.Remarks;
            else
                sqlParams[28].Value = DBNull.Value;

            sqlParams[29].Value = row.isOnSale;

            sqlParams[30].Value = row.ExclFromAutoPO;

            sqlParams[31].Value = row.ExclFromRecpt;
            sqlParams[32].Value = row.isOTCItem;
            sqlParams[33].Value = row.UpdatePrice;

            if (row.PreferredVendor != System.String.Empty)
                sqlParams[34].Value = row.PreferredVendor;
            else
                sqlParams[34].Value = DBNull.Value;

            if (row.SubDepartmentID != 0)
                sqlParams[35].Value = row.SubDepartmentID;
            else
                sqlParams[35].Value = DBNull.Value;

            if (row.PckSize != System.String.Empty)
                sqlParams[36].Value = row.PckSize;
            else
                sqlParams[36].Value = DBNull.Value;

            if (row.PckQty != System.String.Empty)
                sqlParams[37].Value = row.PckQty;
            else
                sqlParams[37].Value = DBNull.Value;

            if (row.PckUnit != System.String.Empty)
                sqlParams[38].Value = row.PckUnit;
            else
                sqlParams[38].Value = DBNull.Value;
            //Added By Shitaljit(QuicSolv) on 18 August
            if (row.TaxPolicy != System.String.Empty)
                sqlParams[39].Value = row.TaxPolicy;
            else
                sqlParams[39].Value = DBNull.Value;
            //End

            //Following Code Added by Krishna on 5 October 2011
            //if (row.ExpDate != null)
            //    sqlParams[40].Value = row.ExpDate;
            //else
            //    sqlParams[40].Value = DBNull.Value;

            //if (row.LotNumber != System.String.Empty)
            //    sqlParams[41].Value = row.LotNumber;
            //else
            //    sqlParams[41].Value = DBNull.Value;
            //Till here Added by Krishna on 5 October 2011
            sqlParams[40].Value = row.ManufacturerName;
            //Added by Ravindra for dale Limit 
            if (row.SaleLimitQty != 0)
                sqlParams[41].Value = row.SaleLimitQty;
            else
                sqlParams[41].Value = 0;
            //Added By Shitaljit(QuicSolv) on3 April 2013
            if (row.DiscountPolicy != System.String.Empty)
                sqlParams[42].Value = row.DiscountPolicy;
            else
                sqlParams[42].Value = DBNull.Value;
            //End
            sqlParams[43].Value = row.IsEBTItem;

            //Addedby shitaljit on 2/6/2014 for PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
            sqlParams[44].Value = row.IsDefaultCLPoint;
            if (row.PointsPerDollar != 0)
            {
                sqlParams[45].Value = row.PointsPerDollar;
            }
            else
            {
                sqlParams[45].Value = 0;
            }
            //END
            #region Sprint-18 - 2041 28-Oct-2014 JY Added
            if (row.CLPointPolicy != System.String.Empty)
                sqlParams[46].Value = row.CLPointPolicy;
            else
                sqlParams[46].Value = DBNull.Value;
            #endregion

            #region Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
            if (row.ExpDate != null)
                sqlParams[47].Value = row.ExpDate;
            else
                sqlParams[47].Value = DBNull.Value;
            #endregion

            sqlParams[48].Value = row.IsActive; //Sprint-21 - 2173 06-Jul-2015 JY Added
            sqlParams[49].Value = row.IsNonRefundable;  //PRIMEPOS-2592 01-Nov-2018 JY Added 

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(ItemRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(50);//Change by Shitaljit for sale Limit old Value 44   //Sprint-18 - 2041 28-Oct-2014 JY changed index to 47   //Sprint-21 - 2206,2173 03-Jul-2015 JY Changed from 47 to 49
            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ItemID, System.Data.SqlDbType.VarChar);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_DepartmentID, System.Data.SqlDbType.Int);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Description, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Itemtype, System.Data.SqlDbType.VarChar);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ProductCode, System.Data.SqlDbType.VarChar);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SaleTypeCode, System.Data.SqlDbType.VarChar);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SeasonCode, System.Data.SqlDbType.VarChar);
            sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Unit, System.Data.SqlDbType.VarChar);
            sqlParams[8] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Freight, System.Data.DbType.Currency);
            sqlParams[9] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SellingPrice, System.Data.DbType.Currency);
            sqlParams[10] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_AvgPrice, System.Data.DbType.Currency);
            sqlParams[11] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LastCostPrice, System.Data.DbType.Currency);
            sqlParams[12] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_isTaxable, System.Data.SqlDbType.Bit);
            sqlParams[13] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_TaxID, System.Data.SqlDbType.Int);
            sqlParams[14] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_isDiscountable, System.Data.SqlDbType.Bit);
            sqlParams[15] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Discount, System.Data.SqlDbType.Float);
            sqlParams[16] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SaleStartDate, System.Data.SqlDbType.DateTime);
            sqlParams[17] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SaleEndDate, System.Data.SqlDbType.DateTime);
            sqlParams[18] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_OnSalePrice, System.Data.DbType.Currency);
            sqlParams[19] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_QtyInStock, System.Data.SqlDbType.Int);
            sqlParams[20] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Location, System.Data.SqlDbType.VarChar);
            sqlParams[21] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_MinOrdQty, System.Data.SqlDbType.Int);
            sqlParams[22] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ReOrderLevel, System.Data.SqlDbType.Int);
            sqlParams[23] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_QtyOnOrder, System.Data.SqlDbType.Int);
            sqlParams[24] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExptDeliveryDate, System.Data.SqlDbType.DateTime);
            sqlParams[25] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LastVendor, System.Data.SqlDbType.VarChar);
            sqlParams[26] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LastRecievDate, System.Data.SqlDbType.DateTime);
            sqlParams[27] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LastSellingDate, System.Data.SqlDbType.DateTime);
            sqlParams[28] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_Remarks, System.Data.SqlDbType.VarChar);
            sqlParams[29] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_isOnSale, System.Data.SqlDbType.Bit);
            sqlParams[30] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExclFromAutoPO, System.Data.SqlDbType.Bit);
            sqlParams[31] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExclFromRecpt, System.Data.SqlDbType.Bit);
            sqlParams[32] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_isOTCItem, System.Data.SqlDbType.Bit);
            sqlParams[33] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_UpdatePrice, System.Data.SqlDbType.Bit);
            sqlParams[34] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_PreferredVendor, System.Data.SqlDbType.VarChar);
            sqlParams[35] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SubDepartmentID, System.Data.SqlDbType.Int);

            sqlParams[36] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckSize, System.Data.SqlDbType.VarChar);
            sqlParams[37] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckQty, System.Data.SqlDbType.VarChar);
            sqlParams[38] = new SqlParameter("@" + clsPOSDBConstants.ItemVendor_Fld_PckUnit, System.Data.SqlDbType.VarChar);
            sqlParams[39] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_TaxPolicy, System.Data.SqlDbType.Char);//Added By Shitaljit(QuicSolv) on 18 August
            //sqlParams[40] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExpDate, System.Data.SqlDbType.SmallDateTime);//Added by Krishna on 5 October 2011
            //sqlParams[41] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_LotNumber, System.Data.SqlDbType.VarChar);//Added by Krishna on 5 October 2011
            sqlParams[40] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ManufacturerName, System.Data.SqlDbType.VarChar);
            //Added by Ravindra For sale Limit 22 March 2013
            sqlParams[41] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_SaleLimitQty, System.Data.SqlDbType.Int);
            sqlParams[42] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_DiscountPolicy, System.Data.SqlDbType.Char);//Added By Shitaljit(QuicSolv) on 3 April 2013
            sqlParams[43] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_IsEBTItem, System.Data.SqlDbType.Bit);
            sqlParams[44] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_IsDefaultCLPoint, System.Data.SqlDbType.Bit);
            sqlParams[45] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_PointsPerDollar, System.Data.SqlDbType.Int);
            sqlParams[46] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_CLPointPolicy, System.Data.SqlDbType.Char);   //Sprint-18 - 2041 28-Oct-2014 JY Added
            sqlParams[47] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_ExpDate, System.Data.SqlDbType.SmallDateTime);    //Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
            sqlParams[48] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_IsActive, System.Data.SqlDbType.Bit); //Sprint-21 - 2173 06-Jul-2015 JY Added
            sqlParams[49] = new SqlParameter("@" + clsPOSDBConstants.Item_Fld_IsNonRefundable, System.Data.SqlDbType.Bit);  //PRIMEPOS-2592 01-Nov-2018 JY Added 

            sqlParams[0].SourceColumn = clsPOSDBConstants.Item_Fld_ItemID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Item_Fld_DepartmentID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Item_Fld_Description;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Item_Fld_Itemtype;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Item_Fld_ProductCode;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Item_Fld_SaleTypeCode;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Item_Fld_SeasonCode;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Item_Fld_Unit;
            sqlParams[8].SourceColumn = clsPOSDBConstants.Item_Fld_Freight;
            sqlParams[9].SourceColumn = clsPOSDBConstants.Item_Fld_SellingPrice;
            sqlParams[10].SourceColumn = clsPOSDBConstants.Item_Fld_AvgPrice;
            sqlParams[11].SourceColumn = clsPOSDBConstants.Item_Fld_LastCostPrice;
            sqlParams[12].SourceColumn = clsPOSDBConstants.Item_Fld_isTaxable;
            sqlParams[13].SourceColumn = clsPOSDBConstants.Item_Fld_TaxID;
            sqlParams[14].SourceColumn = clsPOSDBConstants.Item_Fld_isDiscountable;
            sqlParams[15].SourceColumn = clsPOSDBConstants.Item_Fld_Discount;
            sqlParams[16].SourceColumn = clsPOSDBConstants.Item_Fld_SaleStartDate;
            sqlParams[17].SourceColumn = clsPOSDBConstants.Item_Fld_SaleEndDate;
            sqlParams[18].SourceColumn = clsPOSDBConstants.Item_Fld_OnSalePrice;
            sqlParams[19].SourceColumn = clsPOSDBConstants.Item_Fld_QtyInStock;
            sqlParams[20].SourceColumn = clsPOSDBConstants.Item_Fld_Location;
            sqlParams[21].SourceColumn = clsPOSDBConstants.Item_Fld_MinOrdQty;
            sqlParams[22].SourceColumn = clsPOSDBConstants.Item_Fld_ReOrderLevel;
            sqlParams[23].SourceColumn = clsPOSDBConstants.Item_Fld_QtyOnOrder;
            sqlParams[24].SourceColumn = clsPOSDBConstants.Item_Fld_ExptDeliveryDate;
            sqlParams[25].SourceColumn = clsPOSDBConstants.Item_Fld_LastVendor;
            sqlParams[26].SourceColumn = clsPOSDBConstants.Item_Fld_LastRecievDate;
            sqlParams[27].SourceColumn = clsPOSDBConstants.Item_Fld_LastSellingDate;
            sqlParams[28].SourceColumn = clsPOSDBConstants.Item_Fld_Remarks;
            sqlParams[29].SourceColumn = clsPOSDBConstants.Item_Fld_isOnSale;
            sqlParams[30].SourceColumn = clsPOSDBConstants.Item_Fld_ExclFromAutoPO;
            sqlParams[31].SourceColumn = clsPOSDBConstants.Item_Fld_ExclFromRecpt;
            sqlParams[32].SourceColumn = clsPOSDBConstants.Item_Fld_isOTCItem;
            sqlParams[33].SourceColumn = clsPOSDBConstants.Item_Fld_UpdatePrice;
            sqlParams[34].SourceColumn = clsPOSDBConstants.Item_Fld_PreferredVendor;
            sqlParams[35].SourceColumn = clsPOSDBConstants.Item_Fld_SubDepartmentID;

            sqlParams[36].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckSize;
            sqlParams[37].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckQty;
            sqlParams[38].SourceColumn = clsPOSDBConstants.ItemVendor_Fld_PckUnit;
            sqlParams[39].SourceColumn = clsPOSDBConstants.Item_Fld_TaxPolicy;//Added By Shitaljit(QuicSolv) on 18 August
            //sqlParams[40].SourceColumn = clsPOSDBConstants.Item_Fld_ExpDate;//Added by Krishna on 5 October 2011
            //sqlParams[41].SourceColumn = clsPOSDBConstants.Item_Fld_LotNumber;//Added by Krishna on 5 October 2011
            sqlParams[40].SourceColumn = clsPOSDBConstants.Item_Fld_ManufacturerName;

            sqlParams[41].SourceColumn = clsPOSDBConstants.Item_Fld_SaleLimitQty;//Added by Ravindra for Sale limit 22 March 2013
            sqlParams[42].SourceColumn = clsPOSDBConstants.Item_Fld_DiscountPolicy;
            sqlParams[43].SourceColumn = clsPOSDBConstants.Item_Fld_IsEBTItem;
            sqlParams[44].SourceColumn = clsPOSDBConstants.Item_Fld_IsDefaultCLPoint;

            sqlParams[45].SourceColumn = clsPOSDBConstants.Item_Fld_PointsPerDollar;
            sqlParams[46].SourceColumn = clsPOSDBConstants.Item_Fld_CLPointPolicy;  //Sprint-18 - 2041 28-Oct-2014 JY Added
            sqlParams[47].SourceColumn = clsPOSDBConstants.Item_Fld_ExpDate;    //Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
            sqlParams[48].SourceColumn = clsPOSDBConstants.Item_Fld_IsActive;   //Sprint-21 - 2173 06-Jul-2015 JY Added
            sqlParams[49].SourceColumn = clsPOSDBConstants.Item_Fld_IsNonRefundable;    //PRIMEPOS-2592 01-Nov-2018 JY Added 

            if (row.ItemID != System.String.Empty)
                sqlParams[0].Value = row.ItemID.Trim(); //PRIMEPOS-2582 05-Sep-2018 JY added trim
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.DepartmentID != 0)
                sqlParams[1].Value = row.DepartmentID;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Description != System.String.Empty)
                sqlParams[2].Value = row.Description;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.Itemtype != System.String.Empty)
                sqlParams[3].Value = row.Itemtype;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ProductCode != System.String.Empty)
                sqlParams[4].Value = row.ProductCode;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.SaleTypeCode != System.String.Empty)
                sqlParams[5].Value = row.SaleTypeCode;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.SeasonCode != System.String.Empty)
                sqlParams[6].Value = row.SeasonCode;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.Unit != System.String.Empty)
                sqlParams[7].Value = row.Unit;
            else
                sqlParams[7].Value = DBNull.Value;

            if (row.Freight != 0)
                sqlParams[8].Value = row.Freight;
            else
                sqlParams[8].Value = 0;

            if (row.SellingPrice != 0)
                sqlParams[9].Value = row.SellingPrice;
            else
                sqlParams[9].Value = 0;

            if (row.AvgPrice != 0)
                sqlParams[10].Value = row.AvgPrice;
            else
                sqlParams[10].Value = 0;

            if (row.LastCostPrice != 0)
                sqlParams[11].Value = row.LastCostPrice;
            else
                sqlParams[11].Value = 0;

            sqlParams[12].Value = row.isTaxable;

            if (row.TaxID != 0)
                sqlParams[13].Value = row.TaxID;
            else
                sqlParams[13].Value = DBNull.Value;

            sqlParams[14].Value = row.isDiscountable;

            if (row.Discount != 0)
                sqlParams[15].Value = row.Discount;
            else
                sqlParams[15].Value = 0;

            if (row.SaleStartDate != DBNull.Value)
                sqlParams[16].Value = row.SaleStartDate;
            else
                sqlParams[16].Value = DBNull.Value;

            if (row.SaleEndDate != DBNull.Value)
                sqlParams[17].Value = row.SaleEndDate;
            else
                sqlParams[17].Value = DBNull.Value;

            if (row.OnSalePrice != 0)
                sqlParams[18].Value = row.OnSalePrice;
            else
                sqlParams[18].Value = 0;

            if (row.QtyInStock != 0)
                sqlParams[19].Value = row.QtyInStock;
            else
                sqlParams[19].Value = 0;

            if (row.Location != System.String.Empty)
                sqlParams[20].Value = row.Location;
            else
                sqlParams[20].Value = DBNull.Value;

            if (row.MinOrdQty != 0)
                sqlParams[21].Value = row.MinOrdQty;
            else
                sqlParams[21].Value = 0;

            if (row.ReOrderLevel != 0)
                sqlParams[22].Value = row.ReOrderLevel;
            else
                sqlParams[22].Value = 0;

            if (row.QtyOnOrder != 0)
                sqlParams[23].Value = row.QtyOnOrder;
            else
                sqlParams[23].Value = 0;

            if (row.ExptDeliveryDate != null)
                sqlParams[24].Value = row.ExptDeliveryDate;
            else
                sqlParams[24].Value = DBNull.Value;

            if (row.LastVendor != System.String.Empty)
                sqlParams[25].Value = row.LastVendor;
            else
                sqlParams[25].Value = DBNull.Value;

            if (row.LastRecievDate != System.DateTime.MinValue)
                sqlParams[26].Value = row.LastRecievDate;
            else
                sqlParams[26].Value = DBNull.Value;

            if (row.LastSellingDate != System.DateTime.MinValue)
                sqlParams[27].Value = row.LastSellingDate;
            else
                sqlParams[27].Value = DBNull.Value;

            if (row.Remarks != System.String.Empty)
                sqlParams[28].Value = row.Remarks;
            else
                sqlParams[28].Value = DBNull.Value;

            sqlParams[29].Value = row.isOnSale;
            sqlParams[30].Value = row.ExclFromAutoPO;
            sqlParams[31].Value = row.ExclFromRecpt;
            sqlParams[32].Value = row.isOTCItem;
            sqlParams[33].Value = row.UpdatePrice;
            if (row.PreferredVendor != System.String.Empty)
                sqlParams[34].Value = row.PreferredVendor;
            else
                sqlParams[34].Value = DBNull.Value;

            if (row.SubDepartmentID != 0)
                sqlParams[35].Value = row.SubDepartmentID;
            else
                sqlParams[35].Value = DBNull.Value;

            if (row.PckSize != System.String.Empty)
                sqlParams[36].Value = row.PckSize;
            else
                sqlParams[36].Value = DBNull.Value;

            if (row.PckQty != System.String.Empty)
                sqlParams[37].Value = row.PckQty;
            else
                sqlParams[37].Value = DBNull.Value;

            if (row.PckUnit != System.String.Empty)
                sqlParams[38].Value = row.PckUnit;
            else
                sqlParams[38].Value = DBNull.Value;
            //Added By Shitaljit(QuicSolv) on 18 August
            if (row.TaxPolicy != System.String.Empty)
                sqlParams[39].Value = row.TaxPolicy;
            else
                sqlParams[39].Value = DBNull.Value;
            //End

            //Following Code Added by Krishna on 5 October 2011
            //if (row.ExpDate !=null)
            //    sqlParams[40].Value = row.ExpDate;
            //else
            //    sqlParams[40].Value = DBNull.Value;

            //if (row.LotNumber != System.String.Empty)
            //    sqlParams[41].Value = row.LotNumber;
            //else
            //    sqlParams[41].Value = DBNull.Value;
            //Till here Added by Krishna on 5 October 2011
            sqlParams[40].Value = row.ManufacturerName;

            //Added by Ravindra for Sale limit 22 March 2013
            if (row.SaleLimitQty != 0)
                sqlParams[41].Value = row.SaleLimitQty;
            else
                sqlParams[41].Value = 0;
            //Added By Shitaljit(QuicSolv) on3 April 2013
            if (row.DiscountPolicy != System.String.Empty)
                sqlParams[42].Value = row.DiscountPolicy;
            else
                sqlParams[42].Value = DBNull.Value;
            //End
            sqlParams[43].Value = row.IsEBTItem;
            //Addedby shitaljit on 2/6/2014 for PRIMEPOS-1806 Seperate Rx and OTC point calculation in CL
            sqlParams[44].Value = row.IsDefaultCLPoint;

            if (row.PointsPerDollar != 0)
            {
                sqlParams[45].Value = row.PointsPerDollar;
            }
            else
            {
                sqlParams[45].Value = 0;
            }
            //END

            #region Sprint-18 - 2041 28-Oct-2014 JY Added
            if (row.CLPointPolicy != System.String.Empty)
                sqlParams[46].Value = row.CLPointPolicy;
            else
                sqlParams[46].Value = DBNull.Value;
            #endregion

            #region //Sprint-21 - 2206 03-Jul-2015 JY Added code for item exp. date
            if (row.ExpDate != null)
                sqlParams[47].Value = row.ExpDate;
            else
                sqlParams[47].Value = DBNull.Value;
            #endregion
            sqlParams[48].Value = row.IsActive;    //Sprint-21 - 2173 06-Jul-2015 JY Added
            sqlParams[49].Value = row.IsNonRefundable;  //PRIMEPOS-2592 01-Nov-2018 JY Added 

            return (sqlParams);
        }
        #endregion
        //In following Func. SellingPrice Parameter Added by Krishna on 6 June 2011
        //Modified By Amit Chenged Datatype of LastVendor to Stirng
        #region stock update
        public void AddStock(System.String ItemID, System.Int32 Qty, DateTime RecieveDate, string LastVendor, System.Decimal CostPrice, System.Decimal SellingPrice, object ExpDate, int DeptID, int SubDepartmentID, bool IsEBTItem, IDbTransaction tx, bool isFromPurchaseOrder)    //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added ExpDate
        {
            try
            {
                VendorRow oVendorRow = null;
                POS_Core.DataAccess.ItemSvr oItem = new ItemSvr();
                ItemData oData = oItem.Populate(ItemID);
                POS_Core.BusinessRules.Vendor oVend = new POS_Core.BusinessRules.Vendor();
                VendorData oVData = oVend.Populate(LastVendor);
                if (oVData.Tables[0].Rows.Count > 0)
                {
                    VendorTable oVendorTable = (VendorTable)oVData.Tables[clsPOSDBConstants.Vendor_tbl];
                    oVendorRow = (VendorRow)oVendorTable.Rows[0];
                }
                if (oData.Item.Rows.Count == 0) throw (new Exception("Unable to find item " + ItemID + " for stock update."));

                //Added by SRT(Abhishek) Date : 02/02/2009
                if ((oData.Item[0].PckUnit.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CS) || (oData.Item[0].PckUnit.ToString().ToUpper().Trim() == clsPOSDBConstants.PckUnit_CA)) //Sprint-21 22-Feb-2016 JY Added CA for case item
                {
                    Qty = Qty * MMSUtil.UtilFunc.ValorZeroI(oData.Item[0].PckSize.ToString());
                    oData.Item[0].QtyInStock = oData.Item[0].QtyInStock + Qty;
                }
                else
                {
                    oData.Item[0].QtyInStock = oData.Item[0].QtyInStock + Qty;
                }
                //End Of Added by SRT(Abhishek)

                //oVendorRow
                oData.Item[0].LastRecievDate = RecieveDate;
                oData.Item[0].LastVendor = LastVendor;//Convert.ToString( LastVendor);
                bool itemUpdatePrice = false;
                if (!Boolean.TryParse(oData.Item[0].UpdatePrice.ToString(), out itemUpdatePrice))
                {
                    oData.Item[0].UpdatePrice = false;
                }
                if (oData.Item[0].UpdatePrice)
                {
                    if (isFromPurchaseOrder && (oVendorRow.AckPriceUpdate || oVendorRow.Process810)) // NileshJ - Change passing paratmeter from FORM to bool isFromPurchaseOrder for POS_Core - [Need to Changes in - ItemSvr.cs,InvRecived.cs,InvRecvDetailsSvr.cs]   //PRIMEPOS-2951 14-Apr-2021 JY Added || oVendorRow.Process810
                    {
                        oData.Item[0].LastCostPrice = CostPrice;
                    }
                    if (!isFromPurchaseOrder)
                    {
                        if (oData.Item[0].LastCostPrice != CostPrice && CostPrice != 0)
                            oData.Item[0].LastCostPrice = CostPrice;
                        if (oData.Item[0].SellingPrice != SellingPrice && SellingPrice != 0)
                            oData.Item[0].SellingPrice = SellingPrice;
                    }
                }

                try
                {
                    if (ExpDate != null && ExpDate.ToString() != string.Empty)
                        oData.Item[0].ExpDate = ExpDate;    //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added        
                }
                catch
                {
                }

                #region PRIMEPOS-2419 28-May-2019 JY Added
                if (Configuration.convertNullToInt(DeptID) > 0)
                {
                    oData.Item[0].DepartmentID = DeptID;
                    oData.Item[0].SubDepartmentID = Configuration.convertNullToInt(SubDepartmentID);
                }
                else
                {
                    oData.Item[0].DepartmentID = 0;
                    oData.Item[0].SubDepartmentID = 0;
                }
                oData.Item[0].IsEBTItem = Configuration.convertNullToBoolean(IsEBTItem);
                #endregion
                oItem.Persist(oData, tx);
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
        public void LessStock(System.String ItemID, System.Int32 Qty, IDbTransaction tx)
        {
            try
            {
                POS_Core.DataAccess.ItemSvr oItem = new ItemSvr();
                ItemData oData = oItem.Populate(ItemID, tx);
                if (oData.Item.Rows.Count == 0) throw (new Exception("Unable to find item " + ItemID + " for stock update."));
                oData.Item[0].QtyInStock = oData.Item[0].QtyInStock - Qty;
                oData.Item[0].LastSellingDate = System.DateTime.Now;
                oItem.Persist(oData, tx);

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
        #endregion

        #region Item Price update
        public void AddItemPriceHistory(System.String sItemID, System.Decimal SalePrice, System.Decimal CostPrice, string sUserID, string sChangedIn, string sUpdatedBy, System.Int32 TrnasID, System.Decimal PromotionalPrice, System.Boolean IsPriceChangedByOverride)  //Sprint-25 - PRIMEPOS-294 04-May-2017 JY Added additional column for logging //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added IsPriceChangedByOverride     
        {
            IDbTransaction oTrans = null;
            try
            {
                oTrans = DataHelper.CreateTransaction();
                AddItemPriceHistory(sItemID, SalePrice, CostPrice, sUserID, sChangedIn, sUpdatedBy, oTrans, TrnasID, PromotionalPrice, IsPriceChangedByOverride); //Sprint-25 - PRIMEPOS-294 04-May-2017 JY Added additional column for logging
                oTrans.Commit();
            }
            catch
            {
                if (oTrans != null)
                {
                    oTrans.Rollback();
                }
            }
            finally
            {
                if (oTrans != null)
                {
                    oTrans.Connection.Close();
                    oTrans.Dispose();
                }
            }
        }
        public void AddItemPriceHistory(System.String sItemID, System.Decimal SalePrice, System.Decimal CostPrice, string sUserID, string sChangedIn, string sUpdatedBy, IDbTransaction tx, System.Int32 TransID, System.Decimal PromotionalPrice, System.Boolean IsPriceChangedByOverride, string OverrideRemark = "")  //Sprint-25 - PRIMEPOS-294 03-May-2017 JY Added additional column for logging  //Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added IsPriceChangedByOverride   //PRIMEPOS-3015 26-Oct-2021 JY Added OverrideRemark
        {
            try
            {
                POS_Core.DataAccess.ItemSvr oItem = new ItemSvr();
                ItemData oData = oItem.Populate(sItemID, tx);
                bool bAddToLog = false;

                if (oData.Item.Rows.Count > 0)
                {
                    if (oData.Item[0].SellingPrice != SalePrice || oData.Item[0].LastCostPrice != CostPrice || (TransID.ToString() == "0" && oData.Item[0].OnSalePrice != PromotionalPrice))
                    {
                        bAddToLog = true;
                    }
                }
                else
                {
                    bAddToLog = true;
                }

                oData.Dispose();

                if (bAddToLog)
                {
                    string strSQL = string.Empty;
                    IDbDataParameter[] sqlParams = null;
                    sqlParams = DataFactory.CreateParameterArray(13);   //PRIMEPOS-3015 26-Oct-2021 JY 12 to 13
                    strSQL = "Insert into ItemPriceHistory (ItemID,UserID,SalePrice,CostPrice,AddedOn,ChangedIn,UpdatedBy,TransID,OrgSellingPrice, OrgCostPrice, PromotionalPrice, OrgPromotionalPrice, IsPriceChangedByOverride, Remarks)" +
                            "values (@ItemID,@UserID,@SalePrice,@CostPrice,getdate(),@ChangedIn,@UpdatedBy,@TransID,@OrgSellingPrice, @OrgCostPrice, @PromotionalPrice, @OrgPromotionalPrice, @IsPriceChangedByOverride, @Remarks)";

                    sqlParams[0] = DataFactory.CreateParameter();
                    sqlParams[0].ParameterName = "@ItemId";
                    sqlParams[0].DbType = System.Data.DbType.String;
                    sqlParams[0].Value = sItemID;

                    sqlParams[1] = DataFactory.CreateParameter();
                    sqlParams[1].ParameterName = "@UserID";
                    sqlParams[1].DbType = System.Data.DbType.String;
                    if (string.IsNullOrEmpty(sUserID) == false)
                    {
                        sqlParams[1].Value = sUserID;
                    }
                    else
                    {
                        sqlParams[1].Value = Configuration.UserName;
                    }

                    sqlParams[2] = DataFactory.CreateParameter();
                    sqlParams[2].ParameterName = "@SalePrice";
                    sqlParams[2].DbType = System.Data.DbType.Decimal;
                    sqlParams[2].Value = SalePrice;

                    sqlParams[3] = DataFactory.CreateParameter();
                    sqlParams[3].ParameterName = "@CostPrice";
                    sqlParams[3].DbType = System.Data.DbType.Decimal;
                    sqlParams[3].Value = CostPrice;

                    sqlParams[4] = DataFactory.CreateParameter();
                    sqlParams[4].ParameterName = "@ChangedIn";
                    sqlParams[4].DbType = System.Data.DbType.String;
                    sqlParams[4].Value = sChangedIn;

                    sqlParams[5] = DataFactory.CreateParameter();
                    sqlParams[5].ParameterName = "@UpdatedBy";
                    sqlParams[5].DbType = System.Data.DbType.String;
                    sqlParams[5].Value = sUpdatedBy;

                    sqlParams[6] = DataFactory.CreateParameter();
                    sqlParams[6].ParameterName = "@OrgSellingPrice";
                    sqlParams[6].DbType = System.Data.DbType.Decimal;
                    //Foolowing Code Added by Krishna on 6 October 2011
                    if (TransDetailSvr.strFormatedRXItemID == sItemID)
                    {
                        sqlParams[6].Value = CostPrice;
                    }
                    else
                    //Till here Added by Krishna on 6 October 2011
                    {
                        if ((oData != null) && (oData.Item.Rows.Count > 0))
                            sqlParams[6].Value = oData.Item[0].SellingPrice.ToString();
                        else
                            sqlParams[6].Value = SalePrice;
                    }

                    sqlParams[7] = DataFactory.CreateParameter();
                    sqlParams[7].ParameterName = "@TransID";
                    sqlParams[7].DbType = System.Data.DbType.String;
                    if (TransID.ToString() != "0")
                    {
                        sqlParams[7].Value = TransID;
                    }
                    else
                    {
                        sqlParams[7].Value = DBNull.Value;
                    }

                    #region Sprint-25 - PRIMEPOS-294 03-May-2017 JY Added additional column for logging
                    //, @OrgCostPrice, @PromotionalPrice, @OrgPromotionalPrice
                    sqlParams[8] = DataFactory.CreateParameter();
                    sqlParams[8].ParameterName = "@OrgCostPrice";
                    sqlParams[8].DbType = System.Data.DbType.Decimal;
                    if ((oData != null) && (oData.Item.Rows.Count > 0))
                        sqlParams[8].Value = oData.Item[0].LastCostPrice.ToString();
                    else
                        sqlParams[8].Value = CostPrice;

                    if (TransID.ToString() == "0")
                    {
                        sqlParams[9] = DataFactory.CreateParameter();
                        sqlParams[9].ParameterName = "@PromotionalPrice";
                        sqlParams[9].DbType = System.Data.DbType.Decimal;
                        sqlParams[9].Value = PromotionalPrice;
                    }
                    else
                    {
                        sqlParams[9] = DataFactory.CreateParameter();
                        sqlParams[9].ParameterName = "@PromotionalPrice";
                        sqlParams[9].DbType = System.Data.DbType.Decimal;
                        if ((oData != null) && (oData.Item.Rows.Count > 0))
                            sqlParams[9].Value = oData.Item[0].OnSalePrice.ToString();
                        else
                            sqlParams[9].Value = PromotionalPrice;
                    }

                    sqlParams[10] = DataFactory.CreateParameter();
                    sqlParams[10].ParameterName = "@OrgPromotionalPrice";
                    sqlParams[10].DbType = System.Data.DbType.Decimal;
                    if ((oData != null) && (oData.Item.Rows.Count > 0))
                        sqlParams[10].Value = oData.Item[0].OnSalePrice.ToString();
                    else
                        sqlParams[10].Value = PromotionalPrice;
                    #endregion

                    #region Sprint-26 - PRIMEPOS-2294 27-Jul-2017 JY Added
                    sqlParams[11] = DataFactory.CreateParameter();
                    sqlParams[11].ParameterName = "@IsPriceChangedByOverride";
                    sqlParams[11].DbType = System.Data.DbType.Boolean;
                    sqlParams[11].Value = IsPriceChangedByOverride;
                    #endregion

                    #region PRIMEPOS-3015 26-Oct-2021 JY Added
                    sqlParams[12] = DataFactory.CreateParameter();
                    sqlParams[12].ParameterName = "@Remarks";
                    sqlParams[12].DbType = System.Data.DbType.String;
                    sqlParams[12].Value = OverrideRemark;
                    #endregion

                    DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL, sqlParams);
                }
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
        #endregion

        public void Dispose() { }

        public DataTable GetRecordFromItemPriceHistory(System.String ItemID, System.String ChangedIn)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection()) //Sprint-22 05-Nov-2015 JY Added using clause
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = " Select * From ItemPriceHistory " +
                    " Where ItemID='" + ItemID + "' and changedIn='" + ChangedIn + "'";

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds.Tables[0];
                }
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

        //Sprint-25 - PRIMEPOS-2380 Added logic to check Item exists in PSE_Items table
        public DataTable IsPSEItemData(string sItemCode)
        {
            DataSet ds = new DataSet();
            IDbConnection conn = null;
            try
            {
                //if itemcode is of 12 digit means with check digit then remove last check digit
                if (sItemCode.Length == 12)
                {
                    sItemCode = sItemCode.Substring(0, 11);
                }
                conn = DataFactory.CreateConnection();
                conn.ConnectionString = DBConfig.ConnectionString;
                string sSQL = "SELECT ISNULL(a.ProductGrams,0.00) AS ProductGrams, ISNULL(a.ProductPillCnt,0) AS ProductPillCnt, b.Description FROM PSE_Items a INNER JOIN Item b ON SUBSTRING(a.ProductID,1,11) = SUBSTRING(b.ItemID,1,11) WHERE SUBSTRING(a.ProductID,1,11) = '" + sItemCode.Replace("'", "''") + "' and a.isActive=1 ORDER BY a.CreatedOn DESC";

                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                conn.Close();
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
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
                conn = null;
            }
            return ds.Tables[0];
        }

        #region
        public DataTable GetBlankDescItems(System.Int32 OrderID)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    //PRIMEPO-188 14-Jan-2021 JY Commented
                    //string sSQL = "SELECT a.ItemID, a.Description, d.Qty, d.Cost, c.VendorCode, b.VendorItemID, e.VendorID AS POVendorId, d.OrderID FROM Item a " +
                    //        " LEFT JOIN ItemVendor b ON a.ItemID = b.ItemID " +
                    //        " LEFT JOIN Vendor c ON b.VendorID = c.VendorID " +
                    //        " INNER JOIN PurchaseOrderDetail d ON a.ItemID = d.ItemID " +
                    //        " INNER JOIN PurchaseOrder e ON d.OrderID = e.OrderID " +
                    //        " WHERE d.OrderID = " + OrderID + " AND ISNULL(a.Description, '') = '' " +
                    //        " UNION " +
                    //        "SELECT a.ItemID, a.Description, d.Qty, d.Cost, c.VendorCode, b.VendorItemID, e.VendorID AS POVendorId, d.OrderID FROM Item a " +
                    //        " LEFT JOIN ItemVendor b ON a.ItemID = b.ItemID " +
                    //        " LEFT JOIN Vendor c ON b.VendorID = c.VendorID " +
                    //        " INNER JOIN PurchaseOrderDetail d ON a.ItemID = d.ItemID " +
                    //        " INNER JOIN PurchaseOrder e ON d.OrderID = e.OrderID " +
                    //        " WHERE d.OrderID = " + OrderID + " AND b.VendorID IS NULL";

                    //PRIMEPO-188 14-Jan-2021 JY Added
                    string sSQL = "SELECT a.ItemID, a.Description, d.Qty, d.Cost, c.VendorCode, b.VendorItemID, e.VendorID AS POVendorId, d.OrderID, 'Item description missing' AS Remark FROM PurchaseOrder e" +
                                " INNER JOIN PurchaseOrderDetail d ON d.OrderID = e.OrderID" +
                                " INNER JOIN Item a ON a.ItemID = d.ItemID" +
                                " LEFT JOIN ItemVendor b ON a.ItemID = b.ItemID AND b.VendorID = e.VendorID" +
                                " LEFT JOIN Vendor c ON b.VendorID = c.VendorID" +
                                " WHERE d.OrderID = " + OrderID + " AND ISNULL(a.Description, '') = '' AND d.AckStatus <> 'IS'" +
                                " UNION" +
                                " SELECT d.CHANGEDPRODUCTID AS ItemID, a.Description, d.Qty, d.Cost, c.VendorCode, b.VendorItemID, e.VendorID AS POVendorId, d.OrderID, 'Item not found' AS Remark FROM PurchaseOrder e" +
                                " INNER JOIN PurchaseOrderDetail d ON d.OrderID = e.OrderID" +
                                " LEFT JOIN Item a ON a.ItemID = d.CHANGEDPRODUCTID" +
                                " LEFT JOIN ItemVendor b ON a.ItemID = b.ItemID AND b.VendorID = e.VendorID" +
                                " LEFT JOIN Vendor c ON b.VendorID = c.VendorID" +
                                " WHERE d.OrderID = " + OrderID + " AND (ISNULL(a.Description, '') = '' OR a.ItemID IS NULL) AND d.AckStatus = 'IS'";

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds.Tables[0];
                }
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
        #endregion

        #region PRIMEPOS-2395 22-Jun-2018 JY Added logic to get item details with ItemVendorId and LastInvUpdatedQty
        public DataTable GetItemDetails(System.String ItemId)
        {
            DataTable dtItem = new DataTable();
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;
                    string strSQL = "SELECT I.ItemID, I.ProductCode, I.Description, I.QtyInStock, I.SellingPrice, I.LastCostPrice, I.ExpDate, LIU.LastInvUpdatedQty FROM Item I " +
                                    " LEFT JOIN (SELECT RowNum, ItemID, LastInvUpdatedQty FROM " +
                                                    " (SELECT ROW_NUMBER() OVER(PARTITION BY ItemID ORDER BY TransDate DESC) AS RowNum, ItemID, LastInvUpdatedQty, TransDate FROM " +
                                                                " (SELECT PI.ItemCode AS ItemId, PI.NewQty AS LastInvUpdatedQty, PI.PTransDate AS TransDate FROM PhysicalInv PI " +
                                                                " WHERE PI.isProcessed = 1 " +
                                                                " UNION ALL " +
                                                                " SELECT IRD.ItemID, IRD.Qty AS LastInvUpdatedQty, IR.RecieveDate AS TransDate FROM InventoryRecieved IR " +
                                                                " INNER JOIN InvRecievedDetail IRD  ON IR.InvRecievedID = IRD.InvRecievedID " +
                                                                " INNER JOIN InvTransType ITT ON ITT.ID = IR.InvTransTypeID " +
                                                                " WHERE ITT.TransType = 0) X " +
                                                    " ) Y WHERE Y.RowNum = 1) LIU ON LIU.ItemId = I.ItemID ";

                    ItemData ds = new ItemData();
                    if (ItemId.Length == 11)
                    {
                        strSQL = strSQL + " WHERE I." + clsPOSDBConstants.Item_Fld_ItemID.Trim() + " LIKE '" + ItemId.Trim().Replace("'", "''") + "%'";
                        dtItem = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL);
                    }
                    else
                    {
                        strSQL = strSQL + " WHERE I." + clsPOSDBConstants.Item_Fld_ItemID.Trim() + " = '" + ItemId.Trim().Replace("'", "''") + "'";
                        dtItem = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL);
                        if (ItemId.Trim() != "" && (dtItem == null && dtItem.Rows.Count == 0)) //02-Sep-2015 JY Added condition "ItemId.Trim() != """ to resolve the issue on inventory received screen, also handeled the same issue on inventory received screen 
                        {
                            strSQL = strSQL + " WHERE I." + clsPOSDBConstants.Item_Fld_ProductCode.Trim() + " = '" + ItemId.Trim().Replace("'", "''") + "'";
                            dtItem = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL);
                        }
                    }
                    return dtItem;
                }
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

        public DataTable GetItemDetailsWithVendor(System.String ItemId, System.String vendorCode)
        {
            DataTable dtItem = new DataTable();
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    //PRIMEPOS-2419 09-May-2019 JY Added DeptName, SubDept, IsEBT
                    conn.ConnectionString = DBConfig.ConnectionString;
                    string strSQL = "SELECT I.ItemID, I.ProductCode, I.Description, D.DeptName, SD.Description AS SubDept, I.QtyInStock, I.SellingPrice, I.LastCostPrice, I.ExpDate, LIU.LastInvUpdatedQty, IV.VendorItemID, I.IsEBTItem, D.DeptID, D.DeptCode, I.SUBDEPARTMENTID FROM Item I " +
                                    " LEFT JOIN (SELECT RowNum, ItemID, LastInvUpdatedQty FROM " +
                                                    " (SELECT ROW_NUMBER() OVER(PARTITION BY ItemID ORDER BY TransDate DESC) AS RowNum, ItemID, LastInvUpdatedQty, TransDate FROM " +
                                                                " (SELECT PI.ItemCode AS ItemId, PI.NewQty AS LastInvUpdatedQty, PI.PTransDate AS TransDate FROM PhysicalInv PI " +
                                                                " WHERE PI.isProcessed = 1 " +
                                                                " UNION ALL " +
                                                                " SELECT IRD.ItemID, IRD.Qty AS LastInvUpdatedQty, IR.RecieveDate AS TransDate FROM InventoryRecieved IR " +
                                                                " INNER JOIN InvRecievedDetail IRD  ON IR.InvRecievedID = IRD.InvRecievedID " +
                                                                " INNER JOIN InvTransType ITT ON ITT.ID = IR.InvTransTypeID " +
                                                                " WHERE ITT.TransType = 0) X " +
                                                    " ) Y WHERE Y.RowNum = 1) LIU ON LIU.ItemId = I.ItemID " +
                                    " LEFT JOIN (SELECT RowNum, ItemId, VendorItemID FROM " +
                                                    " (SELECT ROW_NUMBER() OVER(PARTITION BY a.ItemId ORDER BY a.ItemDetailID DESC) AS RowNum, a.ItemId, a.VendorItemID FROM ItemVendor a " +
                                                    " INNER JOIN Vendor b ON a.VendorID = b.VendorID AND b.VendorCode = '" + vendorCode.Trim().Replace("'", "''") + "' " +
                                                    " ) X WHERE RowNum = 1 " +
                                                " ) IV ON IV.ItemID = I.ItemID " +
                                    " LEFT JOIN Department D ON d.DeptID = I.DepartmentID " +
                                    " LEFT JOIN SubDepartment SD ON SD.SubDepartmentID = I.SUBDEPARTMENTID";

                    ItemData ds = new ItemData();
                    if (ItemId.Length == 11)
                    {
                        strSQL = strSQL + " WHERE I." + clsPOSDBConstants.Item_Fld_ItemID.Trim() + " LIKE '" + ItemId.Trim().Replace("'", "''") + "%'";
                        dtItem = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL);
                    }
                    else
                    {
                        strSQL = strSQL + " WHERE I." + clsPOSDBConstants.Item_Fld_ItemID.Trim() + " = '" + ItemId.Trim().Replace("'", "''") + "'";
                        dtItem = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL);
                        if (ItemId.Trim() != "" && (dtItem == null && dtItem.Rows.Count == 0)) //02-Sep-2015 JY Added condition "ItemId.Trim() != """ to resolve the issue on inventory received screen, also handeled the same issue on inventory received screen 
                        {
                            strSQL = strSQL + " WHERE I." + clsPOSDBConstants.Item_Fld_ProductCode.Trim() + " = '" + ItemId.Trim().Replace("'", "''") + "'";
                            dtItem = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL);
                        }
                    }
                    return dtItem;
                }
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
        #endregion

        #region PRIMEPOS-2774 14-Jan-2020 JY Added
        public DataTable GetItemDetailsForS3()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = "SELECT I.ItemID, I.Description, I.DepartmentID, D.DeptName FROM Item I " +
                                " LEFT JOIN Department D ON I.DepartmentID = D.DeptID WHERE I.IsActive = 1 ORDER BY I.ItemID";

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds.Tables[0];
                }
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
        #endregion

        #region PRIMEPOS-1633 31-Dec-2020 JY Added
        public void UpdateItemTax(string strItemIds, string strTaxIds)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;
                    conn.Open();
                    IDbTransaction tx = conn.BeginTransaction();
                    try
                    {
                        if (strTaxIds != "")
                        {
                            string strSQL = "DELETE FROM ItemTax WHERE EntityType = 'I' AND EntityID IN (" + strItemIds + ")";
                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL);

                            string[] arrItemIds = strItemIds.Split(',');
                            strSQL = "";
                            string[] arrTaxIds = strTaxIds.Split(',');
                            for (int i = 0; i < arrItemIds.Length; i++)
                            {
                                if (arrTaxIds.Length > 0)
                                {
                                    for (int j = 0; j < arrTaxIds.Length; j++)
                                    {
                                        int TaxId = Configuration.convertNullToInt(arrTaxIds[j]);
                                        if (TaxId > 0)
                                        {
                                            if (strSQL == "")
                                                strSQL = "INSERT INTO ItemTax(EntityID,EntityType,TaxID,UserID) VALUES(" + arrItemIds[i] + ",'I'," + TaxId + ", '" + Configuration.UserName + "');";
                                            else
                                                strSQL += "INSERT INTO ItemTax(EntityID,EntityType,TaxID,UserID) VALUES(" + arrItemIds[i] + ",'I'," + TaxId + ", '" + Configuration.UserName + "');";
                                        }
                                    }
                                }
                            }
                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL);

                            strSQL = "UPDATE Item SET TaxPolicy = 'I', isTaxable = 1 WHERE ItemID IN (" + strItemIds + ")";
                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL);
                        }
                        else
                        {
                            string strSQL = "DELETE FROM ItemTax WHERE EntityType = 'I' AND EntityID IN (" + strItemIds + ")";
                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL);
                            strSQL = "UPDATE Item SET TaxPolicy = NULL, isTaxable = 0 WHERE ItemID IN (" + strItemIds + ")";
                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL);
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw (ex);
                    }
                }
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
        #endregion
    }
}