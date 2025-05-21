using System;
using System.Data;
using System.Data.SqlClient;
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
	
	// Provides data access methods for TypeName
 
	public class InvTransTypeSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public  void Persist(DataSet updates, SqlTransaction tx) 
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
				logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//POS_Core.ErrorLogging.ErrorHandler.throwException(ex,"","");
			}
		}

    
		// Inserts, updates or deletes rows in a DataSet.

		public  void Persist(DataSet updates) 
		{

			SqlTransaction tx;
			SqlConnection conn = new SqlConnection( POS_Core.Resources.Configuration.ConnectionString);

			conn.Open();
			tx = conn.BeginTransaction();
			try 
			{
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
		#endregion

		#region Get Methods

		// Looks up a TypeName based on its primary-key:System.Int32 TypeName
		public InvTransTypeData Populate(System.Int32 ID, SqlConnection conn) 
		{
			try 
			{
				InvTransTypeData ds = new InvTransTypeData();
				ds.InvTransType.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select " + clsPOSDBConstants.InvTransType_Fld_ID + " , " + clsPOSDBConstants.InvTransType_Fld_TypeName + " , " +  clsPOSDBConstants.InvTransType_Fld_UserID + " , " +  clsPOSDBConstants.InvTransType_Fld_TransType + " FROM " + clsPOSDBConstants.InvTransType_tbl + " WHERE " + clsPOSDBConstants.InvTransType_Fld_ID + " =" + ID  ).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int32 ID, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public InvTransTypeData Populate(System.Int32 ID) 
		{
			using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(ID, conn));
			}
		}

		public InvTransTypeData Populate(System.String TypeName, SqlConnection conn) 
		{
			try 
			{
				InvTransTypeData ds = new InvTransTypeData();
				
				ds.InvTransType.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select " + clsPOSDBConstants.InvTransType_Fld_ID + " , " + clsPOSDBConstants.InvTransType_Fld_TypeName + " , " +  clsPOSDBConstants.InvTransType_Fld_UserID + " , " +  clsPOSDBConstants.InvTransType_Fld_TransType + " FROM " + clsPOSDBConstants.InvTransType_tbl + " WHERE " + clsPOSDBConstants.InvTransType_Fld_TypeName + " ='" + TypeName + "'" ).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.String TypeName, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public InvTransTypeData Populate(System.String TypeName) 
		{
			using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(TypeName, conn));
			}
		}


		// Fills a InvTransTypeData with all TypeName
		public InvTransTypeData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = String.Concat("Select " + clsPOSDBConstants.InvTransType_Fld_ID + " , " + clsPOSDBConstants.InvTransType_Fld_TypeName + " , " +  clsPOSDBConstants.InvTransType_Fld_UserID + " , " +  clsPOSDBConstants.InvTransType_Fld_TransType + " FROM " + clsPOSDBConstants.InvTransType_tbl,sWhereClause);


				InvTransTypeData ds = new InvTransTypeData();
				ds.InvTransType.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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


		// Fills a InvTransTypeData with all TypeName
		public InvTransTypeData PopulateList(string whereClause) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection()) 
			{
				conn.ConnectionString=POS_Core.Resources.Configuration.ConnectionString;
				return(PopulateList(whereClause,conn));
			}			
		}

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, SqlTransaction tx) 
		{

			InvTransTypeTable addedTable = (InvTransTypeTable)ds.Tables[clsPOSDBConstants.InvTransType_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (InvTransTypeRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.InvTransType_tbl,insParam);
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
						logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}


		// Update all rows in a InvTransType DataSet, within a given database transaction.
		public void Update(DataSet ds, SqlTransaction tx) 
		{	
			InvTransTypeTable modifiedTable = (InvTransTypeTable)ds.Tables[clsPOSDBConstants.InvTransType_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (InvTransTypeRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.InvTransType_tbl,updParam);

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
						logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}


		// Delete all rows within a InvTransType DataSet, within a given database transaction.
		public void Delete(DataSet ds, SqlTransaction tx) 
		{
		
			InvTransTypeTable table = (InvTransTypeTable)ds.Tables[clsPOSDBConstants.InvTransType_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (InvTransTypeRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.InvTransType_tbl,delParam);
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
						logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
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
		private IDbDataParameter[] PKParameters(System.String TypeName) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@TypeName";
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = TypeName;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(InvTransTypeRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@TypeName";
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.TypeName;
			sqlParams[0].SourceColumn = clsPOSDBConstants.InvTransType_Fld_TypeName;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(InvTransTypeRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvTransType_Fld_TypeName, System.Data.DbType.String);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvTransType_Fld_UserID, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvTransType_Fld_TransType, System.Data.DbType.Int16);

			sqlParams[0].SourceColumn = clsPOSDBConstants.InvTransType_Fld_TypeName;
			sqlParams[1].SourceColumn = clsPOSDBConstants.InvTransType_Fld_UserID;
			sqlParams[2].SourceColumn = clsPOSDBConstants.InvTransType_Fld_TransType;

			if (row.TypeName != System.String.Empty )
				sqlParams[0].Value = row.TypeName;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.UserID!= System.String.Empty )
				sqlParams[1].Value = row.UserID;
			else
				sqlParams[1].Value = DBNull.Value ;

			if (row.TransType != System.Int16.MinValue )
				sqlParams[2].Value = row.TransType;
			else
				sqlParams[2].Value = 0;

			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(InvTransTypeRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

			sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvTransType_Fld_ID, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvTransType_Fld_TypeName, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvTransType_Fld_UserID, System.Data.DbType.String);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvTransType_Fld_TransType, System.Data.DbType.Int16);

			sqlParams[0].SourceColumn = clsPOSDBConstants.InvTransType_Fld_ID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.InvTransType_Fld_TypeName;
			sqlParams[2].SourceColumn = clsPOSDBConstants.InvTransType_Fld_UserID;
			sqlParams[3].SourceColumn = clsPOSDBConstants.InvTransType_Fld_TransType;

			if (row.ID != System.Int32.MinValue )
				sqlParams[0].Value = row.ID;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.TypeName != System.String.Empty )
				sqlParams[1].Value = row.TypeName;
			else
				sqlParams[1].Value = DBNull.Value ;

			if (row.UserID!= System.String.Empty )
				sqlParams[2].Value = row.UserID;
			else
				sqlParams[2].Value = DBNull.Value ;

			if (row.TransType != System.Int16.MinValue )
				sqlParams[3].Value = row.TransType;
			else
				sqlParams[3].Value = 0;

			return(sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
