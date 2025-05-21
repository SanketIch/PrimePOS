using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
//using POS.Resources;
namespace POS_Core.DataAccess
{

	// Provides data access methods for DeptCode
 
	public class POHeaderSvr: IDisposable  
	{

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public void Persist(DataSet updates, IDbTransaction tx,ref System.Int32 OrderID) 
		{
			try 
			{
				this.Insert(updates, tx,ref OrderID);
				this.Update(updates, tx,ref OrderID);
                this.Delete(updates, tx, ref OrderID);   
				updates.AcceptChanges();
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}
			catch(OtherExceptions ex) 
			{
				throw(ex);
			}
			catch(Exception ex) 
			{
				ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}


        public void PutOnHold(DataSet updates, IDbTransaction tx, ref System.Int32 OrderID)
        {
            try
            {
                this.InsertOnHold(updates, tx, ref OrderID);
                updates.AcceptChanges();
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
                ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }

		#endregion

		#region Get Methods

		// Looks up a DeptCode based on its primary-key:System.Int32 DeptCode

		public POHeaderData Populate(System.Int32 OrderID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
									+ clsPOSDBConstants.POHeader_Fld_OrderID
									+ " , " +  clsPOSDBConstants.POHeader_Fld_OrderNo
                                    + " , POH." + clsPOSDBConstants.POHeader_Fld_Description 
									+ " , " +  clsPOSDBConstants.POHeader_Fld_OrderDate
									+ " , " +  clsPOSDBConstants.POHeader_Fld_ExptDelvDate
									+ " , vend." +   clsPOSDBConstants.POHeader_Fld_VendorID + " as " +   clsPOSDBConstants.POHeader_Fld_VendorID 
									+ " , vend." + clsPOSDBConstants.Vendor_Fld_VendorCode + " as " + clsPOSDBConstants.Vendor_Fld_VendorCode
									+ " , " +  clsPOSDBConstants.Vendor_Fld_VendorName
									//+ " , POH." +  clsPOSDBConstants.fld_UserID + " as " + clsPOSDBConstants.fld_UserID
									+ " , POH." +  clsPOSDBConstants.POHeader_Fld_Status + " as " + clsPOSDBConstants.POHeader_Fld_Status
									+ " , POH." +  clsPOSDBConstants.POHeader_Fld_isFTPUsed + " as " + clsPOSDBConstants.POHeader_Fld_isFTPUsed
									+ " , POH." +  clsPOSDBConstants.POHeader_Fld_AckDate + " as " + clsPOSDBConstants.POHeader_Fld_AckDate
									+ " , POH." +  clsPOSDBConstants.POHeader_Fld_AckStatus + " as " + clsPOSDBConstants.POHeader_Fld_AckStatus 
									+ " , POH." +  clsPOSDBConstants.POHeader_Fld_AckType + " as " + clsPOSDBConstants.POHeader_Fld_AckType
                                    + " , POH." + clsPOSDBConstants.POHeader_Fld_PrimePOrderID + " as " + clsPOSDBConstants.POHeader_Fld_PrimePOrderID
                                    //Added by atul 25-oct-2010
                                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceDate + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceDate
                                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceNumber + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceNumber
                                    //End of Added by atul 25-oct-2010
                                      + " , POH." + clsPOSDBConstants.POHeader_Fld_AckFileType + " as " + clsPOSDBConstants.POHeader_Fld_AckFileType//Added By shitaljit  showing ack file type in POack
                                    + " , POH." + clsPOSDBConstants.POHeader_Fld_RefOrderNo + " as " + clsPOSDBConstants.POHeader_Fld_RefOrderNo//Added By shitaljit for showing ref order no in POack
                                    + " , POH." + clsPOSDBConstants.POHeader_Fld_TransTypeCode + " AS " + clsPOSDBConstants.POHeader_Fld_TransTypeCode  //PRIMEPOS-2901 05-Nov-2020 JY Added
                               + " FROM " 
									+ clsPOSDBConstants.POHeader_tbl + " As POH "
									+ " , " + clsPOSDBConstants.Vendor_tbl + " As Vend"
								+ " WHERE " 
									+ " POH." + clsPOSDBConstants.POHeader_Fld_VendorID + " = Vend." + clsPOSDBConstants.Vendor_Fld_VendorId
									+ " AND " + clsPOSDBConstants.POHeader_Fld_OrderID + " ='" + OrderID + "'";


				POHeaderData ds = new POHeaderData();
				ds.POHeader.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
											,  sSQL
											, PKParameters(OrderID)).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch(Exception ex) 
			{
				ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

        public int DeletePOHeader(String orderID)
        {
            IDbConnection conn = null;
            string sSQL = string.Empty;
            int response = 0;
            try
            {
                conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                sSQL = "delete from  " + clsPOSDBConstants.POHeader_tbl + " where OrderID = '" + orderID +"'";
                response = (int)DataHelper.ExecuteNonQuery(conn, CommandType.Text, sSQL);            
            }
            catch (Exception ex)
            {
                return response;
            }
            return response;
        }
        public String GetNextPOID()
        {
            IDbConnection conn = null;
            string sSQL = string.Empty;
            string response = string.Empty;
            int poID = 1;
            try
            {
                conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                sSQL = "Select MAX(OrderID) from " + clsPOSDBConstants.POHeader_tbl;
                response = DataHelper.ExecuteScalar(conn, CommandType.Text, sSQL).ToString();
                if (response != null)
                {
                    poID = Convert.ToInt32(response);
                    poID++;
                }             
            }
            catch (Exception ex)
            {
                return poID.ToString();
            }
            return poID.ToString();
        }

		public String GetNextPONumber() 
		{            
			Object response=null;
            string stationID = string.Empty;
			string POPre=string.Empty;
			int PONumber=1;
            bool isValid = false;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                POPre = DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString().Substring(2, 2);
                string sSQL = "Select MAX(CAST(OrderNO as INT)) from " + clsPOSDBConstants.POHeader_tbl;
                response = DataHelper.ExecuteScalar(conn, CommandType.Text, sSQL);
                if (response == DBNull.Value)
                {
                    return PONumber.ToString(); 
                }
                isValid = Int32.TryParse(response.ToString(),out PONumber);
                if (!isValid)
                {
                    throw new InvalidCastException("Data for field OrderNo in database is Corrupted");
                }            
            }
            catch (InvalidCastException inValidPONO)
            {
                throw inValidPONO;
            }
            catch (Exception)
            {
                try
                {
                    //Added by SRT(Abhishek) Date : 03/09/2009
                    //Added to select max order no depending upon the  POPre order no.
                    IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                    string sSQL = "Select MAX(CAST(OrderNO as INT))from PurchaseOrder where OrderNO like '" + POPre + "%'";
                    response = DataHelper.ExecuteScalar(conn, CommandType.Text, sSQL);
                    if (response == DBNull.Value)
                    {
                        return POPre+PONumber.ToString();
                    }                    
                    return response.ToString();
                    //End of Added by SRT(Abhishek) Date : 03/09/2009
                }
                catch (Exception ex) { throw ex;}
            }
            return PONumber.ToString();
		}

		public POHeaderData Populate(System.Int32 POID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(POID, conn));
			}
		}

