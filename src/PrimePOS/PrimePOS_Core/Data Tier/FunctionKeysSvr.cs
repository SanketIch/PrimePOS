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
using NLog;
using POS_Core.Resources;
//using POS.Resources;
namespace POS_Core.DataAccess
{
	

    // Provides data access methods for FunKey

    public class FunctionKeysSvr: IDisposable  
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
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
			}
			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
			}
			catch(Exception ex) 
			{
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                //ErrorLogging.ErrorHandler.throwException(ex,"","");
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
                logger.Fatal(ex, "Persist(DataSet updates)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Persist(DataSet updates)");
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
				//ErrorHandler.throwException(ex,"","");
			}

		}
		#endregion

		#region Get Methods

		// Looks up a FunKey based on its primary-key:System.Int32 FunKey

/*		public FunctionKeysData Populate(System.Int32 TaxId, SqlConnection conn) 
		{
			try 
			{
				FunctionKeysData ds = new FunctionKeysData();
				ds.FunctionKeys.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select " + clsPOSDBConstants.FunctionKeys_Fld_FunKey  + " , " + clsPOSDBConstants.FunctionKeys_Fld_Operation + " FROM " + clsPOSDBConstants.FunctionKeys_tbl + " WHERE " + clsPOSDBConstants.FunctionKeys_Fld_FunKey + " =" + TaxId  , PKParameters(TaxId.ToString())).Tables[0]);
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

		public FunctionKeysData Populate(System.Int32 TaxId) 
		{
			using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(TaxId, conn));
			}
		} */

		public FunctionKeysData Populate(System.String FunKey, SqlConnection conn) 
		{
			try 
			{
				FunctionKeysData ds = new FunctionKeysData();
				ds.FunctionKeys.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, 
                                            "Select " + clsPOSDBConstants.FunctionKeys_Fld_FunKey  + " , "
                                            + clsPOSDBConstants.FunctionKeys_Fld_Operation + " , "  
                                            +clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor + " , "
                                            + clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor + " , "
                                            + clsPOSDBConstants.FunctionKeys_Fld_MainPosition + " , " 
                                            +clsPOSDBConstants.FunctionKeys_Fld_SubPosition +
                                            " FROM " + clsPOSDBConstants.FunctionKeys_tbl + " WHERE " + 
                                            clsPOSDBConstants.FunctionKeys_Fld_FunKey + " ='" 
                                            + FunKey + "'" , PKParameters(FunKey)).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.String FunKey, SqlConnection conn)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "Populate(System.String FunKey, SqlConnection conn)");
                throw (ex);
			}

			catch(Exception ex) 
			{
                logger.Fatal(ex, "Populate(System.String FunKey, SqlConnection conn)");
                //ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public FunctionKeysData Populate(System.String FunKey) 
		{
			using(SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(FunKey, conn));
			}
		}

		// Fills a FunctionKeysData with all FunKey

		public FunctionKeysData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = String.Concat("Select " + clsPOSDBConstants.FunctionKeys_Fld_KeyId  +
                                            " , " + clsPOSDBConstants.FunctionKeys_Fld_FunKey  +
                                            " , " + clsPOSDBConstants.FunctionKeys_Fld_Operation +
                                            " , " + clsPOSDBConstants.FunctionKeys_Fld_FunctionType +
                                            " , " + clsPOSDBConstants.FunctionKeys_Fld_Parent + 
                                            " , " + clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor +
                                            " , " + clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor +
                                            " , " + clsPOSDBConstants.FunctionKeys_Fld_MainPosition +
                                            " , " + clsPOSDBConstants.FunctionKeys_Fld_SubPosition +
                                            " , " + clsPOSDBConstants.Item_Fld_Description +
                                            " FROM " + clsPOSDBConstants.FunctionKeys_tbl + " LEFT OUTER JOIN " +
                                             clsPOSDBConstants.Item_tbl + " ON " + 
                                             clsPOSDBConstants.FunctionKeys_tbl +"."+clsPOSDBConstants.FunctionKeys_Fld_Operation +"= "+
                                             clsPOSDBConstants.Item_tbl+"."+clsPOSDBConstants.Item_Fld_ItemID
                                             ,sWhereClause);

				FunctionKeysData ds = new FunctionKeysData();
				ds.FunctionKeys.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
				return ds;
			} 
			catch(POSExceptions ex) 
			{
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
			}

			catch(OtherExceptions ex) 
			{
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
			}

			catch (Exception ex) 
			{
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                //ErrorHandler.throwException(ex,"",""); 
				return null;
			} 
		}

		// Fills a FunctionKeysData with all FunKey

		public FunctionKeysData PopulateList(string whereClause) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection()) 
			{
				conn.ConnectionString=POS_Core.Resources.Configuration.ConnectionString;
				return(PopulateList(whereClause,conn));
			}			
		}
        public DataTable PopulateParents()
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (PopulateParents(conn));
            }
        }
        public DataTable PopulateParents(IDbConnection conn)
        {
            string sWhereClause = " WHERE FunctionType = 'P'";
            string sSQL = String.Concat("Select " + clsPOSDBConstants.FunctionKeys_Fld_KeyId + " , " + clsPOSDBConstants.FunctionKeys_Fld_FunKey +
                    " , " + clsPOSDBConstants.FunctionKeys_Fld_Operation + " FROM " + clsPOSDBConstants.FunctionKeys_tbl, sWhereClause);
            try
            {

                DataTable dt = new DataTable();
                dt = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0];

                return dt;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateParents(IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateParents(IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateParents(IDbConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            } 

        }
		#endregion //Get Method

		#region Insert, Update, and Delete Methods

        public bool DeleteRow(int KeyId)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (DeleteRow(KeyId,conn));
            }
        }
        public bool DeleteRow(int KeyId,IDbConnection conn)
        {
                     
            string sSQL = " DELETE FROM " + clsPOSDBConstants.FunctionKeys_tbl + " WHERE  KeyId='" + KeyId + "'";
            try
            {
                DataHelper.ExecuteNonQuery(conn, CommandType.Text, sSQL);
                return true;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "DeleteRow(int KeyId,IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "DeleteRow(int KeyId,IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(int KeyId,IDbConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return false;
            } 

        }

        public Int32 GetNextKeyPosition(string ParameterName, Int32 ParentID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (GetNextKeyPosition(ParameterName, ParentID,conn));
            }
        }

        public Int32 GetNextKeyPosition(string ParameterName,  Int32 ParentID,IDbConnection conn)
        {
            string sSQL = "";
            int Id = 0;
            conn.Open();
            IDbCommand cmd = DataFactory.CreateCommand();
            try
            {
                sSQL = String.Concat("SELECT ",
                         " MAX(" + ParameterName + ")",
                         "  FROM FunctionKeys ");
                if (ParentID > 0)
                {
                    sSQL += " WHERE Parent = " + ParentID;
                }
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sSQL;
                cmd.Connection = conn;

                Id = Convert.ToInt32(((cmd.ExecuteScalar().ToString() == "") ? "0" : cmd.ExecuteScalar().ToString()));

                if (Id == 0)
                    Id = 1;
                else
                    Id = Id + 1;

                conn.Close();
                return Id;

            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetNextKeyPosition(string ParameterName,  Int32 ParentID,IDbConnection conn)");
                conn.Close();
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetNextKeyPosition(string ParameterName,  Int32 ParentID,IDbConnection conn)");
                conn.Close();
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "GetNextKeyPosition(string ParameterName,  Int32 ParentID,IDbConnection conn)");
                conn.Close();
                //ErrorHandler.throwException(ex, "", "");
                return 0;
            }
        }
        public bool UpdateRow(string FuncKey)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (UpdateRow(FuncKey, conn));
            }
        }
        public bool UpdateRow(string FuncKey, IDbConnection conn)
        {

            string sSQL =  " UPDATE   " + clsPOSDBConstants.FunctionKeys_tbl + 
                           " SET "+ clsPOSDBConstants.FunctionKeys_Fld_FunKey+" = '"+FuncKey+"'" + 
                           ", " + clsPOSDBConstants.FunctionKeys_Fld_Operation + " ='' " + 
                           ", " + clsPOSDBConstants.FunctionKeys_Fld_FunctionType + " = ''" + 
                           ", " + clsPOSDBConstants.FunctionKeys_Fld_Parent + " = ''" + 
                           ", " + clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor + " = 'Color [Transparent]'" +
                           ", " + clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor + " = 'Color [White]'" +
                           " WHERE  FunKey='" + FuncKey + "'";
            try
            {
                DataHelper.ExecuteNonQuery(conn, CommandType.Text, sSQL);
                return true;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "UpdateRow(string FuncKey, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "UpdateRow(string FuncKey, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdateRow(string FuncKey, IDbConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return false;
            }

        }
		public void Insert(DataSet ds, SqlTransaction tx) 
		{

			FunctionKeysTable addedTable = (FunctionKeysTable)ds.Tables[clsPOSDBConstants.FunctionKeys_tbl].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (FunctionKeysRow row in addedTable.Rows) 
				{
					try 
					{

						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.FunctionKeys_tbl,insParam);
						for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
						}
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
						

					} 
					catch(POSExceptions ex) 
					{
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                        throw (ex);
					}

					catch(OtherExceptions ex) 
					{
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                        throw (ex);
					}

					catch (Exception ex) 
					{
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                        //ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}		
		}

		// Update all rows in a FunctionKeys DataSet, within a given database transaction.

		public void Update(DataSet ds, SqlTransaction tx) 
		{	
			FunctionKeysTable modifiedTable = (FunctionKeysTable)ds.Tables[clsPOSDBConstants.FunctionKeys_tbl].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (FunctionKeysRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.FunctionKeys_tbl,updParam);

						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
					} 
					catch(POSExceptions ex) 
					{
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        throw (ex);
					}

					catch(OtherExceptions ex) 
					{
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        throw (ex);
					}

					catch (Exception ex) 
					{
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        //ErrorHandler.throwException(ex,"","");
					}
				} 
				modifiedTable.AcceptChanges();
			}
		}

		// Delete all rows within a FunctionKeys DataSet, within a given database transaction.
		public void Delete(DataSet ds, SqlTransaction tx) 
		{
		
			FunctionKeysTable table = (FunctionKeysTable)ds.Tables[clsPOSDBConstants.FunctionKeys_tbl].GetChanges(DataRowState.Deleted);
			string sSQL;
			IDbDataParameter []delParam;

			if (table != null && table.Rows.Count > 0) 
			{
				table.RejectChanges(); //so we can access the rows
				foreach (FunctionKeysRow row in table.Rows) 
				{
					try 
					{
						delParam = PKParameters(row);
			  
						sSQL = BuildDeleteSQL(clsPOSDBConstants.FunctionKeys_tbl,delParam);
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL,delParam );
					} 
					catch(POSExceptions ex) 
					{
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
                        throw (ex);
					}

					catch(OtherExceptions ex) 
					{
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
                        throw (ex);
					}

					catch (Exception ex) 
					{
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
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
		private IDbDataParameter[] PKParameters(System.String FunKey) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@FunKey";
			sqlParams[0].DbType = System.Data.DbType.String;
			sqlParams[0].Value = FunKey;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(FunctionKeysRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@FunKey";
			sqlParams[0].DbType = System.Data.DbType.String;

			sqlParams[0].Value = row.FunKey;
			sqlParams[0].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_FunKey;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(FunctionKeysRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.FunctionKeys_Fld_FunKey, System.Data.DbType.String);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.FunctionKeys_Fld_Operation, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor, System.Data.DbType.String);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FunctionKeys_Fld_FunctionType, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FunctionKeys_Fld_Parent, System.Data.DbType.Int32);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FunctionKeys_Fld_MainPosition, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FunctionKeys_Fld_SubPosition, System.Data.DbType.Int32);

			sqlParams[0].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_FunKey;
			sqlParams[1].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_Operation;
			sqlParams[2].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor;
			sqlParams[3].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor;
            sqlParams[4].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_FunctionType;
            sqlParams[5].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_Parent;
            sqlParams[6].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_MainPosition;
            sqlParams[7].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_SubPosition;

			if (row.FunKey != System.String.Empty )
				sqlParams[0].Value = row.FunKey;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.Operation != System.String.Empty )
				sqlParams[1].Value = row.Operation;
			else
				sqlParams[1].Value = DBNull.Value ;

			if (row.ButtonBackColor!= System.String.Empty )
				sqlParams[2].Value = row.ButtonBackColor;
			else
				sqlParams[2].Value = DBNull.Value ;

			if (row.ButtonForeColor!= System.String.Empty )
				sqlParams[3].Value = row.ButtonForeColor;
			else
				sqlParams[3].Value = DBNull.Value ;

            if (row.FunctionType != System.String.Empty)
                sqlParams[4].Value = row.FunctionType;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.Parent !=0)
                sqlParams[5].Value = row.Parent;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.MainPosition != 0)
                sqlParams[6].Value = row.MainPosition;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.SubPosition != 0)
                sqlParams[7].Value = row.SubPosition;
            else
                sqlParams[7].Value = DBNull.Value;

			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(FunctionKeysRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(9);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.FunctionKeys_Fld_KeyId, System.Data.DbType.Int32);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.FunctionKeys_Fld_FunKey, System.Data.DbType.String);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.FunctionKeys_Fld_Operation, System.Data.DbType.String);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor, System.Data.DbType.String);
			sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FunctionKeys_Fld_FunctionType, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FunctionKeys_Fld_Parent, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FunctionKeys_Fld_MainPosition, System.Data.DbType.Int32);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.FunctionKeys_Fld_SubPosition, System.Data.DbType.Int32);

			sqlParams[0].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_KeyId;
			sqlParams[1].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_FunKey;
			sqlParams[2].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_Operation;
			sqlParams[3].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_ButtonBackColor;
			sqlParams[4].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_ButtonForeColor;
            sqlParams[5].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_FunctionType;
            sqlParams[6].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_Parent;
            sqlParams[7].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_MainPosition;
            sqlParams[8].SourceColumn = clsPOSDBConstants.FunctionKeys_Fld_SubPosition;
            
            if (row.KeyId != 0 )
				sqlParams[0].Value = row.KeyId;
			else
				sqlParams[0].Value = 0 ;

			if (row.FunKey != System.String.Empty )
				sqlParams[1].Value = row.FunKey;
			else
				sqlParams[1].Value = DBNull.Value ;

			if (row.Operation != System.String.Empty )
				sqlParams[2].Value = row.Operation;
			else
				sqlParams[2].Value = DBNull.Value ;

			if (row.ButtonBackColor!= System.String.Empty )
				sqlParams[3].Value = row.ButtonBackColor;
			else
				sqlParams[3].Value = DBNull.Value ;

			if (row.ButtonForeColor!= System.String.Empty )
				sqlParams[4].Value = row.ButtonForeColor;
			else
				sqlParams[4].Value = DBNull.Value ;

            if (row.FunctionType != System.String.Empty)
                sqlParams[5].Value = row.FunctionType;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.Parent != 0)
                sqlParams[6].Value = row.Parent;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.MainPosition != 0)
                sqlParams[7].Value = row.MainPosition;
            else
                sqlParams[7].Value = DBNull.Value;

            if (row.SubPosition != 0)
                sqlParams[8].Value = row.SubPosition;
            else
                sqlParams[8].Value = DBNull.Value;
			return(sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
