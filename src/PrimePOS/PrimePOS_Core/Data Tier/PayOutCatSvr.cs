
using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using System.Data.Sql;
using System.Data.SqlClient;
using NLog;

namespace POS_Core.DataAccess
{
    

    public class PayOutCatSvr:IDisposable
    {
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist
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
				logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
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
				logger.Fatal(ex, "Persist(DataSet updates) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
			}

		}
		#endregion

		
		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, SqlTransaction tx) 
		{

			PayOutCatTable addedTable = (PayOutCatTable)ds.Tables[clsPOSDBConstants.PayOutCat_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (PayOutCatRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.PayOutCat_tbl,insParam);
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
						logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}


		// Update all rows in a InvTransType DataSet, within a given database transaction.
		public void Update(DataSet ds, SqlTransaction tx) 
		{	
			PayOutCatTable modifiedTable = (PayOutCatTable)ds.Tables[clsPOSDBConstants.PayOutCat_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (PayOutCatRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.PayOutCat_tbl,updParam);

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
						logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}


		// Delete all rows within a InvTransType DataSet, within a given database transaction.
		public void Delete(DataSet ds, SqlTransaction tx) 
		{
		
			PayOutCatTable table = (PayOutCatTable)ds.Tables[clsPOSDBConstants.PayOutCat_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (PayOutCatRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.PayOutCat_tbl,delParam);
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
						logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
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

			//sUpdateSQL = sUpdateSQL + " , UserId  = '" + Configuration.UserName + "'" ;

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
		private IDbDataParameter[] PKParameters(System.String PayoutCatType) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@PayoutCatType";
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = PayoutCatType;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(PayOutCatRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@PayoutCatType";
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.PayoutCatType;
			sqlParams[0].SourceColumn = clsPOSDBConstants.PayOutCat_Fld_PayoutType;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(PayOutCatRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOutCat_Fld_PayoutType, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayOutCat_Fld_UserId, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOutCat_Fld_DefaultDescription, System.Data.DbType.String);

			sqlParams[0].SourceColumn = clsPOSDBConstants.PayOutCat_Fld_PayoutType;
			sqlParams[1].SourceColumn = clsPOSDBConstants.PayOutCat_Fld_UserId;
            sqlParams[2].SourceColumn = clsPOSDBConstants.PayOutCat_Fld_DefaultDescription;

			if (row.PayoutCatType != System.String.Empty )
				sqlParams[0].Value = row.PayoutCatType;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.UserId!= System.String.Empty )
				sqlParams[1].Value = row.UserId;
			else
				sqlParams[1].Value = DBNull.Value ;
            if (row.DefaultDescription != System.String.Empty)
                sqlParams[2].Value = row.DefaultDescription;
            else
                sqlParams[2].Value = DBNull.Value;


			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(PayOutCatRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

			sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.payoutCat_Fld_Id, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayOutCat_Fld_PayoutType, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.PayOutCat_Fld_UserId, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayOutCat_Fld_DefaultDescription, System.Data.DbType.String);

			sqlParams[0].SourceColumn = clsPOSDBConstants.payoutCat_Fld_Id;
			sqlParams[1].SourceColumn = clsPOSDBConstants.PayOutCat_Fld_PayoutType;
			sqlParams[2].SourceColumn = clsPOSDBConstants.PayOutCat_Fld_UserId;

            sqlParams[3].SourceColumn = clsPOSDBConstants.PayOutCat_Fld_DefaultDescription;
			if (row.ID != System.Int32.MinValue )
				sqlParams[0].Value = row.ID;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.PayoutCatType != System.String.Empty )
				sqlParams[1].Value = row.PayoutCatType;
			else
				sqlParams[1].Value = DBNull.Value ;

			if (row.UserId!= System.String.Empty )
				sqlParams[2].Value = row.UserId;
			else
				sqlParams[2].Value = DBNull.Value ;

            if (row.DefaultDescription != System.String.Empty)
                sqlParams[3].Value = row.DefaultDescription;
            else
                sqlParams[3].Value = DBNull.Value;

			return(sqlParams);
		}
		#endregion


        #region Get Methods
        public PayOutCatData Populate(System.String Str)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(Str, conn));
            }
        }

        public PayOutCatData Populate(System.String str,IDbConnection conn)
        {
            PayOutCatData oPayOutCatData = new PayOutCatData();
            
            oPayOutCatData.PayOutCat.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, str).Tables[0]);
            return oPayOutCatData;
        }

        // Looks up a PayoutCatType based on its primary-key:System.Int32 PayoutCatType
        public PayOutCatData Populate(System.Int32 ID, SqlConnection conn)
        {
            try
            {
                PayOutCatData ds = new PayOutCatData();
                ds.PayOutCat.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select " + clsPOSDBConstants.payoutCat_Fld_Id + " , " + clsPOSDBConstants.PayOutCat_Fld_PayoutType + " , " + clsPOSDBConstants.PayOutCat_Fld_UserId + " , " + clsPOSDBConstants.PayOutCat_Fld_DefaultDescription + " FROM " + clsPOSDBConstants.PayOutCat_tbl + " WHERE " + clsPOSDBConstants.payoutCat_Fld_Id + " =" + ID).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int32 ID, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public PayOutCatData Populate(System.Int32 ID)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(ID, conn));
            }
        }

        public PayOutCatData Populate(System.String PayoutCatType, SqlConnection conn)
        {
            try
            {
                PayOutCatData ds = new PayOutCatData();

                ds.PayOutCat.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select " + clsPOSDBConstants.payoutCat_Fld_Id + " , " + clsPOSDBConstants.PayOutCat_Fld_PayoutType + " , " + clsPOSDBConstants.PayOutCat_Fld_UserId + " , " + clsPOSDBConstants.InvTransType_Fld_TransType + " , " + clsPOSDBConstants.PayOutCat_Fld_DefaultDescription + " FROM " + clsPOSDBConstants.PayOutCat_tbl + " WHERE " + clsPOSDBConstants.PayOutCat_Fld_PayoutType + " ='" + PayoutCatType + "'").Tables[0]);
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
				logger.Fatal(ex, "Populate(System.String PayoutCatType, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        //public PayOutCatData Populate(System.String PayoutCatType)
        //{
        //    using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
        //    {
        //        return (Populate(PayoutCatType, conn));
        //    }
        //}


        // Fills a PayOutCatData with all PayoutCatType
        public PayOutCatData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select " + clsPOSDBConstants.payoutCat_Fld_Id + " , " + clsPOSDBConstants.PayOutCat_Fld_PayoutType + " , " + clsPOSDBConstants.PayOutCat_Fld_UserId + " , " + clsPOSDBConstants.PayOutCat_Fld_DefaultDescription + " FROM " + clsPOSDBConstants.PayOutCat_tbl, " "+sWhereClause);


                PayOutCatData ds = new PayOutCatData();
                ds.PayOutCat.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
				logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }


        // Fills a PayOutCatData with all PayoutCatType
        public PayOutCatData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (PopulateList(whereClause, conn));
            }
        }

        #endregion //Get Method

       

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}
