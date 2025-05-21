// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------
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
	public class WarningMessagesDetailSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public  void Persist(WarningMessagesDetailData updates, IDbTransaction tx, System.Int32 WarningMessageID) 
		{
			try 
		
			{
				this.Delete(updates, tx);
				this.Insert(updates,tx,WarningMessageID);
                this.Update(updates, tx, WarningMessageID);

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
				logger.Fatal(ex, "Persist(WarningMessagesDetailData updates, IDbTransaction tx, System.Int32 WarningMessageID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//POS_Core.ErrorLogging.ErrorHandler.throwException(ex,"","");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
			}
		}

    
		// Inserts, updates or deletes rows in a DataSet.

/*		public  void Persist(DataSet updates) 
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
				ErrorHandler.throwException(ex,"","");
			}

		}
*/
		#endregion

		#region Get Methods

		// Looks up a DeptCode based on its primary-key:System.Int32 DeptCode
/*
		public WarningMessagesDetailData Populate(System.Int32 DeptCode, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
									+ clsPOSDBConstants.WarningMessagesDetail_Fld_DeptID 
									+ " , " + clsPOSDBConstants.WarningMessagesDetail_Fld_DeptCode 
									+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_DeptName 
									+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_Discount 
									+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_IsTaxable 
									+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_SaleStartDate 
									+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_SaleEndDate 
									+ " , dept." +   clsPOSDBConstants.WarningMessagesDetail_Fld_TaxID + " as " + clsPOSDBConstants.WarningMessagesDetail_Fld_TaxID
									+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_SalePrice
									+ " , taxcodes." + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " as " + clsPOSDBConstants.TaxCodes_Fld_TaxCode
									+ " , " +  clsPOSDBConstants.TaxCodes_Fld_Description
									+ " , dept." +  clsPOSDBConstants.WarningMessagesDetail_Fld_UserID + " as " + clsPOSDBConstants.WarningMessagesDetail_Fld_UserID
								+ " FROM " 
									+ clsPOSDBConstants.WarningMessagesDetail_tbl + " As Dept "
									+ " , " + clsPOSDBConstants.TaxCodes_tbl + " As TaxCodes "
								+ " WHERE " 
									+ " Dept." + clsPOSDBConstants.WarningMessagesDetail_Fld_TaxID + " *= TaxCodes." + clsPOSDBConstants.TaxCodes_Fld_TaxID
									+ " AND " + clsPOSDBConstants.WarningMessagesDetail_Fld_DeptCode + " ='" + DeptCode + "'";


				WarningMessagesDetailData ds = new WarningMessagesDetailData();
				ds.WarningMessagesDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
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
				ErrorHandler.throwException(ex,"","");
				return null;
			}
		}
*/
		public WarningMessagesDetailData Populate(System.Int32 WarningMessageID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(WarningMessageID, conn));
			}
		}

		public WarningMessagesDetailData Populate(System.Int32 WarningMessageID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
					+ clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID
					+ " , " + clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID 
					+ " , msg." +  clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID 
					+ " , item." +  clsPOSDBConstants.Item_Fld_Description + " as " + clsPOSDBConstants.Item_Fld_Description 
					+ " , msg." +  clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType + " as " + clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType
					+ " FROM " 
					+ clsPOSDBConstants.WarningMessagesDetail_tbl + " As msg"
					+ " , " + clsPOSDBConstants.Item_tbl + " As item"
					+ " WHERE " 
					+ " msg." + clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID + " = item." + clsPOSDBConstants.Item_Fld_ItemID
					+ " AND msg.RefObjectType='I' And " + clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID + " = " + WarningMessageID;

                sSQL += " Union All Select "
                    + clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID
                    + " , " + clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID
                    + " , msg." + clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID
                    + " , dept." + clsPOSDBConstants.Department_Fld_DeptName + " as " + clsPOSDBConstants.Item_Fld_Description
                    + " , msg." + clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType + " as " + clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType
                    + " FROM "
                    + clsPOSDBConstants.WarningMessagesDetail_tbl + " As msg"
                    + " , " + clsPOSDBConstants.Department_tbl + " As dept "
                    + " WHERE "
                    + " msg." + clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID + " = Cast(dept." + clsPOSDBConstants.Department_Fld_DeptID + " as varchar) "
                    + " AND msg.RefObjectType='D' And " + clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID + " = " + WarningMessageID;


				WarningMessagesDetailData ds = new WarningMessagesDetailData();
				ds.WarningMessagesDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
					,  sSQL
					, PKParameters(WarningMessageID)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int32 WarningMessageID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			}
		}
		// Fills a WarningMessagesDetailData with all DeptCode
