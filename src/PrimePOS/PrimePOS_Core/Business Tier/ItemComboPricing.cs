using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
//using POS.Resources;
using Resources;
namespace POS_Core.BusinessRules
{
    

    /// <summary>
    /// This is business object class for Item Combo Pricing.
    /// </summary>
    public class ItemComboPricing : IDisposable
    {
        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating Departments data.
        /// </summary>
        /// <param name="updates">It is department type dataset class. It contains all information of departments.</param>
        public void Persist(ItemComboPricingData oComboData, ItemComboPricingDetailData oComboDetailData)
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            try
            {
                ItemComboPricingSvr oItemComboPricingSvr = new ItemComboPricingSvr();
                ItemComboPricingDetailSvr oItemComboPricingDetailSvr = new ItemComboPricingDetailSvr();

                checkIsValidData(oComboData, oComboDetailData);

                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction();

                int itemComboPricingId = 0;
                oItemComboPricingSvr.Persist(oComboData, oTrans, ref itemComboPricingId);

                oItemComboPricingDetailSvr.Persist(oComboDetailData, itemComboPricingId, oTrans);
                oTrans.Commit();
            }
            catch (Exception ex)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                throw (ex);
            }
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Get inventory recieved information according to its ID.
        /// </summary>
        /// <param name="ItemComboPricingDetailID">Id of inventory recieved.</param>
        /// <returns>ItemComboPricingDetailData type dataset</returns>
        public ItemComboPricingDetailData PopulateDetail(System.Int32 ItemComboPricingId)
        {
            try
            {
                using (ItemComboPricingDetailSvr dao = new ItemComboPricingDetailSvr())
                {
                    return dao.Populate(ItemComboPricingId);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public ItemComboPricingData Populate(System.Int32 ItemComboPricingId)
        {
            try
            {
                using (ItemComboPricingSvr dao = new ItemComboPricingSvr())
                {
                    return dao.Populate(ItemComboPricingId);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region Validation Methods
        /// <summary>
        /// Validates refNo not null, Date and recieved details.
        /// </summary>
        /// <param name="updates">Provide main inventory recieved data.</param>
        /// <param name="updatesDD">Detail data related to inventory detail collection.</param>
        public void checkIsValidData(ItemComboPricingData updates, ItemComboPricingDetailData updatesDD)
        {
            ItemComboPricingTable table;

            ItemComboPricingRow oRow;

            oRow = (ItemComboPricingRow)updates.Tables[0].Rows[0];
            if (String.IsNullOrWhiteSpace( oRow.Description))
                ErrorHandler.throwCustomError(POSErrorENUM.ItemComboPricing_DescriptionMissing);

            table = (ItemComboPricingTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (ItemComboPricingTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((ItemComboPricingTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((ItemComboPricingTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (ItemComboPricingTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((ItemComboPricingTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((ItemComboPricingTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            if (updatesDD.Tables[0].Rows.Count <= 0)
                ErrorHandler.throwCustomError(POSErrorENUM.ItemComboPricing_DetailMissing);
        }
        /// <summary>
        /// Validate that this ID exist in the database or not.
        /// </summary>
        /// <param name="strValue">Inventory Recieved ID.</param>
        /// <exception cref="fgd">fdh</exception>
        public void Validate_ItemID(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.ItemComboPricingDetail_ItemIDCanNotBeNULL);
            }
            else
            {
                ItemData oID = new ItemData();
                Item oI = new Item();
                oID = oI.Populate(strValue);
                if (oID == null)
                {
                    throw (new Exception("Invalid Item code"));
                }
                else if (oID.Item.Rows.Count == 0)
                {
                    throw (new Exception("Invalid Item code"));
                }
            }
        }
        /// <summary>
        /// Validate that Quantity is null or empty.
        /// </summary>
        /// <param name="strValue">Quantity of items.</param>
        public void Validate_Qty(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.ItemComboPricingDetail_QtyCanNotBeNull);
            }
        }

        /// <summary>
        /// Check the sale price in inventory details entity.
        /// </summary>
        /// <param name="strValue">Sale price.</param>
        public void Validate_SalePrice(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.ItemComboPricingDetail_SalePriceCanNotBeNull);
            }
        }
        #endregion
        /// <summary>
        /// Release resources of ItemComboPricing.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
