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
    /// This is Business Tier Class for EndofDay entity.
    /// This class contains all business rules related to EndofDay entity.
    /// </summary>
    public class EndOFDay
    {
        /// <summary>
        /// This function will check that all stations have been closed or not.
        /// It updates end of day operations.
        /// </summary>
        /// <returns>Endofday type dataset</returns>
        public EndOFDayData ProcessEndOFDay()
        {
            try
            {
                EndOFDaySvr oEndOFDay = new EndOFDaySvr();
                return oEndOFDay.EndOFDay();
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
        /// Get end of day data according to its ID.
        /// </summary>
        /// <param name="pEndOfDayId">Id for record of End of day</param>
        /// <returns>End of day entity records.</returns>
        public DataSet GetReportSource(int pEndOfDayId)
        {
            try
            {
                EndOFDaySvr oEndOfDay = new EndOFDaySvr();
                return oEndOfDay.GetReportSource(pEndOfDayId);
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
        /// 
        /// </summary>
        /// <param name="tr"></param>
        /// <param name="conn"></param>
        public string[] CheckIfAllStationClosed()
        {
            try
            {
                EndOFDaySvr oEndOfDay = new EndOFDaySvr();
                return oEndOfDay.CheckIfAllStationClosed();
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
        public DataSet GetSubReportSource(int pEndOfDayId)
        {
            try
            {
                EndOFDaySvr oEndOfDay = new EndOFDaySvr();
                return oEndOfDay.GetSubReportSource(pEndOfDayId);
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

        public DataSet GetDepartmentSource(int pEndOfDayId)
        {
            try
            {
                EndOFDaySvr oEndOfDay = new EndOFDaySvr();
                return oEndOfDay.GetDepartmentSource(pEndOfDayId);
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

        public DataSet GetDepartmentDS(int pEndOfDayId)
        {
            try
            {
                EndOFDaySvr oEndOfDay = new EndOFDaySvr();
                return oEndOfDay.GetDepartmentDS(pEndOfDayId);
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
        /// Added By Amit Date 13 May 2011
        /// </summary>
        /// <param name="pEndOfDayId"></param>
        /// <returns></returns>
        public DataSet GetStationDS(int pEndOfDayId)
        {
            try
            {
                EndOFDaySvr oEndOfDay = new EndOFDaySvr();
                return oEndOfDay.GetStationDS(pEndOfDayId);
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

        public EndOFDayData FillData(int EODID)
        {
            try
            {
                EndOFDaySvr oEODSvr = new EndOFDaySvr();
                return oEODSvr.FillEndOFDayData(EODID);
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
        #region  RX,Taxable,Non-Taxable Sale Details.
        public DataSet GetRxSalesDetails(int EODID)
        {
            try
            {
                EndOFDaySvr oEODSvr = new EndOFDaySvr();
                return oEODSvr.GetRxSalesDetails(EODID);
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

        public DataSet GetTaxableItemSalesDetails(int EODID)
        {
            try
            {
                EndOFDaySvr oEODSvr = new EndOFDaySvr();
                return oEODSvr.GetTaxableItemSalesDetails(EODID);
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

        public DataSet GetNonTaxableItemsSalesDetails(int EODID)
        {
            try
            {
                EndOFDaySvr oEODSvr = new EndOFDaySvr();
                return oEODSvr.GetNonTaxableItemSalesDetails(EODID);
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