/*
		public WarningMessagesDetailData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = String.Concat("Select " 
										+ clsPOSDBConstants.WarningMessagesDetail_Fld_DeptID 
										+ " , " + clsPOSDBConstants.WarningMessagesDetail_Fld_DeptCode 
										+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_DeptName 
										+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_Discount 
										+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_IsTaxable 
										+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_SaleStartDate 
										+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_SaleEndDate 
										+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_TaxID 
										+ " , " +  clsPOSDBConstants.WarningMessagesDetail_Fld_SalePrice
									+ " FROM " 
										+ clsPOSDBConstants.WarningMessagesDetail_tbl 
									,sWhereClause);

				WarningMessagesDetailData ds = new WarningMessagesDetailData();
				ds.WarningMessagesDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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

		public WarningMessagesDetailData Populate(System.String DeptCode) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(DeptCode, conn));
			}
		}

		// Fills a WarningMessagesDetailData with all DeptCode

	// Fills a WarningMessagesDetailData with all DeptCode

		public WarningMessagesDetailData PopulateList(string whereClause) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(PopulateList(whereClause,conn));
			}			
		}
*/
		#endregion //Get Method

		#region Insert, Update, and Delete Methods

		public void Insert(WarningMessagesDetailData ds, IDbTransaction tx, System.Int32 WarningMessageID) 
		{

			WarningMessagesDetailTable addedTable = (WarningMessagesDetailTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (WarningMessagesDetailRow row in addedTable.Rows) 
				{
					try 
					{
						row.WarningMessagesID=WarningMessageID;
						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.WarningMessagesDetail_tbl,insParam);
						
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
						logger.Fatal(ex, "Insert(WarningMessagesDetailData ds, IDbTransaction tx, System.Int32 WarningMessageID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
					}
				}
				addedTable.AcceptChanges();
			}		
		}

		// Update all rows in a DeptCodes DataSet, within a given database transaction.

        public void Update(DataSet ds, IDbTransaction tx, System.Int32 WarningMessageID) 
		{	
			WarningMessagesDetailTable modifiedTable = (WarningMessagesDetailTable)ds.Tables[0].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (WarningMessagesDetailRow row in modifiedTable.Rows) 
				{
					try 
					{
                        row.WarningMessagesID = WarningMessageID;
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.WarningMessagesDetail_tbl,updParam);

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
						logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx, System.Int32 WarningMessageID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}


		// Delete all rows within a DeptCodes DataSet, within a given database transaction.
		public void Delete(DataSet ds, IDbTransaction tx) 
		{
		
			WarningMessagesDetailTable table = (WarningMessagesDetailTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (WarningMessagesDetailRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.WarningMessagesDetail_tbl,delParam);
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
						//ErrorHandler.throwException(ex,"","");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
					}
				} 
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
				sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName ;
			}
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

			sUpdateSQL = 	sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
			return sUpdateSQL;
		}


        // Delete all rows within a ItemGroupPrice DataSet, within a given database transaction.
        private string BuildDeleteSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sDeleteSQL = "DELETE FROM " + tableName + " WHERE ";
            // build where clause
            for (int i = 0; i < delParam.Length; i++)
            {
                sDeleteSQL = sDeleteSQL + delParam[i].SourceColumn + " = " + delParam[i].ParameterName;
            }
            return sDeleteSQL;
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
		private IDbDataParameter[] PKParameters(System.Int32 WarningMessagesDetailID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@WarningMessagesDetailID";
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = WarningMessagesDetailID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(WarningMessagesDetailRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@WarningMessagesDetailID";
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.WarningMessagesDetailID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(WarningMessagesDetailRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            //sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID, System.Data.DbType.Int32);
			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID, System.Data.DbType.Int32);
            
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType, System.Data.DbType.String);

			sqlParams[0].SourceColumn  = clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType;

			sqlParams[0].Value = row.WarningMessagesID;
			sqlParams[1].Value = row.RefObjectID;
			sqlParams[2].Value = row.RefObjectType;

			return(sqlParams);
		}

        private IDbDataParameter[] UpdateParameters(WarningMessagesDetailRow row) 
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID, System.Data.DbType.Int32);

            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType, System.Data.DbType.String);

            sqlParams[0].SourceColumn = clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType;

            sqlParams[0].Value = row.WarningMessagesDetailID;
            sqlParams[1].Value = row.WarningMessagesID;
            sqlParams[2].Value = row.RefObjectID;
            sqlParams[3].Value = row.RefObjectType;

            return (sqlParams);
        }

#endregion

    
		public void Dispose() {}   
	}
}

