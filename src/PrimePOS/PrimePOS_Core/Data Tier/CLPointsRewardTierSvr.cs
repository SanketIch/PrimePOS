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
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
//using POS.Resources;
using NLog;

namespace POS_Core.DataAccess
{


	// Provides data access methods for ID
 
	public class CLPointsRewardTierSvr: IDisposable  
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
				logger.Fatal(ex, "Persist(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
			}

		}
		#endregion

		#region Get Methods

		// Looks up a ID based on its primary-key:System.Int32 ID

		public CLPointsRewardTierData Populate(System.Int32 ID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select * " 
								+ " FROM " 
									+ clsPOSDBConstants.CLPointsRewardTier_tbl 
								+ " WHERE " + clsPOSDBConstants.CLPointsRewardTier_Fld_ID + " =" + ID + "";


				CLPointsRewardTierData ds = new CLPointsRewardTierData();
				ds.CLPointsRewardTier.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
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
				logger.Fatal(ex, "Populate(System.Int32 ID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public CLPointsRewardTierData Populate(System.Int32 ID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(ID, conn));
			}
		}

		public CLPointsRewardTierData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = String.Concat("Select * " 
									+ " FROM " 
										+ clsPOSDBConstants.CLPointsRewardTier_tbl 
									,sWhereClause);

				CLPointsRewardTierData ds = new CLPointsRewardTierData();
				ds.CLPointsRewardTier.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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

		public CLPointsRewardTierData PopulateList(string whereClause) 
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

			CLPointsRewardTierTable addedTable = (CLPointsRewardTierTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (CLPointsRewardTierRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.CLPointsRewardTier_tbl,insParam);
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

		// Update all rows in a IDs DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx) 
		{	
			CLPointsRewardTierTable modifiedTable = (CLPointsRewardTierTable)ds.Tables[0].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (CLPointsRewardTierRow row in modifiedTable.Rows) 
				{
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.CLPointsRewardTier_tbl, updParam);

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
                    catch (SqlException ex)
                    {
                        throw (ex);
                    }
                    catch (Exception ex)
                    {
						logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex, "", "");
                    }
				} 
				modifiedTable.AcceptChanges();
			}
		}

		// Delete all rows within a IDs DataSet, within a given database transaction.
		public void Delete(DataSet ds, IDbTransaction tx) 
		{
		
			CLPointsRewardTierTable table = (CLPointsRewardTierTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (CLPointsRewardTierRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.CLPointsRewardTier_tbl,delParam);
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
						logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				} 
			}
		}

        public bool DeleteRow(int CurrentID)
        {
            try
            {
                IDbDataParameter[] delParam= PKParameters(CurrentID);
			    DataHelper.ExecuteNonQuery(BuildDeleteSQL(clsPOSDBConstants.CLPointsRewardTier_tbl,delParam),delParam );
                
                return true;
            }
            catch (Exception ex)
            {
				logger.Fatal(ex, "DeleteRow(int CurrentID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return false;
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
			sInsertSQL = sInsertSQL + " , UserId ";
			sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

			for(int i = 1;i<delParam.Length;i++)
			{
				sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName ;
			}
			sInsertSQL = 	sInsertSQL + " , '" + Configuration.UserName + "'";
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

			sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'" ;

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

        private IDbDataParameter[] PKParameters(CLPointsRewardTierRow row)
        {
            return PKParameters(row.ID);
        }

		private IDbDataParameter[] PKParameters(int Id) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@ID";
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = Id;
			sqlParams[0].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_ID;

			return(sqlParams);
		}

        private IDbDataParameter[] InsertParameters(CLPointsRewardTierRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsRewardTier_Fld_Description, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsRewardTier_Fld_Discount, System.Data.DbType.Decimal);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsRewardTier_Fld_Points, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod, System.Data.DbType.Int32);
            
            sqlParams[0].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_Description;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_Discount;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_Points;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod;
            
            if (row.Description != System.String.Empty)
                sqlParams[0].Value = row.Description;
            else
                sqlParams[0].Value = DBNull.Value;

            sqlParams[1].Value = row.Discount;

            sqlParams[2].Value = row.Points;
            sqlParams[3].Value = row.RewardPeriod;

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(CLPointsRewardTierRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(5);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsRewardTier_Fld_ID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsRewardTier_Fld_Description, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsRewardTier_Fld_Discount, System.Data.DbType.Decimal);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsRewardTier_Fld_Points, System.Data.DbType.Int32);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_Description;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_Discount;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_Points;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CLPointsRewardTier_Fld_RewardPeriod;

            sqlParams[0].Value = row.ID;

            if (row.Description != System.String.Empty)
                sqlParams[1].Value = row.Description;
            else
                sqlParams[1].Value = DBNull.Value;

            sqlParams[2].Value = row.Discount;

            sqlParams[3].Value = row.Points;
            sqlParams[4].Value = row.RewardPeriod;

            return (sqlParams);
        }
		#endregion

    
		public void Dispose() {}   
	}
}
