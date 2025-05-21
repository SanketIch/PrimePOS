// ----------------------------------------------------------------
// Library: Data Access
// Author: Adeel Shehzad.
// Company: D-P-S, Inc. (www.d-p-s.com)
//
// ----------------------------------------------------------------
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
using NLog;

namespace POS_Core.DataAccess
{
    // Provides data access methods for DeptCode
    public class InvRecvDetailSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(InvRecvDetailData updates, InvRecvHeaderRow oHRow, IDbTransaction tx, System.Int32 RecievedID, bool isFromPurchaseOrder)
        {
            try
            {
                //this.Delete(updates, tx);
                this.Insert(updates, oHRow, tx, RecievedID, isFromPurchaseOrder);
                //this.Update(updates, tx);

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
                logger.Fatal(ex, "Persist(InvRecvDetailData updates, InvRecvHeaderRow oHRow, IDbTransaction tx, System.Int32 RecievedID, bool isFromPurchaseOrder)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
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
        /*
                public InvRecvDetailData Populate(System.Int32 DeptCode, IDbConnection conn) 
                {
                    try 
                    {
                        string sSQL = "Select " 
                                            + clsPOSDBConstants.InvRecvDetail_Fld_DeptID 
                                            + " , " + clsPOSDBConstants.InvRecvDetail_Fld_DeptCode 
                                            + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_DeptName 
                                            + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_Discount 
                                            + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_IsTaxable 
                                            + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_SaleStartDate 
                                            + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_SaleEndDate 
                                            + " , dept." +   clsPOSDBConstants.InvRecvDetail_Fld_TaxID + " as " + clsPOSDBConstants.InvRecvDetail_Fld_TaxID
                                            + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_SalePrice
                                            + " , taxcodes." + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " as " + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                                            + " , " +  clsPOSDBConstants.TaxCodes_Fld_Description
                                            + " , dept." +  clsPOSDBConstants.InvRecvDetail_Fld_UserID + " as " + clsPOSDBConstants.InvRecvDetail_Fld_UserID
                                        + " FROM " 
                                            + clsPOSDBConstants.InvRecvDetail_tbl + " As Dept "
                                            + " , " + clsPOSDBConstants.TaxCodes_tbl + " As TaxCodes "
                                        + " WHERE " 
                                            + " Dept." + clsPOSDBConstants.InvRecvDetail_Fld_TaxID + " *= TaxCodes." + clsPOSDBConstants.TaxCodes_Fld_TaxID
                                            + " AND " + clsPOSDBConstants.InvRecvDetail_Fld_DeptCode + " ='" + DeptCode + "'";


                        InvRecvDetailData ds = new InvRecvDetailData();
                        ds.InvRecvDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                                    ,  sSQL
                                                    , PKParameters(DeptCode)).Tables[0]);
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
        */
        public InvRecvDetailData Populate(System.Int32 InvRecvDID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(InvRecvDID, conn));
            }
        }

