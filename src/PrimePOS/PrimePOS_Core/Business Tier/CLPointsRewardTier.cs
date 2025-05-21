using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
////using POS.Resources;
using Resources;
using POS_Core.Resources;

namespace POS_Core.BusinessRules
{
    
    /// <summary>
    /// This is business object of CLPointsRewardTier. It contains business rules of CLPointsRewardTier.
    /// </summary>
    public class CLPointsRewardTier : IDisposable
    {

        #region Declaration
        public bool IsDeactivatingCard = false;
        #endregion

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating CLPointsRewardTiers data. 
        /// </summary>
        /// <param name="updates">It is CLPointsRewardTier type dataset class. It contains all information of CLPointsRewardTiers.</param>

        public void Persist(CLPointsRewardTierData updates)
        {
            try
            {
                using (CLPointsRewardTierSvr dao = new CLPointsRewardTierSvr())
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
        /// Get typed dataset (CLPointsRewardTierData) according to CLPointsRewardTier code
        /// </summary>
        /// <param name="DeptCode">This code represent each CLPointsRewardTier.</param>
        /// <returns>CLPointsRewardTier type dataset.</returns>
        public CLPointsRewardTierData Populate(System.Int32  iID)
        {
            try
            {
                using (CLPointsRewardTierSvr dao = new CLPointsRewardTierSvr())
                {
                    return dao.Populate(iID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        
        /// <summary>
        /// Get CLPointsRewardTier type dataset with respect to condition.
        /// </summary>
        /// <param name="whereClause">Provide SQL where clause.</param>
        /// <returns>CLPointsRewardTier type dataset.</returns>
        public CLPointsRewardTierData PopulateList(string whereClause)
        {
            try
            {
                using (CLPointsRewardTierSvr dao = new CLPointsRewardTierSvr())
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
        /// Check the CLPointsRewardTiers data edited, added or unchanged is valid. Check code, sale price nad name.
        /// </summary>
        /// <param name="updates">CLPointsRewardTier type dataset.</param>
        public void checkIsValidData(CLPointsRewardTierData updates)
        {
            CLPointsRewardTierTable table;

            CLPointsRewardTierRow oRow;

            oRow = (CLPointsRewardTierRow)updates.Tables[0].Rows[0];

            table = (CLPointsRewardTierTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (CLPointsRewardTierTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((CLPointsRewardTierTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((CLPointsRewardTierTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (CLPointsRewardTierTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((CLPointsRewardTierTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((CLPointsRewardTierTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

        }

        /// <summary>
        /// It checks the voilation of primary key rules for CLPointsRewardTier.
        /// </summary>
        /// <param name="updates">CLPointsRewardTier type dataset.</param>
        public virtual void checkIsValidPrimaryKey(CLPointsRewardTierData updates)
        {
            CLPointsRewardTierTable table = (CLPointsRewardTierTable)updates.Tables[clsPOSDBConstants.CLPointsRewardTier_tbl];
            foreach (CLPointsRewardTierRow row in table.Rows)
            {
                if (this.Populate(row.ID).Tables[clsPOSDBConstants.CLPointsRewardTier_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for CLPointsRewardTier ");
                }
            }
        }

        #endregion
        /// <summary>
        /// Dispose all resources of CLPointsRewardTier.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public void UpdatePointsInTiers(Decimal transactionPoints, int iTransID, long iCLCardID, string ActionType, int TransType, IDbTransaction oTrans)
        {
            ProcessPointsInTiers(transactionPoints, iTransID, iCLCardID, oTrans, DateTime.Now, ActionType, TransType, false);
        }

        public void ProcessPointsInTiers(Decimal transactionPoints, int iTransID, long iCLCardID, DateTime TransDate, string ActionType, int TransType, IDbTransaction oTrans)    //Sprint-18 - 2090 10-Oct-2014 JY Added transdate parameter
        {
            ProcessPointsInTiers(transactionPoints, iTransID, iCLCardID, oTrans, TransDate, ActionType, TransType, true); //Sprint-18 - 2090 10-Oct-2014 JY Added transdate parameter
        }

        public void UpdatePointsInTiers(int transactionPoints, int iTransID, long iCLCardID)
        {
            IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            IDbTransaction oTrans = conn.BeginTransaction();
            ProcessPointsInTiers(transactionPoints, iTransID, iCLCardID, oTrans, DateTime.Now, "",0, false);
            oTrans.Commit();
        }

        public void ProcessPointsInTiers(Decimal transactionPoints, int iTransID, long iCLCardID)
        {
            IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            IDbTransaction oTrans = conn.BeginTransaction();
            ProcessPointsInTiers(transactionPoints, iTransID, iCLCardID, oTrans, DateTime.Now, "",0, true);
            oTrans.Commit();
        }

        private void ProcessPointsInTiers(Decimal transactionPoints, int iTransID, long iCLCardID, IDbTransaction oTrans, DateTime TransDate, string ActionType, int TransType, bool addToCurrentPoints) //Sprint-18 - 2090 10-Oct-2014 JY Added transdate parameter
        {
            
            CLCardsSvr oCLCardsSvr = new CLCardsSvr();
            CLCouponsSvr oCLCouponsSvr = new CLCouponsSvr();
            Decimal iORGTransPoints = transactionPoints;
            CLCoupons oCLCoupons = new CLCoupons();
            CLCards oCLCards = new CLCards();
            IsDeactivatingCard = (transactionPoints == 0) ? true : false;
            Decimal currentTotalPoints = transactionPoints;

            if (addToCurrentPoints == true)
            {
                currentTotalPoints += GetCurrentTotalPoints(iCLCardID, oTrans, Configuration.CLoyaltyInfo.DoNotGenerateCoupons);
            }

            if (iTransID == 0 && currentTotalPoints < 1 && IsDeactivatingCard == false)
            {
                return;
            }

            Decimal remainingPoints=currentTotalPoints;
            if (Configuration.CLoyaltyInfo.DoNotGenerateCoupons == false)
            {
                CLCouponsData oNewCouponData = GetNewCoupons(ref remainingPoints, iCLCardID, iTransID, oTrans);
                if (Configuration.isNullOrEmptyDataSet(oNewCouponData) == false || iTransID == 0)
                {
                    oCLCouponsSvr.DeleteUnusedCoupons(iCLCardID, oTrans);

                    oCLCouponsSvr.Persist(oNewCouponData, oTrans);
                }
                else
                {
                    if (TransType == 2) //Sprint-18 - 2090 16-Oct-2014 JY Added for return transaction
                    {
                        remainingPoints = iORGTransPoints + GetCurrentTotalPoints(iCLCardID, oTrans, false);
                        //balance point should not be negative
                        if (remainingPoints < 0)
                        {
                            remainingPoints = 0;
                            currentTotalPoints = 0;
                        }
                        //delete coupons
                        oCLCouponsSvr.DeleteUnusedCoupons(iCLCardID, oTrans);
                    }
                    else
                    {
                        remainingPoints = iORGTransPoints + GetCurrentTotalPoints(iCLCardID, oTrans, true);
                    }
                }
            }
            
            oCLCardsSvr.UpdateLoyaltyCurrentPoints(iCLCardID, remainingPoints,oTrans);

            #region Sprint-18 - 2090 10-Oct-2014 JY Added logic to insert data in to CL_TransDetail table
            CLTransDetailSvr oCLTransDetailSvr = new CLTransDetailSvr();

            CLTransDetail oBRCLTransDetail = new CLTransDetail();
            CLTransDetailData oCLTransDetailData = new CLTransDetailData();
            CLTransDetailRow oCLTransDetailRow; 
            
            try
            {
                if (oCLTransDetailData != null) oCLTransDetailData.CLTransDetail.Rows.Clear();
                if (remainingPoints < 0)    remainingPoints = 0;
                if (currentTotalPoints  < 0)    currentTotalPoints = 0;
                
                oCLTransDetailRow = oCLTransDetailData.CLTransDetail.AddRow(0, iTransID, iCLCardID, remainingPoints, transactionPoints, currentTotalPoints, ActionType, TransDate);
                oBRCLTransDetail.Persist(oCLTransDetailData, oTrans);
            }
            catch
            {
            }
            finally
            { 
            }
            #endregion
        }

        public Decimal GetCurrentTotalPoints(long iCLCardID)
        {
            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = conn.BeginTransaction();
                return GetCurrentTotalPoints(iCLCardID, tx);
            }
            finally
            {
                if (tx != null)
                {
                    tx.Rollback();
                }
            }
        }

        public Decimal GetCurrentTotalPoints(long iCLCardID, IDbTransaction oTrans)
        {
            return GetCurrentTotalPoints(iCLCardID, oTrans, false);
        }

        public Decimal GetCurrentTotalPoints(long iCLCardID,IDbTransaction oTrans,bool excludeCoupons)
        {
            Decimal currentTotalPoints = 0;
            CLCardsSvr oCLCardsSvr = new CLCardsSvr();
            
            CLCardsData oCLCardsData = oCLCardsSvr.GetByCLCardID(iCLCardID, oTrans);
            foreach (CLCardsRow oRow in oCLCardsData.CLCards.Rows)
            {
                currentTotalPoints += oRow.CurrentPoints;
            }

            if (excludeCoupons == false)
            {
                CLCouponsSvr oCLCouponsSvr = new CLCouponsSvr();
                CLCouponsData oCLCouponsData = oCLCouponsSvr.GetUnUsedCLCoupons(oTrans, iCLCardID);
                foreach (CLCouponsRow oRow in oCLCouponsData.CLCoupons.Rows)
                {
                    currentTotalPoints += oRow.Points;
                }
            }

            return currentTotalPoints;
        }

        public CLCouponsData GetNewCoupons(ref System.Decimal currentTotalPoints, Int64 iCLCardID,int iCreatedInTransID, IDbTransaction oTrans)
        {

            CLCouponsData oCLCouponsData = new CLCouponsData();
            CLPointsRewardTierData oCLPointsRewardTierData = this.PopulateList(" order by Points Desc ");
             //Added By Shitaljit on 4Feb2014 for PRIMEPOS-1703 CL Tier incremental Scheme
            //To check whether Coupon is generated for the Tier of not
            bool isCouponGeneratedForCLTier = false;
            bool isCouponGeneratedForALLCLTier = false;
            CLCoupons oCLCoupons = new CLCoupons();
            oCLPointsRewardTierData.CLPointsRewardTier.Columns.Add("IsCouponGenerated", typeof(System.Int32));
            if (POS_Core.Resources.Configuration.CLoyaltyInfo.SingleCouponPerRewardTier == true && currentTotalPoints > 0)
            {
                int CLTierCount = 0;
                foreach (CLPointsRewardTierRow oRow in oCLPointsRewardTierData.CLPointsRewardTier.Rows)
                {
                    isCouponGeneratedForCLTier = !POS_Core.Resources.Configuration.isNullOrEmptyDataSet(oCLCoupons.IsCouponGeneratedForCLTier(iCLCardID, oRow.ID, oTrans));
                    if (isCouponGeneratedForCLTier == true)
                    {
                        oRow["IsCouponGenerated"] = 1;
                        CLTierCount++;
                    }
                    else
                    {
                        oRow["IsCouponGenerated"] = 0;
                    }
                }
                if (CLTierCount == oCLPointsRewardTierData.CLPointsRewardTier.Rows.Count)
                { 
                    //Reset The Signle Coupon Generation Cylce
                    isCouponGeneratedForALLCLTier = true;
                    oCLCoupons.ResetCouponGenarationCycle(iCLCardID,oTrans);
                }
            }
            //End
            if (POS_Core.Resources.Configuration.CLoyaltyInfo.RedeemMethod == (int)POS_Core.Resources.CLRedeemMethod.Manual)
            {
                CLCouponsRow oCouponRow = oCLCouponsData.CLCoupons.NewCLCouponsRow();

                for (int i = 0; i < oCLPointsRewardTierData.CLPointsRewardTier.Rows.Count; i++)
                {

                    CLPointsRewardTierRow oRow = oCLPointsRewardTierData.CLPointsRewardTier[i];
                    //Added By Shitaljit on 4Feb2014 for PRIMEPOS-1703 CL Tier incremental Scheme
                    //To check whether Coupon is generated for the Tier of not
                    if (POS_Core.Resources.Configuration.CLoyaltyInfo.SingleCouponPerRewardTier == true)
                    {
                        isCouponGeneratedForCLTier = (isCouponGeneratedForALLCLTier == true) ? false : Configuration.convertNullToBoolean(oCLPointsRewardTierData.CLPointsRewardTier[i]["IsCouponGenerated"]);
                    }
                    else
                    {
                        isCouponGeneratedForCLTier = false;
                    }
                    //End

                    if (oRow.Points <= currentTotalPoints && isCouponGeneratedForCLTier == false)
                    {
                        oCouponRow.ID = oCLCouponsData.CLCoupons.Rows.Count + 1;
                        oCouponRow.CLCardID = iCLCardID;
                        oCouponRow.CouponValue += oRow.Discount;
                        oCouponRow.CreatedInTransID = iCreatedInTransID;
                        oCouponRow.ExpiryDays = oRow.RewardPeriod;
                        oCouponRow.Points += oRow.Points;
                        oCouponRow.IsCouponValueInPercentage = POS_Core.Resources.Configuration.CLoyaltyInfo.IsTierValueInPercent;
                        oCouponRow.CLTierID = oRow.ID;
                        currentTotalPoints -= oRow.Points;

                        if (currentTotalPoints < 1)
                        {
                            break;
                        }
                        i--;
                    }
                }

                if (oCouponRow.CouponValue > 0 || oCouponRow.Points > 0)
                {
                    oCLCouponsData.CLCoupons.Rows.Add(oCouponRow);
                }
            }
            else
            {
                #region Sprint-25 - PRIMEPOS-2297 21-Feb-2017 JY Added new logic over here if ApplyDiscountOnlyIfTierIsReached = true then behave like manual else existing behavior
                if (Configuration.CLoyaltyInfo.ApplyDiscountOnlyIfTierIsReached)
                {
                    CLCouponsRow oCouponRow = oCLCouponsData.CLCoupons.NewCLCouponsRow();

                    for (int i = 0; i < oCLPointsRewardTierData.CLPointsRewardTier.Rows.Count; i++)
                    {
                        CLPointsRewardTierRow oRow = oCLPointsRewardTierData.CLPointsRewardTier[i];
                        if (POS_Core.Resources.Configuration.CLoyaltyInfo.SingleCouponPerRewardTier == true)
                        {
                            isCouponGeneratedForCLTier = (isCouponGeneratedForALLCLTier == true) ? false : Configuration.convertNullToBoolean(oCLPointsRewardTierData.CLPointsRewardTier[i]["IsCouponGenerated"]);
                        }
                        else
                        {
                            isCouponGeneratedForCLTier = false;
                        }
                        //End

                        if (oRow.Points <= currentTotalPoints && isCouponGeneratedForCLTier == false)
                        {
                            oCouponRow.ID = oCLCouponsData.CLCoupons.Rows.Count + 1;
                            oCouponRow.CLCardID = iCLCardID;
                            oCouponRow.CouponValue += oRow.Discount;
                            oCouponRow.CreatedInTransID = iCreatedInTransID;
                            oCouponRow.ExpiryDays = oRow.RewardPeriod;
                            oCouponRow.Points += oRow.Points;
                            oCouponRow.IsCouponValueInPercentage = POS_Core.Resources.Configuration.CLoyaltyInfo.IsTierValueInPercent;
                            oCouponRow.CLTierID = oRow.ID;
                            currentTotalPoints -= oRow.Points;

                            if (currentTotalPoints < 1)
                            {
                                break;
                            }
                            i--;
                        }
                    }

                    if (oCouponRow.CouponValue > 0 || oCouponRow.Points > 0)
                    {
                        oCLCouponsData.CLCoupons.Rows.Add(oCouponRow);
                    }
                }
                #endregion
                else
                {
                    if (oCLPointsRewardTierData.CLPointsRewardTier.Rows.Count > 0)
                    {

                        CLPointsRewardTierRow oRow = oCLPointsRewardTierData.CLPointsRewardTier[oCLPointsRewardTierData.CLPointsRewardTier.Rows.Count - 1];

                        decimal perPointValue = oRow.Discount / oRow.Points;

                        if (Math.Round(perPointValue * currentTotalPoints, 2) != 0)
                        {
                            CLCouponsRow oCouponRow = oCLCouponsData.CLCoupons.NewCLCouponsRow();

                            oCouponRow.ID = oCLCouponsData.CLCoupons.Rows.Count + 1;
                            oCouponRow.CLCardID = iCLCardID;
                            oCouponRow.CouponValue = perPointValue * currentTotalPoints;
                            oCouponRow.CreatedInTransID = iCreatedInTransID;
                            oCouponRow.ExpiryDays = oRow.RewardPeriod;
                            oCouponRow.Points = currentTotalPoints;
                            oCouponRow.IsCouponValueInPercentage = POS_Core.Resources.Configuration.CLoyaltyInfo.IsTierValueInPercent;

                            oCLCouponsData.CLCoupons.Rows.Add(oCouponRow);

                            currentTotalPoints = 0;
                        }
                    }
                }
            }
            return oCLCouponsData;
        }

        public bool DeleteRow(int CurrentID)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                CLPointsRewardTierSvr oSvr = new CLPointsRewardTierSvr();
                return oSvr.DeleteRow(CurrentID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}
