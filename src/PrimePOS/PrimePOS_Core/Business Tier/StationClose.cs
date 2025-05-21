// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;


    /// <summary>
    /// StationCloses Business Rules Class  
    /// </summary>
    public class StationClose
    {


        public CloseStationData Close(String DrawNo)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                return oStationClose.closeWorkstation(DrawNo);
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

            }
            return null;
        }

        #region PRIMEPOS-3042 22-Dec-2021 JY Added
        public int CloseStation(String strStationID, ref string strErrMsg)
        {
            int StationCloseID = 0;
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                StationCloseID = oStationClose.CloseStation(strStationID, ref strErrMsg);                
            }
            catch (POSExceptions ex)
            {
                strErrMsg = ex.Message;
            }
            catch (OtherExceptions ex)
            {
                strErrMsg = ex.Message;
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
            }
            return StationCloseID;
        }
        #endregion

        public CloseStationData CurrentStatus(String DrawNo)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                return oStationClose.closeWorkstation(DrawNo);
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

            }
            return null;
        }

        /// <summary>
        /// Added By Shitaljit to check current cash in the drawer
        /// </summary>
        /// <param name="DrawNo"></param>
        /// <returns></returns>
        public Decimal CurrentCashStatus(String DrawNo)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                return oStationClose.CurrentCashStatus(DrawNo);
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

            }
            return 0;
        }
        public CloseStationData FillData(int closeStationId)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                return oStationClose.FillCloseStationData(closeStationId);
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
            }
            return null;
        }

        public DataSet GetReportSource(int pStationCloseId)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                return oStationClose.GetReportSource(pStationCloseId);
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
            }
            return null;
        }

        public DataSet GetSubReportSource(int pStationCloseId)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                return oStationClose.GetSubReportSource(pStationCloseId);
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
            }
            return null;
        }
        //Added By Shitaljit(QuicSolv) on 13 June 2011
        public DataSet PopulateCurrencyList()
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                return oStationClose.PopulateCurrencyList();
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
            }
            return null;
        }


        public decimal GetUserEnterStationCloseCash(int stationCloseID)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                return oStationClose.GetUserEnterStationCloseCash(stationCloseID);
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
            }
            return -1;
        }
        public void SaveStationCloseCashDeatil(int CurrencyID, int count, Decimal total)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                oStationClose.SaveStationCloseCashDeatil(CurrencyID, count, total);
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
            }
        }
        public DataSet GetStationCloseCashDetail(int stationCloseID)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                return oStationClose.GetStationCloseCashDetail(stationCloseID);
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
            }
            return null;
        }
        public void VerifyStationCloseCash(int stationCloseID, int CurrencyID, int count, decimal verifyTotal)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                oStationClose.VerifyStationCloseCash(stationCloseID, CurrencyID, count, verifyTotal);
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
            }

        }
        public void UpdateMaster(int StationCloseID, decimal VerifiedAmt)
        {
            try
            {
                StationCloseSvr oStationClose = new StationCloseSvr();
                oStationClose.UpdateMaster(StationCloseID, VerifiedAmt);
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
            }

        }

        #region Sprint-24 - PRIMEPOS-2326 20-Oct-2016 JY Added
        public DataSet GetDepartmentDS(int iStationCloseId)
        {
            try
            {
                StationCloseSvr oStationCloseSvr = new StationCloseSvr();
                return oStationCloseSvr.GetDepartmentDS(iStationCloseId);
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
            }
            return null;
        }
        #endregion
    }
}
