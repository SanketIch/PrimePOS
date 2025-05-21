
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
    public class CouponSvr:IDisposable
    {
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public  void Persist(DataSet updates, SqlTransaction tx) 
		{
			try 
		
			{
				this.Delete(updates, tx);				
				this.Update(updates, tx);
                this.Insert(updates, tx);

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
				//ErrorHandler.throwException(ex,"","");
			}
		}

    
		// Inserts, updates or deletes rows in a DataSet.

		public  void Persist(DataSet updates) 
		{

			SqlTransaction tx;
			SqlConnection conn = new SqlConnection( DBConfig.ConnectionString);

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

        #region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, SqlTransaction tx) 
		{

			CouponTable addedTable = (CouponTable)ds.Tables[clsPOSDBConstants.Coupon_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (CouponRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.Coupon_tbl,insParam);
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
			CouponTable modifiedTable = (CouponTable)ds.Tables[clsPOSDBConstants.Coupon_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (CouponRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.Coupon_tbl,updParam);

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
		
			CouponTable table = (CouponTable)ds.Tables[clsPOSDBConstants.Coupon_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (CouponRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.Coupon_tbl,delParam);
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
		private IDbDataParameter[] PKParameters(System.String CouponType) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@CouponType";
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = CouponType;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(CouponRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@CouponType";
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.CouponID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.Coupon_Fld_CouponID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(CouponRow row) 
		{
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8); //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY changed from 7 to 8

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.Coupon_Fld_CouponCode, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_UserID, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.Coupon_Fld_DiscountPerc, System.Data.DbType.Decimal);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_CreatedDate, System.Data.DbType.DateTime);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_StartDate, System.Data.DbType.DateTime);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_EndDate, System.Data.DbType.DateTime);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_IsCouponInPercent, System.Data.DbType.Boolean);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_CouponDesc, System.Data.DbType.String);   //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.Coupon_Fld_CouponCode;
			sqlParams[1].SourceColumn = clsPOSDBConstants.Coupon_Fld_UserID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Coupon_Fld_DiscountPerc;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Coupon_Fld_CreatedDate;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Coupon_Fld_StartDate;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Coupon_Fld_EndDate;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Coupon_Fld_IsCouponInPercent;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Coupon_Fld_CouponDesc;    //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY Added

			if (row.CouponCode != System.String.Empty )
                sqlParams[0].Value = row.CouponCode;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.UserID!= System.String.Empty )
				sqlParams[1].Value = row.UserID;
			else
				sqlParams[1].Value = DBNull.Value ;

            if (row.DiscountPerc != 0)
                sqlParams[2].Value = row.DiscountPerc;
            else
                sqlParams[2].Value = 0;

            if (row.CreatedDate != System.DateTime.MinValue)
                sqlParams[3].Value = row.CreatedDate;
            else
                sqlParams[3].Value = System.DateTime.MinValue;
           
            if (row.StartDate != System.DateTime.MinValue)
                sqlParams[4].Value = row.StartDate;
            else
                sqlParams[4].Value = System.DateTime.MinValue;

             if (row.EndDate != System.DateTime.MinValue)
                 sqlParams[5].Value = row.EndDate;
            else
                sqlParams[5].Value = System.DateTime.MinValue;
             
            sqlParams[6].Value = row.IsCouponInPercent;

            //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY Added
            if (row.CouponDesc != System.String.Empty)
                sqlParams[7].Value = row.CouponDesc;
            else
                sqlParams[7].Value = DBNull.Value;

			return(sqlParams);
		}

        private IDbDataParameter[] UpdateParameters(CouponRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(9); //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY changed from 8 to 9 

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_CouponID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_CouponCode, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_UserID, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_DiscountPerc, System.Data.DbType.Decimal);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_CreatedDate, System.Data.DbType.DateTime);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_StartDate, System.Data.DbType.DateTime);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_EndDate, System.Data.DbType.DateTime);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_IsCouponInPercent, System.Data.DbType.Boolean);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Coupon_Fld_CouponDesc, System.Data.DbType.String);   //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.Coupon_Fld_CouponID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Coupon_Fld_CouponCode;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Coupon_Fld_UserID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Coupon_Fld_DiscountPerc;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Coupon_Fld_CreatedDate;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Coupon_Fld_StartDate;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Coupon_Fld_EndDate;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Coupon_Fld_IsCouponInPercent;
            sqlParams[8].SourceColumn = clsPOSDBConstants.Coupon_Fld_CouponDesc;    //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY Added

            if (row.CouponID != 0)
                sqlParams[0].Value = row.CouponID;
            else
                sqlParams[0].Value = 0;
            if (row.CouponCode != System.String.Empty)
                sqlParams[1].Value = row.CouponCode;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.UserID != System.String.Empty)
                sqlParams[2].Value = row.UserID;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.DiscountPerc != 0)
                sqlParams[3].Value = row.DiscountPerc;
            else
                sqlParams[3].Value = 0;

            if (row.CreatedDate != System.DateTime.MinValue)
                sqlParams[4].Value = row.CreatedDate;
            else
                sqlParams[4].Value = System.DateTime.MinValue;

            if (row.StartDate != System.DateTime.MinValue)
                sqlParams[5].Value = row.StartDate;
            else
                sqlParams[5].Value = System.DateTime.MinValue;

            if (row.EndDate != System.DateTime.MinValue)
                sqlParams[6].Value = row.EndDate;
            else
                sqlParams[6].Value = System.DateTime.MinValue;

            sqlParams[7].Value = row.IsCouponInPercent;

            //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY Added
            if (row.CouponDesc != System.String.Empty)
                sqlParams[8].Value = row.CouponDesc;
            else
                sqlParams[8].Value = DBNull.Value;

            return (sqlParams);
        }
		#endregion

        #region Get Methods
        public CouponData Populate(System.String couponCode)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(DBConfig.ConnectionString))
            {

                string WhereClause = " WHERE " + clsPOSDBConstants.Coupon_Fld_CouponCode + " ='" + couponCode + "'";
                return (PopulateList(WhereClause, conn));
            }
        }

        // Looks up a CouponType based on its primary-key:System.Int32 CouponType
        public CouponData Populate(System.Int64 ID, SqlConnection conn) //PRIMEPOS-2034 JY changed from Int32 to Int64
        {
            try
            {
                string sWhereClause = " WHERE " + clsPOSDBConstants.Coupon_Fld_CouponID + " = " + ID;
                string sSQL = String.Concat("Select " + clsPOSDBConstants.Coupon_Fld_CouponID + " , " + clsPOSDBConstants.Coupon_Fld_CouponCode + " , " + clsPOSDBConstants.Coupon_Fld_CouponDesc + //Sprint-23 - PRIMEPOS-2279 18-Mar-2016 JY Added CouponDesc
                        " , " + clsPOSDBConstants.Coupon_Fld_UserID + " , " + clsPOSDBConstants.Coupon_Fld_DiscountPerc +
                        " , " + clsPOSDBConstants.Coupon_Fld_EndDate + " , " + clsPOSDBConstants.Coupon_Fld_StartDate + " , " + clsPOSDBConstants.Coupon_Fld_CreatedDate +
                         " , " + clsPOSDBConstants.Coupon_Fld_IsCouponInPercent +
                       " FROM " + clsPOSDBConstants.Coupon_tbl, sWhereClause);

                CouponData ds = new CouponData();
                ds.Coupon.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int64 ID, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public CouponData Populate(System.Int64 ID) //PRIMEPOS-2034 30-May-2018 JY changed from Int32 to Int64
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                return (Populate(ID, conn));
            }
        }
       
        public CouponData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select " + clsPOSDBConstants.Coupon_Fld_CouponID + " , " + clsPOSDBConstants.Coupon_Fld_CouponCode + " , " + clsPOSDBConstants.Coupon_Fld_CouponDesc + //Sprint-23 - PRIMEPOS-2279 17-Mar-2016 JY Added CouponDesc
                     " , " + clsPOSDBConstants.Coupon_Fld_UserID + " , " + clsPOSDBConstants.Coupon_Fld_DiscountPerc +
                     " , " + clsPOSDBConstants.Coupon_Fld_EndDate + " , " + clsPOSDBConstants.Coupon_Fld_StartDate + " , " + clsPOSDBConstants.Coupon_Fld_CreatedDate +
                     " , " + clsPOSDBConstants.Coupon_Fld_IsCouponInPercent + 
                    " FROM " + clsPOSDBConstants.Coupon_tbl , sWhereClause);

                CouponData ds = new CouponData();
                ds.Coupon.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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

        public CouponData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = DBConfig.ConnectionString;
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
