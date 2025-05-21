// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
//using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
//using POS.Resources;
using POS_Core.Resources;
using Resources;
using NLog;

namespace POS_Core.DataAccess
{
	

	// Provides data access methods for DeptCode
 
	public class CLCardsSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public  void Persist(DataSet updates, IDbTransaction tx) 
		{
			try 
		
			{
				this.Delete(updates, tx);
				this.Insert(updates, tx);
				this.Update(updates, tx);

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
				logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//POS_Core.ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}

    
		// Inserts, updates or deletes rows in a DataSet.

		public  void Persist(DataSet updates) 
		{

			IDbTransaction tx=null;
			try 
			{
				IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
				tx = conn.BeginTransaction();
				this.Persist(updates, tx);
				tx.Commit();
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
				tx.Rollback();
				logger.Fatal(ex, "Persist(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
			}

		}

        public bool DeleteRow(string CurrentID)
        {
            string sSQL;
            try
            {
                DataTable dtItem = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "Select * from Item where CLCardsID = '" + CurrentID + "'").Tables[0];
                if (dtItem.Rows.Count == 0) 
                {
                    sSQL = " delete from CL_Cards where ID= '" + CurrentID + "'";
                    DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
				logger.Fatal(ex, "DeleteRow(string CurrentID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
		#endregion

		#region Get Methods

		// Looks up a DeptCode based on its primary-key:System.Int32 DeptCode

		public CLCardsData Populate(System.Int64 ID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select * " 
								+ " FROM " 
									+ clsPOSDBConstants.CLCards_tbl
                                + " WHERE IsActive = 1 AND " + clsPOSDBConstants.CLCards_Fld_ID + " ='" + ID + "'";


				CLCardsData ds = new CLCardsData();
				ds.CLCards.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
											,  sSQL
											, PKParameters(ID)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int64 ID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public CLCardsData Populate(System.Int64 ID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(ID, conn));
			}
		}

        public CLCardsData GetByCLCardID(System.Int64 iCLCardID,IDbTransaction tx)
        {
            //07-Oct-2016 JY Added logic to deactivate duplicate active card
            CLCardsData oCLCardsData = new CLCardsData();
            oCLCardsData = PopulateList(" where IsActive = 1 AND CardID=" + iCLCardID.ToString(), tx);
            if (oCLCardsData.CLCards.Rows.Count > 1)
            {
                DeActivateDuplicateCards(iCLCardID, tx);
                return PopulateList(" where IsActive = 1 AND CardID=" + iCLCardID.ToString(), tx);
            }
            else
            {
                return oCLCardsData;
            }
        }

        //07-Oct-2016 JY Added logic to deactivate duplicate active card
        public bool DeActivateDuplicateCards(System.Int64 CLCardID, IDbTransaction tx)
        {
            bool returnVal = false;
            try
            {
                string sSQL = "WITH CTE (RowNum, Id, CardId, IsActive) " +
                               " AS " +
                               " (select row_number() over(partition by CardId order by Id desc) RowNum, Id, CardID, IsActive from CL_Cards WHERE IsActive = 1 AND CardID = " + CLCardID + ") " +
                               " update CTE set IsActive = 0 where RowNum > 1 ";
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
                returnVal = true;
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
				logger.Fatal(ex, "DeActivateDuplicateCards(System.Int64 CLCardID, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
            }
            return returnVal;
        }

        #region 07-Oct-2016 JY Added to check CL Card exists irrespective of active/inactive
        public CLCardsData CheckCLCardExists(System.Int64 iCLCardID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return PopulateList(" where CardID=" + iCLCardID.ToString(), conn);
            }
        }
        #endregion

        public CLCardsData GetByCLCardID(System.Int64 iCLCardID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return PopulateList(" where IsActive = 1 AND CardID=" + iCLCardID.ToString(), conn);
            }
        }

        public CLCardsData GetByCustomerID(System.Int64 iCustomerID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where IsActive= 1 AND CustomerID=" + iCustomerID.ToString() + " ORDER BY LastUpdatedOn DESC", conn));
            }
        }

        #region 30-Nov-2017 JY added to get customer loyalty info w.r.t. customer
        public DataTable GetCustomerLoyaltyGrid(System.Int64 iCustomerID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string strSQL = " SELECT a.CardID, CASE WHEN ISNULL(a.IsPrepetual,0) = 0 THEN 'No' ELSE 'Yes' END AS Prepetual, a.RegisterDate, (ISNULL(b.CardPoints,0) + ISNULL(c.CouponPoints,0)) TotalPoints FROM CL_Cards a " +
                                " INNER JOIN (SELECT bb.CardID, SUM(bb.CurrentPoints) AS CardPoints FROM CL_Cards bb WHERE bb.IsActive = 1 GROUP BY bb.CardID)  b ON a.CardID = b.CardID" +
                                " LEFT JOIN (SELECT cc.CardID, SUM(cc.Points) CouponPoints FROM CL_Coupons cc WHERE cc.IsCouponUsed = 0 And cc.isActive = 1 And DateAdd(d, cc.ExpiryDays, cc.CreatedOn) > GetDate() GROUP BY cc.CardId) c ON a.CardID = c.CardID" +
                                " WHERE a.IsActive = 1 AND a.CustomerID = " + iCustomerID + " ORDER BY a.LastUpdatedOn DESC";
                
                DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, (IDbDataParameter[])null);
                return dt;
            }
        }
        #endregion

