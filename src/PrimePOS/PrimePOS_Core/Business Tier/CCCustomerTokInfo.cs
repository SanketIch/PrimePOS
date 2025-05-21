using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using Resources;
using System.Data;
using NLog;

namespace POS_Core.BusinessRules
{
    public class CCCustomerTokInfo : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        public void Persist(CCCustomerTokInfoData updates)
        {
            try
            {
                using (CCCustomerTokInfoSvr dao = new CCCustomerTokInfoSvr())
                {
                    checkIsValidData(updates);
                    dao.Persist(updates);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(CCCustomerTokInfoData updates)");
                throw (ex);
            }
        }


        #endregion

        #region "Validation Methods"

        /// <summary>
        /// Check the CLCardss data edited, added or unchanged is valid
        /// </summary>
        /// <param name="updates">CCCustomerTokInfo type dataset</param>
        public void checkIsValidData(CCCustomerTokInfoData updates)
        {
            CCCustomerTokInfoTable table;

            CCCustomerTokInfoRow oRow;

            oRow = (CCCustomerTokInfoRow)updates.Tables[0].Rows[0];

            table = (CCCustomerTokInfoTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (CCCustomerTokInfoTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((CCCustomerTokInfoTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((CCCustomerTokInfoTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (CCCustomerTokInfoTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((CCCustomerTokInfoTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((CCCustomerTokInfoTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;
        }

        /// <summary>
        /// It checks the voilation of primary key rules for CLCards.
        /// </summary>
        /// <param name="updates">CLCards type dataset.</param>
        public virtual void checkIsValidPrimaryKey(CCCustomerTokInfoData updates)
        {
            CCCustomerTokInfoTable table = (CCCustomerTokInfoTable)updates.Tables[clsPOSDBConstants.CCCustomerTokInfo_tbl];
            foreach (CCCustomerTokInfoRow row in table.Rows)
            {
                /* if (this.Populate(row.CLCardID).Tables[clsPOSDBConstants.CCCustomerTokInfo_tbl].Rows.Count != 0)
                 {
                     throw new Exception("Primary key violation for CLCards ");
                 }*/
            }
        }

        #endregion

        #region "Get Methods"
        //public CCCustomerTokInfoData Populate(System.Int64 iID)
        //{
        //    try
        //    {
        //        using (CCCustomerTokInfoSvr dao = new CCCustomerTokInfoSvr())
        //        {
        //            return dao.Populate(iID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "Populate(System.Int64 iID))");
        //        throw (ex);
        //    }
        //}

        public CCCustomerTokInfoData GetTokenByCustomerID(int iCustomerID)
        {
            try
            {
                using (CCCustomerTokInfoSvr dao = new CCCustomerTokInfoSvr())
                {
                    return dao.GetTokenByCustomerID(iCustomerID);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTokenByCustomerID(int iCustomerID)");
                throw (ex);
            }
        }

        #region 30-Nov-2017 JY added to get tokens w.r.t. customer
        //public CCCustomerTokInfoData GetTokenByCustomerId(int iCustomerID)
        //{
        //    try
        //    {
        //        using (CCCustomerTokInfoSvr dao = new CCCustomerTokInfoSvr())
        //        {
        //            return dao.GetTokenByCustomerId(iCustomerID);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "GetTokenByCustomerID(int iCustomerID)");
        //        throw (ex);
        //    }
        //}
        #endregion

        public CCCustomerTokInfoData GetTokenByCustomerandProcessor(int iCustomerID)
        {
            try
            {
                using (CCCustomerTokInfoSvr dao = new CCCustomerTokInfoSvr())
                {
                    return dao.GetTokenByCustomerandProcessor(iCustomerID);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTokenByCustomerandProcessor(int iCustomerID)");
                throw (ex);
            }
        }

        //public CCCustomerTokInfoData PopulateList(string sWhereClause)
        //{
        //    try
        //    {
        //        using (CCCustomerTokInfoSvr dao = new CCCustomerTokInfoSvr())
        //        {
        //            return dao.PopulateList(sWhereClause);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "PopulateList(string sWhereClause)");
        //        throw (ex);
        //    }
        //}
        #endregion

        #region Sprint-23 - PRIMEPOS-2316 15-Jun-2016 JY Added code to delete token
        public bool DeleteToken(int EntryID)
        {
            bool retValue = false;
            try
            {
                using (IDbConnection oConn = DataFactory.CreateConnection(DBConfig.ConnectionString))
                {
                    using (CCCustomerTokInfoSvr oSvr = new CCCustomerTokInfoSvr())
                    {
                        retValue = oSvr.DeleteToken(EntryID);
                    }
                }
                return retValue;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteToken(int EntryID)");
                throw (ex);
            }
        }
        #endregion

        #region PRIMEPOS-3004 19-Oct-2021 JY Added
        public bool DeleteTokens(StringBuilder sbEntryID)
        {
            bool retValue = false;
            try
            {
                using (CCCustomerTokInfoSvr oSvr = new CCCustomerTokInfoSvr())
                {
                    retValue = oSvr.DeleteTokens(sbEntryID);
                }
                return retValue;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteTokens(StringBuilder sbEntryID)");
                throw (ex);
            }
        }

        public bool DeleteExpiredTokens()
        {
            bool retValue = false;
            try
            {
                using (CCCustomerTokInfoSvr oSvr = new CCCustomerTokInfoSvr())
                {
                    retValue = oSvr.DeleteExpiredTokens();
                }
                return retValue;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteExpiredTokens()");
                throw (ex);
            }
        }
        #endregion

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region PRIMEPOS-2611 13-Nov-2018 JY Added
        public bool IsCustomerTokenExists(int CustomerID)
        {
            bool bReturn = false;
            try
            {
                using (CCCustomerTokInfoSvr dao = new CCCustomerTokInfoSvr())
                {
                    bReturn = dao.IsCustomerTokenExists(CustomerID);
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "IsCustomerTokenExists(int CustomerID)");
                return false;
            }
            return bReturn;
        }
        #endregion

        #region PRIMEPOS-2635 31-Jan-2019 JY Added
        public DataTable GetCardPreferences()
        {
            DataTable dt = new DataTable();
            using (CCCustomerTokInfoSvr oCCCustomerTokInfoSvr = new CCCustomerTokInfoSvr())
            {
                dt = oCCCustomerTokInfoSvr.GetCardPreferences();
            }
            return dt;
        }
        #endregion

        #region PRIMEPOS-3004 19-Oct-2021 JY Added
        public DataTable PopulateProcessors()
        {
            DataTable dt = new DataTable();
            using (CCCustomerTokInfoSvr oCCCustomerTokInfoSvr = new CCCustomerTokInfoSvr())
            {
                dt = oCCCustomerTokInfoSvr.PopulateProcessors();
            }
            return dt;
        }

        public DataTable PopulateCardType()
        {
            DataTable dt = new DataTable();
            using (CCCustomerTokInfoSvr oCCCustomerTokInfoSvr = new CCCustomerTokInfoSvr())
            {
                dt = oCCCustomerTokInfoSvr.PopulateCardType();
            }
            return dt;
        }

        public DataTable GetCreditCardProfiles(int CustomerID, string strProcessor, string strCardType, int nStatus, string strLast4DigitsOfCC, string strTokenDateOption, DateTime TokenDate1, DateTime TokenDate2, string strCardExpDateOption, DateTime CardExpDate1, DateTime CardExpDate2)
        {
            DataTable dt = new DataTable();
            using (CCCustomerTokInfoSvr oCCCustomerTokInfoSvr = new CCCustomerTokInfoSvr())
            {
                dt = oCCCustomerTokInfoSvr.GetCreditCardProfiles(CustomerID, strProcessor, strCardType, nStatus, strLast4DigitsOfCC, strTokenDateOption, TokenDate1, TokenDate2, strCardExpDateOption, CardExpDate1, CardExpDate2);
            }
            return dt;
        }
        #endregion
    }
}
