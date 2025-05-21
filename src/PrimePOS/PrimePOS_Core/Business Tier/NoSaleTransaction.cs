using POS_Core.Data_Tier;
using POS_Core.Resources;
using Resources;
using System;
using System.Data;

namespace POS_Core.BusinessRules
{
    public class NoSaleTransaction : IDisposable
    {
        NoSaleTransactionSvr trDetSvr = null;

        #region GetMethod
        public DataSet GetNoSaleTransactionLog(DateTime dtFrom, DateTime dtTo, string stationID = null, string userID = null)
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                trDetSvr = new NoSaleTransactionSvr();
                dsCcdata = trDetSvr.GeNoSaleTransaction(dtFrom, dtTo, stationID, userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }

        public DataSet getStationID()
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                trDetSvr = new NoSaleTransactionSvr();
                dsCcdata = trDetSvr.GetStation();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }

        public void SetNoSaleTransaction(String UserId, String StationId)
        {
            NoSaleTransactionSvr oNoSale = new NoSaleTransactionSvr();
            oNoSale.SetNoSaleTransactionDetail(UserId, StationId);
        }

        public DataSet getUser()
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                trDetSvr = new NoSaleTransactionSvr();
                dsCcdata = trDetSvr.GetUser();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
        public void Persist(DataSet updates)
        {
            System.Data.IDbTransaction oTrans = null;
            System.Data.IDbConnection oConn = null;
            oConn = DataFactory.CreateConnection(Configuration.ConnectionString);
            oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted);

            NoSaleTransactionSvr oNoSale = new NoSaleTransactionSvr();
            oNoSale.Persist(updates, oTrans);

        }
    }
}