        public long GetNextCardNumber()
        {
            long returnValue = 0;
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string strSQL = " Select Max(CardID) from " + clsPOSDBConstants.CLCards_tbl;
                object objValue = DataHelper.ExecuteScalar(conn, CommandType.Text, strSQL);
                if (objValue == null)
                {
                    returnValue = POS_Core.Resources.Configuration.CLoyaltyInfo.CardRangeFrom;
                }
                else
                {
                    returnValue = Configuration.convertNullToInt64(objValue);
                    returnValue++;
                }
            }
            return returnValue;
        }

		public CLCardsData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = String.Concat("Select * "
									+ " FROM " 
										+ clsPOSDBConstants.CLCards_tbl 
									,sWhereClause);

				CLCardsData ds = new CLCardsData();
				ds.CLCards.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"",""); 
				return null;
			} 
		}

        public CLCardsData PopulateList(string sWhereClause, IDbTransaction tx)
        {
            try
            {
                string sSQL = String.Concat("Select * "
                                    + " FROM "
                                        + clsPOSDBConstants.CLCards_tbl
                                    , sWhereClause);

                CLCardsData ds = new CLCardsData();
                ds.CLCards.MergeTable(DataHelper.ExecuteDataset(tx, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

		public CLCardsData PopulateList(string whereClause) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(PopulateList(whereClause,conn));
			}			
		}

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx) 
		{

			CLCardsTable addedTable = (CLCardsTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (CLCardsRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.CLCards_tbl,insParam);
						for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
						}
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
						

					} 
					catch(POSExceptions ex) 
					{
						throw(ex);
					}

					catch(OtherExceptions ex) 
					{
						throw(ex);
					}
					catch (SqlException ex) 
					{
						throw(ex);
					}

					catch (Exception ex) 
					{
						logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}

		// Update all rows in a DeptCodes DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx) 
		{	
			CLCardsTable modifiedTable = (CLCardsTable)ds.Tables[0].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (CLCardsRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.CLCards_tbl,updParam);

						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
					} 
					catch(POSExceptions ex) 
					{
						throw(ex);
					}

					catch(OtherExceptions ex) 
					{
						throw(ex);
					}
					catch (SqlException ex) 
					{
							throw(ex);
					}


					catch (Exception ex) 
					{
						logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}

		// Delete all rows within a DeptCodes DataSet, within a given database transaction.
		public void Delete(DataSet ds, IDbTransaction tx) 
		{
		
			CLCardsTable table = (CLCardsTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (CLCardsRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.CLCards_tbl,delParam);
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL,delParam );
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
						logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				} 
			}
		}

        public bool DeactivateCard(Int64 CLCardID)
        {
            bool returnVal = false;
            IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            IDbTransaction oTrans = conn.BeginTransaction();
            try
            {
                CLCouponsSvr oCLCouponsSvr = new CLCouponsSvr();
                oCLCouponsSvr.DeleteUnusedCoupons(CLCardID, oTrans);
                string sSQL = "UPDATE CL_Cards SET IsActive= '0' , CurrentPoints = 0 WHERE CardID= " + CLCardID;
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                returnVal = true;
                oTrans.Commit();
            }
            catch (POSExceptions ex)
            {
                oTrans.Rollback();
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
				logger.Fatal(ex, "DeactivateCard(Int64 CLCardID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
            }
            return returnVal;
        }

        public void UpdateLoyaltyCurrentPoints(Int64 CLCardID, Decimal points, IDbTransaction tx)
        {

            try
            {
                string sSQL="update CL_Cards set CurrentPoints=" + points.ToString() + " where CardID="+CLCardID.ToString() ;

                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
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
				logger.Fatal(ex, "UpdateLoyaltyCurrentPoints(Int64 CLCardID, Decimal points, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
            }
        }

		private string BuildDeleteSQL(string tableName, IDbDataParameter[] delParam)
		{
			string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
			// build where clause
			for(int i = 0;i<delParam.Length;i++)
			{
				sDeleteSQL = sDeleteSQL + delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
			}
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
			sInsertSQL = sInsertSQL + " , UserId, LastUpdatedOn ";
			sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

			for(int i = 1;i<delParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName ;
			}
			sInsertSQL = 	sInsertSQL + " , '" + Configuration.UserName + "', GetDate()";
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

			sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "', LastUpdatedOn=GetDate()" ;

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
		
        private IDbDataParameter[] PKParameters(System.Int64 ID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@ID";
			sqlParams[0].DbType = System.Data.DbType.Int64;
			sqlParams[0].Value = ID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(CLCardsRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.ID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.CLCards_Fld_ID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(CLCardsRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CLCards_Fld_IsPrepetual, System.Data.DbType.Boolean);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CLCards_Fld_ExpiryDays, System.Data.DbType.Int32);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CLCards_Fld_CLCardID, System.Data.DbType.Int64);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCards_Fld_Description, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCards_Fld_RegisterDate, System.Data.DbType.Date);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCards_Fld_CustomerID, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCards_Fld_IsActive, System.Data.DbType.Boolean);

			sqlParams[0].SourceColumn  = clsPOSDBConstants.CLCards_Fld_IsPrepetual;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.CLCards_Fld_ExpiryDays;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.CLCards_Fld_CLCardID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CLCards_Fld_Description;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CLCards_Fld_RegisterDate;
            sqlParams[5].SourceColumn = clsPOSDBConstants.CLCards_Fld_CustomerID;
            sqlParams[6].SourceColumn = clsPOSDBConstants.CLCards_Fld_IsActive;

			sqlParams[0].Value = row.IsPrepetual;

		    sqlParams[1].Value = row.ExpiryDays;

			if (row.CLCardID != 0 )
				sqlParams[2].Value = row.CLCardID;
			else
				sqlParams[2].Value = 0 ;

            sqlParams[3].Value = row.Description;
            sqlParams[4].Value = System.DateTime.Now;
            sqlParams[5].Value = row.CustomerID;
            sqlParams[6].Value =true;
			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(CLCardsRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CLCards_Fld_ID, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CLCards_Fld_IsPrepetual, System.Data.DbType.Boolean);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CLCards_Fld_ExpiryDays, System.Data.DbType.Int32);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CLCards_Fld_CLCardID, System.Data.DbType.Int64);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCards_Fld_Description, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCards_Fld_CustomerID, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCards_Fld_IsActive, System.Data.DbType.Boolean);

			sqlParams[0].SourceColumn = clsPOSDBConstants.CLCards_Fld_ID;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.CLCards_Fld_IsPrepetual;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.CLCards_Fld_ExpiryDays;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.CLCards_Fld_CLCardID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CLCards_Fld_Description;
            sqlParams[5].SourceColumn = clsPOSDBConstants.CLCards_Fld_CustomerID;
            sqlParams[6].SourceColumn = clsPOSDBConstants.CLCards_Fld_IsActive;

			if (row.ID != 0 )
				sqlParams[0].Value = row.ID;
			else
				sqlParams[0].Value = 0 ;

			sqlParams[1].Value = row.IsPrepetual;

			sqlParams[2].Value = row.ExpiryDays;

			if (row.CLCardID != 0)
				sqlParams[3].Value = row.CLCardID;
			else
				sqlParams[3].Value = 0 ;

            sqlParams[4].Value=row.Description;

            sqlParams[5].Value = row.CustomerID;
            sqlParams[6].Value = row.IsActive;
            return(sqlParams);
		}
		#endregion

        #region Sprint-18 - 2090 21-Oct-2014 JY Update IsConverted flag in cl_cards table
        public bool UpdateIsConvertedStatus(Int64 clCardId)
        {
            bool returnVal = false;
            IDbConnection conn = DataFactory.CreateConnection(DBConfig.ConnectionString);
            IDbTransaction oTrans = conn.BeginTransaction();
            try
            {
                string strSQL = "UPDATE CL_Cards SET IsConverted = 1 WHERE CardID = " + clCardId;
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, strSQL);
                returnVal = true;
                oTrans.Commit();
            }
            catch (POSExceptions ex)
            {
                oTrans.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                oTrans.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
				logger.Fatal(ex, "UpdateIsConvertedStatus(Int64 clCardId)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
            }
            return returnVal;
        }
        #endregion

        #region Sprint-18 - 2041 28-Oct-2014 JY Added logic to get the pointsperdollar as per the CL Point policy
        public decimal GetPointsPerDollar(string strCLPointPolicy, string strItemId, int DeptId, int SubDeptId)
        {
            decimal returnValue = 0;
            string strSQL = string.Empty;
            object objValue= null;

            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                if (strCLPointPolicy.Trim().ToUpper() == "I")
                    strSQL = "SELECT ISNULL(PointsPerDollar,0) FROM ITEM WHERE ItemID = '" + strItemId + "'";
                else if (strCLPointPolicy.Trim().ToUpper() == "D")
                    strSQL = "SELECT ISNULL(PointsPerDollar,0) FROM Department WHERE deptid = " + DeptId;
                else if (strCLPointPolicy.Trim().ToUpper() == "S")
                    strSQL = "SELECT ISNULL(PointsPerDollar,0) FROM SubDepartment WHERE SubDepartmentID = " + SubDeptId;
                else
                    returnValue = Configuration.CLoyaltyInfo.RedeemValue;
                
                if (strSQL != string.Empty) 
                    objValue = DataHelper.ExecuteScalar(conn, CommandType.Text, strSQL);
                if (objValue == null || Convert.ToDecimal(objValue) <= 0)
                {
                    returnValue = Configuration.CLoyaltyInfo.RedeemValue;
                }
                else
                {
                    returnValue = Convert.ToDecimal(objValue);
                }
            }
            return returnValue;
        }
        #endregion

        public void Dispose() {}   
	}
}
