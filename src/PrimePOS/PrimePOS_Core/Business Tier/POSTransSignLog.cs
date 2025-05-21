namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;
    //using POS.Resources;
    using PharmData;
    using Infragistics.Win.UltraWinGrid;
    using Infragistics.Win;
    using System.Drawing;
    using System.Collections;
    using System.ComponentModel;
    using System.Windows.Forms;
    using POS_Core.BusinessRules;
    using Infragistics.Win.UltraWinMaskedEdit;
    using Infragistics.Win.CalcEngine;
    using Infragistics.Win.UltraWinCalcManager;
    //using POS.UI;
    using System.IO;
    using System.Drawing.Imaging;
    //using POS_Core_UI.Reports.ReportsUI;
    using MMSChargeAccount;
    using MMS.Encryption;
    using NLog;

    /// <summary>
    /// Added By Shitaljit on 3 May 2012 to store Payment related signatures log
    /// </summary>
    public class POSTransSignLog
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        public void Persist(POSTransSignLogData updates, System.Int32 TransID, IDbTransaction oTrans)
        {
            try
            {
                using (POSTransSignLogSvr dao = new POSTransSignLogSvr())
                {
                    checkIsValidData(updates);
                    dao.Persist(updates, oTrans, TransID);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(POSTransSignLogData updates, System.Int32 TransID, IDbTransaction oTrans)");
                throw (ex);
            }
        }
        #endregion


        #region Validation Methods
        public void checkIsValidData(POSTransSignLogData updates)
        {
            POSTransSignLogTable table;
            POSTransSignLogRow oRow = null;

            oRow = (POSTransSignLogRow)updates.Tables[0].Rows[0];

            table = (POSTransSignLogTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (POSTransSignLogTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((POSTransSignLogTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((POSTransSignLogTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (POSTransSignLogTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((POSTransSignLogTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((POSTransSignLogTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

        }

        #endregion

        #region Get Methods

        public POSTransSignLogData Populate(System.Int32 TransLogID)
        {
            try
            {
                using (POSTransSignLogSvr dao = new POSTransSignLogSvr())
                {
                    return dao.Populate(TransLogID);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "POSTransSignLogData Populate(System.Int32 TransLogID)");
                throw (ex);
            }
        }
        public POSTransSignLogData PupulateByTransID(System.Int32 TransId)
        {
            try
            {
                using (POSTransSignLogSvr dao = new POSTransSignLogSvr())
                {
                    return dao.PupulateByTransID(TransId);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "POSTransSignLogData PupulateByTransID(System.Int32 TransId)");
                throw (ex);
            }

        }
        public POSTransSignLogData PopulateList(string whereClause)
        {
            try
            {
                using (POSTransSignLogSvr dao = new POSTransSignLogSvr())
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

        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}