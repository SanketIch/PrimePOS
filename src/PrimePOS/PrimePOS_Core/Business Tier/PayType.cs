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
    

    public class PayType : IDisposable
    {

        #region Persist Methods


        public void Persist(PayTypeData updates)
        {
            try
            {
                using (PayTypeSvr dao = new PayTypeSvr())
                {
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

        public PayTypeData Populate(System.String PayTypeID)
        {
            try
            {
                IDbConnection oconn = DataFactory.CreateConnection(Configuration.ConnectionString);
                using (PayTypeSvr dao = new PayTypeSvr())
                {
                    return dao.Populate(PayTypeID, oconn);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public PayTypeData PopulateList(string whereClause)
        {
            try
            {
                IDbConnection oconn = DataFactory.CreateConnection(Configuration.ConnectionString);
                using (PayTypeSvr dao = new PayTypeSvr())
                {
                    return dao.PopulateList(whereClause, oconn);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //PRIMEPOS-2966 20-May-2021 JY Added
        public System.Int32 GetNextSortOrder()
        {
            try
            {
                using (PayTypeSvr dao = new PayTypeSvr())
                {
                    return dao.GetNextSortOrder();
                }
            }
            catch (Exception ex)
            {
                //throw (ex);
                return 0;
            }
        }

        public System.Int32 GetCreditCardSortOrder()
        {
            try
            {
                using (PayTypeSvr dao = new PayTypeSvr())
                {
                    return dao.GetCreditCardSortOrder();
                }
            }
            catch (Exception ex)
            {
                //throw (ex);
                return 0;
            }
        }
        #endregion


        #region Validation Methods

        public virtual void checkIsValidPrimaryKey(PayTypeData updates)
        {
            PayTypeTable table = (PayTypeTable)updates.Tables[clsPOSDBConstants.PayType_tbl];
            foreach (PayTypeRow row in table.Rows)
            {
                if (this.Populate(row.PaytypeID).Tables[clsPOSDBConstants.PayType_tbl].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for Paytypes. ");
                }
            }
        }

        #endregion

        public bool DeleteRow(string PaytypeID)
        {
            try
            {
                using (PayTypeSvr dao = new PayTypeSvr())
                {
                    return dao.DeleteRow(PaytypeID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
