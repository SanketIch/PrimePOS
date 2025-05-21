
using Resources;
namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;

    /// <summary>
    /// This is business object of CLPointsAdjustmentLog. It contains business rules of CLPointsAdjustmentLog.
    /// </summary>
    public class CLPointsAdjustmentLog : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating CLPointsAdjustmentLogs data. 
        /// </summary>
        /// <param name="updates">It is CLPointsAdjustmentLog type dataset class. It contains all information of CLPointsAdjustmentLogs.</param>
        public void Persist(CLPointsAdjustmentLogData updates)
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

                using (CLPointsAdjustmentLogSvr dao = new CLPointsAdjustmentLogSvr())
                {
                    checkIsValidData(updates);
                    dao.Persist(updates,oTrans);

                    CLPointsRewardTier oCLPointsRewardTier = new CLPointsRewardTier();
                    //oCLPointsRewardTier.UpdatePointsInTiers(updates.CLPointsAdjustmentLog[0].NewPoints, 0, updates.CLPointsAdjustmentLog[0].CLCardID, oTrans);    //Sprint-18 - 2090 13-Oct-2014 JY commented to add ActionType parameter
                    oCLPointsRewardTier.UpdatePointsInTiers(updates.CLPointsAdjustmentLog[0].NewPoints, 0, updates.CLPointsAdjustmentLog[0].CLCardID, "A", 1, oTrans);  //Sprint-18 - 2090 13-Oct-2014 JY Added ActionType parameter
                }
                oTrans.Commit();
                updates.AcceptChanges();
            }
            catch (Exception ex)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                throw (ex);
            }
        }

        public bool DeleteRow(string CurrentID)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                CLPointsAdjustmentLogSvr oSvr = new CLPointsAdjustmentLogSvr();
                return oSvr.DeleteRow(CurrentID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region Get Methods
        /// <summary>
        /// Get typed dataset (CLPointsAdjustmentLogData) according to CLPointsAdjustmentLog code
        /// </summary>
        /// <param name="DeptCode">This code represent each CLPointsAdjustmentLog.</param>
        /// <returns>CLPointsAdjustmentLog type dataset.</returns>
        public CLPointsAdjustmentLogData Populate(System.Int64 iID)
        {
            try
            {
                using (CLPointsAdjustmentLogSvr dao = new CLPointsAdjustmentLogSvr())
                {
                    return dao.Populate(iID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public CLPointsAdjustmentLogData GetByCLCardID(System.Int64 iCLCardID)
        {
            try
            {
                using (CLPointsAdjustmentLogSvr dao = new CLPointsAdjustmentLogSvr())
                {
                    return dao.GetByCLCardID(iCLCardID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public CLPointsAdjustmentLogData GetHistoryByCLCardID(System.Int64 iCLCardID)
        {
            try
            {
                using (CLPointsAdjustmentLogSvr dao = new CLPointsAdjustmentLogSvr())
                {
                    return dao.GetHistoryByCLCardID(iCLCardID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Get CLPointsAdjustmentLog type dataset with respect to condition.
        /// </summary>
        /// <param name="whereClause">Provide SQL where clause.</param>
        /// <returns>CLPointsAdjustmentLog type dataset.</returns>
        public CLPointsAdjustmentLogData PopulateList(string whereClause)
        {
            try
            {
                using (CLPointsAdjustmentLogSvr dao = new CLPointsAdjustmentLogSvr())
                {
                    return dao.PopulateList(whereClause);
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
        /// Check the CLPointsAdjustmentLogs data edited, added or unchanged is valid. Check code, sale price nad name.
        /// </summary>
        /// <param name="updates">CLPointsAdjustmentLog type dataset.</param>
        public void checkIsValidData(CLPointsAdjustmentLogData updates)
        {
            CLPointsAdjustmentLogTable table;

            CLPointsAdjustmentLogRow oRow;

            oRow = (CLPointsAdjustmentLogRow)updates.Tables[0].Rows[0];

            table = (CLPointsAdjustmentLogTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (CLPointsAdjustmentLogTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((CLPointsAdjustmentLogTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((CLPointsAdjustmentLogTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (CLPointsAdjustmentLogTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((CLPointsAdjustmentLogTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((CLPointsAdjustmentLogTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

        }

        /// <summary>
        /// It checks the voilation of primary key rules for CLPointsAdjustmentLog.
        /// </summary>
        /// <param name="updates">CLPointsAdjustmentLog type dataset.</param>
        public virtual void checkIsValidPrimaryKey(CLPointsAdjustmentLogData updates)
        {
            CLPointsAdjustmentLogTable table = (CLPointsAdjustmentLogTable)updates.Tables[clsPOSDBConstants.CLPointsAdjustmentLog_tbl];
            foreach (CLPointsAdjustmentLogRow row in table.Rows)
            {
                if (this.Populate(row.CLCardID).Tables[clsPOSDBConstants.CLPointsAdjustmentLog_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for CLPointsAdjustmentLog ");
                }
            }
        }

        #endregion

        /// <summary>
        /// Dispose all resources of CLPointsAdjustmentLog.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