		public POHeaderData PopulateList(System.String POID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(PopulateList(POID, conn));
			}
		}

        #region Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added
        public POHeaderData PopulateListP(System.String POID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateListP(POID, conn));
            }
        }
        #endregion

        public POHeaderData PopulateListFromHold(System.String whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateListFromHold(whereClause, conn));
            }
        }
		public POHeaderData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = "Select " 
					+ clsPOSDBConstants.POHeader_Fld_OrderID
					+ " , " +  clsPOSDBConstants.POHeader_Fld_OrderNo
					+ " , " +  clsPOSDBConstants.POHeader_Fld_OrderDate
					+ " , " +  clsPOSDBConstants.POHeader_Fld_ExptDelvDate
					+ " ,  vend." +   clsPOSDBConstants.POHeader_Fld_VendorID + " as " + clsPOSDBConstants.POHeader_Fld_VendorID
					+ " , vend." + clsPOSDBConstants.Vendor_Fld_VendorCode + " as " + clsPOSDBConstants.Vendor_Fld_VendorCode
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_VendorName
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_USEVICForEPO
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_PrimePOVendorID 
					//+ " , POH." +  clsPOSDBConstants.fld_UserID + " as " + clsPOSDBConstants.fld_UserID
					+ " , POH." +  clsPOSDBConstants.POHeader_Fld_Status + " as " + clsPOSDBConstants.POHeader_Fld_Status
					+ " , POH." +  clsPOSDBConstants.POHeader_Fld_isFTPUsed + " as " + clsPOSDBConstants.POHeader_Fld_isFTPUsed
					+ " , POH." +  clsPOSDBConstants.POHeader_Fld_AckDate + " as " + clsPOSDBConstants.POHeader_Fld_AckDate
					+ " , POH." +  clsPOSDBConstants.POHeader_Fld_AckStatus + " as " + clsPOSDBConstants.POHeader_Fld_AckStatus 
					+ " , POH." +  clsPOSDBConstants.POHeader_Fld_AckType + " as " + clsPOSDBConstants.POHeader_Fld_AckType
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_PrimePOrderID + " as " + clsPOSDBConstants.POHeader_Fld_PrimePOrderID
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceDate + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceDate
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceNumber + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceNumber
                    //Added by atul 25-oct-2010
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceDate + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceDate
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceNumber + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceNumber
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_AckFileType + " as " + clsPOSDBConstants.POHeader_Fld_AckFileType//Added By shitaljit  showing ack file type in POack
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_RefOrderNo + " as " + clsPOSDBConstants.POHeader_Fld_RefOrderNo//Added By shitaljit for showing ref order no in POack
                                                                                                                                //End of Added by atul 25-oct-2010
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_TransTypeCode + " AS " + clsPOSDBConstants.POHeader_Fld_TransTypeCode  //PRIMEPOS-2901 05-Nov-2020 JY Added
                    + " FROM " 
					+ clsPOSDBConstants.POHeader_tbl + " As POH "
					+ " , " + clsPOSDBConstants.Vendor_tbl + " As Vend"
					+ " WHERE " 
					+ " POH." + clsPOSDBConstants.POHeader_Fld_VendorID + " = Vend." + clsPOSDBConstants.Vendor_Fld_VendorId;

				if (sWhereClause.Trim()!="")
					sSQL=String.Concat(sSQL,sWhereClause);

				POHeaderData ds = new  POHeaderData();
				ds.POHeader.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
				throw(ex);
			}

			catch(OtherExceptions ex) 
			{
				throw(ex);
			}

			catch (Exception ex) 
			{    
				ErrorHandler.throwException(ex,"",""); 
				return null;
			} 
		}

        #region Sprint-22 - PRIMEPOS-2251 03-Dec-2015 JY Added to display ProcessedBy and ProcessedType
        public POHeaderData PopulateListP(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + clsPOSDBConstants.POHeader_Fld_OrderID
                    + " , " + clsPOSDBConstants.POHeader_Fld_OrderNo
                    + " , " + clsPOSDBConstants.POHeader_Fld_OrderDate
                    + " , " + clsPOSDBConstants.POHeader_Fld_ExptDelvDate
                    + " ,  vend." + clsPOSDBConstants.POHeader_Fld_VendorID + " as " + clsPOSDBConstants.POHeader_Fld_VendorID
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_VendorCode + " as " + clsPOSDBConstants.Vendor_Fld_VendorCode
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_VendorName
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_USEVICForEPO
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_PrimePOVendorID
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_Status + " as " + clsPOSDBConstants.POHeader_Fld_Status
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_isFTPUsed + " as " + clsPOSDBConstants.POHeader_Fld_isFTPUsed
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_AckDate + " as " + clsPOSDBConstants.POHeader_Fld_AckDate
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_AckStatus + " as " + clsPOSDBConstants.POHeader_Fld_AckStatus
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_AckType + " as " + clsPOSDBConstants.POHeader_Fld_AckType
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_PrimePOrderID + " as " + clsPOSDBConstants.POHeader_Fld_PrimePOrderID
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceDate + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceDate
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceNumber + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceNumber
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceDate + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceDate
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceNumber + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceNumber
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_AckFileType + " as " + clsPOSDBConstants.POHeader_Fld_AckFileType
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_RefOrderNo + " as " + clsPOSDBConstants.POHeader_Fld_RefOrderNo
                    + " , c.UserID AS " + clsPOSDBConstants.POHeader_Fld_ProcessedBy + " , d.TypeName AS " + clsPOSDBConstants.POHeader_Fld_ProcessedType
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_TransTypeCode + " AS " + clsPOSDBConstants.POHeader_Fld_TransTypeCode  //PRIMEPOS-2901 05-Nov-2020 JY Added
                    + " FROM " + clsPOSDBConstants.POHeader_tbl + " As POH "
                    + " INNER JOIN " + clsPOSDBConstants.Vendor_tbl + " As Vend ON POH." + clsPOSDBConstants.POHeader_Fld_VendorID + " = Vend." + clsPOSDBConstants.Vendor_Fld_VendorId
                    + " LEFT JOIN InventoryRecieved c on POH.OrderNo = c.POOrderNo "
                    + " INNER JOIN InvTransType d on c.InvTransTypeID = d.ID "
                    + " WHERE 1=1 ";

                if (sWhereClause.Trim() != "")
                    sSQL = String.Concat(sSQL, sWhereClause);

                POHeaderData ds = new POHeaderData();
                ds.POHeader.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                return ds;
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
                return null;
            }
        }
        #endregion

        public POHeaderData PopulateListFromHold(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + clsPOSDBConstants.POHeader_Fld_OrderID
                    + " , " + clsPOSDBConstants.POHeader_Fld_OrderNo
                    + " , " + clsPOSDBConstants.POHeader_Fld_OrderDate
                    + " , " + clsPOSDBConstants.POHeader_Fld_ExptDelvDate
                    + " ,  vend." + clsPOSDBConstants.POHeader_Fld_VendorID + " as " + clsPOSDBConstants.POHeader_Fld_VendorID
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_VendorCode + " as " + clsPOSDBConstants.Vendor_Fld_VendorCode
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_VendorName
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_USEVICForEPO
                    + " , vend." + clsPOSDBConstants.Vendor_Fld_PrimePOVendorID
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_Status + " as " + clsPOSDBConstants.POHeader_Fld_Status
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_isFTPUsed + " as " + clsPOSDBConstants.POHeader_Fld_isFTPUsed
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_AckDate + " as " + clsPOSDBConstants.POHeader_Fld_AckDate
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_AckStatus + " as " + clsPOSDBConstants.POHeader_Fld_AckStatus
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_AckType + " as " + clsPOSDBConstants.POHeader_Fld_AckType
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_PrimePOrderID + " as " + clsPOSDBConstants.POHeader_Fld_PrimePOrderID
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceDate + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceDate
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceNumber + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceNumber
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceDate + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceDate
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_InvoiceNumber + " as " + clsPOSDBConstants.POHeader_Fld_InvoiceNumber
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_AckFileType + " as " + clsPOSDBConstants.POHeader_Fld_AckFileType
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_RefOrderNo + " as " + clsPOSDBConstants.POHeader_Fld_RefOrderNo
                    + " , POH." + clsPOSDBConstants.POHeader_Fld_TransTypeCode + " AS " + clsPOSDBConstants.POHeader_Fld_TransTypeCode  //PRIMEPOS-2901 05-Nov-2020 JY Added
                    + " FROM "
                    + clsPOSDBConstants.POHeaderOnHold_tbl + " As POH "
                    + " , " + clsPOSDBConstants.Vendor_tbl + " As Vend"
                    + " WHERE "
                    + " POH." + clsPOSDBConstants.POHeader_Fld_VendorID + " = Vend." + clsPOSDBConstants.Vendor_Fld_VendorId;

                if (sWhereClause.Trim() != "")
                    sSQL = String.Concat(sSQL, sWhereClause);

                POHeaderData ds = new POHeaderData();
                ds.POHeader.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                return ds;
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
                return null;
            }
        }

       

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx, ref System.Int32 OrderID) 
		 {

			POHeaderTable addedTable = (POHeaderTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (POHeaderRow row in addedTable.Rows) 
				{
					try 
					{
                        #region PRIMEPOS-3030 22-Nov-2021 JY Added
                        try
                        {
                            long OrderNo = Configuration.convertNullToInt64(row.OrderNo);
                            while (true)
                            {
                                POHeaderData oPOHeaderData = PopulateList(" AND OrderNo = '" + OrderNo + "'");
                                if (oPOHeaderData != null && oPOHeaderData.Tables.Count > 0 && oPOHeaderData.POHeader.Rows.Count > 0)
                                    OrderNo += 1;
                                else
                                    break;
                            }
                            row.OrderNo = OrderNo.ToString();
                        }
                        catch(Exception Ex)
                        {
                        }
                        #endregion
                        insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.POHeader_tbl,insParam);
						for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
						}

                        //Added by SRT (Abhishek) Date : 21 Aug 2009
                        //Added specially if order no is alphanumeric.
                        //Added to verify whether order with this order no exist or not 
                        //if order no is already exist then it will throw an exception  
                        //POHeaderData poHederData=PopulateList(" AND OrderNo ='" + row.OrderNo +"'");
                        //if (poHederData != null && poHederData.POHeader.Rows.Count > 0)
                        //{
                            //Commented By shitaljit
                            //if user from different stations trying to Create Purchase Order at the same time
                            //It creates conflict in the orderno 
                            //row.OrderNo = GetNextPONumber();                            
                            // throw new Exception("Purchase Order Can Not be Saved.Please Contact Your Administrator.");
                        //}
                        //End of Added by SRT (Abhishek) Date : 21 Aug 2009  
  
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
						OrderID=Convert.ToInt32(DataHelper.ExecuteScalar(tx,CommandType.Text,"select @@identity"));
					} 
					catch(POSExceptions ex) 
					{
						throw(ex);
					}

					catch(OtherExceptions ex) 
					{
						throw(ex);
					}
					catch(ConstraintException )
					{
						//cExp.
					}
					catch (Exception ex) 
					{
                        ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}
        

        /// <summary>
        /// Author: Shitaljit
        /// Added Date: 5/27/2014
        /// To insert records to PurchaseOrder_OnHold table.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="tx"></param>
        /// <param name="OrderID"></param>
        public void InsertOnHold(DataSet ds, IDbTransaction tx, ref System.Int32 OrderID)
        {

            POHeaderTable addedTable = (POHeaderTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (POHeaderRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.POHeaderOnHold_tbl, insParam);
                        for (int i = 0; i < insParam.Length; i++)
                        {
                            Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
                        }
                        POHeaderData poHederData = PopulateListFromHold(" AND OrderNo ='" + row.OrderNo + "'");
                        if (poHederData != null && poHederData.POHeader.Rows.Count > 0)
                        {
                            row.OrderNo = GetNextPONumber();
                        }
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        OrderID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (ConstraintException)
                    {
                        //cExp.
                    }
                    catch (Exception ex)
                    {
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

		// Update all rows in a DeptCodes DataSet, within a given database transaction.

		public void CloseStatus(System.Int32 orderID)
		{
			string sSQL=" update " + clsPOSDBConstants.POHeader_tbl +
						" set " + clsPOSDBConstants.POHeader_Fld_Status + "=1 " +
						" where " + clsPOSDBConstants.POHeader_Fld_OrderID + "=" + orderID.ToString();

			DataHelper.ExecuteNonQuery(DBConfig.ConnectionString,CommandType.Text, sSQL);

		}
		public void ConfirmInventoryRecieved(System.Int32 orderID)
		{
			string sSQL=" update " + clsPOSDBConstants.POHeader_tbl +
				" set " + clsPOSDBConstants.POHeader_Fld_isInvRecieved + "=1 " +
				" where " + clsPOSDBConstants.POHeader_Fld_OrderID + "=" + orderID.ToString();

			DataHelper.ExecuteNonQuery(DBConfig.ConnectionString,CommandType.Text, sSQL);

		}
        public void UpdateFlaggedStatus(System.Int32 orderID, bool Flagged)
        {
            string sSQL = " update " + clsPOSDBConstants.POHeader_tbl +
                " set " + clsPOSDBConstants.POHeader_Fld_Flagged + "='" + Flagged.ToString() + "'" +
                " where " + clsPOSDBConstants.POHeader_Fld_OrderID + "=" + orderID.ToString();

            DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);

        }
		public void Update(DataSet ds, IDbTransaction tx,ref System.Int32 OrderID) 
		{	
			POHeaderTable modifiedTable = (POHeaderTable)ds.Tables[0].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (POHeaderRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.POHeader_tbl,updParam);

						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);

						OrderID=row.OrderID;

					} 
					catch(POSExceptions ex) 
					{
						throw(ex);
					}

					catch(OtherExceptions ex) 
					{
						throw(ex);
					}

					catch (Exception ex) 
					{
						ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}
        // Delete all rows within a DeptCodes DataSet, within a given database transaction.
        public void Delete(DataSet ds, IDbTransaction tx, ref System.Int32 OrderID)
        {

            POHeaderTable table = (POHeaderTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
            string sSQL;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (POHeaderRow row in table.Rows)
                {
                    try
                    {
                        //delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.POHeader_tbl, row);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
                        OrderID = row.OrderID;
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
            }
        }


        private string BuildDeleteSQL(string tableName, POHeaderRow row)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            // build where clause
            //for(int i = 0;i<delParam.Length;i++)
            //{
            //	sDeleteSQL = sDeleteSQL + delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
            //}
            sDeleteSQL += clsPOSDBConstants.POHeader_Fld_OrderID + " = " + row[0].ToString();
            return sDeleteSQL;
        }
		private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
		{
			string sInsertSQL = "INSERT INTO " + tableName + " ( ";
			// build where clause
			sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

			for(int i = 1;i<delParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn ;
			}
			//sInsertSQL = sInsertSQL + " , UserId ";
			sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

			for(int i = 1;i<delParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName ;
			}
			//sInsertSQL = 	sInsertSQL + " , '" + Configuration.UserName + "'";
			sInsertSQL = 	sInsertSQL + " )";
			return sInsertSQL;
		}


		private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
		{
			string sUpdateSQL = "UPDATE " + tableName + " SET ";
			// build where clause
			sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn +"  = " + updParam[1].ParameterName ;

			for(int i = 2;i<updParam.Length;i++)
			{
				sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn +"  = " + updParam[i].ParameterName ;
			}

			//sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'" ;

			sUpdateSQL = 	sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
			return sUpdateSQL;
		}
		#endregion
		#region IDBDataParameter Generator Methods
		private IDbDataParameter[] whereParameters(string swhere) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
			sqlParams[0] = DataFactory.CreateParameter();
	  
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Size = 2000;
			sqlParams[0].ParameterName = "@whereClause";

			sqlParams[0].Value = swhere;
			return(sqlParams);
		}
		private IDbDataParameter[] PKParameters(System.Int32 OrderID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@OrderID";
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = OrderID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(POHeaderRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@OrderID";
			sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.OrderID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.POHeader_Fld_OrderID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(POHeaderRow row) 
		{
            //parameter count change By SRT(Abhishek) Date : 01/07/2009 Wed.
            //initial DataFactory.CreateParameterArray(10)
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(17);    //PRIMEPOS-2901 05-Nov-2020 JY changed from 16 to 17

            sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_OrderNo, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_Description, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_OrderDate, System.Data.DbType.DateTime);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_ExptDelvDate, System.Data.DbType.DateTime);
			sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_VendorID, System.Data.DbType.Int32);
			sqlParams[5] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
			sqlParams[6] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_Status, System.Data.DbType.Int32);
			sqlParams[7] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_isFTPUsed, System.Data.DbType.Int32);
			sqlParams[8] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_AckDate, System.Data.DbType.DateTime);
			sqlParams[9] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_AckType, System.Data.DbType.String);
			sqlParams[10] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_AckStatus, System.Data.DbType.String);

            //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //Coulomns Added for VendorInterface
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_PrimePOrderID, System.Data.DbType.Int32);
            //sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_IsMaxReached, System.Data.DbType.Boolean);
            //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //added by atul 25-oct-2010
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_InvoiceDate, System.Data.DbType.DateTime);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_InvoiceNumber, System.Data.DbType.String);
            //End of added by atul 25-oct-2010
            //Added by Ravindra(QuicSolv) 16 Jan 2013
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_RefOrderNo, System.Data.DbType.String);
            //End of Added by Ravindra(QuicSolv) 16 Jan 2013
            //Added By shitaljit to store file type on 17 May 13
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_AckFileType, System.Data.DbType.String);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_TransTypeCode, System.Data.DbType.String); //PRIMEPOS-2901 05-Nov-2020 JY Added

            sqlParams[0].SourceColumn  = clsPOSDBConstants.POHeader_Fld_OrderNo;
            sqlParams[1].SourceColumn = clsPOSDBConstants.POHeader_Fld_Description;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.POHeader_Fld_OrderDate;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.POHeader_Fld_ExptDelvDate;
			sqlParams[4].SourceColumn  = clsPOSDBConstants.POHeader_Fld_VendorID;
			sqlParams[5].SourceColumn  = clsPOSDBConstants.fld_UserID;
			sqlParams[6].SourceColumn  = clsPOSDBConstants.POHeader_Fld_Status;

			sqlParams[7].SourceColumn  = clsPOSDBConstants.POHeader_Fld_isFTPUsed;
			sqlParams[8].SourceColumn  = clsPOSDBConstants.POHeader_Fld_AckDate;
			sqlParams[9].SourceColumn  = clsPOSDBConstants.POHeader_Fld_AckType;
			sqlParams[10].SourceColumn  = clsPOSDBConstants.POHeader_Fld_AckStatus;

            //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //Coulomns Added for VendorInterface
            sqlParams[11].SourceColumn = clsPOSDBConstants.POHeader_Fld_PrimePOrderID;
            //sqlParams[11].SourceColumn = clsPOSDBConstants.POHeader_Fld_IsMaxReached;
            //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //added by atul 25-oct-2010
            sqlParams[12].SourceColumn = clsPOSDBConstants.POHeader_Fld_InvoiceDate;
            sqlParams[13].SourceColumn = clsPOSDBConstants.POHeader_Fld_InvoiceNumber;
            //End of added by atul 25-oct-2010
            //Add by Ravindra (QuicSolv) 16 Jan 2013
            sqlParams[14].SourceColumn = clsPOSDBConstants.POHeader_Fld_RefOrderNo;
            //End of Added by Ravindra(Quicsolv) 16 Jan 2013
            //Added By shitaljit to store file type on 17 May 13
            sqlParams[15].SourceColumn = clsPOSDBConstants.POHeader_Fld_AckFileType;
            sqlParams[16].SourceColumn = clsPOSDBConstants.POHeader_Fld_TransTypeCode;  //PRIMEPOS-2901 05-Nov-2020 JY Added

            if (row.OrderNo != System.String.Empty )
				sqlParams[0].Value = row.OrderNo;
			else
				sqlParams[0].Value = DBNull.Value ;

            if (row.OrderNo != System.String.Empty)
                sqlParams[1].Value = row.Description;
            else
                sqlParams[1].Value = DBNull.Value;

			if (row.OrderDate!= System.DateTime.MinValue )
				sqlParams[2].Value = row.OrderDate;
			else
				sqlParams[2].Value = System.DateTime.MinValue ;

			if (row.ExptDelvDate!= System.DateTime.MinValue )
				sqlParams[3].Value = row.ExptDelvDate;
			else
				sqlParams[3].Value = System.DateTime.MinValue ;

			if (row.VendorID!= 0 )
				sqlParams[4].Value = row.VendorID;
			else
				sqlParams[4].Value = 0 ;

			sqlParams[5].Value=Configuration.UserName;
			sqlParams[6].Value = row.Status;
			
			sqlParams[7].Value = row.isFTPUsed;
			if (row.AckDate!= System.DateTime.MinValue )
				sqlParams[8].Value = row.AckDate;
			else
				sqlParams[8].Value = DBNull.Value;
			
			sqlParams[9].Value = row.AckType;
			sqlParams[10].Value = row.AckStatus;

            //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //Coulomns Added for VendorInterface
            
            if (row.PrimePOrderId != 0)
                sqlParams[11].Value = row.PrimePOrderId;
            else
                sqlParams[11].Value = 0;

            //sqlParams[11].Value = row.IsMaxReached;
            //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.

            //Added by atul 25-oct-2010
            if (row.InvoiceDate != System.DateTime.MinValue)
                sqlParams[12].Value = row.InvoiceDate;
            else
                sqlParams[12].Value = System.DBNull.Value;


            sqlParams[13].Value = row.InvoiceNumber;
            sqlParams[14].Value = row.RefOrderNO;
            //end of Added by atul 25-oct-2010

            //Added By shitaljit to store file type on 17 May 13
			if (row.AckFileType != System.String.Empty )
				sqlParams[15].Value  = row.AckFileType;
			else
				sqlParams[15].Value  = DBNull.Value ;
            if (row.TransTypeCode != System.String.Empty)   //PRIMEPOS-2901 05-Nov-2020 JY Added
                sqlParams[16].Value = row.TransTypeCode;
            else
                sqlParams[16].Value = DBNull.Value;

            return (sqlParams);
        }
        private IDbDataParameter[] UpdateParameters(POHeaderRow row)
        {

            //parameter count change By SRT(Abhishek) Date : 01/07/2009 Wed.
            //initial DataFactory.CreateParameterArray(11)
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(17);    //PRIMEPOS-2901 05-Nov-2020 JY changed from 16 to 17
            //end of change By By SRT(Abhishek) Date : 01/07/2009 Wed.            

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_OrderID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_Description, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_OrderDate, System.Data.DbType.DateTime);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_ExptDelvDate, System.Data.DbType.DateTime);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_VendorID, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_Status, System.Data.DbType.Int32);

            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_isFTPUsed, System.Data.DbType.Int32);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_AckDate, System.Data.DbType.DateTime);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_AckType, System.Data.DbType.String);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_AckStatus, System.Data.DbType.String);

            //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //Coulomns Added for VendorInterface
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_PrimePOrderID, System.Data.DbType.Int32);
            //sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_IsMaxReached, System.Data.DbType.Boolean);
            //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //added by atul 25-oct-2010
           
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_InvoiceDate, System.Data.DbType.DateTime);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_InvoiceNumber, System.Data.DbType.String);
            //End of added by atul 25-oct-2010
            //Added By Ravindra(Quicsolv) 16 Jan 2013
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_RefOrderNo, System.Data.DbType.String);
            //EndOFDayData of Added By Ravindra (Quicsolv) 16 jan 2013

            //Added By shitaljit to store file type on 17 May 13
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_AckFileType, System.Data.DbType.String);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_TransTypeCode, System.Data.DbType.String); //PRIMEPOS-2901 05-Nov-2020 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.POHeader_Fld_OrderID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.POHeader_Fld_Description;
            sqlParams[2].SourceColumn = clsPOSDBConstants.POHeader_Fld_OrderDate;
            sqlParams[3].SourceColumn = clsPOSDBConstants.POHeader_Fld_ExptDelvDate;
            sqlParams[4].SourceColumn = clsPOSDBConstants.POHeader_Fld_VendorID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.fld_UserID;
            sqlParams[6].SourceColumn = clsPOSDBConstants.POHeader_Fld_Status;

            sqlParams[7].SourceColumn = clsPOSDBConstants.POHeader_Fld_isFTPUsed;
            sqlParams[8].SourceColumn = clsPOSDBConstants.POHeader_Fld_AckDate;
            sqlParams[9].SourceColumn = clsPOSDBConstants.POHeader_Fld_AckType;
            sqlParams[10].SourceColumn = clsPOSDBConstants.POHeader_Fld_AckStatus;
            
            //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //Coulomns Added for VendorInterface
            sqlParams[11].SourceColumn = clsPOSDBConstants.POHeader_Fld_PrimePOrderID;
            //sqlParams[12].SourceColumn = clsPOSDBConstants.POHeader_Fld_IsMaxReached;
            //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //added by atul 25-oct-2010
            sqlParams[12].SourceColumn = clsPOSDBConstants.POHeader_Fld_InvoiceDate;
            sqlParams[13].SourceColumn = clsPOSDBConstants.POHeader_Fld_InvoiceNumber;
            //End of added by atul 25-oct-2010
            //Added By Ravindra(Quicsolv) 16 Jan 2013
            sqlParams[14].SourceColumn = clsPOSDBConstants.POHeader_Fld_RefOrderNo;
            //EndOFDayData of Added By Ravindra (Quicsolv) 16 jan 2013

            //Added By shitaljit to store file type on 17 May 13
            sqlParams[15].SourceColumn = clsPOSDBConstants.POHeader_Fld_AckFileType;
            sqlParams[16].SourceColumn = clsPOSDBConstants.POHeader_Fld_TransTypeCode;  //PRIMEPOS-2901 05-Nov-2020 JY Added

            if (row.OrderID != 0)
                sqlParams[0].Value = row.OrderID;
            else
                sqlParams[0].Value = 0;

            if (row.OrderID != 0)
                sqlParams[1].Value = row.Description;
            else
                sqlParams[1].Value = 0;

            //if (row.OrderNo != System.String.Empty)
            //    sqlParams[1].Value = row.OrderNo;
            //else
            //    sqlParams[1].Value = DBNull.Value;

            if (row.OrderDate != System.DateTime.MinValue)
                sqlParams[2].Value = row.OrderDate;
            else
                sqlParams[2].Value = System.DateTime.MinValue;

            if (row.ExptDelvDate != System.DateTime.MinValue)
                sqlParams[3].Value = row.ExptDelvDate;
            else
                sqlParams[3].Value = System.DateTime.MinValue;

            if (row.VendorID != 0)
                sqlParams[4].Value = row.VendorID;
            else
                sqlParams[4].Value = 0;

            sqlParams[5].Value = Configuration.UserName;

            sqlParams[6].Value = row.Status;

            sqlParams[7].Value = row.isFTPUsed;
            if (row.AckDate != System.DateTime.MinValue)
                sqlParams[8].Value = row.AckDate;
            else
                sqlParams[8].Value = DBNull.Value;

            sqlParams[9].Value = row.AckType;
            sqlParams[10].Value = row.AckStatus;

            //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
            //Coulomns Added for VendorInterface

            if (row.PrimePOrderId != 0)
                sqlParams[11].Value = row.PrimePOrderId;
            else
                sqlParams[11].Value = 0;

            //sqlParams[12].Value = row.IsMaxReached;
            //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.

            //Added by atul 25-oct-2010
            if (row.InvoiceDate != System.DateTime.MinValue)
                sqlParams[12].Value = row.InvoiceDate;
            else
                sqlParams[12].Value = System.DBNull.Value;


            sqlParams[13].Value = row.InvoiceNumber;

            //Added By Ravindra(Quicsolv) 16 Jan 2013
            sqlParams[14].Value = row.RefOrderNO;
            //EndOFDayData of Added By Ravindra (Quicsolv) 16 jan 2013

            //end of Added by atul 25-oct-2010

            //Added By shitaljit to store file type on 17 May 13
            if (row.AckFileType != System.String.Empty)
                sqlParams[15].Value = row.AckFileType;
            else
                sqlParams[15].Value = DBNull.Value;
            if (row.TransTypeCode != System.String.Empty) //PRIMEPOS-2901 05-Nov-2020 JY Added
                sqlParams[16].Value = row.TransTypeCode;
            else
                sqlParams[16].Value = DBNull.Value;

            return (sqlParams);
        }
        #region commented
        //private IDbDataParameter[] UpdateParameters(POHeaderRow row) 
        //{

        //    //parameter count change By SRT(Abhishek) Date : 01/07/2009 Wed.
        //    //initial DataFactory.CreateParameterArray(11)
        //    IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(12);
        //    //end of change By By SRT(Abhishek) Date : 01/07/2009 Wed.            
            

        //    sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_OrderID, System.Data.DbType.Int32);
        //    sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_OrderNo, System.Data.DbType.String);
        //    sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_OrderDate, System.Data.DbType.DateTime);
        //    sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_ExptDelvDate, System.Data.DbType.DateTime);
        //    sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_VendorID, System.Data.DbType.Int32);
        //    sqlParams[5] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
        //    sqlParams[6] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_Status, System.Data.DbType.Int32);

        //    sqlParams[7] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_isFTPUsed, System.Data.DbType.Int32);
        //    sqlParams[8] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_AckDate, System.Data.DbType.DateTime);
        //    sqlParams[9] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_AckType, System.Data.DbType.String);
        //    sqlParams[10] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POHeader_Fld_AckStatus, System.Data.DbType.String);

        //    //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //    //Coulomns Added for VendorInterface
        //    sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_PrimePOrderID, System.Data.DbType.Int32);
        //    //sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POHeader_Fld_IsMaxReached, System.Data.DbType.Boolean);
        //    //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.


        //    sqlParams[0].SourceColumn  = clsPOSDBConstants.POHeader_Fld_OrderID;
        //    sqlParams[1].SourceColumn  = clsPOSDBConstants.POHeader_Fld_OrderNo;
        //    sqlParams[2].SourceColumn  = clsPOSDBConstants.POHeader_Fld_OrderDate;
        //    sqlParams[3].SourceColumn  = clsPOSDBConstants.POHeader_Fld_ExptDelvDate;
        //    sqlParams[4].SourceColumn  = clsPOSDBConstants.POHeader_Fld_VendorID;
        //    sqlParams[5].SourceColumn  = clsPOSDBConstants.fld_UserID;
        //    sqlParams[6].SourceColumn  = clsPOSDBConstants.POHeader_Fld_Status;

        //    sqlParams[7].SourceColumn  = clsPOSDBConstants.POHeader_Fld_isFTPUsed;
        //    sqlParams[8].SourceColumn  = clsPOSDBConstants.POHeader_Fld_AckDate;
        //    sqlParams[9].SourceColumn  = clsPOSDBConstants.POHeader_Fld_AckType;
        //    sqlParams[10].SourceColumn  = clsPOSDBConstants.POHeader_Fld_AckStatus;


        //    //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //    //Coulomns Added for VendorInterface
        //    sqlParams[11].SourceColumn = clsPOSDBConstants.POHeader_Fld_PrimePOrderID;
        //    //sqlParams[12].SourceColumn = clsPOSDBConstants.POHeader_Fld_IsMaxReached;
        //    //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.

        //    if (row.OrderID!= 0 )
        //        sqlParams[0].Value = row.OrderID;
        //    else
        //        sqlParams[0].Value = 0 ;

        //    if (row.OrderNo != System.String.Empty )
        //        sqlParams[1].Value = row.OrderNo;
        //    else
        //        sqlParams[1].Value = DBNull.Value ;

        //    if (row.OrderDate!= System.DateTime.MinValue )
        //        sqlParams[2].Value = row.OrderDate;
        //    else
        //        sqlParams[2].Value = System.DateTime.MinValue ;

        //    if (row.ExptDelvDate!= System.DateTime.MinValue )
        //        sqlParams[3].Value = row.ExptDelvDate;
        //    else
        //        sqlParams[3].Value = System.DateTime.MinValue ;

        //    if (row.VendorID!= 0 )
        //        sqlParams[4].Value = row.VendorID;
        //    else
        //        sqlParams[4].Value = 0 ;

        //    sqlParams[5].Value=Configuration.UserName;

        //    sqlParams[6].Value=row.Status;

        //    sqlParams[7].Value = row.isFTPUsed;
        //    if (row.AckDate!= System.DateTime.MinValue )
        //        sqlParams[8].Value = row.AckDate;
        //    else
        //        sqlParams[8].Value = DBNull.Value ;
			
        //    sqlParams[9].Value = row.AckType;
        //    sqlParams[10].Value = row.AckStatus;

        //    //Added By SRT(Abhishek) Date : 01/07/2009 Wed.
        //    //Coulomns Added for VendorInterface

        //    if (row.PrimePOrderId != 0)
        //        sqlParams[11].Value = row.PrimePOrderId;
        //    else
        //        sqlParams[11].Value = 0;

        //    //sqlParams[12].Value = row.IsMaxReached;
        //    //End of Added By SRT(Abhishek) Date : 01/07/2009 Wed.

        //    return(sqlParams);
        //}
        #endregion
        #endregion


        public void Dispose() {}   
	}
}
