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

    public class PhysicalInvSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

        public void Persist(DataSet updates, IDbTransaction tx)
        {
            try

            {
                this.Insert(updates, tx);
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
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                   //ErrorLogging.ErrorHandler.throwException(ex,"","");
            }
        }


        // Inserts, updates or deletes rows in a DataSet.

        public void Persist(DataSet updates)
        {

            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
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
                logger.Fatal(ex, "Persist(DataSet updates) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                //ErrorHandler.throwException(ex,"","");
            }

        }
        #endregion

        #region Get Methods
        // Looks up a DeptCode based on its primary-key:System.Int32 DeptCode

        public PhysicalInvData Populate(System.Int32 isProcessed, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                                    + clsPOSDBConstants.PhysicalInv_Fld_ID
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_ItemCode
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_OldQty
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_NewQty
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice
                                    + " , item." + clsPOSDBConstants.Item_Fld_Description
                                    + " , PhysicalInv." + clsPOSDBConstants.PhysicalInv_Fld_UserID
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_TransDate
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_isProcessed
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_PUserID
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_PTransDate
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_OldExpDate  //Sprint-21 - 2206 09-Mar-2016 JY Added
                                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_NewExpDate  //Sprint-21 - 2206 09-Mar-2016 JY Added
                                + " FROM " + clsPOSDBConstants.PhysicalInv_tbl
                                + " INNER JOIN " + clsPOSDBConstants.Item_tbl + " ON " + clsPOSDBConstants.Item_Fld_ItemID + " = " + clsPOSDBConstants.PhysicalInv_Fld_ItemCode
                                + " WHERE " + clsPOSDBConstants.PhysicalInv_Fld_isProcessed + " =" + isProcessed;

                PhysicalInvData ds = new PhysicalInvData();
                ds.PhysicalInv.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
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
                logger.Fatal(ex, "Populate(System.Int32 isProcessed, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                             //ErrorHandler.throwException(ex,"","");
                return null;
            }
        }

        public PhysicalInvData Populate(System.Int32 isProcessed)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(isProcessed, conn));
            }
        }

        public PhysicalInvData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + clsPOSDBConstants.PhysicalInv_Fld_ID
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_ItemCode
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_OldQty
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_NewQty
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice
                    + " , item." + clsPOSDBConstants.Item_Fld_Description
                    + " , PhysicalInv." + clsPOSDBConstants.PhysicalInv_Fld_UserID
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_TransDate
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_isProcessed
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_PUserID
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_PTransDate
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_OldExpDate  //Sprint-21 - 2206 09-Mar-2016 JY Added
                    + " , " + clsPOSDBConstants.PhysicalInv_Fld_NewExpDate  //Sprint-21 - 2206 09-Mar-2016 JY Added
                    + " FROM " + clsPOSDBConstants.PhysicalInv_tbl
                    + " INNER JOIN " + clsPOSDBConstants.Item_tbl + " ON " + clsPOSDBConstants.Item_Fld_ItemID + " = " + clsPOSDBConstants.PhysicalInv_Fld_ItemCode
                    + " WHERE " + sWhereClause;

                PhysicalInvData ds = new PhysicalInvData();
                ds.PhysicalInv.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
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
                                                                                            //ErrorHandler.throwException(ex,"",""); 
                return null;
            }
        }

        // Fills a PhysicalInvData with all DeptCode

        public PhysicalInvData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(whereClause, conn));
            }
        }

        #region PRIMEPOS-2396 11-Jun-2018 JY Added logic to get last inventory updated quantity
        public System.Int32 GetLastInvUpdatedQty(string ItemId)
        {
            try
            {
                string sSQL = "SELECT TOP 1 Qty AS LastInvUpdated FROM " +
                                " (SELECT TOP 1 PI.NewQty AS Qty, PI.PTransDate AS TransDate FROM PhysicalInv PI " +
                                " INNER JOIN Item I ON I.ItemID = PI.ItemCode " +
                                " WHERE PI.ItemCode = '" + ItemId.Replace("'", "''").Trim() + "' AND PI.isProcessed = 1 ORDER BY PTransDate DESC " +
                                " UNION ALL " +
                                " SELECT TOP 1 IRD.Qty, IR.RecieveDate AS TransDate FROM InventoryRecieved IR " +
                                " INNER JOIN InvRecievedDetail IRD  ON IR.InvRecievedID = IRD.InvRecievedID " +
                                " INNER JOIN InvTransType ITT ON ITT.ID = IR.InvTransTypeID " +
                                " WHERE ITT.TransType = 0 AND IRD.ItemID = '" + ItemId.Replace("'", "''").Trim() + "' ORDER BY IR.RecieveDate DESC " +
                                " ) as TempTable " +
                            " Order by TransDate Desc";

                object obj = DataHelper.ExecuteScalar(Configuration.ConnectionString, CommandType.Text, sSQL);
                return (Configuration.convertNullToInt(obj));
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
                logger.Fatal(ex, "GetLastInvUpdatedQty(string ItemId)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                          //ErrorHandler.throwException(ex, "", "");
                return 0;
            }
        }
        #endregion

        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx)
        {
            //ErrorLogging.Logs.Logger(" ** Start PhysicalInv Insert  ");
            logger.Trace("Insert(DataSet ds, IDbTransaction tx) - ** Start PhysicalInv Insert");
            PhysicalInvTable addedTable = (PhysicalInvTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (PhysicalInvRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.PhysicalInv_tbl, insParam);
                        for (int i = 0; i < insParam.Length; i++)
                        {
                            Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
                        }
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);

                        //ErrorLogging.Logs.Logger(" ** END PhysicalInv Insert  ");
                        logger.Trace("Insert(DataSet ds, IDbTransaction tx) - ** END PhysicalInv Insert");
                    }
                    catch (POSExceptions ex)
                    {
                        //ErrorLogging.Logs.Logger(" ** Start PhysicalInv Insert Error " + ex.Message);
                        logger.Fatal(ex, "** Start PhysicalInv Insert Error - Insert(DataSet ds, IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        //ErrorLogging.Logs.Logger(" ** Start PhysicalInv Insert Error " + ex.Message);
                        logger.Fatal(ex, "Start PhysicalInv Insert Error - Insert(DataSet ds, IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        //ErrorLogging.Logs.Logger(" ** Start PhysicalInv Insert Error " + ex.Message);
                        logger.Fatal(ex, "Start PhysicalInv Insert Error - Insert(DataSet ds, IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                                                      //ErrorHandler.throwException(ex,"","");
                    }
                }
                addedTable.AcceptChanges();
            }
        }
        public void ProcessData(IDbTransaction tx)
        {

            string sSQL;
            try
            {
                //ErrorLogging.Logs.Logger(" ** Start ProcessData (IDbTransaction tx)  Physical InventorySVR");
                logger.Trace("ProcessData(IDbTransaction tx) - ** Start ProcessData (IDbTransaction tx)  Physical InventorySVR");
                sSQL = " update item set qtyinstock=newqty,sellingprice=newsellingprice,lastcostprice=newcostprice, ExpDate = NewExpDate from physicalinv where item.itemid=physicalinv.itemcode and isprocessed=0 ";   //Sprint-21 - 2206 11-Mar-2016 JY Added ExpDate 
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, null);
                sSQL = " update physicalinv set isprocessed=1, ptransdate='" + DateTime.Now + "' , puserid='" + Configuration.UserName + "' where isprocessed=0 ";
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, null);
                //ErrorLogging.Logs.Logger(" ** END ProcessData (IDbTransaction tx)  Physical InventorySVR");
                logger.Trace("ProcessData(IDbTransaction tx) - ** END ProcessData (IDbTransaction tx)  Physical InventorySVR");
            }
            catch (Exception ex)
            {
                tx.Rollback();
                //ErrorLogging.Logs.Logger(" **  ProcessData (IDbTransaction tx) PhysicalInv  Error:" + ex.Message);
                logger.Fatal(ex, " **  ProcessData (IDbTransaction tx) PhysicalInv  Error: ProcessData(IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                                                               //ErrorHandler.throwException(ex,"","");
            }
        }

        public void DeleteRow(System.Int32 CurrentID)
        {

            string sSQL;
            try
            {
                //ErrorLogging.Logs.Logger(" ** Start delete from PhysicalInv  ID:" + CurrentID);
                logger.Trace("DeleteRow(System.Int32 CurrentID) - ** Start delete from PhysicalInv ID:" + CurrentID);
                sSQL = " delete from PhysicalInv where id=" + CurrentID;
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                //ErrorLogging.Logs.Logger(" ** End delete from PhysicalInv  ID:" + CurrentID);
                logger.Trace("DeleteRow(System.Int32 CurrentID) - ** End delete from PhysicalInv ID:" + CurrentID);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(System.Int32 CurrentID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                        //ErrorHandler.throwException(ex,"","");
                                                                        //ErrorLogging.Logs.Logger(" **  delete from PhysicalInv Error  ID:" + CurrentID+" PhysicalInv  Error:" + ex.Message);
            }
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
            //sInsertSQL = sInsertSQL + " , isProcessed ";
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            //sInsertSQL = sInsertSQL + " ,0 ";
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
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
        private IDbDataParameter[] PKParameters(System.String PhysicalInvID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@PhysicalInvID";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = PhysicalInvID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(PhysicalInvRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@PhysicalInvID";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_ID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(PhysicalInvRow row)
        {
            //PRIMEPOS-3121 12-Sep-2022 JY modified

            IDbDataParameter[] sqlParams;
            if (row.isProcessed)
                sqlParams = DataFactory.CreateParameterArray(14);
            else
                sqlParams = DataFactory.CreateParameterArray(12);    //Sprint-21 - 2206 11-Mar-2016 JY changed from 9 to 11

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_ItemCode, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_OldQty, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_NewQty, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.fld_UserID, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_TransDate, System.Data.DbType.DateTime);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice, System.Data.DbType.Decimal);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice, System.Data.DbType.Decimal);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice, System.Data.DbType.Decimal);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice, System.Data.DbType.Decimal);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_OldExpDate, System.Data.DbType.DateTime);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_NewExpDate, System.Data.DbType.DateTime);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_isProcessed, System.Data.DbType.Boolean);

            if (row.isProcessed)
            {
                sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_PTransDate, System.Data.DbType.DateTime);
                sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PhysicalInv_Fld_PUserID, System.Data.DbType.String);
            }

            sqlParams[0].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_ItemCode;
            sqlParams[1].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_OldQty;
            sqlParams[2].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_NewQty;
            sqlParams[3].SourceColumn = clsPOSDBConstants.fld_UserID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_TransDate;
            sqlParams[5].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_OldSellingPrice;
            sqlParams[6].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_NewSellingPrice;
            sqlParams[7].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_OldCostPrice;
            sqlParams[8].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_NewCostPrice;
            sqlParams[9].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_OldExpDate;
            sqlParams[10].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_NewExpDate;
            sqlParams[11].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_isProcessed;

            if (row.isProcessed)
            {                
                sqlParams[12].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_PTransDate;
                sqlParams[13].SourceColumn = clsPOSDBConstants.PhysicalInv_Fld_PUserID;
            }

            if (row.ItemCode != System.String.Empty)
                sqlParams[0].Value = row.ItemCode;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.OldQty.ToString() != null)
                sqlParams[1].Value = row.OldQty;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.NewQty.ToString() != null)
                sqlParams[2].Value = row.NewQty;
            else
                sqlParams[2].Value = 0;

            sqlParams[3].Value = Configuration.UserName;

            if (row.TransDate.ToString() != null)
                sqlParams[4].Value = row.TransDate;
            else
                sqlParams[4].Value = DateTime.Now;

            if (row.OldSellingPrice.ToString() != null)
                sqlParams[5].Value = row.OldSellingPrice;
            else
                sqlParams[5].Value = 0;

            if (row.NewSellingPrice.ToString() != null)
                sqlParams[6].Value = row.NewSellingPrice;
            else
                sqlParams[6].Value = 0;

            if (row.OldCostPrice.ToString() != null)
                sqlParams[7].Value = row.OldCostPrice;
            else
                sqlParams[7].Value = 0;

            if (row.NewCostPrice.ToString() != null)
                sqlParams[8].Value = row.NewCostPrice;
            else
                sqlParams[8].Value = 0;

            #region Sprint-21 - 2206 11-Mar-2016 JY Added
            if (row.OldExpDate != null)
                sqlParams[9].Value = row.OldExpDate;
            else
                sqlParams[9].Value = DBNull.Value;

            if (row.OldExpDate != null)
                sqlParams[10].Value = row.NewExpDate;
            else
                sqlParams[10].Value = DBNull.Value;
            #endregion
            sqlParams[11].Value = row.isProcessed;

            if (row.isProcessed)
            {
                if (row.PTransDate.ToString() != null)
                    sqlParams[12].Value = row.PTransDate;
                else
                    sqlParams[12].Value = DBNull.Value;
                sqlParams[13].Value = row.PUserID;
            }
            return (sqlParams);
        }
        #endregion


        public void Dispose() { }
    }
}
