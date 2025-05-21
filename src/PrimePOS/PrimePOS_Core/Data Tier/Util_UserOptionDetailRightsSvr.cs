using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
//using POS.Resources;
using POS_Core.Resources;
using NLog;

namespace POS_Core.DataAccess
{
    public class Util_UserOptionDetailRightsSvr : IDisposable  
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void Persist(DataSet updates, IDbTransaction tx)
       {
           try
           {
               this.Delete(updates, tx);
               this.Insert(updates, tx);
               this.Update(updates, tx);

               updates.AcceptChanges();
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
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
       }
       public bool DeleteRow(Int64 ID)
       {
           string sSQL;
           try
           {
               sSQL = " delete from " + clsPOSDBConstants.Util_UserOptionDetailRights_tbl + " where " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID+ "= '" + ID.ToString() + "'";
               DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
               return true;
           }
           catch (Exception ex)
           {
                logger.Fatal(ex, "DeleteRow(Int64 ID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return false;
           }
       }
       public void Persist(DataSet updates)
       {

           IDbTransaction tx = null;
           try
           {
               IDbConnection conn = DataFactory.CreateConnection(DBConfig.ConnectionString);
               tx = conn.BeginTransaction();
               this.Persist(updates, tx);
               tx.Commit();
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
               tx.Rollback();
                logger.Fatal(ex, "Persist(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }

       }
       public Util_UserOptionDetailRightsData Populate(string whereClause)
       {
           using (IDbConnection conn = DataFactory.CreateConnection(DBConfig.ConnectionString))
           {
               return (PopulateList(whereClause, conn));
           }
       }
       public Util_UserOptionDetailRightsData PopulateList(string sWhereClause, IDbConnection conn)
       {
           try
           {
               string sSQL = String.Concat("Select "
                                       + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID
                                       + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID
                                       + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID
                                        + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId
                                        + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID
                                        + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed
                                        + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId

                                   + " FROM "
                                       + clsPOSDBConstants.Util_UserOptionDetailRights_tbl
                                   , sWhereClause);

               Util_UserOptionDetailRightsData ds = new Util_UserOptionDetailRightsData();
               ds.Util_UserOptionDetailRights.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
           }
       }
       public Util_UserOptionDetailRightsData Populate(System.Int32 ID, IDbConnection conn)
       {
           try
           {
               string sSQL = "Select "
                                   + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID
                                   + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed
                                   + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID
                                   + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId
                                   + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID
                                   + " , " + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID
                                
                               + " FROM "
                                   + clsPOSDBConstants.Util_UserOptionDetailRights_tbl;
                               

               Util_UserOptionDetailRightsData ds = new Util_UserOptionDetailRightsData();
               ds.Util_UserOptionDetailRights.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                           , sSQL
                                           , PKParameters(ID)).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.Int32 ID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
           }
       }
       public Util_UserOptionDetailRightsData Populate(System.Int32 PayOutID)
       {
           using (IDbConnection conn = DataFactory.CreateConnection(DBConfig.ConnectionString))
           {
               return (Populate(PayOutID, conn));
           }
       }
       #region Insert, Update, and Delete Methods
       public void Insert(DataSet ds, IDbTransaction tx)
       {

           Util_UserOptionDetailRightsTable addedTable = (Util_UserOptionDetailRightsTable)ds.Tables[0].GetChanges(DataRowState.Added);
           string sSQL;
           IDbDataParameter[] insParam;

           if (addedTable != null && addedTable.Rows.Count > 0)
           {
               foreach (DataRow row in addedTable.Rows)
               {
                   try
                   {

                       insParam = InsertParameters(row);
                       sSQL = BuildInsertSQL(clsPOSDBConstants.Util_UserOptionDetailRights_tbl, insParam);
                       for (int i = 0; i < insParam.Length; i++)
                       {
                           Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
                       }
                       DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);


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
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                    }
               }
               addedTable.AcceptChanges();
           }
       }
       // Update all rows in a DeptCodes DataSet, within a given database transaction.
       public void Update(DataSet ds, IDbTransaction tx)
       {
           Util_UserOptionDetailRightsTable modifiedTable = (Util_UserOptionDetailRightsTable)ds.Tables[0].GetChanges(DataRowState.Modified);

           string sSQL;
           IDbDataParameter[] updParam;

           if (modifiedTable != null && modifiedTable.Rows.Count > 0)
           {
               foreach (DataRow row in modifiedTable.Rows)
               {
                   try
                   {
                       updParam = UpdateParameters(row);
                       sSQL = BuildUpdateSQL(clsPOSDBConstants.Util_UserOptionDetailRights_tbl, updParam);

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
                        //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                    }
               }
               modifiedTable.AcceptChanges();
           }
       }

       // Delete all rows within a DeptCodes DataSet, within a given database transaction.
       public void Delete(DataSet ds, IDbTransaction tx)
       {

           Util_UserOptionDetailRightsTable table = (Util_UserOptionDetailRightsTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
           string sSQL;
           IDbDataParameter[] delParam;

           if (table != null && table.Rows.Count > 0)
           {
               table.RejectChanges(); //so we can access the rows
               foreach (DataRow row in table.Rows)
               {
                   try
                   {
                       delParam = PKParameters(row);

                       sSQL = BuildDeleteSQL(clsPOSDBConstants.Util_UserOptionDetailRights_tbl, delParam);
                       DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, delParam);
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
                        logger.Fatal(ex, "Delete(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                    }
               }
           }
       }
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
       private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
       {
           string sInsertSQL = "INSERT INTO " + tableName + " ( ";
           // build where clause
           sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

           for (int i = 1; i < delParam.Length; i++)
           {
               sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn;
           }
           //sInsertSQL = sInsertSQL + " , UserId ";
           sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

           for (int i = 1; i < delParam.Length; i++)
           {
               sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
           }
           //sInsertSQL = 	sInsertSQL + " , '" + Configuration.UserName + "'";
           sInsertSQL = sInsertSQL + " )";
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
           return (sqlParams);
       }
       private IDbDataParameter[] PKParameters(System.Int32 Util_UserOptionDetailRightsID)
       {
           IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

           sqlParams[0] = DataFactory.CreateParameter();
           sqlParams[0].ParameterName = "@ID";
           sqlParams[0].DbType = System.Data.DbType.Int32;
           sqlParams[0].Value = Util_UserOptionDetailRightsID;

           return (sqlParams);
       }

       private IDbDataParameter[] PKParameters(DataRow row)
       {
           //return a SqlParameterCollection
           IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


           sqlParams[0] = DataFactory.CreateParameter();
           sqlParams[0].ParameterName = "@ID";
           sqlParams[0].DbType = System.Data.DbType.Int32;

           sqlParams[0].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID];
           sqlParams[0].SourceColumn = clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID;

           return (sqlParams);
       }

       private IDbDataParameter[] InsertParameters(DataRow row)
       {
           IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);
          // sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID, System.Data.DbType.Int64);
           sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID, System.Data.DbType.String);
           sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID, System.Data.DbType.Int32);
           sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId, System.Data.DbType.Int32);
           sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID, System.Data.DbType.Int32);
           sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed, System.Data.DbType.Boolean);
           sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId, System.Data.DbType.Int32);



           //sqlParams[0].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID];

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID] != System.String.Empty)
               sqlParams[0].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID];
           else
               sqlParams[0].Value = Configuration.UserName;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID].ToString() != null)
               sqlParams[1].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID];
           else
               sqlParams[1].Value = 0;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId].ToString() != null)
               sqlParams[2].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId];
           else
               sqlParams[2].Value = 0;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID].ToString() != null)
               sqlParams[3].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID];
           else
               sqlParams[3].Value = 0;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed].ToString() != null)
               sqlParams[4].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed];
           else
               sqlParams[4].Value = false;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId].ToString() != null)
               sqlParams[5].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId];
           else
               sqlParams[5].Value = 0;
           //sqlParams[7].Value =row.
           return (sqlParams);
       }

       private IDbDataParameter[] UpdateParameters(DataRow row)
       {
           IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);
           sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID, System.Data.DbType.Int64);
           sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID, System.Data.DbType.String);
           sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID, System.Data.DbType.Int32);
           sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId, System.Data.DbType.Int32);
           sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID, System.Data.DbType.Int32);
           sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed, System.Data.DbType.Boolean);
           sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId, System.Data.DbType.Int32);



           sqlParams[0].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ID];

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID] != System.String.Empty)
               sqlParams[1].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_UserID];
           else
               sqlParams[1].Value = Configuration.UserName;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID].ToString() != null)
               sqlParams[2].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ModuleID];
           else
               sqlParams[2].Value = 0;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId].ToString() != null)
               sqlParams[3].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_PermissionId];
           else
               sqlParams[3].Value = 0;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID].ToString() != null)
               sqlParams[4].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_ScreenID];
           else
               sqlParams[4].Value = 0;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed].ToString() != null)
               sqlParams[5].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_isAllowed];
           else
               sqlParams[5].Value = false;

           if (row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId].ToString() != null)
               sqlParams[6].Value = row[clsPOSDBConstants.Util_UserOptionDetailRights_Fld_DetailId];
           else
               sqlParams[6].Value = 0;
           //sqlParams[7].Value =row.

          

           return (sqlParams);
       }
       #endregion
       public void Dispose() { }   
    }
}