        public InvRecvDetailData Populate(System.Int32 InvRecvDID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + clsPOSDBConstants.InvRecvDetail_Fld_InvRecvDetailID
                    + " , " + clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID
                    + " , inv." + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + " as " + clsPOSDBConstants.InvRecvDetail_Fld_ItemID
                    + " , item." + clsPOSDBConstants.Item_Fld_Description + " as " + clsPOSDBConstants.Item_Fld_Description
                    + " , inv." + clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered + " as " + clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered
                    + " , inv." + clsPOSDBConstants.InvRecvDetail_Fld_QTY + " as " + clsPOSDBConstants.InvRecvDetail_Fld_QTY
                    + " , inv." + clsPOSDBConstants.InvRecvDetail_Fld_Cost + " as " + clsPOSDBConstants.InvRecvDetail_Fld_Cost
                    + " , inv." + clsPOSDBConstants.InvRecvDetail_Fld_SalePrice + " as " + clsPOSDBConstants.InvRecvDetail_Fld_SalePrice
                    + " , inv." + clsPOSDBConstants.InvRecvDetail_Fld_DeptID + " as " + clsPOSDBConstants.InvRecvDetail_Fld_DeptID
                    + " , inv." + clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID + " as " + clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID
                    + " , inv." + clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem + " as " + clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem
                    + " FROM "
                    + clsPOSDBConstants.InvRecvDetail_tbl + " As inv"
                    + " , " + clsPOSDBConstants.Item_tbl + " As item"
                    + " WHERE "
                    + " inv." + clsPOSDBConstants.InvRecvDetail_Fld_ItemID + " = item." + clsPOSDBConstants.Item_Fld_ItemID
                    + " AND " + clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID + " = " + InvRecvDID;


                InvRecvDetailData ds = new InvRecvDetailData();
                ds.InvRecvDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                    , sSQL
                    , PKParameters(InvRecvDID)).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.Int32 InvRecvDID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        // Fills a InvRecvDetailData with all DeptCode
        /*
                public InvRecvDetailData PopulateList(string sWhereClause, IDbConnection conn) 
                {
                    try 
                    { 
                        string sSQL = String.Concat("Select " 
                                                + clsPOSDBConstants.InvRecvDetail_Fld_DeptID 
                                                + " , " + clsPOSDBConstants.InvRecvDetail_Fld_DeptCode 
                                                + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_DeptName 
                                                + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_Discount 
                                                + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_IsTaxable 
                                                + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_SaleStartDate 
                                                + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_SaleEndDate 
                                                + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_TaxID 
                                                + " , " +  clsPOSDBConstants.InvRecvDetail_Fld_SalePrice
                                            + " FROM " 
                                                + clsPOSDBConstants.InvRecvDetail_tbl 
                                            ,sWhereClause);

                        InvRecvDetailData ds = new InvRecvDetailData();
                        ds.InvRecvDetail.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL , whereParameters(sWhereClause)).Tables[0]);
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
                        ErrorHandler.throwException(ex,"",""); 
                        return null;
                    } 
                }

                public InvRecvDetailData Populate(System.String DeptCode) 
                {
                    using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
                    {
                        return(Populate(DeptCode, conn));
                    }
                }

                // Fills a InvRecvDetailData with all DeptCode

            // Fills a InvRecvDetailData with all DeptCode

                public InvRecvDetailData PopulateList(string whereClause) 
                {
                    using(IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString)) 
                    {
                        return(PopulateList(whereClause,conn));
                    }			
                }
        */
        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(InvRecvDetailData ds, InvRecvHeaderRow oHRow, IDbTransaction tx, System.Int32 RecievedID, bool isFromPurchaseOrder)
        {

            InvRecvDetailTable addedTable = (InvRecvDetailTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (InvRecvDetailRow row in addedTable.Rows)
                {
                    try
                    {
                        row.InvRecievedID = RecievedID;
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.InvRecvDetail_tbl, insParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);


                        ItemSvr oISvr = new ItemSvr();
                        //Modified By Amit Date 18 Aug 2011 changed paramter oHRow.VendorID to oHRow.VendorCode
                        oISvr.AddStock(row.ItemID, row.QTY, oHRow.RecvDate, oHRow.VendorCode, row.Cost, row.SalePrice, row.ExpDate, row.DeptID, row.SubDepartmentID, row.IsEBTItem, tx, isFromPurchaseOrder);//SalePrice Parameter Added by Krishna on 6 June 2011// NileshJ - Add paratmeter - isFromPurchaseOrder

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
                        logger.Fatal(ex, "Insert(InvRecvDetailData ds, InvRecvHeaderRow oHRow, IDbTransaction tx, System.Int32 RecievedID, bool isFromPurchaseOrder)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a DeptCodes DataSet, within a given database transaction.
        /*
                public void Update(DataSet ds, IDbTransaction tx) 
                {	
                    InvRecvDetailTable modifiedTable = (InvRecvDetailTable)ds.Tables[0].GetChanges(DataRowState.Modified);

                    string sSQL;
                    IDbDataParameter []updParam;

                    if (modifiedTable != null && modifiedTable.Rows.Count > 0) 
                    {
                        foreach (InvRecvDetailRow row in modifiedTable.Rows) 
                        {
                            try 
                            {
                                updParam = UpdateParameters(row);
                                sSQL = BuildUpdateSQL(clsPOSDBConstants.InvRecvDetail_tbl,updParam);

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
                                ErrorHandler.throwException(ex,"","");
                            }
                        } 
                        modifiedTable.AcceptChanges();
                    }
                }
        */
        /*
                // Delete all rows within a DeptCodes DataSet, within a given database transaction.
                public void Delete(DataSet ds, IDbTransaction tx) 
                {

                    InvRecvDetailTable table = (InvRecvDetailTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
                    string sSQL;
                    IDbDataParameter []delParam;

                    if (table != null && table.Rows.Count > 0) 
                    {
                        table.RejectChanges(); //so we can access the rows
                        foreach (InvRecvDetailRow row in table.Rows) 
                        {
                            try 
                            {
                                delParam = PKParameters(row);

                                sSQL = BuildDeleteSQL(clsPOSDBConstants.InvRecvDetail_tbl,delParam);
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
                                ErrorHandler.throwException(ex,"","");
                            }
                        } 
                    }
                }
        */

        /*
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
        */
        private string BuildInsertSQL(string tableName, IDbDataParameter[] delParam)
        {
            string sInsertSQL = "INSERT INTO " + tableName + " ( ";
            // build where clause
            sInsertSQL = sInsertSQL + delParam[0].SourceColumn;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn;
            }
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }

        /*
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
        */
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
        private IDbDataParameter[] PKParameters(System.Int32 InvRecvDetailID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@InvRecvDetailID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = InvRecvDetailID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(InvRecvDetailRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@InvRecvDetailID";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.InvRecvDetailID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_InvRecvDetailID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(InvRecvDetailRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(11);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_QTY, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_SalePrice, System.Data.DbType.Currency);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_Cost, System.Data.DbType.Currency);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_ItemID, System.Data.DbType.Int32);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_Comments, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered, System.Data.DbType.Int32);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_ExpDate, System.Data.DbType.DateTime); //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added

            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_DeptID, System.Data.DbType.Int32);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID, System.Data.DbType.Int32);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_InvRecievedID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_QTY;
            sqlParams[2].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_SalePrice;
            sqlParams[3].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_Cost;
            sqlParams[4].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_ItemID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_Comments;
            sqlParams[6].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_QtyOrdered;
            sqlParams[7].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_ExpDate;

            sqlParams[8].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_DeptID;
            sqlParams[9].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_SubDepartmentID;
            sqlParams[10].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_IsEBTItem;

            if (row.InvRecievedID.ToString() != System.String.Empty)
                sqlParams[0].Value = row.InvRecievedID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.QTY.ToString() != System.String.Empty)
                sqlParams[1].Value = row.QTY;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.SalePrice.ToString() != System.String.Empty)
                sqlParams[2].Value = row.SalePrice;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.Cost.ToString() != System.String.Empty)
                sqlParams[3].Value = row.Cost;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ItemID.ToString() != System.String.Empty)
                sqlParams[4].Value = row.ItemID;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.Comments != System.String.Empty)
                sqlParams[5].Value = row.Comments;
            else
                sqlParams[5].Value = "";

            if (row.QtyOrdered.ToString() != System.String.Empty)
                sqlParams[6].Value = row.QtyOrdered;
            else
                sqlParams[6].Value = DBNull.Value;

            //Sprint-26 - PRIMEPOS-2387 14-Jul-2017 JY Added ExpDate
            if (row.ExpDate != null)
                sqlParams[7].Value = row.ExpDate;
            else
                sqlParams[7].Value = DBNull.Value;

            if (row.DeptID != 0)
                sqlParams[8].Value = row.DeptID;
            else
            {
                //sqlParams[8].Value = DBNull.Value;
                sqlParams[8].Value = Configuration.CInfo.DefaultDeptId;
            }
            if (row.SubDepartmentID != 0)
                sqlParams[9].Value = row.SubDepartmentID;
            else
                sqlParams[9].Value = DBNull.Value;
            sqlParams[10].Value = row.IsEBTItem;

            return (sqlParams);
        }

        /*		private IDbDataParameter[] UpdateParameters(InvRecvDetailRow row) 
                {
                    IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(9);

                    sqlParams[0] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvDetail_Fld_DeptID, System.Data.DbType.Int32);
                    sqlParams[1] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvDetail_Fld_DeptCode, System.Data.DbType.String);
                    sqlParams[2] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvDetail_Fld_DeptName, System.Data.DbType.String);
                    sqlParams[3] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvDetail_Fld_Discount, System.Data.DbType.Int32);
                    sqlParams[4] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvDetail_Fld_IsTaxable, System.Data.DbType.Boolean);
                    sqlParams[5] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvDetail_Fld_SaleEndDate, System.Data.DbType.DateTime);
                    sqlParams[6] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvDetail_Fld_SalePrice, System.Data.DbType.Int32);
                    sqlParams[7] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvDetail_Fld_SaleStartDate, System.Data.DbType.DateTime);
                    sqlParams[8] = DataFactory.CreateParameter("@"+clsPOSDBConstants.InvRecvDetail_Fld_TaxID, System.Data.DbType.Int32);

                    sqlParams[0].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_DeptID;
                    sqlParams[1].SourceColumn = clsPOSDBConstants.InvRecvDetail_Fld_DeptCode;
                    sqlParams[2].SourceColumn  = clsPOSDBConstants.InvRecvDetail_Fld_DeptName;
                    sqlParams[3].SourceColumn  = clsPOSDBConstants.InvRecvDetail_Fld_Discount;
                    sqlParams[4].SourceColumn  = clsPOSDBConstants.InvRecvDetail_Fld_IsTaxable;
                    sqlParams[5].SourceColumn  = clsPOSDBConstants.InvRecvDetail_Fld_SaleEndDate;
                    sqlParams[6].SourceColumn  = clsPOSDBConstants.InvRecvDetail_Fld_SalePrice;
                    sqlParams[7].SourceColumn  = clsPOSDBConstants.InvRecvDetail_Fld_SaleStartDate;
                    sqlParams[8].SourceColumn  = clsPOSDBConstants.InvRecvDetail_Fld_TaxID;

                    if (row.DeptID != 0 )
                        sqlParams[0].Value = row.DeptID;
                    else
                        sqlParams[0].Value = 0 ;

                    if (row.DeptCode != System.String.Empty )
                        sqlParams[1].Value = row.DeptCode;
                    else
                        sqlParams[1].Value = DBNull.Value ;

                    if (row.DeptName != System.String.Empty )
                        sqlParams[2].Value = row.DeptName;
                    else
                        sqlParams[2].Value = DBNull.Value ;

                    if (row.Discount != 0 )
                        sqlParams[3].Value = row.Discount;
                    else
                        sqlParams[3].Value = 0;

                    sqlParams[4].Value = row.IsTaxable;

                    if (row.SaleEndDate != System.DateTime.MinValue )
                        sqlParams[5].Value = row.SaleEndDate;
                    else
                        sqlParams[5].Value = System.DateTime.MinValue ;

                    if (row.SalePrice != 0 )
                        sqlParams[6].Value = row.SalePrice;
                    else
                        sqlParams[6].Value = 0 ;

                    if (row.SaleStartDate != System.DateTime.MinValue)
                        sqlParams[7].Value = row.SaleStartDate;
                    else
                        sqlParams[7].Value = System.DateTime.MinValue;

                    if (row.TaxId != 0)
                        sqlParams[8].Value = row.TaxId;
                    else
                        sqlParams[8].Value = 0 ;

                    return(sqlParams);
                }
        */
        #endregion

        public void Dispose() { }
    }
}