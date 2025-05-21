using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using System.Data;

namespace POS_Core.BusinessRules
{
    /// <summary>
    /// Summary description for Message Template
    /// </summary>
    public class MsgTemplate : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public delegate void DataRowSavedHandler();
        public event DataRowSavedHandler DataRowSaved;

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating, Delete MsgTemplate data it will delete rows from database which has been deleted from dataset. 
        /// </summary>
        /// <param name="updates">It is MsgTemplate type dataset class. It contains all information of MsgTemplate.</param>
        public void Persist(MsgTemplateData updates)
        {
            Persist(updates, false);
        }

        public void Persist(MsgTemplateData updates, bool ignoreValidation)
        {
            try
            {
                logger.Trace("Persist() - " + clsPOSDBConstants.Log_Entering);
                if (ignoreValidation == false)
                {
                    checkIsValidData(updates);
                }
                using (MsgTemplateSvr dao = new MsgTemplateSvr())
                {
                    dao.DataRowSaved += new MsgTemplateSvr.DataRowSavedHandler(dao_DataRowSaved);
                    dao.Persist(updates);
                }
                logger.Trace("Persist() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist()");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist()");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(MsgTemplateData updates, bool ignoreValidation)");
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        void dao_DataRowSaved()
        {
            RaiseDataRowSaved();
        }
        #endregion

        #region Validation Methods
        public void checkIsValidData(MsgTemplateData updates)
        {
            MsgTemplateTable table;
            MsgTemplateRow oRow;

            oRow = (MsgTemplateRow)updates.Tables[0].Rows[0];
            table = (MsgTemplateTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (MsgTemplateTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((MsgTemplateTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((MsgTemplateTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (MsgTemplateTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((MsgTemplateTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((MsgTemplateTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;
        }
        #endregion

        public bool DeleteRow(string CurrentID)
        {
            //System.Data.IDbConnection oConn = null;
            try
            {
                logger.Trace("DeleteRow() - " + clsPOSDBConstants.Log_Entering);
                bool retValue;
                //oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                using (MsgTemplateSvr dao = new MsgTemplateSvr())
                {
                    retValue = dao.DeleteRow(CurrentID);
                }
                RaiseDataRowSaved();
                logger.Trace("DeleteRow() - " + clsPOSDBConstants.Log_Exiting);
                return retValue;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow()");
                throw (ex);
            }
        }

        private void RaiseDataRowSaved()
        {
            if (DataRowSaved != null)
            {
                DataRowSaved();
            }
        }

        #region Get Methods
        public MsgTemplateData Populate(System.Int32 RecID)
        {
            try
            {
                using (MsgTemplateSvr dao = new MsgTemplateSvr())
                {
                    return dao.Populate(RecID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        /// <summary>
        /// Dispose customer contents.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
