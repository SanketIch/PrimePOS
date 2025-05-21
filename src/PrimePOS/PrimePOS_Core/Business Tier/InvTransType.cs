
namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;

    /// <summary>
    /// This is business object class for inventory transaction type.
    /// Inventory transaction type is like inventory recieved or purchase return.
    /// It also contains user, transaction type identifier.
    /// </summary>
    public class InvTransType : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// Insert or update InvTransType data.
        /// </summary>
        /// <param name="updates">Collection of InvTransType type dataset.</param>
        public void Persist(InvTransTypeData updates)
        {
            try
            {
                using (InvTransTypeSvr dao = new InvTransTypeSvr())
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

        #region Get Methods
        /// <summary>
        /// Get transaction type with respect to id.
        /// </summary>
        /// <param name="ID">Primary ID of tractions types entity.</param>
        /// <returns>InvTransTypeData type record.</returns>
        public InvTransTypeData Populate(System.Int32 ID)
        {
            try
            {
                using (InvTransTypeSvr dao = new InvTransTypeSvr())
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
        /// Get transaction type with respect to name.
        /// </summary>
        /// <param name="TypeName">Transaction type name like inventory recieved.</param>
        /// <returns>InvTransTypeData type data.</returns>
        public InvTransTypeData Populate(System.String TypeName)
        {
            try
            {
                using (InvTransTypeSvr dao = new InvTransTypeSvr())
                {
                    return dao.Populate(TypeName);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        /// <summary>
        /// Get transaction data with respect to SQL WHERE CLAUSE.
        /// </summary>
        /// <param name="whereClause">Condition for filering the data.</param>
        /// <returns>InvTransTypeData type dataset</returns>
        public InvTransTypeData PopulateList(string whereClause)
        {
            try
            {
                using (InvTransTypeSvr dao = new InvTransTypeSvr())
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
        /// Validate the InvTransTypeData type data.
        /// </summary>
        /// <param name="updates">InvTransTypeData type dataset for validation.</param>
        /// <returns></returns>
        public bool checkIsValidData(InvTransTypeData updates)
        {
            InvTransTypeTable table;

            InvTransTypeRow oRow;

            oRow = (InvTransTypeRow)updates.Tables[0].Rows[0];

            table = (InvTransTypeTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (InvTransTypeTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((InvTransTypeTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((InvTransTypeTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (InvTransTypeTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((InvTransTypeTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((InvTransTypeTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return true;
            return true;
        }

        #endregion
        /// <summary>
        /// Free InvTransTypeData resources.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
