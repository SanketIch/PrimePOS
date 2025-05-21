using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
namespace POS_Core.BusinessRules
{
   

    /// <summary>
    /// This is business object class for inventory recieved.
    /// Inventory recieved is the collection recieved inventory ID with respect to dates.
    /// It also contains user, vendor, reference no information.
    /// </summary>
    public class WarningMessages : IDisposable
    {
        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating Departments data.
        /// </summary>
        /// <param name="updates">It is department type dataset class. It contains all information of departments.</param>
        public void Persist(WarningMessagesData oWarningMsgHData,WarningMessagesDetailData oWarningMsgDData)
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            try
            {
                WarningMessagesSvr oWarningMsgHeaderSvr = new WarningMessagesSvr();
                WarningMessagesDetailSvr oWarningMsgDetailSvr = new WarningMessagesDetailSvr();

                checkIsValidData(oWarningMsgHData,oWarningMsgDData);

                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction();

                System.Int32 HeaderID = 0;
                oWarningMsgHeaderSvr.Persist(oWarningMsgHData, oTrans, ref HeaderID);
                if (HeaderID> 0)
                {
                    oWarningMsgDetailSvr.Persist(oWarningMsgDData,  oTrans, HeaderID);
                    oTrans.Commit();
                }
            }
            catch (Exception ex)
            {
                if (oTrans != null)
                    oTrans.Rollback();
                throw (ex);
            }
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Get inventory recieved information according to its ID.
        /// </summary>
        /// <param name="InvRecvDetailID">Id of inventory recieved.</param>
        /// <returns>InvRecvDetailData type dataset</returns>
        public WarningMessagesDetailData PopulateDetail(System.Int32 WarningMessageID)
        {
            try
            {
                using (WarningMessagesDetailSvr dao = new WarningMessagesDetailSvr())
                {
                    return dao.Populate(WarningMessageID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public WarningMessagesData Populate(System.Int32 WarningMessageID)
        {
            try
            {
                using (WarningMessagesSvr dao = new WarningMessagesSvr())
                {
                    return dao.Populate(WarningMessageID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public WarningMessagesData GetByItemID(System.String sItemID)
        {
            try
            {
                using (WarningMessagesSvr dao = new WarningMessagesSvr())
                {
                    return dao.GetByItemID(sItemID);
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
        /// Validates refNo not null, Date and recieved details.
        /// </summary>
        /// <param name="updates">Provide main inventory recieved data.</param>
        /// <param name="updatesDD">Detail data related to inventory detail collection.</param>
        public void checkIsValidData(WarningMessagesData updates,WarningMessagesDetailData updatesDD)
        {
            
            WarningMessagesTable table;

            WarningMessagesRow oRow;

            oRow = (WarningMessagesRow)updates.Tables[0].Rows[0];

            table = (WarningMessagesTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (WarningMessagesTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((WarningMessagesTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((WarningMessagesTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (WarningMessagesTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((WarningMessagesTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((WarningMessagesTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (WarningMessagesRow row in table.Rows)
            {
                if (row.WarningMessage.Trim() == "")
                    throw( new Exception("Warning Message cannot be empty."));
                
            }
            /*if (updatesDD.Tables[0].Rows.Count <= 0)
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvHeader_RecvDetailMissing);
            */
        }

        #endregion

        /// <summary>
        /// Release resources of WarningMessages.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
