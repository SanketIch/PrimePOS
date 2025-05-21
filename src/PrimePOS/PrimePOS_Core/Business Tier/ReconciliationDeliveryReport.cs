using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.BusinessRules
{
    // PRIMERX-7688 - Added new file by NileshJ 25-Sept-2019
    public class ReconciliationDeliveryReport : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public DataTable GetBatchDeliveryDetails(string BatchNo)
        {
            DataTable dt = new DataTable();
            try
            {
                PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                dt = oPharmBL.GetBatchDeliveryDetails(BatchNo);
            }
            catch (Exception Ex)
            {
                return null;
            }
            return dt;
        }

        public DataTable GetBatchDeliveryPatient(string BatchNo)
        {
            DataTable dt = new DataTable();
            try
            {
                PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                dt = oPharmBL.GetBatchDeliveryPatient(BatchNo);
            }
            catch (Exception Ex)
            {
                return null;
            }
            return dt;
        }

        public DataTable GetBatchDeliveryRx(string BatchNo)
        {
            DataTable dt = new DataTable();
            try
            {
                PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                dt = oPharmBL.GetBatchDeliveryRx(BatchNo);
            }
            catch (Exception Ex)
            {
                return null;
            }
            return dt;
        }

        public DataTable GetBatchDeliveryData(DateTime dtFrom, DateTime dtTo)
        {
            DataTable dt = new DataTable();
            try
            {
                PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                dt = oPharmBL.GetBatchDeliveryData(dtFrom, dtTo);
            }
            catch (Exception Ex)
            {
                return null;
            }
            return dt;
        }

        public void UpdateBatchDeliveryPaymentStatus(DataTable dtBatchDeliveryData)
        {
            DataTable dt = new DataTable();
            try
            {
                PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                oPharmBL.UpdateBatchDeliveryPaymentStatus(dtBatchDeliveryData);
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "An Error Occured in UpdateBatchDeliveryPaymentStatus() ");
                throw Ex;
            }
        }

        public void UpdateBatchDeliveryOrderPaymentStatus(DataTable dtBatchDeliveryData)
        {
            DataTable dt = new DataTable();
            try
            {
                PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                oPharmBL.UpdateBatchDeliveryOrderPaymentStatus(dtBatchDeliveryData);
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "An Error Occured in UpdateBatchDeliveryOrderPaymentStatus()");
                throw Ex;
            }
        }

        public void UpdateBatchDeliveryDetailPaymentStatus(DataTable dtBatchDeliveryData)
        {
            DataTable dt = new DataTable();
            try
            {
                PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
                oPharmBL.UpdateBatchDeliveryDetailPaymentStatus(dtBatchDeliveryData);
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "An Error Occured in UpdateBatchDeliveryDetailPaymentStatus()");
                throw Ex;
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
