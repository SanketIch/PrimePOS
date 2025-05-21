using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
namespace POS_Core.BusinessRules
{


    public class TaxCodes : IDisposable
    {

        #region Persist Methods


        public void Persist(TaxCodesData updates)
        {
            try
            {
                using (TaxCodesSvr dao = new TaxCodesSvr())
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

        public TaxCodesData Populate(System.String taxCode)
        {
            try
            {
                using (TaxCodesSvr dao = new TaxCodesSvr())
                {
                    return dao.Populate(taxCode);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public TaxCodesData Populate(System.Int32 taxId)
        {
            try
            {
                using (TaxCodesSvr dao = new TaxCodesSvr())
                {
                    return dao.Populate(taxId);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public TaxCodesData PopulateList(string whereClause)
        {
            try
            {
                using (TaxCodesSvr dao = new TaxCodesSvr())
                {
                    return dao.PopulateList(whereClause);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool DeleteRow(string CurrentID, ref bool bRxTax)
        {
            try
            {
                using (TaxCodesSvr oTaxCodesSvr = new TaxCodesSvr())
                {
                    return oTaxCodesSvr.DeleteRow(CurrentID, ref bRxTax);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion


        #region Validation Methods
        public bool checkIsValidData(TaxCodesData updates)
        {
            TaxCodesTable table;

            TaxCodesRow oRow;

            oRow = (TaxCodesRow)updates.Tables[0].Rows[0];

            table = (TaxCodesTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (TaxCodesTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((TaxCodesTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((TaxCodesTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (TaxCodesTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((TaxCodesTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((TaxCodesTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return true;

            foreach (TaxCodesRow row in table.Rows)
            {
                this.Validate_TaxCode(row.TaxCode.Trim());
                this.Validate_Description(row.Description.Trim());
                this.Validate_Amount(row.Amount.ToString());
            }
            return true;
        }

        public virtual void checkIsValidPrimaryKey(TaxCodesData updates)
        {
            TaxCodesTable table = (TaxCodesTable)updates.Tables[clsPOSDBConstants.TaxCodes_tbl];
            foreach (TaxCodesRow row in table.Rows)
            {
                if (this.Populate(row.TaxCode).Tables[clsPOSDBConstants.TaxCodes_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for TaxCodes ");
                }
            }
        }

        public void Validate_TaxCode(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.TaxCodes_PrimaryKeyVoilation);
            }
        }

        public void Validate_Description(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.TaxCodes_DescriptionCanNotBeNull);
            }
        }

        public void Validate_Amount(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.TaxCodes_AmountCanNotBeNull);
            }
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
