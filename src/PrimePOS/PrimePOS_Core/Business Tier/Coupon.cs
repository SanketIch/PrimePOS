namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;
    //using POS.Resources;
    
    public class Coupon : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// Insert or update InvTransType data.
        /// </summary>
        /// <param name="updates">Collection of InvTransType type dataset.</param>
        public void Persist(CouponData updates)
        {
            try
            {
                using (CouponSvr dao = new CouponSvr())
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
        public CouponData Populate(System.String ID)
        {
            try
            {
                using (CouponSvr dao = new CouponSvr())
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
        /// <returns>CouponData type record.</returns>
        public CouponData Populate(System.Int64 ID) //PRIMEPOS-2034 JY changed from Int32 to Int64
        {
            try
            {
                using (CouponSvr dao = new CouponSvr())
                {
                    return dao.Populate(ID);
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
        /// Validate the CouponData type data.
        /// </summary>
        /// <param name="updates">CouponData type dataset for validation.</param>
        /// <returns></returns>
        public bool checkIsValidData(CouponData updates)
        {
            CouponTable table;

            CouponRow oRow;

            oRow = (CouponRow)updates.Tables[0].Rows[0];

            table = (CouponTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (CouponTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((CouponTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((CouponTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (CouponTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((CouponTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((CouponTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

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
