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
using POS_Core.Resources;
//using POS.Resources;

namespace POS_Core.BusinessRules
{
    public class PSEItem : IDisposable
    {
        #region Persist Methods
        public void Persist(PSEItemData updates)
        {
            try
            {
                using (PSEItemSvr dao = new PSEItemSvr())
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
        public PSEItemData Populate(System.String sItemCode)
        {
            try
            {
                using (PSEItemSvr dao = new PSEItemSvr())
                {
                    return dao.Populate(sItemCode);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public PSEItemData Populate(System.Int32 Id)
        {
            try
            {
                using (PSEItemSvr dao = new PSEItemSvr())
                {
                    return dao.Populate(Id);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public PSEItemData PopulateList(string whereClause)
        {
            try
            {
                using (PSEItemSvr dao = new PSEItemSvr())
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
        public bool checkIsValidData(PSEItemData updates)
        {
            PSEItemTable table;
            
            PSEItemRow oRow;
            
            oRow = (PSEItemRow)updates.Tables[0].Rows[0];

            table = (PSEItemTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (PSEItemTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((PSEItemTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((PSEItemTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (PSEItemTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((PSEItemTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((PSEItemTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return true;

            foreach (PSEItemRow row in table.Rows)
            {
                this.Validate_ProductId(row.ProductId.Trim());
                this.Validate_ProductName(row.ProductName.Trim());
                this.Validate_ProductGrams(row.ProductGrams.ToString());
            }
            return true;
        }

        public virtual void checkIsValidPrimaryKey(PSEItemData updates)
        {
            PSEItemTable table = (PSEItemTable)updates.Tables[clsPOSDBConstants.PSE_Items_tbl];
            foreach (PSEItemRow row in table.Rows)
            {
                if (this.Populate(row.Id).Tables[clsPOSDBConstants.PSE_Items_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for TaxCodes ");
                }
            }
        }

        public void Validate_ProductId(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.PSE_Items_ProductIdCanNotBeNULL);
            }
        }

        public void Validate_ProductName(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.PSE_Items_ProductNameCanNotBeNULL);
            }
        }

        public void Validate_ProductGrams(string strValue)
        {
            if (strValue == null || strValue.Trim() == "" || Configuration.convertNullToDecimal(strValue) <= 0)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.PSE_Items_ProductGramsCanNotBeNULL);
            }
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
