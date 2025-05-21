using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
////using POS.Resources;
using Resources;
namespace POS_Core.BusinessRules
{
    
    /// <summary>
    /// This is business object of CLCoupons. It contains business rules of CLCoupons.
    /// </summary>
    public class CLCoupons : IDisposable
    {

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating CLCouponss data. 
        /// </summary>
        /// <param name="updates">It is CLCoupons type dataset class. It contains all information of CLCouponss.</param>

        public void Persist(CLCouponsData updates)
        {
            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
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
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                CLCouponsSvr oSvr = new CLCouponsSvr();
                return oSvr.DeleteRow(CurrentID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public bool DeleteCoupons(int iCLCardID, IDbTransaction oTrans)
        {
            try
            {
                CLCouponsSvr oSvr = new CLCouponsSvr();
                return oSvr.DeleteCoupons(iCLCardID,oTrans);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region Get Methods
        /// <summary>
        /// Get typed dataset (CLCouponsData) according to CLCoupons code
        /// </summary>
        /// <param name="DeptCode">This code represent each CLCoupons.</param>
        /// <returns>CLCoupons type dataset.</returns>
        public CLCouponsData Populate(System.Int64 iID)
        {
            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
                {
                    return dao.Populate(iID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public CLCouponsData GetByCLCardID(System.Int64 iCLCardID)
        {
            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
                {
                    return dao.GetByCLCardID(iCLCardID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //PRIMEPOS-2794 SAJID DHUKKA
        public CLCouponsData GetByCLCardID(string iCLCardID)
        {
            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
                {
                    return dao.GetByCLCardID(iCLCardID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Author: Shitaljit
        /// CreatedvDate : 2/4/2014
        /// Added to check whether customer has alrady availed coupon for the give reward tier or not
        /// </summary>
        /// <param name="iCLCardID"></param>
        /// <param name="iCLTierID"></param>
        /// <returns></returns>
        public CLCouponsData IsCouponGeneratedForCLTier(System.Int64 iCLCardID, System.Int64 iCLTierID, IDbTransaction oTrans)
        {
            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
                {
                    return dao.IsCouponGeneratedForCLTier(iCLCardID, iCLTierID, oTrans);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public CLCouponsData GetUnUsedCLCoupons(Int64 iCLCardID)
        {

            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
                {
                    return dao.GetUnUsedCLCoupons(iCLCardID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public CLCouponsData GetUnUsedCLCoupons(IDbTransaction tx, Int64 iCLCardID)
        {
            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
                {
                    return dao.GetUnUsedCLCoupons(tx,iCLCardID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Get CLCoupons type dataset with respect to condition.
        /// </summary>
        /// <param name="whereClause">Provide SQL where clause.</param>
        /// <returns>CLCoupons type dataset.</returns>
        public CLCouponsData PopulateList(string whereClause)
        {
            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
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
        /// Check the CLCouponss data edited, added or unchanged is valid. Check code, sale price nad name.
        /// </summary>
        /// <param name="updates">CLCoupons type dataset.</param>
        public void checkIsValidData(CLCouponsData updates)
        {
            CLCouponsTable table;

            CLCouponsRow oRow;

            oRow = (CLCouponsRow)updates.Tables[0].Rows[0];

            table = (CLCouponsTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (CLCouponsTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((CLCouponsTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((CLCouponsTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (CLCouponsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((CLCouponsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((CLCouponsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

        }

        /// <summary>
        /// It checks the voilation of primary key rules for CLCoupons.
        /// </summary>
        /// <param name="updates">CLCoupons type dataset.</param>
        public virtual void checkIsValidPrimaryKey(CLCouponsData updates)
        {
            CLCouponsTable table = (CLCouponsTable)updates.Tables[clsPOSDBConstants.CLCoupons_tbl];
            foreach (CLCouponsRow row in table.Rows)
            {
                if (this.Populate(row.CLCardID).Tables[clsPOSDBConstants.CLCoupons_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for CLCoupons ");
                }
            }
        }

        #endregion

        /// <summary>
        /// Dispose all resources of CLCoupons.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public void ConsumeCoupon(POSTransPaymentRow oTransPRow, System.Data.IDbTransaction oDBTrans)
        {
            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
                {
                    dao.ConsumeCoupon(oTransPRow,oDBTrans);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public decimal CalculateAndGetCouponValue(CLCouponsRow oCouponRow, decimal amount)
        {
            decimal returnValue=0;
            if (oCouponRow.IsCouponValueInPercentage)
            {
                if (amount > 0 && oCouponRow.CouponValue > 0)
                {
                    returnValue = oCouponRow.CouponValue / 100 * amount;
                }
            }
            else
            {
                returnValue= oCouponRow.CouponValue;
            }
            return Math.Round( returnValue,2);
        }

        /// <summary>
        /// Author:Shitaljit
        /// Date: 2/6/2014
        /// Created to reset single coupon per reawrd tier logic for specific card.
        /// </summary>
        /// <param name="iCLCardID"></param>
        public bool ResetCouponGenarationCycle(Int64 iCLCardID)
        {
            IDbTransaction oTrans = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = conn.BeginTransaction();
                return ResetCouponGenarationCycle(iCLCardID, oTrans);
                oTrans.Commit();
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw (ex);
            }
        }
        
        /// <summary>
        /// Author:Shitaljit
        /// Date: 2/6/2014
        /// Created to reset single coupon per reawrd tier logic for specific card.
        /// </summary>
        /// <param name="iCLCardID"></param>
        /// <param name="oTrans"></param>
        public bool ResetCouponGenarationCycle(Int64 iCLCardID, IDbTransaction oTrans)
        {
            try
            {
                using (CLCouponsSvr dao = new CLCouponsSvr())
                {
                  return  dao.ResetCouponGenarationCycle(iCLCardID, oTrans);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
