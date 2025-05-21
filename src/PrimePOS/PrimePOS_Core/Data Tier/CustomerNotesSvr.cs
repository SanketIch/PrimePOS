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
 
	public class CustomerNotesSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.
		public  void Persist(DataSet updates, IDbTransaction tx) 
		{
			try 
		
			{
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
				logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
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

		// Looks up a DeptCode based on its primary-key:System.Int32 DeptCode
        public CustomerNotesData Populate(System.Int32 iCustomerID, bool activeOnly, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                                    + clsPOSDBConstants.CustomerNotes_Fld_ID
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_Notes
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_CustomerID
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_UserID
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_IsActive
                                + " FROM "
                                    + clsPOSDBConstants.CustomerNotes_tbl
                                + " WHERE "
                                    + clsPOSDBConstants.CustomerNotes_Fld_CustomerID + " =" + iCustomerID;


                if (activeOnly == true)
                {
                    sSQL += " And IsActive=1 ";
                }

                sSQL += " Order By "
                + clsPOSDBConstants.CustomerNotes_Fld_ID + " Desc";


                CustomerNotesData ds = new CustomerNotesData();
                ds.CustomerNotes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                            , sSQL
                                            , PKParameters("ID")).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int32 iCustomerID, bool activeOnly, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

		public CustomerNotesData Populate(System.Int32 iCustomerID,bool activeOnly) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
                return (Populate(iCustomerID,activeOnly, conn));
			}
		}

        public CustomerNotesData PopulateByID(System.Int32 iID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateByID(iID, conn));
            }
        }

        public CustomerNotesData PopulateByID(System.Int32 iID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                                    + clsPOSDBConstants.CustomerNotes_Fld_ID
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_Notes
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_CustomerID
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_UserID
                                    + " , " + clsPOSDBConstants.CustomerNotes_Fld_IsActive
                                + " FROM "
                                    + clsPOSDBConstants.CustomerNotes_tbl
                                + " WHERE "
                                    + clsPOSDBConstants.CustomerNotes_Fld_ID + " =" + iID;


                CustomerNotesData ds = new CustomerNotesData();
                ds.CustomerNotes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                            , sSQL
                                            , PKParameters("ID")).Tables[0]);
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
				logger.Fatal(ex, "PopulateByID(System.Int32 iID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

		#endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx) 
		{

			CustomerNotesTable addedTable = (CustomerNotesTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (CustomerNotesRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.CustomerNotes_tbl,insParam);
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

        public void Update(DataSet ds, IDbTransaction tx)
        {
            CustomerNotesTable modifiedTable = (CustomerNotesTable)ds.Tables[0].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (CustomerNotesRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.CustomerNotes_tbl, updParam);

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
						logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

		public void DeleteRow(System.Int32 CurrentID) 
		{

			string sSQL;
			try 
			{
				sSQL = " delete from CustomerNotes where id="+CurrentID;
				DataHelper.ExecuteNonQuery(DBConfig.ConnectionString,CommandType.Text, sSQL);
			}
			catch (Exception ex) 
			{
				logger.Fatal(ex, "DeleteRow(System.Int32 CurrentID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
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

			for(int i =1;i<delParam.Length;i++)
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
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

            for (int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }

            //sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'" ;

            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
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
	
        private IDbDataParameter[] PKParameters(System.String CustomerNotesID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@ID";
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = CustomerNotesID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(CustomerNotesRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@ID";
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.ID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_ID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(CustomerNotesRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(5);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CustomerNotes_Fld_Notes, System.Data.DbType.String);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CustomerNotes_Fld_CustomerID, System.Data.DbType.Int32);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn, System.Data.DbType.DateTime);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CustomerNotes_Fld_IsActive, System.Data.DbType.Boolean);

			sqlParams[0].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_Notes;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.CustomerNotes_Fld_CustomerID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_UserID;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_IsActive;
			
			if (row.Notes != System.String.Empty )
				sqlParams[0].Value = row.Notes;
			else
				sqlParams[0].Value = DBNull.Value ;

			sqlParams[1].Value = row.CustomerID;
			
			sqlParams[2].Value=Configuration.UserName;

            sqlParams[3].Value = DateTime.Now;

            sqlParams[4].Value = row.IsActive;
			return(sqlParams);
		}

        private IDbDataParameter[] UpdateParameters(CustomerNotesRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CustomerNotes_Fld_ID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CustomerNotes_Fld_Notes, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CustomerNotes_Fld_CustomerID, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn, System.Data.DbType.DateTime);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CustomerNotes_Fld_IsActive, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_Notes;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_CustomerID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_UserID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_LastUpdatedOn;
            sqlParams[5].SourceColumn = clsPOSDBConstants.CustomerNotes_Fld_IsActive;

            sqlParams[0].Value = row.ID;

            if (row.Notes != System.String.Empty)
                sqlParams[1].Value = row.Notes;
            else
                sqlParams[1].Value = DBNull.Value;

            sqlParams[2].Value = row.CustomerID;

            sqlParams[3].Value = Configuration.UserName;

            sqlParams[4].Value = DateTime.Now;

            sqlParams[5].Value = row.IsActive;

            return (sqlParams);
        }
		
#endregion

    
		public void Dispose() {}   
	}
}
