using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
//using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using System.Collections.Generic;
//using POS.Resources;
using POS_Core.Resources;
using Resources;
using NLog;

namespace POS_Core.DataAccess
{

	// Provides data access methods for DeptCode
 
	public class InvRecvHeaderSvr: IDisposable  
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		#region Persist Methods

		// Inserts, updates or deletes rows in a DataSet, within a database transaction.

		public  void Persist(DataSet updates, IDbTransaction tx,ref System.Int32 RecievedID) 
		{
			try 
		
			{
				this.Insert(updates, tx,ref RecievedID);
				//this.Update(updates, tx);
				if (RecievedID>0)
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
				logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx,ref System.Int32 RecievedID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//POS_Core.ErrorLogging.ErrorHandler.throwException(ex,"","");
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

		public InvRecvHeaderData Populate(System.Int32 InvRecvID, IDbConnection conn) 
		{
			try 
			{
				string sSQL = "Select " 
									+ clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID
									+ " , " +  clsPOSDBConstants.InvRecvHeader_Fld_RefNo
									+ " , " +  clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate
									+ " , " +   clsPOSDBConstants.InvRecvHeader_Fld_VendorID + " as " + clsPOSDBConstants.InvRecvHeader_Fld_VendorID
									+ " , vend." + clsPOSDBConstants.Vendor_Fld_VendorCode + " as " + clsPOSDBConstants.Vendor_Fld_VendorCode
									+ " , " +  clsPOSDBConstants.Vendor_Fld_VendorName
									+ " , InvRecvH." +  clsPOSDBConstants.fld_UserID + " as " + clsPOSDBConstants.fld_UserID
								+ " FROM " 
									+ clsPOSDBConstants.InvRecvHeader_tbl + " As InvRecvH "
									+ " left join " + clsPOSDBConstants.Vendor_tbl + " As Vend"
									+ " on InvRecvH." + clsPOSDBConstants.InvRecvHeader_Fld_VendorID + " = Vend." + clsPOSDBConstants.Vendor_Fld_VendorId
								+ " WHERE " 
									+ " AND " + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID + " ='" + InvRecvID + "'";


				InvRecvHeaderData ds = new InvRecvHeaderData();
				ds.InvRecievedHeader.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
											,  sSQL
											, PKParameters(InvRecvID)).Tables[0]);
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
				logger.Fatal(ex, "Populate(System.Int32 InvRecvID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex,"","");
				return null;
			}
		}

		public InvRecvHeaderData Populate(System.Int32 InvRecvID) 
		{
			using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
			{
				return(Populate(InvRecvID, conn));
			}
		}

		public InvRecvHeaderData PopulateList(string sWhereClause, IDbConnection conn) 
		{
			try 
			{ 
				string sSQL = "Select " 
					+ clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID
					+ " , " +  clsPOSDBConstants.InvRecvHeader_Fld_RefNo
					+ " , " +  clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate
					+ " , " +   clsPOSDBConstants.InvRecvHeader_Fld_VendorID + " as " + clsPOSDBConstants.InvRecvHeader_Fld_VendorID
					+ " , vend." + clsPOSDBConstants.Vendor_Fld_VendorCode + " as " + clsPOSDBConstants.Vendor_Fld_VendorCode
					+ " , " +  clsPOSDBConstants.Vendor_Fld_VendorName
					+ " , InvRecvH." +  clsPOSDBConstants.fld_UserID + " as " + clsPOSDBConstants.fld_UserID
					+ " FROM " 
					+ clsPOSDBConstants.InvRecvHeader_tbl + " As InvRecvH "
					+ " left join " + clsPOSDBConstants.Vendor_tbl + " As Vend"
					+ " on InvRecvH." + clsPOSDBConstants.InvRecvHeader_Fld_VendorID + " = Vend." + clsPOSDBConstants.Vendor_Fld_VendorId;

				if (sWhereClause.Trim()!="")
					sSQL=String.Concat(sSQL,sWhereClause);

				InvRecvHeaderData ds = new  InvRecvHeaderData();
				ds.InvRecievedHeader.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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
        //Added By Amit Date 28 Nov 2011
        public DataSet ItemInvRecDate(List<string> lstData)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (ItemInvRecDate(lstData, conn));
            }
        }

        public DataSet ItemInvRecDate(List<string> lstData, IDbConnection conn)
        {
            string sSQL;
            string ItemIDInClause = "1=1";
            if (lstData.Count > 0)
            {
                ItemIDInClause = "ItemId in (" + string.Join(",", lstData.ToArray()) + ")";
            }
            try
            {
                sSQL = " Select IRD." + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + ", Max (IR." + clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate + ") as InvRecDate " +
                                    " from "+clsPOSDBConstants.InvRecvDetail_tbl+" IRD "+ 
                                    " inner join "+ clsPOSDBConstants.InvRecvHeader_tbl+" IR "+
                                    " on IRD."+clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID+"= IR."+clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID+
                                    " where " + ItemIDInClause +
                                    " group by IRD."+clsPOSDBConstants.InvRecvDetail_Fld_ItemID+"";

                return DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
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
				logger.Fatal(ex, "ItemInvRecDate(List<string> lstData, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
                return null;
            } 

        }
        //End
		#endregion //Get Method
		public DataSet ItemInvRecDate(string strDistItems)
		{
			using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
			{
				return (ItemInvRecDate(strDistItems, conn));
			}
		}

		public DataSet ItemInvRecDate(string strDistItems, IDbConnection conn)
		{
			string sSQL;
			string ItemIDInClause = "1=1";
			if (strDistItems != string.Empty)
			{
				ItemIDInClause = "ItemId in (" + strDistItems + ")";
			}
			try
			{
				sSQL = " Select IRD." + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + ", Max (IR." + clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate + ") as InvRecDate " +
									" from " + clsPOSDBConstants.InvRecvDetail_tbl + " IRD " +
									" inner join " + clsPOSDBConstants.InvRecvHeader_tbl + " IR " +
									" on IRD." + clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID + "= IR." + clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID +
									" where " + ItemIDInClause +
									" group by IRD." + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + "";

				return DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
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
				logger.Fatal(ex, "ItemInvRecDate(string strDistItems, IDbConnection conn)");
				return null;
			}
		}

		#region Insert, Update, and Delete Methods
		public void Insert(DataSet ds, IDbTransaction tx, ref System.Int32 RecievedID) 
		{

			InvRecvHeaderTable addedTable = (InvRecvHeaderTable)ds.Tables[0].GetChanges(DataRowState.Added);
			string sSQL;
			IDbDataParameter []insParam;

			if (addedTable != null && addedTable.Rows.Count > 0) 
			{
				foreach (InvRecvHeaderRow row in addedTable.Rows) 
				{
					try 
					{
						insParam = InsertParameters(row);
						sSQL = BuildInsertSQL(clsPOSDBConstants.InvRecvHeader_tbl,insParam);
						for(int i = 0; i< insParam.Length;i++)
						{
							Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
						}
						DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
						RecievedID=Convert.ToInt32(DataHelper.ExecuteScalar(tx,CommandType.Text,"select @@identity"));
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
						logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx, ref System.Int32 RecievedID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex,"","");
					}
				}
				addedTable.AcceptChanges();
			}
		}

		// Update all rows in a DeptCodes DataSet, within a given database transaction.

		public void Update(DataSet ds, IDbTransaction tx) 
		{	
			InvRecvHeaderTable modifiedTable = (InvRecvHeaderTable)ds.Tables[0].GetChanges(DataRowState.Modified);

			string sSQL;
			IDbDataParameter []updParam;

			if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
			{
				foreach (InvRecvHeaderRow row in modifiedTable.Rows) 
				{
					try 
					{
						updParam = UpdateParameters(row);
						sSQL = BuildUpdateSQL(clsPOSDBConstants.InvRecvHeader_tbl,updParam);

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
		private IDbDataParameter[] PKParameters(System.Int32 InvRecvID) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@InvRecvID";
			sqlParams[0].DbType = System.Data.DbType.Int32;
			sqlParams[0].Value = InvRecvID;

			return(sqlParams);
		}

		private IDbDataParameter[] PKParameters(InvRecvHeaderRow row) 
		{
			//return a SqlParameterCollection
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


			sqlParams[0] = DataFactory.CreateParameter();
			sqlParams[0].ParameterName = "@InvRecvID";
			sqlParams[0].DbType = System.Data.DbType.Int32;

			sqlParams[0].Value = row.InvRecvID;
			sqlParams[0].SourceColumn = clsPOSDBConstants.InvRecvHeader_Fld_InvRecvID;

			return(sqlParams);
		}

		private IDbDataParameter[] InsertParameters(InvRecvHeaderRow row) 
		{
            //IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);//ORG Commented by shitaljit(QuicSolv) on june 24 2011
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);
			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvHeader_Fld_RefNo, System.Data.DbType.String);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate, System.Data.DbType.DateTime);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvHeader_Fld_VendorID, System.Data.DbType.Int32);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvHeader_Fld_InvTransTypeID, System.Data.DbType.Int32);////Added By Shitaljit(QuicSolv) on june 24 2011
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo, System.Data.DbType.String);

			sqlParams[0].SourceColumn  = clsPOSDBConstants.InvRecvHeader_Fld_RefNo;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.InvRecvHeader_Fld_VendorID;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.fld_UserID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.InvRecvHeader_Fld_InvTransTypeID;//Added By Shitaljit(QuicSolv) on june 24 2011
            sqlParams[5].SourceColumn = clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo;//Added By Shitaljit(QuicSolv) on 25 April 2013 for JIRA-577

			if (row.RefNo != System.String.Empty )
				sqlParams[0].Value = row.RefNo;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.RecvDate!= System.DateTime.MinValue )
				sqlParams[1].Value = row.RecvDate;
			else
				sqlParams[1].Value = System.DateTime.MinValue ;

			if (row.VendorID!= 0 )
				sqlParams[2].Value = row.VendorID;
			else
				sqlParams[2].Value = 0 ;
            sqlParams[3].Value = Configuration.UserName;//Added By Shitaljit(QuicSolv) on june 24 2011
             sqlParams[4].Value = row.InvTransTypeID;

             if (row.POOrderNo != System.String.Empty)
                 sqlParams[5].Value = row.POOrderNo;
             else
                 sqlParams[5].Value = DBNull.Value;

			return(sqlParams);
		}

		private IDbDataParameter[] UpdateParameters(InvRecvHeaderRow row) 
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);

			sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvHeader_Fld_RefNo, System.Data.DbType.String);
			sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate, System.Data.DbType.DateTime);
			sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvHeader_Fld_VendorID, System.Data.DbType.Int32);
			sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvHeader_Fld_InvTransTypeID, System.Data.DbType.Int32);////Added By Shitaljit(QuicSolv) on june 24 2011
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo, System.Data.DbType.String);

			sqlParams[0].SourceColumn  = clsPOSDBConstants.InvRecvHeader_Fld_RefNo;
			sqlParams[1].SourceColumn  = clsPOSDBConstants.InvRecvHeader_Fld_RecieveDate;
			sqlParams[2].SourceColumn  = clsPOSDBConstants.InvRecvHeader_Fld_VendorID;
			sqlParams[3].SourceColumn  = clsPOSDBConstants.fld_UserID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.InvRecvHeader_Fld_InvTransTypeID;//Added By Shitaljit(QuicSolv) on june 24 2011
            sqlParams[5].SourceColumn = clsPOSDBConstants.InvRecvHeader_Fld_POOrderNo;//Added By Shitaljit(QuicSolv) on 25 April 2013 for JIRA-577
			if (row.RefNo != System.String.Empty )
				sqlParams[0].Value = row.RefNo;
			else
				sqlParams[0].Value = DBNull.Value ;

			if (row.RecvDate!= System.DateTime.MinValue )
				sqlParams[1].Value = row.RecvDate;
			else
				sqlParams[1].Value = System.DateTime.MinValue ;

			if (row.VendorID!= 0 )
				sqlParams[2].Value = row.VendorID;
			else
				sqlParams[2].Value = 0 ;

			sqlParams[3].Value=Configuration.UserName;
            sqlParams[4].Value = row.InvTransTypeID;

            if (row.POOrderNo != System.String.Empty)
                sqlParams[5].Value = row.POOrderNo;
            else
                sqlParams[5].Value = DBNull.Value;

			return(sqlParams);
		}
		#endregion

    
		public void Dispose() {}   
	}
}
