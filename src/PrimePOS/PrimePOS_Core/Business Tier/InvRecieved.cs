using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
//using POS.Resources;
using Resources;
using POS_Core.Resources;

namespace POS_Core.BusinessRules
{
    

    /// <summary>
    /// This is business object class for inventory recieved.
    /// Inventory recieved is the collection recieved inventory ID with respect to dates.
    /// It also contains user, vendor, reference no information.
    /// </summary>
    public class InvRecieved : IDisposable
    {
        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating Departments data.
        /// </summary>
        /// <param name="updates">It is department type dataset class. It contains all information of departments.</param>
        public void Persist(InvRecvHeaderData oInvRecvHData, InvRecvDetailData oInvRecvDData, System.String PONumber, bool isFromPurchaseOrder)
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            try
            {
                InvRecvHeaderSvr oInvRecvHeaderSvr = new InvRecvHeaderSvr();
                InvRecvDetailSvr oInvRecvDetailSvr = new InvRecvDetailSvr();

                checkIsValidData(oInvRecvHData, oInvRecvDData);

                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction();

                System.Int32 RecievedID = 0;
                oInvRecvHeaderSvr.Persist(oInvRecvHData, oTrans, ref RecievedID);
                if (RecievedID > 0)
                {
                    oInvRecvDetailSvr.Persist(oInvRecvDData, oInvRecvHData.InvRecievedHeader[0], oTrans, RecievedID, isFromPurchaseOrder);
                    if (PONumber.Trim() != "")
                    {
                        PurchaseOrder oPO = new PurchaseOrder();
                        oPO.ConfirmInvenotryRecieved(Convert.ToInt32(PONumber));
                    }
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
        public InvRecvDetailData PopulateDetail(System.Int32 InvRecvDetailID)
        {
            try
            {
                using (InvRecvDetailSvr dao = new InvRecvDetailSvr())
                {
                    return dao.Populate(InvRecvDetailID);
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
        public void checkIsValidData(InvRecvHeaderData updates, InvRecvDetailData updatesDD)
        {
            InvRecvHeaderTable table;

            InvRecvHeaderRow oRow;

            oRow = (InvRecvHeaderRow)updates.Tables[0].Rows[0];

            table = (InvRecvHeaderTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (InvRecvHeaderTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((InvRecvHeaderTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((InvRecvHeaderTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (InvRecvHeaderTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((InvRecvHeaderTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((InvRecvHeaderTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (InvRecvHeaderRow row in table.Rows)
            {
                //if (row.VendorID == 0 || row.VendorID.ToString()==null)
                //	ErrorHandler.throwCustomError(POSErrorENUM.InvRecvHeader_VendorIDNotNull); 
                if (row.RefNo.Trim() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.InvRecvHeader_RefNoNotNull);
                if (row.RecvDate.ToString() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.InvRecvHeader_RecvDateNotNull);
            }
            if (updatesDD.Tables[0].Rows.Count <= 0)
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvHeader_RecvDetailMissing);
        }
        /// <summary>
        /// Validate that this ID exist in the database or not.
        /// </summary>
        /// <param name="strValue">Inventory Recieved ID.</param>
        /// <exception cref="fgd">fdh</exception>
        public void Validate_ItemID(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_ItemCodeCanNotNull);
            }
            else
            {
                ItemData oID = new ItemData();
                Item oI = new Item();
                oID = oI.Populate(strValue);
                if (oID == null)
                {
                    throw (new Exception("Invalid Item code"));
                }
                else if (oID.Item.Rows.Count == 0)
                {
                    throw (new Exception("Invalid Item code"));
                }
            }
        }
        /// <summary>
        /// Validate that Quantity is null or empty.
        /// </summary>
        /// <param name="strValue">Quantity of items.</param>
        public void Validate_Qty(string strValue, bool isFromPurchaseOrder) //Sprint-27 - PRIMEPOS-2474 10-Jan-2018 JY Added isFromPurchaseOrder parameter
        {
            //if (strValue.Trim() == "" || strValue == null)    //Sprint-27 - PRIMEPOS-2474 10-Jan-2018 JY Commented
            if (!isFromPurchaseOrder && Configuration.convertNullToInt(strValue) == 0)  //Sprint-27 - PRIMEPOS-2474 10-Jan-2018 JY Added
            {
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_QTyCanNotNull);
            }
        }
        /// <summary>
        /// Check that cost exist or not. If cost is null then it will through an exception.
        /// </summary>
        /// <param name="strValue">Cost of item.</param>
        public void Validate_Cost(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_CostCanNotBeNull);
            }
        }
        /// <summary>
        /// Check the sale price in inventory details entity.
        /// </summary>
        /// <param name="strValue">Sale price.</param>
        public void Validate_SalePrice(string strValue)
        {
            if (strValue.Trim() == "" || strValue == null)
            {
                ErrorHandler.throwCustomError(POSErrorENUM.InvRecvDetail_SalePriceCanNotBeNull);
            }
        }
        #endregion
        /// <summary>
        /// Release resources of invRecieved.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
