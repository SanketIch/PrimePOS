
namespace POS_Core.BusinessRules  {
	using System;
	using System.Data;
	using POS_Core.DataAccess;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData;
	using POS_Core.ErrorLogging;
    using NLog;

    public class ItemMonitorCategory : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        public void Persist(ItemMonitorCategoryData updates)
        {
            try
            {
                using (ItemMonitorCategorySvr dao = new ItemMonitorCategorySvr())
                {
                    checkIsValidData(updates);
                    dao.Persist(updates);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(ItemMonitorCategoryData updates)");
                throw (ex);
            }
        }
        #endregion

        #region Get Methods

        public ItemMonitorCategoryData Populate(System.Int32 id)
        {
            try
            {
                using (ItemMonitorCategorySvr dao = new ItemMonitorCategorySvr())
                {
                    return dao.Populate(id);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 id)");
                throw (ex);
            }
        }

        public ItemMonitorCategoryData PopulateList(string whereClause)
        {
            try
            {
                using (ItemMonitorCategorySvr dao = new ItemMonitorCategorySvr())
                {
                    return dao.PopulateList(whereClause);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");
                throw (ex);
            }
        }

        #region Sprint-25 - PRIMEPOS-2380 15-Feb-2017 JY Added
        public ItemMonitorCategoryData PopulateList()
        {
            try
            {
                using (ItemMonitorCategorySvr dao = new ItemMonitorCategorySvr())
                {
                    return dao.PopulateList();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList()");
                throw (ex);
            }
        }
        #endregion

        #endregion

        #region Validation Methods

        public bool checkIsValidData(ItemMonitorCategoryData updates)
        {
            ItemMonitorCategoryTable table;

            ItemMonitorCategoryRow oRow;

            oRow = (ItemMonitorCategoryRow)updates.Tables[0].Rows[0];

            table = (ItemMonitorCategoryTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (ItemMonitorCategoryTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((ItemMonitorCategoryTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((ItemMonitorCategoryTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (ItemMonitorCategoryTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((ItemMonitorCategoryTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((ItemMonitorCategoryTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return true;

            foreach (ItemMonitorCategoryRow row in table.Rows)
            {
                this.Validate_Description(row.Description.Trim());
            }
            return true;
        }

        public virtual void checkIsValidPrimaryKey(ItemMonitorCategoryData updates)
        {
            ItemMonitorCategoryTable table = (ItemMonitorCategoryTable)updates.Tables[clsPOSDBConstants.ItemMonitorCategory_tbl];
            foreach (ItemMonitorCategoryRow row in table.Rows)
            {
                if (this.Populate(row.ID).Tables[clsPOSDBConstants.ItemMonitorCategory_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for ItemMonitorCategory ");
                }
            }
        }

        public void Validate_Description(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                throw(new Exception("Monitor type description cannot be empty."));
            }
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
