using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using NLog;

namespace POS_Core.DataAccess
{

	// Provides data access methods for DeptCode
 
	public class POSTransPaymentSvr: IDisposable  
	{
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

        public  void Persist(POSTransPaymentData updates, IDbTransaction tx,System.Int32 TransID) 
		{
			try 
		
			{
				this.Insert(updates, tx, TransID);
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "Persist(POSTransPaymentData updates, IDbTransaction tx,System.Int32 TransID)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Persist(POSTransPaymentData updates, IDbTransaction tx,System.Int32 TransID)");
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "Persist(POSTransPaymentData updates, IDbTransaction tx,System.Int32 TransID)");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}
        #endregion

        public void PutOnHold(POSTransPaymentData updates, IDbTransaction tx, System.Int32 TransID)//2915
        {
            try

            {
                this.DeleteOnHoldPayment(TransID, tx); //PRIMEPOS-3283
                this.InsertOnHold(updates, tx, TransID);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(POSTransPaymentData updates, IDbTransaction tx,System.Int32 TransID)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(POSTransPaymentData updates, IDbTransaction tx,System.Int32 TransID)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(POSTransPaymentData updates, IDbTransaction tx,System.Int32 TransID)");
                POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }

        public void InsertOnHold(POSTransPaymentData ds, IDbTransaction tx, System.Int32 TransID)//2915
        {

            //POSTransPaymentTable addedTable = (POSTransPaymentTable)ds.Tables[0].GetChanges();;
            string sSQL;
            IDbDataParameter[] insParam;

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (POSTransPaymentRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        row.TransID = TransID;
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.POSTransPayment_OnHold_tbl, insParam);
                        /*for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
						}*/

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);

                        if (row.CLCouponID > 0)
                        {
                            CLCouponsSvr oCLCouponsSvr = new CLCouponsSvr();
                            oCLCouponsSvr.ConsumeCoupon(row, tx);
                        }
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(POSTransPaymentData ds, IDbTransaction tx,System.Int32 TransID)");
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(POSTransPaymentData ds, IDbTransaction tx,System.Int32 TransID)");
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(POSTransPaymentData ds, IDbTransaction tx,System.Int32 TransID)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                ds.Tables[0].AcceptChanges();
            }
        }

        #region PRIMEPOS-3283
        public void DeleteOnHoldPayment(Int32 TransID, IDbTransaction tx)
        {
            string sSQL;
            try
            {
                sSQL = BuildDeleteOnHoldSQL(clsPOSDBConstants.POSTransPayment_OnHold_tbl, TransID);
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "DeleteOnHoldPayment(Int32 TransID, IDbTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "DeleteOnHoldPayment(Int32 TransID, IDbTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteOnHoldPayment(Int32 TransID, IDbTransaction tx)");
                ErrorHandler.throwException(ex, "", "");
            }
        }

        private string BuildDeleteOnHoldSQL(string tableName, Int32 TransID)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            sDeleteSQL += clsPOSDBConstants.TransDetail_Fld_TransID + " = " + TransID.ToString();
            return sDeleteSQL;
        }
        #endregion

        #region Insert Methods
        public void Insert(POSTransPaymentData ds, IDbTransaction tx,System.Int32 TransID) 
		{
			//POSTransPaymentTable addedTable = (POSTransPaymentTable)ds.Tables[0].GetChanges();;
			string sSQL;
			IDbDataParameter []insParam;

			if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0) 
			{
				foreach (POSTransPaymentRow row in ds.Tables[0].Rows) 
				{
					try 
					{
						row.TransID=TransID;
						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.POSTransPayment_tbl,insParam);
						/*for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
						}*/

						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        int TransPayID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));  //PRIMEPOS-2402 09-Jul-2021 JY Added

                        if (row.CLCouponID > 0)
                        {
                            CLCouponsSvr oCLCouponsSvr = new CLCouponsSvr();
                            oCLCouponsSvr.ConsumeCoupon(row, tx);
                        }

                        #region PRIMEPOS-2402 08-Jul-2021 JY Added
                        //Override Housecharge Limit
                        try
                        {
                            if (row.OverrideHousechargeLimitUser != "")
                            {
                                using (TransDetailSvr oTransDetailSvr = new TransDetailSvr())
                                {
                                    string[] arr = row.OverrideHousechargeLimitUser.Split('~');
                                    string strUserID = string.Empty, strRemark = string.Empty;
                                    if (arr.Length > 0)
                                        strUserID = arr[0];
                                    if (arr.Length > 1)
                                        strRemark = arr[1];
                                    oTransDetailSvr.InsertOverrideDetails(2, row.TransID, TransPayID, "", strRemark, strUserID, tx);
                                }
                            }
                        }
                        catch (Exception Exp)
                        {
                            logger.Fatal(Exp, "Insert(POSTransPaymentData ds, IDbTransaction tx,System.Int32 TransID) - Update Override Housecharge Limit");
                        }
                        //Max. Tendered Amount
                        try
                        {
                            if (row.MaxTenderedAmountOverrideUser != "")
                            {
                                using (TransDetailSvr oTransDetailSvr = new TransDetailSvr())
                                {
                                    object MaxTenderedAmountOfOverrideUser = DataHelper.ExecuteScalar("SELECT MaxTenderedAmountLimit FROM Users  WHERE UserID = '" + row.MaxTenderedAmountOverrideUser + "'");
                                    oTransDetailSvr.InsertOverrideDetails(15, row.TransID, TransPayID, POS_Core.Resources.Configuration.convertNullToString(POS_Core.Resources.Configuration.UserMaxTenderedAmountLimit), POS_Core.Resources.Configuration.convertNullToString(MaxTenderedAmountOfOverrideUser), row.MaxTenderedAmountOverrideUser, tx);
                                }
                            }
                        }
                        catch (Exception Exp)
                        {
                            logger.Fatal(Exp, "Insert(POSTransPaymentData ds, IDbTransaction tx,System.Int32 TransID) - Update Max. Tendered Amount");
                        }
                        #endregion
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(POSTransPaymentData ds, IDbTransaction tx,System.Int32 TransID)");
                        throw (ex);
					}

					catch(OtherExceptions ex) 
					{
                        logger.Fatal(ex, "Insert(POSTransPaymentData ds, IDbTransaction tx,System.Int32 TransID)");
                        throw (ex);
					}

					catch (Exception ex) 
					{
                        logger.Fatal(ex, "Insert(POSTransPaymentData ds, IDbTransaction tx,System.Int32 TransID)");
                        ErrorHandler.throwException(ex,"","");
					}
				}
				ds.Tables[0].AcceptChanges();
			}		
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
			sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

			for(int i = 1;i<delParam.Length;i++)
			{
                if (delParam[i].SourceColumn.ToString() == "BinarySign")
                {
                    sInsertSQL = sInsertSQL + " , cast(" + delParam[i].ParameterName + " as varbinary(MAX))";
                }
                else
                {
                    sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
                }
			}
			sInsertSQL = 	sInsertSQL + " )";
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
		private IDbDataParameter[] PKParameters(System.String POSTransTypeID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@POSTransTypeID";
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = POSTransTypeID;

			return(sqlParams);
		}


		private IDbDataParameter[] InsertParameters(POSTransPaymentRow row) 
		{
            //Updated By SRT(Gaurav)  Date : 21-Jul-2009
            //Updated from value 11 to 12, increased one parameter for accessing PaymentProcessor from database table.
            //Updated from value 37 to 40 to save data of Evertec Receipt Arvind PRIMEPOS-2664
            //PRIMEPOS-2664 AND PRIMEPOS-2786 EVERTEC CHANGED FROM 40 O 42
            //PRIMEPOS-2636 CHANGED FROM 43 TO 52 FOR VANTIV
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(71);    //Sprint-19 - 2139 06-Jan-2015 JY Changed from 16 to 17 // PRIMEPOS-2761 Changed from 42 to 43//Changed from 52 to 58 PRIMEPOS-2915//Changed from 58 to 63 Arvind PRIMEPOS-2664//2943//2990 //End Of Updated By SRT(Gaurav)//3009 Changed from 65 to 66 //PRIMEPOS-3117 11-Jul-2022 JY changed from 66 to 67    //PRIMEPOS-3145 28-Sep-2022 JY changed from 67 to 68 //PRIMEPOS-3375 changed from 68 to 71

            sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POSTransPayment_Fld_TransID, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POSTransPayment_Fld_RefNo, System.Data.DbType.String);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POSTransPayment_Fld_HC_Posted, System.Data.DbType.Boolean);
			sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POSTransPayment_Fld_TransDate, System.Data.DbType.DateTime);
			sqlParams[5] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POSTransPayment_Fld_CCTransNo, System.Data.DbType.String);
			sqlParams[6] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POSTransPayment_Fld_CCName, System.Data.DbType.String);
			sqlParams[7] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POSTransPayment_Fld_AuthNo, System.Data.DbType.String);
			sqlParams[8] = DataFactory.CreateParameter("@"+clsPOSDBConstants.POSTransPayment_Fld_Amount, System.Data.DbType.Currency);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_CustomerSign, System.Data.DbType.String);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_BinarySign, System.Data.DbType.Binary);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_SigType, System.Data.DbType.String);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment, System.Data.DbType.Boolean);
            //Added By SRT(Gaurav) Date : 21-Jul-2009
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor, System.Data.DbType.String);
            //End Of Added By SRT(Gaurav)
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_CLCouponID, System.Data.DbType.Int32);
            //Added By Shitaljit on 19 july 2012 to store Processor TransID
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID, System.Data.DbType.String);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_IsManual, System.Data.DbType.String);   //Sprint-19 - 2139 06-Jan-2015 JY Added

            #region EMV
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_AID, System.Data.DbType.String);
            //sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_AIDNAME, System.Data.DbType.String); // Commented for PRIMEPOS-2754 - Nileshj - 20_Jan_2020
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_A_IDNAME, System.Data.DbType.String); // PRIMEPOS-2754 - NileshJ - 20_Jan_2020
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_CRYTOGRAM, System.Data.DbType.String);
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TransactionCounter, System.Data.DbType.String);
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TerminalTVR, System.Data.DbType.String);
            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TransStatusInfo, System.Data.DbType.String);
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_AuthorizationResponse, System.Data.DbType.String);
            sqlParams[24] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TransRefNum, System.Data.DbType.String);
            sqlParams[25] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ValidateCode, System.Data.DbType.String);
            sqlParams[26] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_MerchantID, System.Data.DbType.String);
            sqlParams[27] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_EntryLegend, System.Data.DbType.String);
            sqlParams[28] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_EntryMethod, System.Data.DbType.String);
            sqlParams[29] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ProfiledID, System.Data.DbType.String);
            sqlParams[30] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_CardType, System.Data.DbType.String);
            sqlParams[31] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ProcTransType, System.Data.DbType.String);
            sqlParams[32] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_Verbiage, System.Data.DbType.String);
            sqlParams[33] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_RTransactionID, System.Data.DbType.String);

            //added by Rohit Nair For Wp EMV
            sqlParams[34] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_IssuerAppData, System.Data.DbType.String);
            sqlParams[35] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_CardVerificationMethod, System.Data.DbType.String);

            #endregion EMV

            sqlParams[36] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_S3TransID, System.Data.DbType.String);// Added for Solutran - PRIMEPOS-2663 - NileshJ

            #region Added by Arvind PRIMEPOS-2664
            sqlParams[37] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_BatchNumber, System.Data.DbType.String);
            sqlParams[38] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TraceNumber, System.Data.DbType.String);
            sqlParams[39] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_InvoiceNumber, System.Data.DbType.String);
            #endregion
            sqlParams[40] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TicketNumber, System.Data.DbType.String); // PRIMEPOS-2761 
            #region PRIMEPOS-2664 ADDED BY ARVIND
            sqlParams[41] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ControlNumber, System.Data.DbType.String);
            #endregion

            #region PRIMEPOS-2786 EVERTEC EBTBALANCE
            sqlParams[42] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_EbtBalance, System.Data.DbType.String);
            #endregion

            #region Added by Arvind PRIMEPOS-2636
            sqlParams[43] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TerminalID, System.Data.DbType.String);
            sqlParams[44] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ReferenceNumber, System.Data.DbType.String);
            sqlParams[45] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TransactionID, System.Data.DbType.String);
            sqlParams[46] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ResponseCode, System.Data.DbType.String);
            sqlParams[47] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ApprovalCode, System.Data.DbType.String);
            #endregion

            #region PRIMEPOS-2793
            sqlParams[48] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ApplicationLabel, System.Data.DbType.String);
            sqlParams[49] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_PinVerified, System.Data.DbType.Boolean);
            sqlParams[50] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_LaneID, System.Data.DbType.String);
            sqlParams[51] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_CardLogo, System.Data.DbType.String);
            #endregion

            #region PRIMEPOS-2915
            sqlParams[52] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID, System.Data.DbType.String);
            sqlParams[53] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount, System.Data.DbType.Decimal);
            sqlParams[54] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_Status, System.Data.DbType.String);

            sqlParams[55] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_Email, System.Data.DbType.String);
            sqlParams[56] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_Mobile, System.Data.DbType.String);
            sqlParams[57] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode, System.Data.DbType.Int32);
            #endregion

            #region primepos-2831
            sqlParams[58] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_IsEvertecForceTransaction, System.Data.DbType.Boolean);
            sqlParams[59] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_IsEvertecSign, System.Data.DbType.Boolean);
            #endregion

            sqlParams[60] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_EvertecTaxBreakdown, System.Data.DbType.String);//PRIMEPOS-2857

            sqlParams[61] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_CashBack, System.Data.DbType.String);//PRIMEPOS-2857

            sqlParams[62] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ATHMovil, System.Data.DbType.String);//2664

            sqlParams[63] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_ExpDate, System.Data.DbType.DateTime);//2943
            sqlParams[64] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_IsFsaCard, System.Data.DbType.Boolean);//2990
            sqlParams[65] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TokenID, System.Data.DbType.Int32);//3009
            sqlParams[66] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt, System.Data.DbType.Currency);  //PRIMEPOS-3117 11-Jul-2022 JY Added
            sqlParams[67] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_Tokenize, System.Data.DbType.Boolean);  //PRIMEPOS-3145 28-Sep-2022 JY Added
            sqlParams[68] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_NBSTransId, System.Data.DbType.String);//PRIMEPOS-3375
            sqlParams[69] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid, System.Data.DbType.String);//PRIMEPOS-3375
            sqlParams[70] = DataFactory.CreateParameter("@" + clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType, System.Data.DbType.String);//PRIMEPOS-3375

            sqlParams[0].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TransID;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.POSTransPayment_Fld_RefNo;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.POSTransPayment_Fld_HC_Posted;
			sqlParams[4].SourceColumn  = clsPOSDBConstants.POSTransPayment_Fld_TransDate;
			sqlParams[5].SourceColumn  = clsPOSDBConstants.POSTransPayment_Fld_CCTransNo;
			sqlParams[6].SourceColumn  = clsPOSDBConstants.POSTransPayment_Fld_CCName;
			sqlParams[7].SourceColumn  = clsPOSDBConstants.POSTransPayment_Fld_AuthNo;
			sqlParams[8].SourceColumn  = clsPOSDBConstants.POSTransPayment_Fld_Amount;
            sqlParams[9].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_CustomerSign;
            sqlParams[12].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_BinarySign;
            sqlParams[13].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_SigType;
            sqlParams[10].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_IsIIASPayment;
            //Added By SRT(Gaurav) Date : 21-Jul-2009
            sqlParams[11].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor;
            //End Of Added By SRT(Gaurav)
            sqlParams[14].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_CLCouponID;
            //Added By Shitaljit on 19 july 2012 to store Processor TransID
            sqlParams[15].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID;
            sqlParams[16].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_IsManual;
            sqlParams[17].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_AID;
            sqlParams[18].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_AIDNAME;
            sqlParams[19].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_CRYTOGRAM;
            sqlParams[20].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TransactionCounter;
            sqlParams[21].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TerminalTVR;
            sqlParams[22].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TransStatusInfo;
            sqlParams[23].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_AuthorizationResponse;
            sqlParams[24].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TransRefNum;
            sqlParams[25].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ValidateCode;
            sqlParams[26].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_MerchantID;
            sqlParams[27].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_EntryLegend;
            sqlParams[28].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_EntryMethod;
            sqlParams[29].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ProfiledID;
            sqlParams[30].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_CardType;
            sqlParams[31].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ProcTransType;
            sqlParams[32].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_Verbiage;
            sqlParams[33].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_RTransactionID;

            //Added by Rohit Nair for WP EMV
            sqlParams[34].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_IssuerAppData;
            sqlParams[35].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_CardVerificationMethod;

            sqlParams[36].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_S3TransID; // Added for Solutran - PRIMEPOS-2663 - NileshJ

            //Added by Arvind PRIMEPOS-2664
            sqlParams[37].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_BatchNumber;
            sqlParams[38].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TraceNumber;
            sqlParams[39].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_InvoiceNumber;
            sqlParams[40].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TicketNumber; //PRIMEPOS-2761 - NileshJ
            //PRIMEPOS-2664 ADDED BY ARVIND
            sqlParams[41].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ControlNumber;
            //

            #region PRIMEPOS-2786 EVERTEC EBTBALANCE
            sqlParams[42].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_EbtBalance;
            #endregion

            //Added by Arvind PRIMEPOS-2636
            sqlParams[43].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TerminalID;
            sqlParams[44].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ReferenceNumber;
            sqlParams[45].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TransactionID;
            sqlParams[46].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ResponseCode;
            sqlParams[47].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ApprovalCode;

            #region PRIMEPOS-2793
            sqlParams[48].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ApplicationLabel;
            sqlParams[49].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_PinVerified;
            sqlParams[50].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_LaneID;
            sqlParams[51].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_CardLogo;
            #endregion

            #region PRIMEPOS-2915
            sqlParams[52].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_PrimeRxPayTransID;//2915
            sqlParams[53].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount;//2915
            sqlParams[54].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_Status;//2915

            sqlParams[55].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_Email;//2915
            sqlParams[56].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_Mobile;//2915
            sqlParams[57].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TransactionProcessingMode;//2915
            #endregion

            #region primepos-2831
            sqlParams[58].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_IsEvertecForceTransaction;
            sqlParams[59].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_IsEvertecSign;
            #endregion
            sqlParams[60].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_EvertecTaxBreakdown;//PRIMEPOS-2857
            sqlParams[61].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_CashBack;//PRIMEPOS-2857

            sqlParams[62].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ATHMovil;//2664
            sqlParams[63].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_ExpDate;//2943
            sqlParams[64].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_IsFsaCard;//2990
            sqlParams[65].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TokenID;//3009
            sqlParams[66].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_TransFeeAmt; //PRIMEPOS-3117 11-Jul-2022 JY Added
            sqlParams[67].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_Tokenize;    //PRIMEPOS-3145 28-Sep-2022 JY Added
            sqlParams[68].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_NBSTransId;//PRIMEPOS-3375
            sqlParams[69].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid;//PRIMEPOS-3375
            sqlParams[70].SourceColumn = clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType;//PRIMEPOS-3375

            if (row.TransID != 0 )
				sqlParams[0].Value = row.TransID;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.TransTypeCode != System.String.Empty )
				sqlParams[1].Value = row.TransTypeCode;
			else
				sqlParams[1].Value = DBNull.Value ;

            if (row.RefNo != System.String.Empty)
            {
                if (row.RefNo.Length > 50)
                {
                    sqlParams[2].Value = row.RefNo.Substring(0,50);
                }
                else
                {
                    sqlParams[2].Value = row.RefNo;
                }
            }
            else
                sqlParams[2].Value = DBNull.Value;

			sqlParams[3].Value = row.HC_Posted;

			if (row.TransDate != System.DateTime.MinValue )
				sqlParams[4].Value = row.TransDate;
			else
				sqlParams[4].Value = System.DateTime.MinValue ;

			if (row.CCTransNo != System.String.Empty )
				sqlParams[5].Value = row.CCTransNo;
			else
				sqlParams[5].Value = DBNull.Value ;

            if (row.CCName != System.String.Empty)
            {
                if (row.CCName.Length > 100)
                {
                    sqlParams[6].Value = row.CCName.Substring(0, 100);
                }
                else
                {
                    sqlParams[6].Value = row.CCName;
                }
            }
            else
                sqlParams[6].Value = DBNull.Value;

			if (row.AuthNo != System.String.Empty )
				sqlParams[7].Value = row.AuthNo;
			else
				sqlParams[7].Value = DBNull.Value ;

			if (row.Amount != 0 )
				sqlParams[8].Value = row.Amount;
			else
				sqlParams[8].Value = 0 ;

            if (row.CustomerSign!= System.String.Empty)
                sqlParams[9].Value = row.CustomerSign;
            else
                sqlParams[9].Value = DBNull.Value;

            if (row.BinarySign != null)
                sqlParams[12].Value = row.BinarySign;
            else
                sqlParams[12].Value = DBNull.Value;

            if (row.SigType != System.String.Empty)
                sqlParams[13].Value = row.SigType;
            else
                sqlParams[13].Value = DBNull.Value;

            sqlParams[10].Value = row.IsIIASPayment;
            
            //Added By SRT(Gaurav) Date : 21-Jul-2009
            //If payment processor value of row is absent then the noprocessor value from clsposdbconstant is set to table.
            if (row.PaymentProcessor != System.String.Empty)
                sqlParams[11].Value = row.PaymentProcessor;
            else
                sqlParams[11].Value = clsPOSDBConstants.NOPROCESSOR;
            //End Of Added by SRT(Gaurav)

            if (row.CLCouponID != 0)
                sqlParams[14].Value = row.CLCouponID;
            else
                sqlParams[14].Value = DBNull.Value;

            //Added By Shitaljit on 19 july 2012 to store Processor TransID
            if (row.ProcessorTransID != System.String.Empty)
            {
                if (row.ProcessorTransID.Length > 100)
                {
                    sqlParams[15].Value = row.ProcessorTransID.Substring(0, 100);
                }
                else
                {
                    sqlParams[15].Value = row.ProcessorTransID;
                }
            }
            else
                sqlParams[15].Value = DBNull.Value;

            //Sprint-19 - 2139 06-Jan-2015 JY Added
            if (row.IsManual != System.String.Empty)
                sqlParams[16].Value = row.IsManual;
            else
                sqlParams[16].Value = DBNull.Value;

            #region EMV
            sqlParams[17].Value = !string.IsNullOrEmpty(row.Aid) ? row.Aid : (object)DBNull.Value;
            sqlParams[18].Value = !string.IsNullOrEmpty(row.AidName) ? row.AidName : (object)DBNull.Value;
            sqlParams[19].Value = !string.IsNullOrEmpty(row.Cryptogram) ? row.Cryptogram : (object)DBNull.Value;
            sqlParams[20].Value = !string.IsNullOrEmpty(row.TransCounter) ? row.TransCounter : (object)DBNull.Value;
            sqlParams[21].Value = !string.IsNullOrEmpty(row.TerminalTvr) ? row.TerminalTvr : (object)DBNull.Value;
            sqlParams[22].Value = !string.IsNullOrEmpty(row.TransStatusInfo) ? row.TransStatusInfo : (object)DBNull.Value;
            sqlParams[23].Value = !string.IsNullOrEmpty(row.AuthorizeRespCode) ? row.AuthorizeRespCode : (object)DBNull.Value;
            sqlParams[24].Value = !string.IsNullOrEmpty(row.TransRefNum) ? row.TransRefNum : (object)DBNull.Value;
            sqlParams[25].Value = !string.IsNullOrEmpty(row.ValidateCode) ? row.ValidateCode : (object)DBNull.Value;
            sqlParams[26].Value = !string.IsNullOrEmpty(row.MerchantID) ? row.MerchantID : (object)DBNull.Value;
            sqlParams[27].Value = !string.IsNullOrEmpty(row.EntryLegend) ? row.EntryLegend : (object)DBNull.Value;
            sqlParams[28].Value = !string.IsNullOrEmpty(row.EntryMethod) ? row.EntryMethod : (object)DBNull.Value;
            sqlParams[29].Value = !string.IsNullOrEmpty(row.ProfiledID) ? row.ProfiledID : (object)DBNull.Value;
            sqlParams[30].Value = !string.IsNullOrEmpty(row.CardType) ? row.CardType : (object)DBNull.Value;
            sqlParams[31].Value = !string.IsNullOrEmpty(row.ProcTransType) ? row.ProcTransType : (object)DBNull.Value;
            sqlParams[32].Value = !string.IsNullOrEmpty(row.Verbiage) ? row.Verbiage : (object)DBNull.Value;
            sqlParams[33].Value = !string.IsNullOrEmpty(row.RTransID) ? row.RTransID : (object)DBNull.Value;

            //Added by Rohit Nair for WP EMV
            sqlParams[34].Value = !string.IsNullOrEmpty(row.IssuerAppData) ? row.IssuerAppData : (object)DBNull.Value;
            sqlParams[35].Value = !string.IsNullOrEmpty(row.CardVerificationMethod) ? row.CardVerificationMethod : (object)DBNull.Value;


            #endregion EMV

            sqlParams[36].Value = !string.IsNullOrEmpty(row.S3TransID) ? row.S3TransID : (object)DBNull.Value;  // Added for Solutran - PRIMEPOS-2663 - NileshJ

            #region Added by Arvind PRIMEPOS-2664
            if (row.BatchNumber != System.String.Empty)
                sqlParams[37].Value = row.BatchNumber;
            else
                sqlParams[37].Value = DBNull.Value;

            if (row.TraceNumber != System.String.Empty)
                sqlParams[38].Value = row.TraceNumber;
            else
                sqlParams[38].Value = DBNull.Value;

            if (row.InvoiceNumber != System.String.Empty)
                sqlParams[39].Value = row.InvoiceNumber;
            else
                sqlParams[39].Value = DBNull.Value;
            #endregion

            sqlParams[40].Value = !string.IsNullOrEmpty(row.TicketNumber) ? row.TicketNumber : (object)DBNull.Value;  //  PRIMEPOS-2761 - NileshJ

            //PRIMEPOS-2664 ADDED BY ARVIND
            if (row.ControlNumber != System.String.Empty)
                sqlParams[41].Value = row.ControlNumber;
            else
                sqlParams[41].Value = DBNull.Value;
            //
            #region PRIMEPOS-2786 ADDED BY ARVIND EVERTEC EBTBALANCE
            if (row.EbtBalance != System.String.Empty)
                sqlParams[42].Value = row.EbtBalance;
            else
                sqlParams[42].Value = DBNull.Value;
            #endregion

            //Added by Arvind PRIMEPOS-2636
            if (row.TerminalID != System.String.Empty)
                sqlParams[43].Value = row.TerminalID;
            else
                sqlParams[43].Value = DBNull.Value;
            if (row.ReferenceNumber != System.String.Empty)
                sqlParams[44].Value = row.ReferenceNumber;
            else
                sqlParams[44].Value = DBNull.Value;
            if (row.TransactionID != System.String.Empty)
                sqlParams[45].Value = row.TransactionID;
            else
                sqlParams[45].Value = DBNull.Value;
            if (row.ResponseCode != System.String.Empty)
                sqlParams[46].Value = row.ResponseCode;
            else
                sqlParams[46].Value = DBNull.Value;
            if (row.ApprovalCode != System.String.Empty)
                sqlParams[47].Value = row.ApprovalCode;
            else
                sqlParams[47].Value = DBNull.Value;

            #region PRIMEPOS-2793
            if (row.ApplicaionLabel != System.String.Empty)
                sqlParams[48].Value = row.ApplicaionLabel;
            else
                sqlParams[48].Value = DBNull.Value;

            sqlParams[49].Value = row.PinVerified;

            if (row.LaneID != System.String.Empty)
                sqlParams[50].Value = row.LaneID;
            else
                sqlParams[50].Value = DBNull.Value;
            if (row.CardLogo != System.String.Empty)
                sqlParams[51].Value = row.CardLogo;
            else
                sqlParams[51].Value = DBNull.Value;
            #endregion

            #region PRIMEPOS- 2915
            if (row.PrimeRxPayTransID != System.String.Empty)
                sqlParams[52].Value = row.PrimeRxPayTransID;
            else
                sqlParams[52].Value = DBNull.Value;

            if (row.ApprovedAmount != 0)
                sqlParams[53].Value = row.ApprovedAmount;
            else
                sqlParams[53].Value = 0;

            if (row.Status != System.String.Empty)
                sqlParams[54].Value = row.Status;
            else
                sqlParams[54].Value = DBNull.Value;

            if (row.Email != System.String.Empty)
                sqlParams[55].Value = row.Email;
            else
                sqlParams[55].Value = DBNull.Value;

            if (row.Mobile != System.String.Empty)
                sqlParams[56].Value = row.Mobile;
            else
                sqlParams[56].Value = DBNull.Value;

            if (row.TransactionProcessingMode != 0)
                sqlParams[57].Value = row.TransactionProcessingMode;
            else
                sqlParams[57].Value = 0;
            #endregion

            #region primepos-2831
            sqlParams[58].Value = row.IsEvertecForceTransaction;
            sqlParams[59].Value = row.IsEvertecSign;
            #endregion
            if (row.EvertecTaxBreakdown != System.String.Empty)//primepos-2857
                sqlParams[60].Value = row.EvertecTaxBreakdown;
            else
                sqlParams[60].Value = DBNull.Value;

            if (row.CashBack != System.String.Empty)//primepos-2857
                sqlParams[61].Value = row.CashBack;
            else
                sqlParams[61].Value = DBNull.Value;

            //PRIMEPOS-2664 ADDED BY ARVIND
            if (row.ATHMovil != System.String.Empty)
                sqlParams[62].Value = row.ATHMovil;
            else
                sqlParams[62].Value = DBNull.Value;

            if (row.ExpiryDate != System.DateTime.MinValue)//2943
                sqlParams[63].Value = row.ExpiryDate;
            else
                sqlParams[63].Value = Convert.ToDateTime("1900-01-01");

            sqlParams[64].Value = row.IsFsaCard;//2990

            if (row.TokenID != 0)//3009
                sqlParams[65].Value = row.TokenID;
            else
                sqlParams[65].Value = DBNull.Value;

            if (row.TransFeeAmt != 0)   //PRIMEPOS-3117 11-Jul-2022 JY Added
                sqlParams[66].Value = row.TransFeeAmt;
            else
                sqlParams[66].Value = 0;
            sqlParams[67].Value = row.Tokenize; //PRIMEPOS-3145 28-Sep-2022 JY Added

            if (row.NBSTransId != System.String.Empty) //PRIMEPOS-3375
                sqlParams[68].Value = row.NBSTransId;
            else
                sqlParams[68].Value = DBNull.Value;

            if (row.NBSTransUid != System.String.Empty) //PRIMEPOS-3375
                sqlParams[69].Value = row.NBSTransUid;
            else
                sqlParams[69].Value = DBNull.Value;

            if (row.NBSPaymentType != System.String.Empty) //PRIMEPOS-3375
                sqlParams[70].Value = row.NBSPaymentType;
            else
                sqlParams[70].Value = DBNull.Value;

            return (sqlParams);
		}

		#endregion

		#region Get Methods

		// Looks up a ItemVendor based on its primary-key:System.String VendorItemID

		public DataSet Populate(System.Int32 TransId, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
					+ " TransPmt.* " 
					+ " , PType." + clsPOSDBConstants.PayType_Fld_PayTypeDescription
                    + " , POSTrans." + clsPOSDBConstants.TransHeader_Fld_TenderedAmount
                    + " FROM " 
					+ clsPOSDBConstants.POSTransPayment_tbl + " as TransPmt "
					+ " , " + clsPOSDBConstants.PayType_tbl + " as PType"
                    + " , " + clsPOSDBConstants.TransHeader_tbl + " as POSTrans"
                    + " WHERE " 
					+ " TransPmt. " + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " = PType." + clsPOSDBConstants.PayType_Fld_PayTypeID
                    + " and POSTrans." + clsPOSDBConstants.POSTransPayment_Fld_TransID + " = TransPmt." + clsPOSDBConstants.POSTransPayment_Fld_TransID
                    + " and TransPmt." + clsPOSDBConstants.POSTransPayment_Fld_TransID + " = " + TransId;
	
				DataSet ds = new DataSet();
				ds=DataHelper.ExecuteDataset(conn, CommandType.Text,sSQL );
				return ds;
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

        //public POSTransPaymentData PopulateSignaturedata(System.Int32 TransId)
        //{
        //    IDbConnection conn = DataFactory.CreateConnection();
        //    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
        //    try
        //    {
        //        string sSQL = "Select "
        //            + " TransPmt.* "
        //            + " , PType." + clsPOSDBConstants.PayType_Fld_PayTypeDescription
        //            + " FROM "
        //            + clsPOSDBConstants.POSTransPayment_tbl + " as TransPmt "
        //            + " , " + clsPOSDBConstants.PayType_tbl + " as PType"
        //            + " WHERE "
        //            + " TransPmt. " + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " = PType." + clsPOSDBConstants.PayType_Fld_PayTypeID
        //             + " and TransPmt." + clsPOSDBConstants.POSTransPayment_Fld_SigType+ " IS NOT NULL "
        //            + " and TransPmt." + clsPOSDBConstants.POSTransPayment_Fld_TransID + " = " + TransId;

        //        DataSet ds = new DataSet();
        //        ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
        //        POSTransPaymentData oTP = new POSTransPaymentData();
        //        oTP.POSTransPayment.MergeTable(ds.Tables[0]);
        //        return oTP;
        //    }
        //    catch (POSExceptions ex)
        //    {
        //        logger.Fatal(ex, "PopulateSignaturedata(System.Int32 TransId)");
        //        throw (ex);
        //    }

        //    catch (OtherExceptions ex)
        //    {
        //        logger.Fatal(ex, "PopulateSignaturedata(System.Int32 TransId)");
        //        throw (ex);
        //    }

        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "PopulateSignaturedata(System.Int32 TransId)");
        //        ErrorHandler.throwException(ex, "", "");
        //        return null;
        //    }
        //}

        public POSTransPaymentData PopulateSignaturedata(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            try
            {
                string sSQL = "Select "
                    + " TransPmt.* "
                    + " , PType." + clsPOSDBConstants.PayType_Fld_PayTypeDescription
                    + " FROM "
                    + clsPOSDBConstants.POSTransPayment_tbl + " as TransPmt "
                    + " , " + clsPOSDBConstants.PayType_tbl + " as PType"
                    + " WHERE "
                    + " TransPmt. " + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " = PType." + clsPOSDBConstants.PayType_Fld_PayTypeID
                     + " and TransPmt." + clsPOSDBConstants.POSTransPayment_Fld_SigType + " IS NOT NULL "
                    + " and TransPmt." + clsPOSDBConstants.POSTransPayment_Fld_TransID + " = " + TransId;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                POSTransPaymentData oTP = new POSTransPaymentData();
                oTP.POSTransPayment.MergeTable(ds.Tables[0]);
                return oTP;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateSignaturedata(System.Int32 TransId)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateSignaturedata(System.Int32 TransId)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateSignaturedata(System.Int32 TransId)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        #region PRIMEPOS-2939 03-Mar-2021 JY Added
        public POSTransPaymentData PopulateSignaturedata(System.Int32 TransId, System.Int32 TransPayID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            try
            {
                string sSQL = "SELECT TransPmt.*, PType.PayTypeDesc FROM POSTransPayment AS TransPmt " +
                            " INNER JOIN PayType AS PType ON TransPmt.TransTypeCode = PType.PayTypeID " +
                            " WHERE TransPmt.SigType IS NOT NULL and TransPmt.TransPayID = " + TransPayID;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                POSTransPaymentData oTP = new POSTransPaymentData();
                oTP.POSTransPayment.MergeTable(ds.Tables[0]);
                return oTP;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateSignaturedata(System.Int32 TransId, System.Int32 TransPayID)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateSignaturedata(System.Int32 TransId, System.Int32 TransPayID)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateSignaturedata(System.Int32 TransId, System.Int32 TransPayID)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        public POSTransPaymentData Populate(System.Int32 TransId) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			DataSet ds=Populate(TransId, conn);
			
			POSTransPaymentData oTP = new POSTransPaymentData();
			oTP.POSTransPayment.MergeTable(ds.Tables[0]);
			return oTP;
		}

        public DataSet ValidateReturnTransID(System.Int32 TransId, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + " TransPmt.* "
                    + " , PType." + clsPOSDBConstants.PayType_Fld_PayTypeDescription
                    + " FROM "
                    + clsPOSDBConstants.POSTransPayment_tbl + " as TransPmt "
                    + " , " + clsPOSDBConstants.PayType_tbl + " as PType"
                    + " WHERE "
                    + " TransPmt. " + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " = PType." + clsPOSDBConstants.PayType_Fld_PayTypeID
                    + " and TransPmt." + clsPOSDBConstants.POSTransPayment_Fld_TransID + " = " + TransId;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "ValidateReturnTransID(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "ValidateReturnTransID(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "ValidateReturnTransID(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

		#endregion //Get Method
    
		public void Dispose() {}

        //  Added for Solutran - PRIMEPOS-2663 - NileshJ
        public DataSet GetTransPaymentDetail(String TransId, IDbConnection conn, String TranstypeCode)
        {
            try
            {
                string sSQL = sSQL = String.Concat("SELECT  ", clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_Amount, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_RefNo, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_TransDate
                    , " FROM "
                    , clsPOSDBConstants.POSTransPayment_tbl
                    , "  WHERE "
                    , clsPOSDBConstants.POSTransPayment_Fld_TransID, " = " + TransId + " AND ", clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " IN (" + TranstypeCode + ")");

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetTransPaymentDetail( TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetTransPaymentDetail( TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTransPaymentDetail( TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #region PRIMEPOS-2738 
        public void SetReversedAmountDetails(DataSet ds)
        {
            for (int count = 0; count < ds.Tables[0].Rows.Count; count++)
            {
                IDbConnection conn = null;
                conn = DataFactory.CreateConnection(Resources.Configuration.ConnectionString);
                IDbTransaction otrans = null;
                otrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {

                    //Add columns for ReversedAmount in the table
                    string sSQL;
                    sSQL = sSQL = String.Format("Update " + clsPOSDBConstants.POSTransPayment_tbl + " SET "
                        + clsPOSDBConstants.POSTransPayment_Fld_ReversedAmount +
                         " = " +
                         ds.Tables[0].Rows[count]["ReversedAmount"].ToString() +
                         "  WHERE " +
                         clsPOSDBConstants.POSTransPayment_Fld_TransPayID + " = " + ds.Tables[0].Rows[count]["TransPayID"].ToString());
                    DataHelper.ExecuteNonQuery(otrans, CommandType.Text, sSQL);
                    otrans.Commit();
                }
                catch (POSExceptions ex)
                {
                    logger.Fatal(ex, "SetReversedAmountDetails( TransId, IDbConnection conn)");
                    if (otrans != null)
                        otrans.Rollback();
                    throw (ex);
                }

                catch (OtherExceptions ex)
                {
                    logger.Fatal(ex, "SetReversedAmountDetails( TransId, IDbConnection conn)");
                    if (otrans != null)
                        otrans.Rollback();
                    throw (ex);
                }

                catch (Exception ex)
                {
                    logger.Fatal(ex, "SetReversedAmountDetails( TransId, IDbConnection conn)");
                    if (otrans != null)
                        otrans.Rollback();
                    ErrorHandler.throwException(ex, "", "");
                }
            }
        }
        #endregion
        #region PRIMEPOS-2738 
        public DataSet GetPaymentDetails(String transId, IDbConnection conn, String transtypeCode, DataSet oOrigTransID, bool isPrimeRxPay)
        {
            try
            {

                //Add columns for ReversedAmount in the table
                string sSQL;
                sSQL = sSQL = String.Concat("SELECT  ", clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_Amount, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_RefNo, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_TransRefNum, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_ReversedAmount, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_TransPayID, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_TransDate, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_NBSTransUid, ","
                    , clsPOSDBConstants.POSTransPayment_Fld_NBSPaymentType
                    , " FROM "
                    , clsPOSDBConstants.POSTransPayment_tbl
                    , " Left Join "
                    , clsPOSDBConstants.PayType_tbl
                    , " ON  POSTransPayment.TransTypeCode = PayType.PayTypeID"
                    , "  WHERE "
                    , clsPOSDBConstants.POSTransPayment_Fld_TransID, " = " + transId + " AND " + clsPOSDBConstants.POSTransPayment_Fld_Amount + " > " + clsPOSDBConstants.POSTransPayment_Fld_ReversedAmount);
                
                if(transtypeCode !="N") //PRIMEPOS-3375
                {
                    if (transtypeCode.Contains("3") || transtypeCode.Contains("4") || transtypeCode.Contains("5") || transtypeCode.Contains("6"))
                    {

                        sSQL += " AND " + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " in (" + transtypeCode + ")";
                    }
                    else
                    {
                        sSQL += " AND " + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " in ('" + transtypeCode + "')";
                    }
                }
                else if (transtypeCode =="N") 
                {
                    sSQL += " and " + clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor + "='NB_VANTIV' "; //PRIMEPOS-3482
                }

                #region PRIMEPOS-3189
                if (isPrimeRxPay)
                {
                    sSQL += " and " + clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor + "='PRIMERXPAY' ";
                }
                else
                {
                    sSQL += " and " + clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor + "<>'PRIMERXPAY' ";
                }
                #endregion

                if (oOrigTransID.Tables.Count > 0)
                {
                    var transPayIDs = oOrigTransID.Tables[0].AsEnumerable().Where(r => r.Field<decimal>("ReversedAmount") == r.Field<decimal>("Amount")).Select(a => a.Field<int>("TransPayID"));

                    foreach (int transPayID in transPayIDs)
                    {
                        sSQL += " AND " + clsPOSDBConstants.POSTransPayment_Fld_TransPayID + " <> " + transPayID.ToString();
                    }
                }
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetTransPaymentDetail( TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetTransPaymentDetail( TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTransPaymentDetail( TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion
        #region PRIMEPOS-2738
        public DataSet GetPOSPaymentDetail(String TransId, IDbConnection conn, String TranstypeCode)
        {
            try
            {
                string sSQL = sSQL = String.Concat("SELECT ", clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID, ","
                        , clsPOSDBConstants.POSTransPayment_Fld_Amount, ","
                        , clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode, ","
                        , clsPOSDBConstants.POSTransPayment_Fld_RefNo, ","
                        , clsPOSDBConstants.POSTransPayment_Fld_TransRefNum, ","
                        , clsPOSDBConstants.POSTransPayment_Fld_ReversedAmount, ","
                        , clsPOSDBConstants.POSTransPayment_Fld_TransDate, ","
                        , " UPPER(LTRIM(RTRIM(PAYMENTPROCESSOR))) AS PAYMENTPROCESSOR FROM "   //PRIMEPOS-3181 19-Jan-2023 JY Added
                        , clsPOSDBConstants.POSTransPayment_tbl
                        , " Left Join "
                        , clsPOSDBConstants.PayType_tbl
                        , " ON  POSTransPayment.TransTypeCode = PayType.PayTypeID "
                        , "  WHERE "
                        , clsPOSDBConstants.POSTransPayment_Fld_TransID, " = " + TransId + " AND " + clsPOSDBConstants.POSTransPayment_Fld_Amount + " > " + clsPOSDBConstants.POSTransPayment_Fld_ReversedAmount);
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetTransPaymentDetail( TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetTransPaymentDetail( TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTransPaymentDetail( TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region PRIMEPOS-2761
        public DataSet GetTransPaymentID(String TransId, IDbConnection conn)
        {
            logger.Trace("UpdateRxTransDataLocally(GetTransPaymentID(String TransId, IDbConnection conn) - " + clsPOSDBConstants.Log_Entering);
            try
            {
                string sSQL = sSQL = String.Concat("SELECT  ", clsPOSDBConstants.POSTransPayment_Fld_TransPayID, " , ", clsPOSDBConstants.POSTransPayment_Fld_TransID, " , ", clsPOSDBConstants.POSTransPayment_Fld_TicketNumber
                    , " FROM "
                    , clsPOSDBConstants.POSTransPayment_tbl
                    , "  WHERE "
                    , clsPOSDBConstants.POSTransPayment_Fld_TransID, " = " + TransId);// + " AND ", clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " IN (" + TicketNum + ")");

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                logger.Trace("UpdateRxTransDataLocally(GetTransPaymentID(String TransId, IDbConnection conn) - " + clsPOSDBConstants.Log_Exiting);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "UpdateRxTransDataLocally(GetTransPaymentID(String TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "UpdateRxTransDataLocally(GetTransPaymentID(String TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdateRxTransDataLocally(GetTransPaymentID(String TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }

        }
        #endregion
        #region PRIMEPOS-2841
        public DataSet GetPharmacyNPI(IDbConnection oConn)
        {
            try
            {

                //Add columns for ReversedAmount in the table
                string sSQL;
                sSQL = "SELECT " + clsPOSDBConstants.PHNPINO + " FROM " + clsPOSDBConstants.Util_Company_Info;
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(oConn, CommandType.Text, sSQL);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetPharmacyNPI()");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetPharmacyNPI()");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "GetPharmacyNPI()");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region PRIMEPOS-2915
        public void SetPrimeRxPayPartialTrans(string TransId, string ApprovedAmount, string TransactionID, string TransTypeCode, string RefNo, string ProfiledID, Nullable<DateTime> ExpDate, bool IsFsaCard)//PRIMEPOS-3186 Added 4 field add in Query also
        {
            IDbConnection conn = null;
            conn = DataFactory.CreateConnection(Resources.Configuration.ConnectionString);
            IDbTransaction otrans = null;
            otrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {

                //Add columns for ReversedAmount in the table
                string sSQL;
                sSQL = sSQL = String.Format("Update " + clsPOSDBConstants.POSTransPayment_OnHold_tbl + " SET "
                    + clsPOSDBConstants.POSTransPayment_Fld_ApprovedAmount +
                     " = " +
                     ApprovedAmount +
                    "," + clsPOSDBConstants.POSTransPayment_Fld_Status +
                     " = " +
                     "'" + clsPOSDBConstants.PosTransPayment_Status_Completed + "'" +
                     "," + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode +
                     " = " +
                     "'" + TransTypeCode + "'" +
                     "," + clsPOSDBConstants.POSTransPayment_Fld_PaymentProcessor +
                     " = " +
                     "'PRIMERXPAY'" +
                     "," + clsPOSDBConstants.POSTransPayment_Fld_ProcessorTransID +
                     " = " +
                     "'" + TransactionID + "'" +
                     "," + clsPOSDBConstants.POSTransPayment_Fld_RefNo +
                     " = " +
                     "'" + RefNo + "'" +
                     "," + clsPOSDBConstants.POSTransPayment_Fld_ProfiledID +
                     " = " +
                     "'" + ProfiledID + "'" +
                     "," + clsPOSDBConstants.POSTransPayment_Fld_ExpDate +
                     " = " +
                     "'" + ExpDate + "'" +
                     "," + clsPOSDBConstants.POSTransPayment_Fld_IsFsaCard +
                     " = " +
                     "'" + IsFsaCard + "'" +
                     "  WHERE " +
                     clsPOSDBConstants.POSTransPayment_Fld_TransPayID + " = " + TransId);
                DataHelper.ExecuteNonQuery(otrans, CommandType.Text, sSQL);
                otrans.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "SetPrimeRxPayPartialTrans( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "SetPrimeRxPayPartialTrans( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "SetPrimeRxPayPartialTrans( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }

        public void DeleteDuplicateRows()
        {
            IDbConnection conn = null;
            conn = DataFactory.CreateConnection(Resources.Configuration.ConnectionString);
            IDbTransaction otrans = null;
            otrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {

                //Add columns for ReversedAmount in the table
                string sSQL;
                sSQL = " WITH CTE AS(SELECT[PrimeRxPayTransID],RN = ROW_NUMBER()OVER(PARTITION BY PrimeRxPaytransid ORDER BY PrimeRxPaytransid)FROM dbo.POSTransPayment_OnHold  where PrimeRxPaytransid is not null) DELETE FROM CTE WHERE RN > 1";
                DataHelper.ExecuteNonQuery(otrans, CommandType.Text, sSQL);
                otrans.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "DeleteDuplicateRows( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "DeleteDuplicateRows( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteDuplicateRows( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }

        public void SetPrimeRxPayInProgressTrans(string TransId)//PRIMEPOS-3189
        {
            IDbConnection conn = null;
            conn = DataFactory.CreateConnection(Resources.Configuration.ConnectionString);
            IDbTransaction otrans = null;
            otrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {

                //Add columns for ReversedAmount in the table
                string sSQL;
                sSQL = sSQL = String.Format("Update " + clsPOSDBConstants.POSTransPayment_OnHold_tbl + " SET "
                    + clsPOSDBConstants.POSTransPayment_Fld_Status +
                     " = " +
                     "'" + clsPOSDBConstants.PosTransPayment_Status_InProgress + "'" +
                     "  WHERE " +
                     clsPOSDBConstants.POSTransPayment_Fld_TransPayID + " = " + TransId);
                DataHelper.ExecuteNonQuery(otrans, CommandType.Text, sSQL);
                otrans.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "SetPrimeRxPayInProgressTrans( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "SetPrimeRxPayInProgressTrans( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "SetPrimeRxPayInProgressTrans( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }

        public void SetPrimeRxPayExpiredTrans(string TransId)
        {
            IDbConnection conn = null;
            conn = DataFactory.CreateConnection(Resources.Configuration.ConnectionString);
            IDbTransaction otrans = null;
            otrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {

                //Add columns for ReversedAmount in the table
                string sSQL;
                sSQL = sSQL = String.Format("Update " + clsPOSDBConstants.POSTransPayment_OnHold_tbl + " SET "
                    + clsPOSDBConstants.POSTransPayment_Fld_Status +
                     " = " +
                     "'" + clsPOSDBConstants.PosTransPayment_Status_Expired + "'" +
                     "  WHERE " +
                     clsPOSDBConstants.POSTransPayment_Fld_TransPayID + " = " + TransId);
                DataHelper.ExecuteNonQuery(otrans, CommandType.Text, sSQL);
                otrans.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "SetReversedAmountDetails( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "SetReversedAmountDetails( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "SetReversedAmountDetails( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }
        public void DeletePrimeRxPayCanceledTrans(string TransId)
        {
            IDbConnection conn = null;
            conn = DataFactory.CreateConnection(Resources.Configuration.ConnectionString);
            IDbTransaction otrans = null;
            otrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {

                //Add columns for ReversedAmount in the table
                string sSQL;
                sSQL = sSQL = String.Format("delete from Postranspayment_onhold  " +
                     "  WHERE " +
                     clsPOSDBConstants.POSTransPayment_Fld_TransPayID + " = " + TransId);
                DataHelper.ExecuteNonQuery(otrans, CommandType.Text, sSQL);
                otrans.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "DeletePrimeRxPayCanceledTrans( TransId )");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "DeletePrimeRxPayCanceledTrans( TransId )");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "DeletePrimeRxPayCanceledTrans( TransId )");
                if (otrans != null)
                    otrans.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
        }
        public POSTransPaymentData PopulateOnHold(System.Int32 TransId)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
            DataSet ds = PopulateOnHold(TransId, conn);

            POSTransPaymentData oTP = new POSTransPaymentData();
            oTP.POSTransPayment.MergeTable(ds.Tables[0]);
            return oTP;
        }
        public DataSet PopulateOnHold(System.Int32 TransId, IDbConnection conn)//PRIMEPOS-3189
        {
            try
            {
                string sSQL = "Select "
                    + " TransPmt.* "
                    + " , PType." + clsPOSDBConstants.PayType_Fld_PayTypeDescription
                    + " FROM "
                    + clsPOSDBConstants.POSTransPayment_OnHold_tbl + " as TransPmt "
                    + " , " + clsPOSDBConstants.PayType_tbl + " as PType"
                    + " WHERE  ISNULL(TransPmt.[Status],'') not in ('Expired','In Progress') AND "
                    + " TransPmt. " + clsPOSDBConstants.POSTransPayment_Fld_TransTypeCode + " = PType." + clsPOSDBConstants.PayType_Fld_PayTypeID
                    + " and TransPmt." + clsPOSDBConstants.POSTransPayment_Fld_TransID + " = " + TransId;

                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 TransId, IDbConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion
    }
}
