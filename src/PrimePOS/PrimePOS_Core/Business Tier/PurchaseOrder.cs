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
	

    public enum PurchseOrdStatus
    {
        Incomplete,
        Pending,
        Queued,
        Submitted,
        Canceled,
        Acknowledge,
        AcknowledgeManually,
        MaxAttempt,
        Processed,
        Expired,
        PartiallyAck,
        PartiallyAckReorder,
        Error,
        Overdue,
        SubmittedManually,
        //Added by SRT (Sachin) Date: 17 Feb 2010
        DirectAcknowledge,
        //End of Added by SRT (Sachin) Date: 17 Feb 2010
        DeliveryReceived, // added by atul 15-oct-2010
        DirectDelivery, //Added by shitaljit for 810 file
        All
    }

	public class PurchaseOrder : IDisposable 
	{

		#region Persist Methods

        private bool isInserted = true;
               
		public void Persist(POHeaderData oPOHData,PODetailData oPODData)
		{
			System.Data.IDbConnection oConn=null;
			System.Data.IDbTransaction oTrans=null;
			try 
			{
				oConn=DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
				oTrans=oConn.BeginTransaction();
				if (Persist(oPOHData,oPODData,oTrans))
				    oTrans.Commit();
                else
                    oTrans.Rollback();  //PRIMEPOS-3030 26-Nov-2021 JY Added
            } 
			catch(Exception ex) 
			{
				if (oTrans!=null)
					oTrans.Rollback();
				throw(ex);
			}
		}

        public bool Persist(POHeaderData oPOHData, PODetailData oPODData, IDbTransaction oTrans)
        {
            bool bStatus = true;   //PRIMEPOS-3030 26-Nov-2021 JY Added
            POHeaderSvr oPOHeaderSvr = new POHeaderSvr();
            PODetailSvr oPODetailSvr = new PODetailSvr();

            checkIsValidData(oPOHData, oPODData);
            System.Int32 OrderID = 0;
            if (isInserted == true)
            {
                oPOHeaderSvr.Persist(oPOHData, oTrans, ref OrderID);
                if (OrderID > 0)
                    bStatus = oPODetailSvr.Persist(oPODData, oTrans, OrderID);
                else
                    bStatus = false;
                oPOHData.POHeader.Rows[0][clsPOSDBConstants.POHeader_Fld_OrderID] = OrderID;
            }
            else
            {
                oPOHeaderSvr.Persist(oPOHData, oTrans, ref OrderID);
                if (OrderID > 0)
                    bStatus = oPODetailSvr.Persist(oPODData, oTrans, OrderID);
                else
                    bStatus = false;
            }
            return bStatus;
        }

        /// <summary>
        /// To put the POs on hold
        /// </summary>
        /// <param name="oPOHData"></param>
        /// <param name="oPODData"></param>
        public void PutOnHold(POHeaderData oPOHData, PODetailData oPODData)
        {
            System.Data.IDbConnection oConn = null;
            System.Data.IDbTransaction oTrans = null;
            POHeaderSvr oPOHeaderSvr = new POHeaderSvr();
            PODetailSvr oPODetailSvr = new PODetailSvr();

            checkIsValidData(oPOHData, oPODData);
            System.Int32 OrderID = 0;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                oTrans = oConn.BeginTransaction();
                checkIsValidData(oPOHData, oPODData);
                oPOHeaderSvr.PutOnHold(oPOHData, oTrans, ref OrderID);
                if (OrderID > 0)
                {
                    oPODetailSvr.PutOnHold(oPODData, oTrans, OrderID);
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

        public int DeletePOHeader(System.Int32 OrderID)
        {
            try
            {
                using (POHeaderSvr oPOSvr = new POHeaderSvr())
                {
                    return oPOSvr.DeletePOHeader(OrderID.ToString());
                }                
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public int DeletePODetail(System.Int32 PODetailID)
        {
            try
            {
                using (PODetailSvr dao = new PODetailSvr())
                {
                    return dao.DeletePODetails(PODetailID.ToString()); 
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
		public void ClosePO(System.Int32 OrderID)
		{
			POHeaderSvr oPOSvr=new POHeaderSvr();
			oPOSvr.CloseStatus(OrderID);
		}

		public void ConfirmInvenotryRecieved(System.Int32 OrderID)
		{
			POHeaderSvr oPOHeader=new POHeaderSvr();
			oPOHeader.ConfirmInventoryRecieved(OrderID);
		}
		#endregion

		#region Get Methods

		public  PODetailData PopulateDetail(System.Int32 PODetailID, Boolean bSkipIncompleteItems = false)  //Sprint-27 - PRIMEPOS-2026 13-Oct-2017 JY Added optional parameter
        {
			try 
			{
				using(PODetailSvr dao = new PODetailSvr()) 
				{
					return dao.Populate(PODetailID, bSkipIncompleteItems);  //Sprint-27 - PRIMEPOS-2026 13-Oct-2017 JY Added optional parameter
                }
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}

        /// <summary>
        /// Author: Shitaljit 
        /// To get recodrs from OnHold Detail table
        /// </summary>
        /// <param name="PODetailID"></param>
        /// <returns></returns>
        public PODetailData PopulateDetailFromHold(System.Int32 PODetailID)
        {
            try
            {
                using (PODetailSvr dao = new PODetailSvr())
                {
                    return dao.PopulateDetailFromHold(PODetailID);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        //Added By Gaurav
        public PODetailData AutoPOForcedVendor(System.String vendorCode,System.Boolean byReorderLevel,System.Int16 noOfDays, System.DateTime fromDate)
        {
            try
            {
                using (PODetailSvr dao = new PODetailSvr())
                {
                    string sqlQuery = string.Empty;
                    try
                    {
                            sqlQuery = " SELECT " +
                                " itm.ItemID " +
                                " , itm.Description " +
                                " , Vendor.VendorCode " +
                                " , itm.QtyInStock " +
                                " , itm.ReOrderLevel " +
                                " , Sum(Qty) QtySold100Days " +
                                " , Sum(Qty) as newOrder " +
                                " FROM " +
                                " Item itm " +
                                " left outer join itemVendor on (itemVendor.ItemID=Itm.ItemID) " +
                                " inner join vendor on (vendor.vendorid=itemvendor.vendorid) " +
                                " , POSTransaction  Trans " +
                                " , POSTransactionDetail TransDetail " +
                                " WHERE " +
                                " Trans.TransID = TransDetail.TransID " +
                                " AND itm.ItemID = TransDetail.ItemID " +
                                " AND isnull(itm.ExclFromAutoPO,0) = 0 ";

                            if (byReorderLevel)
                            {
                                sqlQuery += " AND Trans.TransDate >= DateAdd(D,-" + noOfDays.ToString() + ",getdate()) ";
                            }
                            else
                            {
                                sqlQuery += " AND Trans.TransDate >= cast('" + DateTime.Parse(fromDate.ToString()).ToString() + "' as datetime) ";
                            }

                            sqlQuery += " AND Vendor.VendorCode='" + vendorCode + "'";


                            sqlQuery += " Group By " +
                                " itm.ItemID " +
                                " , itm.Description " +
                                " , Vendor.VendorCode " +
                                " , itm.QtyInStock " +
                                " , itm.ReOrderLevel ";
                        
                    }
                    catch (Exception Ex)
                    {
                    }
                    return dao.Populate(sqlQuery);
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return null;
        }
        public PODetailData AutoPOForcedVendor(System.String vendorCode, System.DateTime fromDate)
        {
            try
            {
                using (PODetailSvr dao = new PODetailSvr())
                {
                    string sqlQuery = string.Empty;
                    try
                    {
                        sqlQuery = " SELECT " +
                       " itm.ItemID " +
                       " , itm.Description " +
                       " , itm.QtyInStock " +
                       " , itm.ReOrderLevel " +
                       " , itm.ReOrderLevel as newOrder " +
                       " FROM " +
                       " Item itm " +
                       " left outer join itemVendor on (itemVendor.ItemID=Itm.ItemID) " +
                       " inner join vendor on (vendor.vendorid=itemvendor.vendorid) " +
                       " WHERE " +
                       " itm.QtyInStock < itm.ReOrderLevel " +
                       " and isnull(itm.ExclFromAutoPO,0) = 0" +
                       " AND itm.ReOrderLevel>0";


                        sqlQuery += " AND Vendor.VendorCode='" + vendorCode + "'";
                    }
                    catch (Exception Ex)
                    {
                    }

                    return dao.Populate(sqlQuery);
                }
            }

            catch (Exception ex)
            {
                throw (ex);
            }
            return null;
        }
        private void AutoPOForcedVendor()
        {
            
        }
		public  POHeaderData PopulateHeader(System.Int32 POID) 
		{
			try 
			{
				using(POHeaderSvr dao = new POHeaderSvr()) 
				{
					return dao.Populate(POID);
				}
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}

		public  String GetNextPONumber() 
		{
			try 
			{
				using(POHeaderSvr dao = new POHeaderSvr()) 
				{
					return dao.GetNextPONumber();
				}
			} 
			catch(Exception ex) 
			{
				throw(ex);
			}
		}
        public String GetNextPOID()
        {
            try
            {
                using (POHeaderSvr dao = new POHeaderSvr())
                {
                    return dao.GetNextPOID();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public String GetNextPODID()
        {
            try
            {
                using (PODetailSvr dao = new PODetailSvr())
                {
                    return dao.GetNextPODID();
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }


		#endregion

        //Added BY   SRT(Abhishek) Date : 01/10/2009 SAT

        public POHeaderData PopulateUnAcknowledged()
        {
            try
            {
                using (POHeaderSvr dao = new POHeaderSvr())
                {
                    IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                    return dao.PopulateList(" AND status=4");
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //End of Added BY   SRT(Abhishek) Date : 01/10/2009 SAT        

		#region Validation Methods
		public void checkIsValidData(POHeaderData updates, PODetailData details) 
		{
			POHeaderTable table;

			POHeaderRow oRow ;
				
			oRow = (POHeaderRow)updates.Tables[0].Rows[0];

			table = (POHeaderTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
            {
                table = (POHeaderTable)updates.Tables[0].GetChanges(DataRowState.Modified);
                isInserted = false;
            }
            else if ((POHeaderTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
            {
                table.MergeTable((POHeaderTable)updates.Tables[0].GetChanges(DataRowState.Modified));
                isInserted = false;
            }
		  
			if (table == null)
				table= (POHeaderTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
			else if ((POHeaderTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
				table.MergeTable( (POHeaderTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

			if (table==null) return;

			foreach(POHeaderRow row in table.Rows)
			{ 
				if (row.VendorID == 0 || row.VendorID.ToString()==null)
					ErrorHandler.throwCustomError(POSErrorENUM.POHeader_VendorCodeCanNotNull); 
				if (row.OrderNo.ToString().Trim()=="")
					ErrorHandler.throwCustomError(POSErrorENUM.POHeader_OrderNoNotNull); 
				if (row.OrderDate.ToString()=="")
					ErrorHandler.throwCustomError(POSErrorENUM.POHeader_OrderDateCanNotNull); 
				if (row.ExptDelvDate<row.OrderDate)
					ErrorHandler.throwCustomError(POSErrorENUM.POHeader_ExpectedDlryDateShouldBeGreaterThanOrderDate); 
			}
			if (details.PODetail.Rows.Count==0)
				ErrorHandler.throwCustomError(POSErrorENUM.PODetail_AtleastOneDetailItemReq); 
		}

		public void Validate_ItemID(string strValue)
		{
			if (strValue.Trim()=="" || strValue==null )
			{
				ErrorHandler.throwCustomError(POSErrorENUM.PODetail_ItemCodeCanNotNull); 
			}
		}

		public void Validate_Qty(string strValue)
		{
            if (strValue.Trim() == "" || strValue == null || strValue == "______" || strValue == "0_____")
			{
				ErrorHandler.throwCustomError(POSErrorENUM.PODetail_QTYCanNotNull); 
			}
		}

		public void Validate_Cost(string strValue)
		{
			if (strValue.Trim()=="" || strValue==null )
			{
				ErrorHandler.throwCustomError(POSErrorENUM.PODetail_CostCanNotBeNull); 
			}
		}
		
		#endregion

        //Added By (SRT)Abhishek  Date : 01/15/2009 

        public POHeaderData PopulateList(System.String POID)
        {
            using (POHeaderSvr poSvr = new POHeaderSvr())
            {
                return (poSvr.PopulateList(POID));
            }
        }

        #region Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added
        public POHeaderData PopulateListP(System.String POID)
        {
            using (POHeaderSvr poSvr = new POHeaderSvr())
            {
                return (poSvr.PopulateListP(POID));
            }
        }
        #endregion

        //End of Added By (SRT)Abhishek  Date : 01/15/2009 
        //Added By SRT(Gaurav)
        public PODetailData AutoPOData(string strQuery)
        {
            using (PODetailSvr oPOD=new PODetailSvr())
            {
                bool result=false;
                return(oPOD.PopulateList(strQuery,out result));
            }
        }
        //End Of Added By SRT(Gaurav)

        /// <summary>
        /// Author: Shitaljit 
        /// Added  to get list of POs from PurchaseOrder_OnHold table
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public POHeaderData PopulateListFromHold(System.String whereClause)
        {
            using (POHeaderSvr poSvr = new POHeaderSvr())
            {
                return (poSvr.PopulateListFromHold(whereClause));
            }
        }

		public void Dispose() 
		{
			GC.SuppressFinalize(true);
		}

	}
}
