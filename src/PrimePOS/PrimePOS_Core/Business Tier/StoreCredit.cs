using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
using System.Collections.Generic;
using POS_Core.Resources;
using POS_Core.TransType;
using POS_Core.Data_Tier;

namespace POS_Core.BusinessRules
{
    /// <summary>
    /// This is business object of StoreCredit. It contains business rules of StoreCredit.
    /// </summary>
    public class StoreCredit : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating StoreCredit data. 
        /// </summary>
        /// <param name="updates">It is StoreCredit type dataset class. It contains all information of StoreCredit.</param>        
        public void Persist(StoreCreditData updates)
        {
            try
            {
                using (StoreCreditSvr dao = new StoreCreditSvr())
                {
                    checkIsValidData(updates);
                    dao.Persist(updates);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void Persist(StoreCreditData updates, IDbTransaction tx)
        {
            try
            {
                using (StoreCreditSvr dao = new StoreCreditSvr())
                {
                    checkIsValidData(updates);
                    dao.Persist(updates, tx);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool DeleteRow(string CurrentID)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(DBConfig.ConnectionString);
                StoreCreditSvr oSvr = new StoreCreditSvr();
                return oSvr.DeleteRow(CurrentID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //Doubt for below method modify it 30/09/19 didnt implemented
        public bool DeactivateCard(Int64 CLCardID)
        {
            try
            {
                CLCardsSvr oSvr = new CLCardsSvr();
                return oSvr.DeactivateCard(CLCardID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //Doubt for below method modify it 30/09/19 didnt implemented
        public void UpdateLoyaltyCurrentPoints(int CLCardID, int points, System.Data.IDbTransaction oTrans)
        {
            CLCardsSvr oSvr = new CLCardsSvr();
            oSvr.UpdateLoyaltyCurrentPoints(CLCardID, points, oTrans);
        }

        #endregion


        #region Get Methods
        /// <summary>
        /// Get typed dataset (StoreCreditData) according to StoreCredit code
        /// </summary>
        /// <param name="DeptCode">This code represent each StoreCredit.</param>
        /// <returns>StoreCredit type dataset.</returns>
        public StoreCreditData Populate(System.Int32 iID)
        {
            try
            {
                using (StoreCreditSvr dao = new StoreCreditSvr())
                {
                    return dao.Populate(iID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void UpdateCreditAmount(StoreCreditData updates, IDbTransaction tx)
        {
            StoreCreditSvr oSvr = new StoreCreditSvr();
            oSvr.UpdateCreditAmount(updates, tx);
        }

        public void UpdateCreditAmount(StoreCreditData updates)
        {
            IDbTransaction tx = null;
            try
            {
                using (IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    tx = oConn.BeginTransaction();
                    UpdateCreditAmount(updates, tx);
                    tx.Commit();
                }
            }
            catch (POSExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                tx.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }

        public StoreCreditData GetByCustomerID(System.Int32 iCustomerID)
        {
            try
            {
                using (StoreCreditSvr dao = new StoreCreditSvr())
                {
                    return dao.GetByCustomerID(iCustomerID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
                
        public StoreCreditData GetByCustomerID(System.Int32 iCustomerID, IDbTransaction tx)
        {
            try
            {
                using (StoreCreditSvr dao = new StoreCreditSvr())
                {
                    return dao.GetByCustomerID(iCustomerID, tx);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Get StoreCredit type dataset with respect to condition.
        /// </summary>
        /// <param name="whereClause">Provide SQL where clause.</param>
        /// <returns>StoreCredit type dataset.</returns>
        public StoreCreditData PopulateList(string whereClause)
        {
            try
            {
                using (StoreCreditSvr dao = new StoreCreditSvr())
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
        /// Check the StoreCredit data edited, added or unchanged is valid. Check code, sale price nad name.
        /// </summary>
        /// <param name="updates">StoreCredit type dataset.</param>
        public void checkIsValidData(StoreCreditData updates)
        {
            StoreCreditTable table;

            StoreCreditRow oRow;

            oRow = (StoreCreditRow)updates.Tables[0].Rows[0];

            table = (StoreCreditTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (StoreCreditTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((StoreCreditTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((StoreCreditTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (StoreCreditTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((StoreCreditTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((StoreCreditTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null)
                return;

        }

        /// <summary>
        /// It checks the voilation of primary key rules for CLCards.
        /// </summary>
        /// <param name="updates">v type dataset.</param>
        public virtual void checkIsValidPrimaryKey(StoreCreditData updates)
        {
            StoreCreditTable table = (StoreCreditTable)updates.Tables[clsPOSDBConstants.StoreCredit_tbl];
            foreach (StoreCreditRow row in table.Rows)
            {
                if (this.Populate(row.StoreCreditID).Tables[clsPOSDBConstants.StoreCredit_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for CLCards ");
                }
            }
        }

        #endregion

        /// <summary>
        /// Dispose all resources of CLCards.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}