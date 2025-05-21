
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
	
	// Provides data access methods for SubDepartment
 
	public class SubDepartmentSvr: IDisposable  
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

			IDbTransaction tx;
			IDbConnection conn = DataFactory.CreateConnection(); 
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			
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

		// Looks up a SubDepartment based on its primary-key:System.String VendorItemID

		public SubDepartmentData Populate(System.Int32 departmentID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select * FROM " 
						+ clsPOSDBConstants.SubDepartment_tbl + " As SubDepartment "
						+ " WHERE " 
						+ " SubDepartment." + clsPOSDBConstants.SubDepartment_Fld_DepartmentID + " = " + departmentID.ToString();
	
				SubDepartmentData ds = new SubDepartmentData();
				ds.SubDepartment.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , PKParameters(departmentID)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int32 departmentID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public SubDepartmentData Populate(System.Int32 departmentID) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			return(Populate(departmentID, conn));
		}

		// Fills a SubDepartmentData with all SubDepartment

		public SubDepartmentData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL1 = "Select * FROM " 
						+ clsPOSDBConstants.SubDepartment_tbl + " As SubDepartment "
					    + " WHERE " 
						+ " 1=1 " ;
				
				sWhereClause = sWhereClause.ToUpper().Replace("WHERE" , "AND");
				string sSQL = String.Concat(sSQL1,sWhereClause);

				SubDepartmentData ds = new SubDepartmentData();
				ds.SubDepartment.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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

		// Fills a SubDepartmentData with all SubDepartment

		public SubDepartmentData PopulateList(string whereClause) 
		{
			IDbConnection conn = DataFactory.CreateConnection();
			conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
			
			return(PopulateList(whereClause,conn));
		}

        // Fills the DataSet with All the DaptName and DeptCode
        public DataSet PopulateListWithIdName(string condition)
        {
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                DataSet ds = new DataSet();
                string sSQL = "";
                sSQL = "SELECT  "
                                    + clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID
                                    + " , " + clsPOSDBConstants.SubDepartment_Fld_Description
                                    + " , " +clsPOSDBConstants.SubDepartment_Fld_EXCLUDEFROMCLCouponPay
                                    + " FROM "
                                    + clsPOSDBConstants.SubDepartment_tbl;
                if (!String.IsNullOrEmpty(condition))
                {
                    sSQL += condition;
                }

                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (Exception ex)
            {
				logger.Fatal(ex, "PopulateListWithIdName(string condition)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion //Get Method

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx) 
		{

			SubDepartmentTable addedTable = (SubDepartmentTable)ds.Tables[clsPOSDBConstants.SubDepartment_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (SubDepartmentRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.SubDepartment_tbl,insParam);
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

		// Update all rows in a SubDepartment DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx) 
		{	
			SubDepartmentTable modifiedTable = (SubDepartmentTable)ds.Tables[clsPOSDBConstants.SubDepartment_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (SubDepartmentRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.SubDepartment_tbl,updParam);

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

		// Delete all rows within a SubDepartment DataSet, within a given database transaction.
		public void Delete(DataSet ds, IDbTransaction tx) 
		{
		
			SubDepartmentTable table = (SubDepartmentTable)ds.Tables[clsPOSDBConstants.SubDepartment_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (SubDepartmentRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.SubDepartment_tbl,delParam);
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
			sInsertSQL = sInsertSQL + " ";
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

		private IDbDataParameter[] PKParameters(System.Int32 SubDepartmentID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@" + clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID;
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = SubDepartmentID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(SubDepartmentRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@" + clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID;
			sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.SubDepartmentID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(SubDepartmentRow row) 
		{
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);      //(15);
			
			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.SubDepartment_Fld_DepartmentID, System.Data.SqlDbType.Int);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.SubDepartment_Fld_Description, System.Data.SqlDbType.VarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale, System.Data.SqlDbType.Bit);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar, System.Data.SqlDbType.Int);  //Sprint-18 - 2041 27-Oct-2014 JY  Added
			
			sqlParams[0].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_DepartmentID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_Description;
            sqlParams[2].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale;
            sqlParams[3].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar;    //Sprint-18 - 2041 27-Oct-2014 JY  Added

			sqlParams[0].Value = row.DepartmentID;

			sqlParams[1].Value = row.Description;
            sqlParams[2].Value = row.IncludeOnSale;
            sqlParams[3].Value = row.PointsPerDollar;   //Sprint-18 - 2041 27-Oct-2014 JY  Added
			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(SubDepartmentRow row) 
		{
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(5); //(15);
			
			sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID, System.Data.SqlDbType.Int);
			sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.SubDepartment_Fld_DepartmentID, System.Data.SqlDbType.Int);
			sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.SubDepartment_Fld_Description, System.Data.SqlDbType.VarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale, System.Data.SqlDbType.Bit);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar, System.Data.SqlDbType.Int);  //Sprint-18 - 2041 27-Oct-2014 JY  Added
			
			sqlParams[0].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_SubDepartmentID;
			sqlParams[1].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_DepartmentID;
			sqlParams[2].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_Description;
            sqlParams[3].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_IncludeOnSale;
            sqlParams[4].SourceColumn = clsPOSDBConstants.SubDepartment_Fld_PointsPerDollar;    //Sprint-18 - 2041 27-Oct-2014 JY  Added
			
			sqlParams[0].Value = row.SubDepartmentID;

			sqlParams[1].Value = row.DepartmentID;

            sqlParams[2].Value = row.Description; 
            sqlParams[3].Value = row.IncludeOnSale;
            sqlParams[4].Value = row.PointsPerDollar; //Sprint-18 - 2041 27-Oct-2014 JY  Added
			
			return(sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
