using NLog;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.ErrorLogging;
using Resources;
using System;
using System.Data;

namespace POS_Core.Data_Tier
{
    public class NoSaleTransactionSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public DataSet GeNoSaleTransaction(DateTime dtFrom, DateTime dtTo, string stationID = null, string userID = null)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                try
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "select Id,StationID, UserID , DrawerOpenedDate from NoSale ";
                    if (dtFrom != null && dtTo != null)
                    {
                        sSQL += " where Convert(datetime, DrawerOpenedDate, 109) between cast('" + dtFrom.ToShortDateString() + " 00:00:00 ' as datetime) and cast('" + dtTo.ToShortDateString() + "  23:59:59 ' as datetime)";
                    }
                    if (!string.IsNullOrWhiteSpace(stationID) && stationID != "All")
                    {
                        if (sSQL.Contains("where"))
                            sSQL += " and StationID = '" + stationID + "'";
                        else
                            sSQL += " where StationID = '" + stationID + "'";
                    }
                    if (!string.IsNullOrWhiteSpace(userID) && userID != "All")
                    {
                        if (sSQL.Contains("where"))
                            sSQL += " and UserID = '" + userID + "'";
                        else
                            sSQL += " where UserID = '" + userID + "'";
                    }

                    sSQL += " order by Id desc";
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "GeNoSaleTransaction(DateTime dtFrom , DateTime dtTo ,  string stationID , string userID )");
                    //ErrorHandler.throwException(ex, "", "");
                    return null;
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        public DataSet GetStation()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = " select 'All' as StationID union select distinct StationID from NoSale order by StationID desc ";
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetStation()");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        public DataSet GetUser()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = " select 'All' as UserID union select distinct UserID  from NoSale ";
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetUser()");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        public void Dispose()
        {
        }

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(DataSet updates, IDbTransaction tx)
        {
            try
            {
                this.Insert(updates, tx);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(NoSaleTransactionData updates, IDbTransaction tx,System.Int32 TransID)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(NoSaleTransactionData updates, IDbTransaction tx,System.Int32 TransID)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(NoSaleTransactionData updates, IDbTransaction tx,System.Int32 TransID)");
                //POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region Insert Methods
        public void Insert(DataSet ds, IDbTransaction tx)
        {
            string sSQL;
            IDbDataParameter[] insParam;

            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (NoSaleTransactionRow row in ds.Tables[0].Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.NoSaleTableName, insParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(NoSaleTransactionData ds, IDbTransaction tx,System.Int32 TransID)");
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(NoSaleTransactionData ds, IDbTransaction tx,System.Int32 TransID)");
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(NoSaleTransactionData ds, IDbTransaction tx,System.Int32 TransID)");
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                ds.Tables[0].AcceptChanges();
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
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                if (delParam[i].SourceColumn.ToString() == "BinarySign")
                {
                    sInsertSQL = sInsertSQL + " , cast(" + delParam[i].ParameterName + " as varbinary(MAX))";
                }
                else
                {
                    sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
                }
            }
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }
        #endregion
        #region IDBDataParameter Generator Methods
        private IDbDataParameter[] InsertParameters(NoSaleTransactionRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.NoSaleTable_Fld_UserId, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.NoSaleTable_Fld_StationId, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.NoSaleTable_Fld_DrawerOpenedDate, System.Data.DbType.DateTime);
            #region
            sqlParams[0].SourceColumn = clsPOSDBConstants.NoSaleTable_Fld_UserId;
            sqlParams[1].SourceColumn = clsPOSDBConstants.NoSaleTable_Fld_StationId;
            sqlParams[2].SourceColumn = clsPOSDBConstants.NoSaleTable_Fld_DrawerOpenedDate;
            #endregion

            if (row.UserId != System.String.Empty)
                sqlParams[0].Value = row.UserId;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.StationId != System.String.Empty)
                sqlParams[1].Value = row.StationId;
            else
                sqlParams[1].Value = DBNull.Value;

            sqlParams[2].Value = row.DrawerOpenedDate;

            return (sqlParams);
        }
        #endregion

        public void SetNoSaleTransactionDetail(String UserId, String StationId)
        {
            IDbConnection conn = null;
            conn = DataFactory.CreateConnection(Resources.Configuration.ConnectionString);
            IDbTransaction otrans = null;
            otrans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
            try
            {
                ////NoSaleTransactionRow row = row.;
                string sSQL = string.Empty;
                //IDbDataParameter[] insParam = InsertParameters(row);
                //sSQL = BuildInsertSQL(clsPOSDBConstants.NoSaleTableName, insParam);
                ///*for(int i = 0; i< insParam.Length;i++)
                //{
                //    Console.WriteLine( insParam[i].ParameterName + "  " + insParam[i].Value);
                //}*/

                //DataHelper.ExecuteNonQuery(otrans, CommandType.Text, sSQL, insParam);
                //Add columns for ReversedAmount in the table

                sSQL = "Insert into NoSale(UserId , StationId, DrawerOpenedDate) values('" + UserId + "','" + StationId + "', GetDate())";

                //sSQL = sSQL = String.Format("Update " + clsPOSDBConstants.POSTransPayment_tbl + " SET "
                //    + clsPOSDBConstants.POSTransPayment_Fld_ReversedAmount +
                //     " = " +
                //     ds.Tables[0].Rows[count]["ReversedAmount"].ToString() +
                //     "  WHERE " +
                //     clsPOSDBConstants.POSTransPayment_Fld_TransPayID + " = " + ds.Tables[0].Rows[count]["TransPayID"].ToString());
                DataHelper.ExecuteNonQuery(otrans, CommandType.Text, sSQL);
                otrans.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "SetReversedAmountDetails( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "SetReversedAmountDetails( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "SetReversedAmountDetails( TransId, IDbConnection conn)");
                if (otrans != null)
                    otrans.Rollback();
                //ErrorHandler.throwException(ex, "", "");
            }
        }
    }
}
