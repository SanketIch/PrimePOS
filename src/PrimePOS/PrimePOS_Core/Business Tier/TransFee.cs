using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
using POS_Core.Resources;

namespace POS_Core.BusinessRules
{
    public class TransFee : IDisposable
    {
        #region Persist Methods
        public void Persist(TransFeeData updates)
        {
            try
            {
                using (TransFeeSvr dao = new TransFeeSvr())
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

        #region Validation Methods
        /// <summary>
        /// Validate the TransFeeData type data.
        /// </summary>
        /// <param name="updates">TransFeeData type dataset for validation.</param>
        /// <returns></returns>
        public bool checkIsValidData(TransFeeData updates)
        {
            TransFeeTable table;
            TransFeeRow oRow;

            oRow = (TransFeeRow)updates.Tables[0].Rows[0];
            table = (TransFeeTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (TransFeeTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((TransFeeTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((TransFeeTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (TransFeeTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((TransFeeTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((TransFeeTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return true;
            return true;
        }
        #endregion

        public bool Delete(System.Int32 TransFeeID)
        {
            try
            {
                using (TransFeeSvr oTransFeeSvr = new TransFeeSvr())
                {
                    return oTransFeeSvr.Delete(TransFeeID);
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
                return false;
            }
        }

        public TransFeeData GetTransFeeDataByPayTypeID(System.String PayTypeID, TransType.POSTransactionType oTransactionType)
        {
            try
            {
                using (TransFeeSvr oTransFeeSvr = new TransFeeSvr())
                {
                    return oTransFeeSvr.GetTransFeeDataByPayTypeID(PayTypeID, oTransactionType);
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

        public TransFeeData GetTransFeeDataByTransFeeID(System.Int32 TransFeeID)
        {
            try
            {
                using (TransFeeSvr oTransFeeSvr = new TransFeeSvr())
                {
                    return oTransFeeSvr.GetTransFeeDataByTransFeeID(TransFeeID);
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
                
        public DataTable GetPayTypeData(bool ShowOnlyOneCCType = true)
        {
            try
            {
                using (TransFeeSvr oTransFeeSvr = new TransFeeSvr())
                {
                    return oTransFeeSvr.GetPayTypeData(ShowOnlyOneCCType);
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

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
