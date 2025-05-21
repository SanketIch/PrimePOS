

namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;

    public class PayOutCat :IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// Insert or update InvTransType data.
        /// </summary>
        /// <param name="updates">Collection of InvTransType type dataset.</param>
        public void Persist(PayOutCatData updates)
        {
            try
            {
                using (PayOutCatSvr dao = new PayOutCatSvr())
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
        #endregion


        #region Get Data
        public PayOutData Populate(System.String ID)
        {
            try
            {
                using (PayOutSvr dao = new PayOutSvr())
                {
                    return dao.Populate(ID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


        /// <summary>
        /// Get transaction type with respect to id.
        /// </summary>
        /// <param name="ID">Primary ID of tractions types entity.</param>
        /// <returns>PayOutCatData type record.</returns>
        public PayOutCatData Populate(System.Int32 ID)
        {
            try
            {
                using (PayOutCatSvr dao = new PayOutCatSvr())
                {
                    return dao.Populate(ID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        
        /// <summary>
        /// Get All Payout category filtered by where clasue provided
        /// </summary>
        /// <param name="sWhereClause"></param>
        /// <returns></returns>
        public PayOutCatData PopulateList(string sWhereClause)
        {
            try
            {
                using (PayOutCatSvr dao = new PayOutCatSvr())
                {
                    return dao.PopulateList(sWhereClause);
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
        /// Validate the PayOutCatData type data.
        /// </summary>
        /// <param name="updates">PayOutCatData type dataset for validation.</param>
        /// <returns></returns>
        public bool checkIsValidData(PayOutCatData updates)
        {
            PayOutCatTable table;

            PayOutCatRow oRow;

            oRow = (PayOutCatRow)updates.Tables[0].Rows[0];

            table = (PayOutCatTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (PayOutCatTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((PayOutCatTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((PayOutCatTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (PayOutCatTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((PayOutCatTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((PayOutCatTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return true;
            return true;
        }

         #endregion
        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
