//----------------------------------------------------------------------------------------------------
//Sprint-18 - 2090 07-Oct-2014 JY Added business object for CL_TransDetail table
//----------------------------------------------------------------------------------------------------
using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
////using POS.Resources;
using NLog;
using Resources;
namespace POS_Core.BusinessRules
{
    
    
    public class CLTransDetail : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating CL_TransDetail data. 
        /// </summary>
        /// <param name="updates">It is CL_TransDetail type dataset class. It contains all information of CL_TransDetail.</param>

        public void Persist(CLTransDetailData updates)
        {
            try
            {
                using (CLTransDetailSvr dao = new CLTransDetailSvr())
                {
                    //checkIsValidData(updates);
                    dao.Persist(updates);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(CLTransDetailData updates)");
                throw (ex);
            }
        }

        public void Persist(CLTransDetailData updates, IDbTransaction tx)
        {
            try
            {
                using (CLTransDetailSvr dao = new CLTransDetailSvr())
                {
                    //checkIsValidData(updates);
                    dao.Persist(updates,tx);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(CLTransDetailData updates, IDbTransaction tx)");
                throw (ex);
            }
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Get typed dataset (CLTransDetail) according to CLTransDetail code
        /// </summary>
        /// <param name="DeptCode">This code represent each CLTransDetail.</param>
        /// <returns>CLTransDetail type dataset.</returns>
        public CLTransDetailData Populate(System.Int64 iID)
        {
            try
            {
                using (CLTransDetailSvr dao = new CLTransDetailSvr())
                {
                    return dao.Populate(iID);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int64 iID)");
                throw (ex);
            }
        }
        #endregion

        #region Validation Methods
        /// <summary>
        /// Check the CLTransDetail data edited, added or unchanged is valid. Check code, sale price nad name.
        /// </summary>
        /// <param name="updates">CLTransDetail type dataset.</param>
        public void checkIsValidData(CLTransDetailData updates)
        {
            CLTransDetailTable table;

            CLTransDetailRow oRow;

            oRow = (CLTransDetailRow)updates.Tables[0].Rows[0];

            table = (CLTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (CLTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((CLTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((CLTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (CLTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((CLTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((CLTransDetailTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

        }

        /// <summary>
        /// It checks the voilation of primary key rules for CLCoupons.
        /// </summary>
        /// <param name="updates">CLCoupons type dataset.</param>
        public virtual void checkIsValidPrimaryKey(CLTransDetailData updates)
        {
            CLTransDetailTable table = (CLTransDetailTable)updates.Tables[clsPOSDBConstants.CLCoupons_tbl];
            foreach (CLTransDetailRow row in table.Rows)
            {
                if (this.Populate(row.ID).Tables[clsPOSDBConstants.CLTransDetail_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for CLTransDetail");
                }
            }
        }

        #endregion

        public bool DeleteRow(long CurrentID)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(DBConfig.ConnectionString);
                CLTransDetailSvr oSvr = new CLTransDetailSvr();
                return oSvr.DeleteRow(CurrentID);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(long CurrentID)");
                throw (ex);
            }
        }

        /// <summary>
        /// Dispose all resources of CLCoupons.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
