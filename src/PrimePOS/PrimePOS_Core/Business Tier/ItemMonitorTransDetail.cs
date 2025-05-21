//----------------------------------------------------------------------------------------------------
//Sprint-23 - PRIMEPOS-2029 19-Apr-2016 JY Added to maintain item monitor trans log
//----------------------------------------------------------------------------------------------------
using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
//using POS.Resources;
using NLog;
using Resources;
namespace POS_Core.BusinessRules
{
    

    class ItemMonitorTransDetail : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating ItemMonitorTransDetail data. 
        /// </summary>
        /// <param name="updates">It is ItemMonitorTransDetail type dataset class. It contains all information of ItemMonitorTransDetail.</param>

        public void Persist(ItemMonitorTransDetailData updates)
        {
            try
            {
                using (ItemMonitorTransDetailSvr dao = new ItemMonitorTransDetailSvr())
                {
                    dao.Persist(updates);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(ItemMonitorTransDetailData updates)");
                throw (ex);
            }
        }

        public void Persist(ItemMonitorTransDetailData updates, IDbTransaction tx)
        {
            try
            {
                using (ItemMonitorTransDetailSvr dao = new ItemMonitorTransDetailSvr())
                {
                    dao.Persist(updates, tx);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(ItemMonitorTransDetailData updates, IDbTransaction tx)");
                throw (ex);
            }
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Get typed dataset (ItemMonitorTransDetail) according to ItemMonitorTransDetail code
        /// </summary>
        /// <param name="DeptCode">This code represent each ItemMonitorTransDetail.</param>
        /// <returns>ItemMonitorTransDetail type dataset.</returns>
        public ItemMonitorTransDetailData Populate(System.Int64 iID)
        {
            try
            {
                using (ItemMonitorTransDetailSvr dao = new ItemMonitorTransDetailSvr())
                {
                    return dao.Populate(iID);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int64 iID)");
                throw (ex);
            }
        }
        #endregion

        #region Validation Methods
        /// <summary>
        /// Check the ItemMonitorTransDetail data edited, added or unchanged is valid. Check code, sale price nad name.
        /// </summary>
        /// <param name="updates">ItemMonitorTransDetail type dataset.</param>
        public void checkIsValidData(ItemMonitorTransDetailData updates)
        {
            ItemMonitorTransDetailTable table;

            ItemMonitorTransDetailRow oRow;

            oRow = (ItemMonitorTransDetailRow)updates.Tables[0].Rows[0];

            table = (ItemMonitorTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (ItemMonitorTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((ItemMonitorTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((ItemMonitorTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (ItemMonitorTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((ItemMonitorTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((ItemMonitorTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;
        }

        /// <summary>
        /// It checks the voilation of primary key rules for CLCoupons.
        /// </summary>
        /// <param name="updates">CLCoupons type dataset.</param>
        public virtual void checkIsValidPrimaryKey(ItemMonitorTransDetailData updates)
        {
            ItemMonitorTransDetailTable table = (ItemMonitorTransDetailTable)updates.Tables[clsPOSDBConstants.ItemMonitorTransDetail_tbl];
            foreach (ItemMonitorTransDetailRow row in table.Rows)
            {
                if (this.Populate(row.ID).Tables[clsPOSDBConstants.ItemMonitorTransDetail_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for ItemMonitorTransDetail");
                }
            }
        }
        #endregion

        public bool DeleteRow(long CurrentID)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                ItemMonitorTransDetailSvr oSvr = new ItemMonitorTransDetailSvr();
                return oSvr.DeleteRow(CurrentID);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(long CurrentID)");
                throw (ex);
            }
        }

        /// <summary>
        /// Dispose all resources of CLCoupons.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
