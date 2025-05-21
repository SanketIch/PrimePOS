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
using NLog;

namespace POS_Core.DataAccess
{
	

	// Provides data access methods for DeptCode
 
	public class PayOutSvr: IDisposable  
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
				//ErrorLogging.ErrorHandler.throwException(ex,"","");
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
				logger.Fatal(ex, "Persist(DataSet updates) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
			}

		}
		#endregion

		#region Get Methods

		// Looks up a DeptCode based on its primary-key:System.Int32 DeptCode

		public PayOutData Populate(System.String DeptCode, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
									+ clsPOSDBConstants.PayOut_Fld_PayOutID 
									+ " , " + clsPOSDBConstants.PayOut_Fld_Description
									+ " , " +  clsPOSDBConstants.PayOut_Fld_Amount
									+ " , " +  clsPOSDBConstants.PayOut_Fld_TransDate
									+ " , " +  clsPOSDBConstants.PayOut_Fld_StationID
									+ " , po." +  clsPOSDBConstants.PayOut_Fld_UserID
									+ " , " +  clsPOSDBConstants.PayOut_Fld_DrawNo
                                    + " , " + clsPOSDBConstants.PayOut_Fld_PayoutCatType //Shitaljit
                                    +","+ clsPOSDBConstants.PayOut_Fld_PayoutCatID//Shitaljit
								+ " FROM " 
									+ clsPOSDBConstants.PayOut_tbl
                                + " po inner join PayOutCategory cat on po.PayoutCatID=cat.ID WHERE " 
									+ clsPOSDBConstants.PayOut_Fld_PayOutID + " ='" + DeptCode + "'";

				PayOutData ds = new PayOutData();
				ds.PayOut.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
											,  sSQL
											, PKParameters(DeptCode)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.String DeptCode, IDbConnection conn) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public PayOutData Populate(System.String PayOutID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(PayOutID, conn));
			}
		}

		public PayOutData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = String.Concat("Select " 
										+ clsPOSDBConstants.PayOut_Fld_PayOutID 
										+ " , " + clsPOSDBConstants.PayOut_Fld_Description
										+ " , " +  clsPOSDBConstants.PayOut_Fld_Amount
										+ " , ps.stationname as StationID "   
										+ " , po." + clsPOSDBConstants.PayOut_Fld_UserID
										+ " , " +  clsPOSDBConstants.PayOut_Fld_TransDate
										+ " , " +  clsPOSDBConstants.PayOut_Fld_DrawNo
                                        + " , " + clsPOSDBConstants.PayOut_Fld_PayoutCatID //Shitaljit
                                        +" ,cat.PayoutCatType"
                                        //+","+ clsPOSDBConstants.PayOut_Fld_PayoutCatType //Shitaljit
									+ " FROM "
                                        + clsPOSDBConstants.PayOut_tbl + " po inner join PayOutCategory cat on(po.PayoutCatID=cat.ID), util_POSSet ps  where ps.stationid=po.stationid "
									,sWhereClause);

				PayOutData ds = new PayOutData();
				ds.PayOut.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"",""); 
				return null;
			} 
		}

		// Fills a PayOutData with all DeptCode

		public PayOutData PopulateList(string whereClause) 
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

			PayOutTable addedTable = (PayOutTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (PayOutRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.PayOut_tbl,insParam);
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

					catch (Exception ex) 
					{
						logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}
		// Update all rows in a DeptCodes DataSet, within a given database transaction.
		public void Update(DataSet ds, IDbTransaction tx) 
		{	
			PayOutTable modifiedTable = (PayOutTable)ds.Tables[0].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (PayOutRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.PayOut_tbl,updParam);

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
		
			PayOutTable table = (PayOutTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (PayOutRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.PayOut_tbl,delParam);
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
		private IDbDataParameter[] PKParameters(System.String PayOutID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@PayOutID";
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = PayOutID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(PayOutRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@PayOutID";
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.PayOutId;
			sqlParams[0].SourceColumn = clsPOSDBConstants.PayOut_Fld_PayOutID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(PayOutRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOut_Fld_Description, System.Data.DbType.String);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOut_Fld_Amount, System.Data.DbType.Currency);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOut_Fld_StationID, System.Data.DbType.String);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
			sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOut_Fld_TransDate, System.Data.DbType.DateTime);
			sqlParams[5] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOut_Fld_DrawNo, System.Data.DbType.Int32);
            //sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayOutCat_Fld_PayoutType, System.Data.DbType.String); //Shitaljit
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayOut_Fld_PayoutCatID, System.Data.DbType.Int32);//Shitaljit

			sqlParams[0].SourceColumn = clsPOSDBConstants.PayOut_Fld_Description;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.PayOut_Fld_Amount;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.PayOut_Fld_StationID;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.fld_UserID;
			sqlParams[4].SourceColumn  = clsPOSDBConstants.PayOut_Fld_TransDate;
			sqlParams[5].SourceColumn  = clsPOSDBConstants.PayOut_Fld_DrawNo;
            //sqlParams[6].SourceColumn = clsPOSDBConstants.PayOutCat_Fld_PayoutType; //Shitaljit
            sqlParams[6].SourceColumn = clsPOSDBConstants.PayOut_Fld_PayoutCatID;//Shitaljit
			
			if (row.Description!= System.String.Empty )
				sqlParams[0].Value = row.Description;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.Amount.ToString()!= null)
				sqlParams[1].Value = row.Amount;
			else
				sqlParams[1].Value = DBNull.Value ;

			sqlParams[2].Value=Configuration.StationID;
			sqlParams[3].Value=Configuration.UserName;

			if (row.TransDate.ToString()!= null)
				sqlParams[4].Value = row.TransDate;
			else
				sqlParams[4].Value = DateTime.Now;

			sqlParams[5].Value=Configuration.DrawNo;
            sqlParams[6].Value = row.PayoutCatID;//Shitaljit
            //sqlParams[7].Value =row.
			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(PayOutRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayOut_Fld_PayOutID, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOut_Fld_Description, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOut_Fld_Amount, System.Data.DbType.Currency);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
			sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_StationID, System.Data.DbType.String);
			sqlParams[5] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOut_Fld_TransDate, System.Data.DbType.DateTime);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayOut_Fld_PayoutCatID, System.Data.DbType.Int32);//Shitaljit
            sqlParams[0].SourceColumn = clsPOSDBConstants.PayOut_Fld_PayOutID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.PayOut_Fld_Description;
			sqlParams[2].SourceColumn = clsPOSDBConstants.PayOut_Fld_Amount;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.fld_UserID;
			sqlParams[4].SourceColumn  = clsPOSDBConstants.fld_StationID;
			sqlParams[5].SourceColumn = clsPOSDBConstants.PayOut_Fld_TransDate;
            sqlParams[6].SourceColumn = clsPOSDBConstants.PayOut_Fld_PayoutCatID;


            sqlParams[0].Value = row.PayOutId;

            if (row.Description!= System.String.Empty  )
				sqlParams[1].Value = row.Description;
			else
				sqlParams[1].Value = String.Empty;

			if (row.Amount.ToString()!= null)
				sqlParams[2].Value = row.Amount;
			else
				sqlParams[2].Value = 0;

				sqlParams[3].Value = Configuration.UserName;
				sqlParams[4].Value = Configuration.StationID;

			if (row.TransDate.ToString()!= null)
				sqlParams[5].Value = row.TransDate;
			else
				sqlParams[5].Value = DateTime.Now;

            sqlParams[6].Value = row.PayoutCatID;

			return(sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
