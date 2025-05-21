using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
//using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using NLog;
using POS_Core.Resources;
using Resources;
using POS_Core.BusinessRules;
using System.Collections.Generic;
////using POS.Resources;
namespace POS_Core.DataAccess
{
	

    // Provides data access methods for DeptCode
    public class TransHeaderSvr: IDisposable  
	{
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public  void Persist(TransHeaderData updates, IDbTransaction tx,ref System.Int32 TransID) 
		{
			try 		
			{
				this.Insert(updates, tx,ref TransID);
				//this.Update(updates, tx);
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "Persist(TransHeaderData updates, IDbTransaction tx,ref System.Int32 TransID)");
                throw (ex);
			}
			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Persist(TransHeaderData updates, IDbTransaction tx,ref System.Int32 TransID) ");
                throw (ex);
			}
			catch(Exception ex) 
			{
                logger.Fatal(ex, "Persist(TransHeaderData updates, IDbTransaction tx,ref System.Int32 TransID) ");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}

		public  void PutOnHold(TransHeaderData updates, IDbTransaction tx,ref System.Int32 TransID) 
		{
			try 		
			{
				this.InsertOnHold(updates, tx,ref TransID);
				this.UpdateOnHold(updates, tx,ref TransID);
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "PutOnHold(TransHeaderData updates, IDbTransaction tx,ref System.Int32 TransID)");
                throw (ex);
			}
			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "PutOnHold(TransHeaderData updates, IDbTransaction tx,ref System.Int32 TransID)");
                throw (ex);
			}
			catch(Exception ex) 
			{
                logger.Fatal(ex, "PutOnHold(TransHeaderData updates, IDbTransaction tx,ref System.Int32 TransID)");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}

		#endregion

		#region Insert, Update, and Delete Methods
		public void Insert(TransHeaderData ds, IDbTransaction tx, ref System.Int32 TransID) 
		{
			TransHeaderTable addedTable = (TransHeaderTable)ds.Tables[0];
			//.GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;
			
			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (TransHeaderRow row in addedTable.Rows) 
				{
					try 
					{						
						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.TransHeader_tbl,insParam);
						for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
						}
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
						TransID=Convert.ToInt32(DataHelper.ExecuteScalar(tx,CommandType.Text,"select @@identity"));
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
				addedTable.AcceptChanges();
			}		
			else
			{
				ErrorHandler.throwCustomError(POSErrorENUM.TransHeader_UnableToSaveData);
			}
		}

		public void DeleteOnHoldRows( IDbTransaction tx,System.Int32 TransID, bool IsDeletePaymentTransOnHold = true)//2915
		{
			try
			{
				string sSQL;

                TransDetailRXSvr oSvr = new TransDetailRXSvr();
                oSvr.DeleteOnHold(TransID, tx);

                #region Sprint-22 - 01-Oct-2015 JY Added to delete the record from POSTransactionDetailTax_OnHold table
                sSQL = "DELETE FROM " + clsPOSDBConstants.TransDetailTax_OnHold_tbl + " WHERE " +
                        clsPOSDBConstants.TransDetail_Fld_TransID + "=" + TransID.ToString();
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
                #endregion

                sSQL ="delete from " + clsPOSDBConstants.TransDetail_OnHold_tbl + " where " + 
						clsPOSDBConstants.TransDetail_Fld_TransID + "=" +TransID.ToString();
				DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
				
				sSQL="delete from " + clsPOSDBConstants.TransHeader_OnHold_tbl + " where " + 
					clsPOSDBConstants.TransHeader_Fld_TransID + "=" +TransID.ToString();
				DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);

				if (IsDeletePaymentTransOnHold)
				{
					//2915
					sSQL = "delete from " + clsPOSDBConstants.POSTransPayment_OnHold_tbl + " where " +
						clsPOSDBConstants.TransHeader_Fld_TransID + "=" + TransID.ToString();
					DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
				}
			}		
			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "DeleteOnHoldRows( IDbTransaction tx,System.Int32 TransID)");
                throw (ex);
			}
		}

        #region PRIMEPOS-2639 27-Mar-2019 JY Added
        public void DeleteOnHoldRows(IDbTransaction tx, List<OnholdRxs> lstOnHoldRxs)
        {
            try
            {
                //search record from RxNo and Refill No in Transdetail_onhold table
                string sSQL = string.Empty;

                foreach (OnholdRxs objOnholdRxs in lstOnHoldRxs)
                {
                    string strRxNo = objOnholdRxs.RxNo.ToString() + "-" + objOnholdRxs.NRefill.ToString();
                    sSQL = "SELECT TOP 1 TransID, TransDetailID FROM POSTransactionDetail_OnHold WHERE ItemDescription LIKE '" + strRxNo + "%' ORDER BY TransDetailID DESC";
                    DataTable dtOnHoldTransDetail = DataHelper.ExecuteDataTable(tx, CommandType.Text, sSQL);
                    if (dtOnHoldTransDetail != null && dtOnHoldTransDetail.Rows.Count > 0)
                    {
                        int nTransID = Configuration.convertNullToInt(dtOnHoldTransDetail.Rows[0]["TransID"]);
                        int nTransDetailID = Configuration.convertNullToInt(dtOnHoldTransDetail.Rows[0]["TransDetailID"]);

                        sSQL = "DELETE FROM " + clsPOSDBConstants.TransDetailRX_OnHold_tbl + " WHERE TransDetailID = " + nTransDetailID;
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);

                        sSQL = "DELETE FROM " + clsPOSDBConstants.TransDetailTax_OnHold_tbl + " WHERE TransID = " + nTransID;
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);

                        sSQL = "DELETE FROM " + clsPOSDBConstants.TransDetail_OnHold_tbl + " WHERE TransDetailID = " + nTransDetailID;
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);

                        sSQL = "SELECT TransID FROM " + clsPOSDBConstants.TransDetail_OnHold_tbl + " WHERE TransID = " + nTransID;
                        DataTable dt = DataHelper.ExecuteDataTable(tx, CommandType.Text, sSQL);
                        if (dt != null && dt.Rows.Count == 0)
                        {
                            sSQL = "DELETE FROM " + clsPOSDBConstants.TransHeader_OnHold_tbl + " WHERE TransID = " + nTransID;
                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
                        }
                    }
                }
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "DeleteOnHoldRows(IDbTransaction tx, List<OnholdRxs> lstOnHoldRxs)");
                throw (ex);
            }
        }
        #endregion

        public void InsertOnHold(TransHeaderData ds, IDbTransaction tx, ref System.Int32 TransID) 
		{
			TransHeaderTable addedTable = (TransHeaderTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;
			
			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (TransHeaderRow row in addedTable.Rows) 
				{
					try 
					{
						
						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.TransHeader_OnHold_tbl,insParam);
						for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
						}
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        TransID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text,"select @@identity"));
					} 
					catch(POSExceptions ex) 
					{
                        logger.Fatal(ex, "InsertOnHold(TransHeaderData ds, IDbTransaction tx, ref System.Int32 TransID)");
                        throw (ex);
					}
					catch(OtherExceptions ex) 
					{
                        logger.Fatal(ex, "InsertOnHold(TransHeaderData ds, IDbTransaction tx, ref System.Int32 TransID)");
                        throw (ex);
					}
					catch (Exception ex) 
					{
                        logger.Fatal(ex, "InsertOnHold(TransHeaderData ds, IDbTransaction tx, ref System.Int32 TransID)");
                        ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}

		public void UpdateOnHold(TransHeaderData ds, IDbTransaction tx, ref System.Int32 TransID) 
		{
			TransHeaderTable addedTable = (TransHeaderTable)ds.Tables[0].GetChanges(DataRowState.Modified);
			string sSQL;
			IDbDataParameter []insParam;
			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (TransHeaderRow row in addedTable.Rows) 
				{
					try 
					{						
						insParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.TransHeader_OnHold_tbl,insParam);
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
						TransID=row.TransID;
					} 
					catch(POSExceptions ex) 
					{
                        logger.Fatal(ex, "UpdateOnHold(TransHeaderData ds, IDbTransaction tx, ref System.Int32 TransID)");
                        throw (ex);
					}
					catch(OtherExceptions ex) 
					{
                        logger.Fatal(ex, "UpdateOnHold(TransHeaderData ds, IDbTransaction tx, ref System.Int32 TransID)");
                        throw (ex);
					}
					catch (Exception ex) 
					{
                        logger.Fatal(ex, "UpdateOnHold(TransHeaderData ds, IDbTransaction tx, ref System.Int32 TransID)");
                        ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}
		}

		// Update all rows in a DeptCodes DataSet, within a given database transaction.

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

		private string BuildUpdateSQL(string tableName, IDbDataParameter[] delParam)
		{
			string sInsertSQL = "update " + tableName + " set ";
			// build where clause
			sInsertSQL = sInsertSQL + delParam[1].SourceColumn + " = " + delParam[1].ParameterName ;

			for(int i = 2;i<delParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
			}
			//sInsertSQL = 	sInsertSQL + " , '" + Configuration.UserName + "'";
			sInsertSQL = 	sInsertSQL + " where " + delParam[0].SourceColumn + " = " + delParam[0].ParameterName;
			return sInsertSQL;
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
		
        private IDbDataParameter[] PKParameters(System.Int32 TransID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@TransID";
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = TransID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(TransHeaderRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
            
			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@TransID";
			sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.TransID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.TransHeader_Fld_TransID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(TransHeaderRow row) 
		{
            //IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(19);//Orignal commented by Krishna on 2 June 2011
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(32);//This Added by Krishna on 2 June 2011//Update By Shitaljit make no of parameters 20 from 21    //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY update by 22 to 24 // Replace 24 to 28 - Solutran  - NileshJ  - PRIMEPOS-2663    //PRIMEPOS-2865 16-Jul-2020 JY changed to 29//PRIMEPOS-2915 Changed from 29 to 30 Arvind	//PRIMEPOS-3053 08-Feb-2021 JY changed from 30 to 31	//PRIMEPOS-3117 11-Jul-2022 JY changed from 31 to 32

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TransDate, System.Data.DbType.DateTime);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_CustomerID, System.Data.DbType.Int32);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TransType, System.Data.DbType.String);
			sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_GrossTotal, System.Data.DbType.String);
			sqlParams[5] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount, System.Data.DbType.String);
			sqlParams[6] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount, System.Data.DbType.String);
			sqlParams[7] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TenderedAmount, System.Data.DbType.String);
			sqlParams[8] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TotalPaid, System.Data.DbType.String);
			sqlParams[9] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_isStationClosed, System.Data.DbType.String);
			sqlParams[10] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_isEOD, System.Data.DbType.String);
			sqlParams[11] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_DrawerNo, System.Data.DbType.String);
			sqlParams[12] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_StClosedID, System.Data.DbType.String);
			sqlParams[13] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_EODID, System.Data.DbType.String);
			sqlParams[14] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_StationID, System.Data.DbType.String);
			sqlParams[15] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_Account_No, System.Data.DbType.Int64);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_ReturnTransID, System.Data.DbType.Int32);
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_IsDelivery, System.Data.DbType.Boolean);
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_LoyaltyPoints, System.Data.DbType.Int32);

            //Following Code Added by Krishna on 2 June 2011
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_TransactionStartDate, System.Data.DbType.DateTime);
            //Till here Added by Krishna on 2 June 2011

            //Added By Shitaljit(QuicSolv) on 31 August 2011
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_InvoiceDiscount, System.Data.DbType.Decimal);
            //Till HereAdded By Shitaljit(QuicSolv) on 31 August 2011

            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_DeliveryAddress, System.Data.DbType.String);

            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_WasonHold, System.Data.DbType.Boolean); //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_DeliverySigSkipped, System.Data.DbType.Boolean);    //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added

            #region NileshJ Solutran 
            sqlParams[24] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_S3TransID, System.Data.DbType.Int64); //PRIMEPOS-3265
            sqlParams[25] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_S3DiscountAmount, System.Data.DbType.String);
            sqlParams[26] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_S3PurAmount, System.Data.DbType.String);
            sqlParams[27] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_S3TaxAmount, System.Data.DbType.String);
            #endregion
            sqlParams[28] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_AllowRxPicked, System.Data.DbType.Int32);   //PRIMEPOS-2865 16-Jul-2020 JY Added
			sqlParams[29] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_IsCustomerDriven, System.Data.DbType.Boolean);//2915
			sqlParams[30] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_RxTaxPolicyID, System.Data.DbType.Int32);   //PRIMEPOS-3053 08-Feb-2021 JY Added
			sqlParams[31] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_TotalTransFeeAmt, System.Data.DbType.Decimal);  //PRIMEPOS-3117 11-Jul-2022 JY Added

			sqlParams[0].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TransDate;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_CustomerID;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.fld_UserID;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TransType;
			sqlParams[4].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_GrossTotal;
			sqlParams[5].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount;
			sqlParams[6].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount;
			sqlParams[7].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TenderedAmount;
			sqlParams[8].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TotalPaid;
			sqlParams[9].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_isStationClosed;
			sqlParams[10].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_isEOD;
			sqlParams[11].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_DrawerNo;
			sqlParams[12].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_StClosedID;
			sqlParams[13].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_EODID;
			sqlParams[14].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_StationID;
			sqlParams[15].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_Account_No;
            sqlParams[16].SourceColumn = clsPOSDBConstants.TransHeader_Fld_ReturnTransID;
            sqlParams[17].SourceColumn = clsPOSDBConstants.TransHeader_Fld_IsDelivery;
            sqlParams[18].SourceColumn = clsPOSDBConstants.TransHeader_Fld_LoyaltyPoints;
            //Following Code Added by Krishna on 2 June 2011
            sqlParams[19].SourceColumn = clsPOSDBConstants.TransHeader_Fld_TransactionStartDate;
            //Till here Added by Krishna on 2 June 2011
            //Added By Shitaljit(QuicSolv) on 31 August 2011
            sqlParams[20].SourceColumn =  clsPOSDBConstants.TransHeader_Fld_InvoiceDiscount;
            //Till HereAdded By Shitaljit(QuicSolv) on 31 August 2011
            sqlParams[21].SourceColumn = clsPOSDBConstants.TransHeader_Fld_DeliveryAddress;
            sqlParams[22].SourceColumn = clsPOSDBConstants.TransHeader_Fld_WasonHold;   //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
            sqlParams[23].SourceColumn = clsPOSDBConstants.TransHeader_Fld_DeliverySigSkipped;  //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added

            #region Added for Solutran - PRIMEPOS-2663 - NileshJ
            sqlParams[24].SourceColumn = clsPOSDBConstants.TransHeader_Fld_S3TransID;
            sqlParams[25].SourceColumn = clsPOSDBConstants.TransHeader_Fld_S3DiscountAmount;
            sqlParams[26].SourceColumn = clsPOSDBConstants.TransHeader_Fld_S3PurAmount;
            sqlParams[27].SourceColumn = clsPOSDBConstants.TransHeader_Fld_S3TaxAmount;
            #endregion
            sqlParams[28].SourceColumn = clsPOSDBConstants.TransHeader_Fld_AllowRxPicked;   //PRIMEPOS-2865 16-Jul-2020 JY Added
			sqlParams[29].SourceColumn = clsPOSDBConstants.TransHeader_Fld_IsCustomerDriven;//2915
			sqlParams[30].SourceColumn = clsPOSDBConstants.TransHeader_Fld_RxTaxPolicyID;   //PRIMEPOS-3053 08-Feb-2021 JY Added
			sqlParams[31].SourceColumn = clsPOSDBConstants.TransHeader_Fld_TotalTransFeeAmt;    //PRIMEPOS-3117 11-Jul-2022 JY Added

			if (row.CustomerID != 0)
				sqlParams[1].Value = row.CustomerID;
			else
				sqlParams[1].Value = DBNull.Value ;

			if (row.TransDate!= System.DateTime.MinValue )
				sqlParams[0].Value = row.TransDate;
			else
				sqlParams[0].Value = System.DateTime.MinValue ;

			sqlParams[2].Value=Configuration.UserName;
			sqlParams[3].Value = row.TransType;
			sqlParams[4].Value = row.GrossTotal;
			sqlParams[5].Value = row.TotalDiscAmount;
			sqlParams[6].Value = row.TotalTaxAmount;
			sqlParams[7].Value = row.TenderedAmount;
			sqlParams[8].Value = row.TotalPaid;
			sqlParams[9].Value = row.isStationClosed;
			sqlParams[10].Value = row.isEOD;
			sqlParams[11].Value = row.DrawerNo;
			sqlParams[12].Value = row.stClosedID;
			sqlParams[13].Value = row.EODID;
			sqlParams[14].Value = row.StationID;

			sqlParams[15].Value = row.Acc_No;
            sqlParams[16].Value = row.ReturnTransID;

            sqlParams[17].Value = row.IsDelivery;
            sqlParams[18].Value = row.LoyaltyPoints;

            //Following Code Added by Krishna on 2 June 2011
            if (row.TransactionStartDate != System.DateTime.MinValue)
                sqlParams[19].Value = row.TransactionStartDate;
            else
                sqlParams[19].Value = System.DateTime.MinValue;
            //Till here Added by Krishna on 2 June 2011

            //Added By Shitaljit(QuicSolv) on 23 August 2011
            sqlParams[20].Value = row.InvoiceDiscount;
            //Till Here Added By Shitaljit(QuicSolv) on 23 August 2011
            sqlParams[21].Value = row.DeliveryAddress;
            sqlParams[22].Value = row.WasonHold;   //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
            sqlParams[23].Value = row.DeliverySigSkipped;   //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added

            #region Added for Solutran - PRIMEPOS-2663 - NileshJ
            sqlParams[24].Value = row.S3TransID;
            sqlParams[25].Value = row.S3DiscountAmount;
            sqlParams[26].Value = row.S3PurAmount;
            sqlParams[27].Value = row.S3TaxAmount;
            #endregion
            sqlParams[28].Value = row.AllowRxPicked;    //PRIMEPOS-2865 16-Jul-2020 JY Added
			sqlParams[29].Value = row.IsCustomerDriven;//2915
			sqlParams[30].Value = row.RxTaxPolicyID;    //PRIMEPOS-3053 08-Feb-2021 JY Added
			sqlParams[31].Value = row.TotalTransFeeAmt; //PRIMEPOS-3117 11-Jul-2022 JY Added

			return (sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(TransHeaderRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(32);//Update By Shitaljit on 1 September 2011 make no of parameterrs to 21 from 20 // replace 24 to 28  - add new paramter for Solutran - PRIMEPOS-2663 - Nileshj   //PRIMEPOS-2865 16-Jul-2020 JY Changed to 29//PRIMEPOS-2915 Changed from 29 to 30 Arvind	//PRIMEPOS-3053 08-Feb-2021 JY changed from 30 to 31	//PRIMEPOS-3117 11-Jul-2022 JY changed from 31 to 32

			sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_TransID, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TransDate, System.Data.DbType.DateTime);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_CustomerID, System.Data.DbType.Int32);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
			sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TransType, System.Data.DbType.String);
			sqlParams[5] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_GrossTotal, System.Data.DbType.String);
			sqlParams[6] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount, System.Data.DbType.String);
			sqlParams[7] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount, System.Data.DbType.String);
			sqlParams[8] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TenderedAmount, System.Data.DbType.String);
			sqlParams[9] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_TotalPaid, System.Data.DbType.String);
			sqlParams[10] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_isStationClosed, System.Data.DbType.String);
			sqlParams[11] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_isEOD, System.Data.DbType.String);
			sqlParams[12] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_DrawerNo, System.Data.DbType.String);
			sqlParams[13] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_StClosedID, System.Data.DbType.String);
			sqlParams[14] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_EODID, System.Data.DbType.String);
			sqlParams[15] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_StationID, System.Data.DbType.String);
			sqlParams[16] = DataFactory.CreateParameter("@"+clsPOSDBConstants.TransHeader_Fld_Account_No, System.Data.DbType.Int64);
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_ReturnTransID, System.Data.DbType.Int32);
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_IsDelivery, System.Data.DbType.Boolean);
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_LoyaltyPoints, System.Data.DbType.Int32);

            //Added By Shitaljit(QuicSolv) on 31 August 2011
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_InvoiceDiscount, System.Data.DbType.Decimal);
            //Till HereAdded By Shitaljit(QuicSolv) on 31 August 2011
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_DeliveryAddress, System.Data.DbType.String);
            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_WasonHold, System.Data.DbType.Boolean); //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_DeliverySigSkipped, System.Data.DbType.Boolean);    //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added

            #region NileshJ Solutran PRIMEPOS-2663
            sqlParams[24] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_S3TransID, System.Data.DbType.Int64); //PRIMEPOS-3265
            sqlParams[25] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_S3DiscountAmount, System.Data.DbType.String);
            sqlParams[26] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_S3PurAmount, System.Data.DbType.String);
            sqlParams[27] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_S3TaxAmount, System.Data.DbType.String);
            #endregion
            sqlParams[28] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_AllowRxPicked, System.Data.DbType.Int32);
			sqlParams[29] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_IsCustomerDriven, System.Data.DbType.Boolean);//2915
			sqlParams[30] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_RxTaxPolicyID, System.Data.DbType.Int32);   //PRIMEPOS-3053 08-Feb-2021 JY Added
			sqlParams[31] = DataFactory.CreateParameter("@" + clsPOSDBConstants.TransHeader_Fld_TotalTransFeeAmt, System.Data.DbType.Decimal);  //PRIMEPOS-3117 11-Jul-2022 JY Added

			sqlParams[0].SourceColumn = clsPOSDBConstants.TransHeader_Fld_TransID;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TransDate;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_CustomerID;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.fld_UserID;
			sqlParams[4].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TransType;
			sqlParams[5].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_GrossTotal;
			sqlParams[6].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TotalDiscAmount;
			sqlParams[7].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TotalTaxAmount;
			sqlParams[8].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TenderedAmount;
			sqlParams[9].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_TotalPaid;
			sqlParams[10].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_isStationClosed;
			sqlParams[11].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_isEOD;
			sqlParams[12].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_DrawerNo;
			sqlParams[13].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_StClosedID;
			sqlParams[14].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_EODID;
			sqlParams[15].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_StationID;
			sqlParams[16].SourceColumn  = clsPOSDBConstants.TransHeader_Fld_Account_No;
            sqlParams[17].SourceColumn = clsPOSDBConstants.TransHeader_Fld_ReturnTransID;
            sqlParams[18].SourceColumn = clsPOSDBConstants.TransHeader_Fld_IsDelivery;
            sqlParams[19].SourceColumn = clsPOSDBConstants.TransHeader_Fld_LoyaltyPoints;
            //Added By Shitaljit(QuicSolv) on 31 August 2011
            sqlParams[20].SourceColumn = clsPOSDBConstants.TransHeader_Fld_InvoiceDiscount;
            //Till HereAdded By Shitaljit(QuicSolv) on 31 August 2011
            sqlParams[21].SourceColumn = clsPOSDBConstants.TransHeader_Fld_DeliveryAddress;
            sqlParams[22].SourceColumn = clsPOSDBConstants.TransHeader_Fld_WasonHold;   //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added   
            sqlParams[23].SourceColumn = clsPOSDBConstants.TransHeader_Fld_DeliverySigSkipped;  //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added

            #region Added for Solutran - PRIMEPOS-2663 Nileshj
            sqlParams[24].SourceColumn = clsPOSDBConstants.TransHeader_Fld_S3TransID;
            sqlParams[25].SourceColumn = clsPOSDBConstants.TransHeader_Fld_S3DiscountAmount;
            sqlParams[26].SourceColumn = clsPOSDBConstants.TransHeader_Fld_S3PurAmount;
            sqlParams[27].SourceColumn = clsPOSDBConstants.TransHeader_Fld_S3TaxAmount;
            #endregion
            sqlParams[28].SourceColumn = clsPOSDBConstants.TransHeader_Fld_AllowRxPicked;   //PRIMEPOS-2865 16-Jul-2020 JY Added
			sqlParams[29].SourceColumn = clsPOSDBConstants.TransHeader_Fld_IsCustomerDriven;//2915
			sqlParams[30].SourceColumn = clsPOSDBConstants.TransHeader_Fld_RxTaxPolicyID;   //PRIMEPOS-3053 08-Feb-2021 JY Added
			sqlParams[31].SourceColumn = clsPOSDBConstants.TransHeader_Fld_TotalTransFeeAmt;    //PRIMEPOS-3117 11-Jul-2022 JY Added

			sqlParams[0].Value = row.TransID;

			if (row.CustomerID != 0)
				sqlParams[2].Value = row.CustomerID;
			else
				sqlParams[2].Value = DBNull.Value ;

			if (row.TransDate!= System.DateTime.MinValue )
				sqlParams[1].Value = row.TransDate;
			else
				sqlParams[1].Value = System.DateTime.MinValue ;

			sqlParams[3].Value=Configuration.UserName;
			sqlParams[4].Value = row.TransType;
			sqlParams[5].Value = row.GrossTotal;
			sqlParams[6].Value = row.TotalDiscAmount;
			sqlParams[7].Value = row.TotalTaxAmount;
			sqlParams[8].Value = row.TenderedAmount;
			sqlParams[9].Value = row.TotalPaid;
			sqlParams[10].Value = row.isStationClosed;
			sqlParams[11].Value = row.isEOD;
			sqlParams[12].Value = row.DrawerNo;
			sqlParams[13].Value = row.stClosedID;
			sqlParams[14].Value = row.EODID;
			sqlParams[15].Value = row.StationID;
			
			sqlParams[16].Value = row.Acc_No;
            sqlParams[17].Value = row.ReturnTransID;
            sqlParams[18].Value = row.IsDelivery;
            sqlParams[19].Value = row.LoyaltyPoints;
            //Added By Shitaljit(QuicSolv) on 23 August 2011
            sqlParams[20].Value = row.InvoiceDiscount;
            //Till Here Added By Shitaljit(QuicSolv) on 23 August 2011
            sqlParams[21].Value = row.DeliveryAddress;
            sqlParams[22].Value = row.WasonHold;   //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added
            sqlParams[23].Value = row.DeliverySigSkipped;   //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added

            #region NileshJ Solutran - PRIMEPOS-2663
            sqlParams[24].Value = row.S3TransID;
            sqlParams[25].Value = row.S3DiscountAmount;
            sqlParams[26].Value = row.S3PurAmount;
            sqlParams[27].Value = row.S3TaxAmount;
            #endregion
            sqlParams[28].Value = row.AllowRxPicked;    //PRIMEPOS-2865 16-Jul-2020 JY Added
			sqlParams[29].Value = row.IsCustomerDriven;//2915
			sqlParams[30].Value = row.RxTaxPolicyID;    //PRIMEPOS-3053 08-Feb-2021 JY Added
			sqlParams[31].Value = row.TotalTransFeeAmt; //PRIMEPOS-3117 11-Jul-2022 JY Added

			return (sqlParams);
		}
		#endregion
    
		#region Get Methods
        public DataSet PopulateROATransForReturn(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (PopulateROATransForReturn(TransId, conn));
        }

        #region Customer Engagement Details PRIMEPOS-2794
        public string PopulateROALastTransaction(int customerId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            return (PopulateLastTransaction(conn, customerId));
        }
        #endregion

        public DataSet PopulateROATransForReturn(System.Int32 TransId, IDbConnection conn)
        {
            try
            {
                string sSQL = @"Select * FROM POSTransaction T WHERE T.TransID = '"+TransId+"' AND"+
                            " TotalPaid > 0 AND TransID NOT IN(SELECT ISNULL(RETURNTRANSID,0) from POSTransaction)  "; //PRIMEPOS-3385

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                ds.AcceptChanges();
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateROATransForReturn(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateROATransForReturn(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateROATransForReturn(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #region Customer Engagement Details PRIMEPOS-2794
        private string PopulateLastTransaction(IDbConnection conn, int customerId)
        {
            try
            {
                string sSQL = @"Select TOP 1 TransDate FROM POSTransaction T WHERE CustomerID = '" + customerId + "' ORDER BY TransDate DESC";

                return Convert.ToString(DataHelper.ExecuteScalar(conn, CommandType.Text, sSQL));
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateLastTransaction(IDbConnection conn, int customerId)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateLastTransaction(IDbConnection conn, int customerId)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateLastTransaction(IDbConnection conn, int customerId)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion
        public TransHeaderData Populate(System.Int32 ID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
					+ "Trans.*"
					+ " , cus." +  clsPOSDBConstants.Customer_Fld_CustomerCode
					+ " , cus." +  clsPOSDBConstants.Customer_Fld_CustomerName
					+ " FROM " 
					+ clsPOSDBConstants.TransHeader_tbl + " as Trans, " + clsPOSDBConstants.Customer_tbl + " as cus "
					+ " WHERE " 
					+ " cus.CustomerID=Trans.CustomerID "
					+ " AND " + clsPOSDBConstants.TransHeader_Fld_TransID + " =" + ID;


				TransHeaderData ds = new TransHeaderData();
				ds.TransHeader.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
					,  sSQL
					, PKParameters(ID)).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.Int32 ID, IDbConnection conn)");
                throw (ex);
			}
			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.Int32 ID, IDbConnection conn)");
                throw (ex);
			}
			catch(Exception ex) 
			{
                logger.Fatal(ex, "Populate(System.Int32 ID, IDbConnection conn)");
                ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public TransHeaderData GetOnHoldData(System.Int32 ID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
					+ "Trans.*"
					+ " , cus." +  clsPOSDBConstants.Customer_Fld_CustomerCode
					+ " , cus." +  clsPOSDBConstants.Customer_Fld_CustomerName
                    + ", CAST(0 AS BIT) AS WasonHold, CAST(0 AS BIT) AS DeliverySigSkipped "  //Sprint-24 - PRIMEPOS-2342 14-Oct-2016 JY Added      
                    + " FROM " 
					+ clsPOSDBConstants.TransHeader_OnHold_tbl + " as Trans, " + clsPOSDBConstants.Customer_tbl + " as cus "
					+ " WHERE " 
					+ " cus.CustomerID=Trans.CustomerID "
					+ " AND " + clsPOSDBConstants.TransHeader_Fld_TransID + " =" + ID;


				TransHeaderData ds = new TransHeaderData();
				ds.TransHeader.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
					,  sSQL
					, PKParameters(ID)).Tables[0]);
				ds.AcceptChanges();
				return ds;
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "GetOnHoldData(System.Int32 ID, IDbConnection conn)");
                throw (ex);
			}
			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "GetOnHoldData(System.Int32 ID, IDbConnection conn)");
                throw (ex);
			}
			catch(Exception ex) 
			{
                logger.Fatal(ex, "GetOnHoldData(System.Int32 ID, IDbConnection conn)");
                ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public TransHeaderData GetOnHoldData(System.Int32 ID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(GetOnHoldData(ID, conn));
			}
		}

		public TransHeaderData Populate(System.Int32 ID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(ID, conn));
			}
		}

		public DataSet PopulateList(String strWhere) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(PopulateList(strWhere, conn));
			}
		}

		public DataSet PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = "Select " 
					+ "Trans.*"
					+ " , cus." +  clsPOSDBConstants.Customer_Fld_CustomerCode
					+ " , cus." +  clsPOSDBConstants.Customer_Fld_CustomerName
					+ " FROM " 
					+ clsPOSDBConstants.TransHeader_tbl + " as Trans, " + clsPOSDBConstants.Customer_tbl + " as cus "
					+ " WHERE " 
					+ " cus.CustomerID=Trans.CustomerID ";

				if (sWhereClause.Trim()!="")
					sSQL=String.Concat(sSQL," and " ,sWhereClause, " order by transid desc");


				DataSet ds = new  DataSet();
				ds=DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
			}
			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
			}
			catch (Exception ex) 
			{
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                ErrorHandler.throwException(ex,"",""); 
				return null;
			} 
		}

		public DataSet PopulateList(String strWhere, out string strSQL)
		{
			using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
			{
				return (PopulateList(strWhere, conn, out strSQL));
			}
		}

		public DataSet PopulateList(string sWhereClause, IDbConnection conn, out string strSQL)
		{
			DataSet ds = new DataSet();
			strSQL = "";
			try
			{
				string sSQL = "Select "
					+ "Trans.*"
					+ " , cus." + clsPOSDBConstants.Customer_Fld_CustomerCode
					+ " , cus." + clsPOSDBConstants.Customer_Fld_CustomerName
					+ " FROM "
					+ clsPOSDBConstants.TransHeader_tbl + " as Trans, " + clsPOSDBConstants.Customer_tbl + " as cus "
					+ " WHERE "
					+ " cus.CustomerID=Trans.CustomerID ";

				strSQL = "Select Trans.TransID FROM "
					+ clsPOSDBConstants.TransHeader_tbl + " as Trans, " + clsPOSDBConstants.Customer_tbl + " as cus "
					+ "where cus.CustomerID = Trans.CustomerID ";

				if (sWhereClause.Trim() != "")
				{
					sSQL = String.Concat(sSQL, " and ", sWhereClause, " order by transid desc");
					strSQL = String.Concat(strSQL, " and ", sWhereClause);
				}
							
				ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
			}
			catch (POSExceptions ex)
			{
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, out string strSQL)");
				throw (ex);
			}
			catch (OtherExceptions ex)
			{
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, out string strSQL)");
				throw (ex);
			}
			catch (Exception ex)
			{
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, out string strSQL)");
				ErrorHandler.throwException(ex, "", "");
			}
			return ds;
		}
        public bool IsTransAlreadyReturned(int iTransID)
        {
            bool retVal = false;
            DataSet ds = new DataSet();
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    //string sSQL = "Select "
                    //    + " 1 "
                    //    + " FROM "
                    //    + clsPOSDBConstants.TransHeader_tbl + " as Trans " + " WHERE "
                    //    + " ReturnTransID= " + iTransID.ToString();

                    //Following code Added by Krishna on 11 May 2011
                    string sSQL = "select top 1 1 from POSTransactionDetail where itemid not in ( select pod.Itemid from POSTransactionDetail as pod where transid in  (Select trans.transid FROM POSTransaction as Trans WHERE  trans.returntransid='"+iTransID+"')  ) and transid='"+iTransID+"'";
                    //till here added by krishna
                    
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    retVal = true;
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "IsTransAlreadyReturned(int iTransID)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "IsTransAlreadyReturned(int iTransID)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "IsTransAlreadyReturned(int iTransID)");
                ErrorHandler.throwException(ex, "", "");
            }
            return retVal;
        }

        public System.Int32 GetMaxTransId()
        {
            IDbCommand cmd = DataFactory.CreateCommand();
            string sSQL = "";

            int Id = 0;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = DBConfig.ConnectionString;
            conn.Open();

            try
            {
                sSQL = String.Concat("SELECT ",
                    " MAX(TransID)",
                    "  FROM POSTransaction");

                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                Id = Convert.ToInt32((cmd.ExecuteScalar().ToString() == "") ? "0" : cmd.ExecuteScalar().ToString());

                if (Id == 0)
                    Id = 1;
                else
                    Id = Id + 1;

                conn.Close();
                return Id;
            }
            catch (NullReferenceException Ex)
            {
                logger.Fatal(Ex, "GetMaxTransId()");
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                ErrorHandler.throwCustomError(POSErrorENUM.User_InvalidUserName);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetMaxTransId()");
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                throw (exp);
            }
            return Id;
        }
        #endregion //Get Method

        public void Dispose() {}   
	}
}