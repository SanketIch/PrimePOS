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
 
	public class WarningMessagesSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public  void Persist(DataSet updates, IDbTransaction tx,ref System.Int32 WarningMessagesID) 
		{
			try 
		
			{
				this.Insert(updates, tx,ref WarningMessagesID);
                this.Update(updates, tx, ref WarningMessagesID);
				if (WarningMessagesID>0)
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
				logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx,ref System.Int32 WarningMessagesID) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");	//PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
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

		public WarningMessagesData Populate(System.Int32 WarningMessageID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
									+ clsPOSDBConstants.WarningMessages_Fld_WarningMessageID
									+ " , " +  clsPOSDBConstants.WarningMessages_Fld_WarningMessage
									+ " , " +  clsPOSDBConstants.WarningMessages_Fld_IsActive
								+ " FROM " 
									+ clsPOSDBConstants.WarningMessages_tbl
								+ " WHERE " 
							    + clsPOSDBConstants.WarningMessages_Fld_WarningMessageID + " ='" + WarningMessageID + "'";


				WarningMessagesData ds = new WarningMessagesData();
				ds.WarningMessages.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
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
				logger.Fatal(ex, "Populate(System.Int32 WarningMessageID, IDbConnection conn) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");	//PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			}
		}

		public WarningMessagesData Populate(System.Int32 WarningMessageID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(WarningMessageID, conn));
			}
		}

        public WarningMessagesData GetByItemID(System.String sItemID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (GetByItemID(sItemID, conn));
            }
        }

        public WarningMessagesData GetByItemID(string sItemID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + " master."+clsPOSDBConstants.WarningMessages_Fld_WarningMessageID
                    + " , " + clsPOSDBConstants.WarningMessages_Fld_WarningMessage
                    + " , " + clsPOSDBConstants.WarningMessages_Fld_IsActive
                    + " FROM "
                    + clsPOSDBConstants.WarningMessages_tbl + " as master "
                    + " Inner Join " + clsPOSDBConstants.WarningMessagesDetail_tbl + " as detail "
                    + " On (master.WarningMessageID=detail.WarningMessageID) "
                    + " Where (detail.RefObjectID='" + sItemID + "' And detail.RefObjectType='I' ) "
                    + " Or (detail.RefObjectType='D' And detail.RefObjectID=(Select DepartmentID from Item where ItemID='" + sItemID + "') )";

                WarningMessagesData ds = new WarningMessagesData();
                ds.WarningMessages.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
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
				logger.Fatal(ex, "GetByItemID(string sItemID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");	//PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
            }
        }

		public WarningMessagesData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{
                string sSQL = "Select "
                    + clsPOSDBConstants.WarningMessages_Fld_WarningMessageID
                    + " , " + clsPOSDBConstants.WarningMessages_Fld_WarningMessage
                    + " , " + clsPOSDBConstants.WarningMessages_Fld_IsActive
                    + " FROM "
                    + clsPOSDBConstants.WarningMessages_tbl; 

				if (sWhereClause.Trim()!="")
					sSQL=String.Concat(sSQL,sWhereClause);

				WarningMessagesData ds = new  WarningMessagesData();
				ds.WarningMessages.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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
				//ErrorHandler.throwException(ex,"","");	//PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				return null;
			} 
		}

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx, ref System.Int32 WarningMessagesID) 
		{

			WarningMessagesTable addedTable = (WarningMessagesTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (WarningMessagesRow row in addedTable.Rows) 
				{
					try 
					{
						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.WarningMessages_tbl,insParam);
						
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
						WarningMessagesID=Convert.ToInt32(DataHelper.ExecuteScalar(tx,CommandType.Text,"select @@identity"));
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
						logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx, ref System.Int32 WarningMessagesID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");	//PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
					}
				}
				addedTable.AcceptChanges();
			}
		}

		// Update all rows in a DeptCodes DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx,ref System.Int32 WarningMessagesID) 
		{	
			WarningMessagesTable modifiedTable = (WarningMessagesTable)ds.Tables[0].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                WarningMessagesRow row = modifiedTable[0];

                try
                {
                    updParam = UpdateParameters(row);
                    sSQL = BuildUpdateSQL(clsPOSDBConstants.WarningMessages_tbl, updParam);

                    DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
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
					logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx,ref System.Int32 WarningMessagesID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
					//ErrorHandler.throwException(ex, "", "");	//PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
				}

				modifiedTable.AcceptChanges();
            }

            if (((WarningMessagesTable)ds.Tables[0])[0].WarningMessageID > 0)
            {
                WarningMessagesID = ((WarningMessagesTable)ds.Tables[0])[0].WarningMessageID;
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
		
        private IDbDataParameter[] PKParameters(System.Int32 WarningMessageID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@WarningMessageID";
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = WarningMessageID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(WarningMessagesRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@WarningMessageID";
			sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.WarningMessageID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.WarningMessages_Fld_WarningMessageID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(WarningMessagesRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(2);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.WarningMessages_Fld_WarningMessage, System.Data.DbType.String);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.WarningMessages_Fld_IsActive, System.Data.DbType.Boolean);

			sqlParams[0].SourceColumn  = clsPOSDBConstants.WarningMessages_Fld_WarningMessage;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.WarningMessages_Fld_IsActive;

			if (row.WarningMessage != System.String.Empty )
				sqlParams[0].Value = row.WarningMessage;
			else
				sqlParams[0].Value = DBNull.Value ;

			sqlParams[1].Value = row.IsActive;
			
			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(WarningMessagesRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.WarningMessages_Fld_WarningMessageID, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.WarningMessages_Fld_WarningMessage, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.WarningMessages_Fld_IsActive, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.WarningMessages_Fld_WarningMessageID;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.WarningMessages_Fld_WarningMessage;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.WarningMessages_Fld_IsActive;

            sqlParams[0].Value = row.WarningMessageID;

			sqlParams[1].Value = row.WarningMessage;

			sqlParams[2].Value=row.IsActive;

			return(sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
