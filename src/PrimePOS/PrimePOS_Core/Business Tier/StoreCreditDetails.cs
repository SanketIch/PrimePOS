namespace POS_Core.BusinessRules
{
    using POS_Core.DataAccess;
    using POS_Core.CommonData;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData.Tables;
    using Resources;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using global::Resources;


    /// <summary>
    /// This is business object of StoreCredit. It contains business rules of StoreCredit.
    /// </summary>
    public class StoreCreditDetails : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating StoreCreditDetails data. 
        /// </summary>
        /// <param name="updates">It is StoreCreditDetails type dataset class. It contains all information of StoreCreditDetails.</param>

        public void Persist(StoreCreditDetailsData updates)
        {
            try
            {
                using (StoreCreditDetailsSvr dao = new StoreCreditDetailsSvr())
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

        public bool DeleteRow(string CurrentID)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(DBConfig.ConnectionString);
                StoreCreditDetailsSvr oSvr = new StoreCreditDetailsSvr();
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
        /// Get typed dataset (StoreCreditDetailsData) according to StoreCreditDetailsData code
        /// </summary>
        /// <param name="DeptCode">This code represent each StoreCreditDetailsData.</param>
        /// <returns>StoreCreditDetailsData type dataset.</returns>
        public StoreCreditDetailsData Populate(System.Int32 iID)
        {
            try
            {
                using (StoreCreditDetailsSvr dao = new StoreCreditDetailsSvr())
                {
                    return dao.Populate(iID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        public StoreCreditDetailsData GetByCustomerID(System.Int32 iCustomerID)
        {
            try
            {
                using (StoreCreditDetailsSvr dao = new StoreCreditDetailsSvr())
                {
                    return dao.GetByCustomerID(iCustomerID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Get StoreCreditDetailsData type dataset with respect to condition.
        /// </summary>
        /// <param name="whereClause">Provide SQL where clause.</param>
        /// <returns>StoreCreditDetails type dataset.</returns>
        public StoreCreditDetailsData PopulateList(string whereClause)
        {
            try
            {
                using (StoreCreditDetailsSvr dao = new StoreCreditDetailsSvr())
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
        /// <param name="updates">StoreCreditDetails type dataset.</param>
        public void checkIsValidData(StoreCreditDetailsData updates)
        {
            StoreCreditDetailsTable table;

            StoreCreditDetailsRow oRow;

            oRow = (StoreCreditDetailsRow)updates.Tables[0].Rows[0];

            table = (StoreCreditDetailsTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (StoreCreditDetailsTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((StoreCreditDetailsTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((StoreCreditDetailsTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (StoreCreditDetailsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((StoreCreditDetailsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((StoreCreditDetailsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null)
                return;

        }

        /// <summary>
        /// It checks the voilation of primary key rules for StoreCreditDetails.
        /// </summary>
        /// <param name="updates">StoreCreditDetails type dataset.</param>
        public virtual void checkIsValidPrimaryKey(StoreCreditDetailsData updates)
        {
            StoreCreditDetailsTable table = (StoreCreditDetailsTable)updates.Tables[clsPOSDBConstants.StoreCredit_tbl];
            foreach (StoreCreditDetailsRow row in table.Rows)
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
