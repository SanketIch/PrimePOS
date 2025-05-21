using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class CCCustomerTokInfoSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region "Persist Methods"
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(CCCustomerTokInfoData updates, IDbTransaction tx)
        {
            try
            {
                this.Insert(updates, tx);
                this.Update(updates, tx);

                updates.AcceptChanges();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(CCCustomerTokInfoData updates, IDbTransaction tx)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(CCCustomerTokInfoData updates, IDbTransaction tx)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(CCCustomerTokInfoData updates, IDbTransaction tx)");
                //POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }

        // Inserts, updates or deletes rows in a DataSet.
        public void Persist(CCCustomerTokInfoData updates)
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
                logger.Fatal(ex, "Persist(CCCustomerTokInfoData updates)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(CCCustomerTokInfoData updates)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(CCCustomerTokInfoData updates)");
                tx.Rollback();
                //ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

        #region "Insert, Update, and Delete Methods"
        public void Insert(CCCustomerTokInfoData ds, IDbTransaction tx)
        {
            CCCustomerTokInfoTable addedTable = new CCCustomerTokInfoTable();//(CCCustomerTokInfoTable)ds.Tables[0].GetChanges(DataRowState.Added);
            DataTable dt = ds.CCCustomerTokInfo.GetChanges(DataRowState.Added);
            if(dt!=null && dt.Rows.Count > 0)
            {
                addedTable.MergeTable(dt);
                string sSQL;
                IDbDataParameter[] insParam;

                if (addedTable != null && addedTable.Rows.Count > 0)
                {
                    foreach (CCCustomerTokInfoRow row in addedTable.Rows)
                    {
                        try
                        {
                            insParam = InsertParameters(row);
                            sSQL = BuildInsertSQL(clsPOSDBConstants.CCCustomerTokInfo_tbl, insParam);

                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        }
                        catch (POSExceptions ex)
                        {
                            logger.Fatal(ex, "Insert(CCCustomerTokInfoData ds, IDbTransaction tx)");
                            throw (ex);
                        }

                        catch (OtherExceptions ex)
                        {
                            logger.Fatal(ex, "Insert(CCCustomerTokInfoData ds, IDbTransaction tx)");
                            throw (ex);
                        }
                        catch (SqlException ex)
                        {
                            logger.Fatal(ex, "Insert(CCCustomerTokInfoData ds, IDbTransaction tx)");
                            throw (ex);
                        }

                        catch (Exception ex)
                        {
                            logger.Fatal(ex, "Insert(CCCustomerTokInfoData ds, IDbTransaction tx)");
                            //ErrorHandler.throwException(ex, "", "");
                        }
                    }
                    addedTable.AcceptChanges();
                }
            }
        }

        public void Update(CCCustomerTokInfoData ds, IDbTransaction tx)
        {
            // CCCustomerTokInfoTable modifiedTable = (CCCustomerTokInfoTable)ds.Tables[0].GetChanges(DataRowState.Modified);
            CCCustomerTokInfoTable modifiedTable = new CCCustomerTokInfoTable();
            DataTable dt = ds.CCCustomerTokInfo.GetChanges(DataRowState.Modified);
            //CCCustomerTokInfoTable addedTable = ds.CCCustomerTokInfo.get
            if (dt != null && dt.Rows.Count > 0)
            {
                modifiedTable.MergeTable(dt);
                string sSQL;
                IDbDataParameter[] updParam;

                if (modifiedTable != null && modifiedTable.Rows.Count > 0)
                {
                    foreach (CCCustomerTokInfoRow row in modifiedTable.Rows)
                    {
                        try
                        {
                            updParam = UpdateParameters(row);
                            sSQL = BuildUpdateSQL(clsPOSDBConstants.CCCustomerTokInfo_tbl, updParam);

                            DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                        }
                        catch (POSExceptions ex)
                        {
                            logger.Fatal(ex, "Update(CCCustomerTokInfoData ds, IDbTransaction tx)");
                            throw (ex);
                        }

                        catch (OtherExceptions ex)
                        {
                            logger.Fatal(ex, "Update(CCCustomerTokInfoData ds, IDbTransaction tx)");
                            throw (ex);
                        }
                        catch (SqlException ex)
                        {
                            logger.Fatal(ex, "Update(CCCustomerTokInfoData ds, IDbTransaction tx)");
                            throw (ex);
                        }
                        catch (Exception ex)
                        {
                            logger.Fatal(ex, "Update(CCCustomerTokInfoData ds, IDbTransaction tx)");
                            //ErrorHandler.throwException(ex, "", "");
                        }
                    }
                    modifiedTable.AcceptChanges();
                }
            }
        }

        private string BuildInsertSQL(string tablename, IDbDataParameter[] insParam)
        {
            string sInsertSQL = "INSERT INTO " + tablename + " ( ";

            sInsertSQL = sInsertSQL + insParam[0].SourceColumn;

            for (int i = 1; i < insParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + insParam[i].SourceColumn;
            }

            sInsertSQL = sInsertSQL + " ) VALUES ( " + insParam[0].ParameterName;
            for (int i = 1; i < insParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + insParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " )";

            return sInsertSQL;
        }

        private string BuildUpdateSQL(string tablename, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tablename + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[1].SourceColumn + "  = " + updParam[1].ParameterName;

            for (int i = 2; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }

            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            return sUpdateSQL;
        }
        #endregion

        #region "Get Methods"
        //public CCCustomerTokInfoData Populate(Int64 EntryID, IDbConnection conn)
        //{
        //    try
        //    {
        //        string sSQL = @"SELECT * FROM " +
        //                        clsPOSDBConstants.CCCustomerTokInfo_tbl +
        //                        " WHERE " + clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryID + " =" + EntryID;

        //        CCCustomerTokInfoData ds = new CCCustomerTokInfoData();
        //        ds.CCCustomerTokInfo.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(EntryID)).Tables[0]);

        //        return ds;
        //    }
        //    catch (POSExceptions ex)
        //    {
        //        logger.Fatal(ex, "Populate(Int64 EntryID,IDbConnection conn)");
        //        throw (ex);
        //    }
        //    catch (OtherExceptions ex)
        //    {
        //        logger.Fatal(ex, "Populate(Int64 EntryID,IDbConnection conn)");
        //        throw (ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "Populate(Int64 EntryID,IDbConnection conn)");
        //        //ErrorHandler.throwException(ex, "", "");
        //        return null;
        //    }
        //}

        //public CCCustomerTokInfoData Populate(Int64 EntryID)
        //{
        //    using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
        //    {
        //        return Populate(EntryID, conn);
        //    }
        //}

        //public CCCustomerTokInfoData PopulateList(string sWhereClause, IDbTransaction tx)
        //{
        //    try
        //    {
        //        string sSQL = String.Concat("Select * "
        //                                    + "From "
        //                                    + clsPOSDBConstants.CCCustomerTokInfo_tbl, sWhereClause);
        //        CCCustomerTokInfoData ds = new CCCustomerTokInfoData();
        //        ds.CCCustomerTokInfo.MergeTable(DataHelper.ExecuteDataset(tx, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
        //        return ds;
        //    }
        //    catch (POSExceptions ex)
        //    {
        //        logger.Fatal(ex, "PopulateList(string sWhereClause, IDbTransaction tx)");
        //        throw (ex);
        //    }
        //    catch (OtherExceptions ex)
        //    {
        //        logger.Fatal(ex, "PopulateList(string sWhereClause, IDbTransaction tx)");
        //        throw (ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Fatal(ex, "PopulateList(string sWhereClause, IDbTransaction tx)");
        //        //ErrorHandler.throwException(ex, "", "");
        //        return null;
        //    }
        //}

        public CCCustomerTokInfoData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select * "
                                            + "From "
                                            + clsPOSDBConstants.CCCustomerTokInfo_tbl, sWhereClause, " ORDER BY PreferenceId, EntryID");
                CCCustomerTokInfoData ds = new CCCustomerTokInfoData();
                ds.CCCustomerTokInfo.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause,IDbConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause,IDbConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause,IDbConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public CCCustomerTokInfoData PopulateList(string sWhereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return PopulateList(sWhereClause, conn);
            }
        }

        public CCCustomerTokInfoData GetTokenByCustomerID(int CustomerID)
        {
            string sWhere = " Where ISNULL(IsActive,1) = 1 AND " + clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID + " = " + CustomerID.ToString();

            return PopulateList(sWhere);
        }

        #region 30-Nov-2017 JY added to get tokens w.r.t. customer
        //public CCCustomerTokInfoData GetTokenByCustomerId(int CustomerID)
        //{
        //    using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
        //    {
        //        IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
        //        sqlParams[0] = DataFactory.CreateParameter();

        //        sqlParams[0].DbType = System.Data.DbType.Int32;
        //        sqlParams[0].Direction = ParameterDirection.Input;
        //        sqlParams[0].ParameterName = "@CustomerID";
        //        sqlParams[0].Value = CustomerID;

        //        CCCustomerTokInfoData oCCCustomerTokInfoData = new CCCustomerTokInfoData();
        //        oCCCustomerTokInfoData.CCCustomerTokInfo.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "POS_GetTokenByCustomerId", sqlParams).Tables[0]);
        //        return oCCCustomerTokInfoData;
        //    }
        //}
        #endregion

        public CCCustomerTokInfoData GetTokenByCustomerandProcessor(int CustomerID)
        {
            string sWhere = " Where ISNULL(IsActive,1) = 1 AND " + clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID + " = " + CustomerID.ToString();
            //if (Resources.Configuration.isPrimeRxPay == true)
            //{
            //    sWhere += " and Upper(rtrim(" + clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor + ")) = Upper(rtrim('" + "PRIMERXPAY" + "'))";
            //}
            //else
            //{
            if (Resources.Configuration.CSetting.OnlinePayment)//PRIMEPOS-2902
                sWhere += " and (Upper(rtrim(" + clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor + ")) = Upper(rtrim('" + POS_Core.Resources.Configuration.CPOSSet.PaymentProcessor + "'))" + " OR Upper(rtrim(" + clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor + ")) = Upper(rtrim('" + POS_Core.Resources.Configuration.CSetting.PayProviderName + "')) )";
            else
                sWhere += " and Upper(rtrim(" + clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor + ")) = Upper(rtrim('" + POS_Core.Resources.Configuration.CPOSSet.PaymentProcessor + "'))";
            //}
            return PopulateList(sWhere);
        }
        #endregion

        #region "IDBDataParameter Generator Methods"
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

        private IDbDataParameter[] PKParameters(Int64 EntryID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@EntryID";
            sqlParams[0].DbType = System.Data.DbType.Int64;
            sqlParams[0].Value = EntryID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(CCCustomerTokInfoRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@EntryID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.EntryID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(CCCustomerTokInfoRow row)
        {
            //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added token date
            //PRIMEPOS-2634 30-Jan-2019 JY Added CardAlias
            //PRIMEPOS-2635 31-Jan-2019 JY Added PreferenceId

            bool saveExpDate = false;
            int col = 10;//2990
            if (row.ExpDate > Convert.ToDateTime("1/1/1900"))
            {
                saveExpDate = true;
                col = 11;//2990
            }

            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(col);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID, DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_CardType, DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_Last4, DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID, DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor, DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryType, DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate, DbType.DateTime);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_CardAlias, DbType.String);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId, DbType.Int32);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_IsFsaCard, DbType.Boolean);//2990

            sqlParams[0].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_CardType;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_Last4;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor;
            sqlParams[5].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryType;
            sqlParams[6].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate;
            sqlParams[7].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_CardAlias;
            sqlParams[8].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId;
            sqlParams[9].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_IsFsaCard;//2990

            sqlParams[0].Value = row.CustomerID;
            sqlParams[1].Value = row.CardType;
            sqlParams[2].Value = row.Last4;
            sqlParams[3].Value = row.ProfiledID;
            sqlParams[4].Value = row.Processor;
            sqlParams[5].Value = row.EntryType;
            sqlParams[6].Value = row.TokenDate;

            try
            {
                sqlParams[7].Value = row.CardAlias;
            }
            catch
            {
                sqlParams[7].Value = "";
            }
            try
            {
                sqlParams[8].Value = row.PreferenceId;
            }
            catch
            {
                sqlParams[8].Value = DBNull.Value;
            }

            sqlParams[9].Value = row.IsFsaCard;//2990

            if (saveExpDate)
            {
                //2990 changed to 10 from 09
                sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate, DbType.DateTime);
                sqlParams[10].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate;
                sqlParams[10].Value = row.ExpDate;
            }
            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(CCCustomerTokInfoRow row)
        {
            //Sprint-23 - PRIMEPOS-2315 20-Jun-2016 JY Added token date
            //PRIMEPOS-2634 30-Jan-2019 JY Added CardAlias
            //PRIMEPOS-2635 31-Jan-2019 JY Added PreferenceId

            bool saveExpDate = false;
            int col = 11;//2990
            if (row.ExpDate > Convert.ToDateTime("1/1/1900"))
            {
                saveExpDate = true;
                col = 12;
            }

            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(col);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryID, DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID, DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_CardType, DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_Last4, DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID, DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor, DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryType, DbType.String);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate, DbType.DateTime);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_CardAlias, DbType.String);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId, DbType.Int32);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_IsFsaCard, DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_CustomerID;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_CardType;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_Last4;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_ProfiedID;
            sqlParams[5].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_Processor;
            sqlParams[6].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_EntryType;
            sqlParams[7].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_TokenDate;
            sqlParams[8].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_CardAlias;
            sqlParams[9].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_PreferenceId;
            sqlParams[10].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_IsFsaCard;

            sqlParams[0].Value = row.EntryID;
            sqlParams[1].Value = row.CustomerID;
            sqlParams[2].Value = row.CardType;
            sqlParams[3].Value = row.Last4;
            sqlParams[4].Value = row.ProfiledID;
            sqlParams[5].Value = row.Processor;
            sqlParams[6].Value = row.EntryType;
            sqlParams[7].Value = row.TokenDate;

            try
            {
                sqlParams[8].Value = row.CardAlias;
            }
            catch
            {
                sqlParams[8].Value = "";
            }
            try
            {
                sqlParams[9].Value = row.PreferenceId;
            }
            catch
            {
                sqlParams[9].Value = DBNull.Value;
            }

            sqlParams[10].Value = row.IsFsaCard;//2990

            if (saveExpDate)
            {
                //2990 changed to 11 from 10 
                sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate, DbType.DateTime);
                sqlParams[11].SourceColumn = clsPOSDBConstants.CCCustomerTokInfo__Fld_ExpDate;
                sqlParams[11].Value = row.ExpDate;
            }
            return (sqlParams);
        }
        #endregion

        #region Sprint-23 - PRIMEPOS-2316 15-Jun-2016 JY Added code to delete token
        public bool DeleteToken(int EntryID)
        {
            string sSQL;
            try
            {
                //sSQL = " DELETE FROM CCCustomerTokInfo WHERE EntryID = " + EntryID;
                sSQL = "UPDATE CCCustomerTokInfo SET IsActive = 0 WHERE EntryID = " + EntryID;  //PRIMEPOS-3004 19-Oct-2021 JY Added
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteToken(int EntryID)");
                return false;
            }
        }
        #endregion

        #region PRIMEPOS-3004 19-Oct-2021 JY Added
        public bool DeleteTokens(StringBuilder sbEntryID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
                {
                    string sSQL = "UPDATE CCCustomerTokInfo SET IsActive = 0 WHERE EntryID IN (" + sbEntryID + ")";
                    DataHelper.ExecuteNonQuery(conn, CommandType.Text, sSQL);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteTokens(StringBuilder sbEntryID)");
                return false;
            }
        }

        public bool DeleteExpiredTokens()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
                {
                    string sSQL = "UPDATE CCCustomerTokInfo SET IsActive = 0 WHERE (IsActive IS NULL OR IsActive = 1) AND ExpDate < GETDATE()";
                    DataHelper.ExecuteNonQuery(conn, CommandType.Text, sSQL);
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteExpiredTokens()");
                return false;
            }
        }
        #endregion

        #region "IDisposible Methods"
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
        #endregion

        #region PRIMEPOS-2611 13-Nov-2018 JY Added 
        public bool IsCustomerTokenExists(int CustomerID)
        {
            string sSQL;
            try
            {
                sSQL = "SELECT 1 FROM CCCustomerTokInfo WHERE CustomerID = " + CustomerID;
                object obj = DataHelper.ExecuteScalar(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return (Resources.Configuration.convertNullToBoolean(obj));
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "IsCustomerTokenExists(int CustomerID)");
                //ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        #endregion

        #region PRIMEPOS-2635 31-Jan-2019 JY Added
        public DataTable GetCardPreferences()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = "SELECT Id, PreferenceDesc FROM CardPreference ORDER BY Id";

                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds.Tables[0];
                }
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
                logger.Fatal(ex, "GetCardPreferences()");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region PRIMEPOS-3004 19-Oct-2021 JY Added
        public DataTable PopulateProcessors()
        {
            DataTable dt = new DataTable();
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = "SELECT DISTINCT UPPER(LTRIM(RTRIM(ISNULL(Processor,'')))) AS Processor FROM CCCustomerTokInfo ORDER BY Processor";

                    dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, sSQL);
                    return dt;
                }
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
                logger.Fatal(ex, "PopulateProcessors()");
                return null;
            }
        }

        public DataTable PopulateCardType()
        {
            DataTable dt = new DataTable();
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;

                    string sSQL = "SELECT DISTINCT UPPER(LTRIM(RTRIM(ISNULL(CardType,'')))) AS CardType FROM CCCustomerTokInfo ORDER BY CardType";

                    dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, sSQL);
                    return dt;
                }
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
                logger.Fatal(ex, "PopulateCardType()");
                return null;
            }
        }

        public DataTable GetCreditCardProfiles(int CustomerID, string strProcessor, string strCardType, int nStatus, string strLast4DigitsOfCC, string strTokenDateOption, DateTime TokenDate1, DateTime TokenDate2, string strCardExpDateOption, DateTime CardExpDate1, DateTime CardExpDate2)
        {
            logger.Trace("GetCreditCardProfiles() - " + clsPOSDBConstants.Log_Entering);

            string sSQL = string.Empty;
            DataTable dt = new DataTable();

            sSQL = "SELECT a.EntryID,a.CustomerID,b.CustomerName + ', ' + ISNULL(b.FIRSTNAME,'') AS CustName,a.CardAlias," + 
                "CASE WHEN ISNULL(a.PreferenceId, 0) = 1 THEN 'Preferred' WHEN ISNULL(a.PreferenceId,0) = 2 THEN 'Secondary' ELSE '' END AS Preference,"+
                "UPPER(LTRIM(RTRIM(ISNULL(a.CardType,'')))) AS CardType,a.Last4,a.ProfiledID,UPPER(LTRIM(RTRIM(ISNULL(a.Processor,'')))) AS Processor,a.EntryType,a.TokenDate,a.ExpDate,a.IsFsaCard," +
                "a.IsActive,CASE WHEN (a.IsActive IS NULL OR a.IsActive = 1) AND a.ExpDate < GETDATE() THEN 'Expired' WHEN (a.IsActive IS NULL OR a.IsActive = 1) AND ISNULL(a.ExpDate,GETDATE()) >= GETDATE() THEN 'Active' WHEN a.IsActive = 0 THEN 'InActive' END AS CardStatus" +
                " FROM CCCustomerTokInfo a INNER JOIN Customer b ON a.CustomerID = b.CustomerID WHERE 1 = 1 ";

            string strWhereClause = string.Empty;

            if (strTokenDateOption.Trim() != "" && strTokenDateOption.Trim().ToUpper() != "ALL")
            {
                if (strTokenDateOption == "NULL")
                {
                    strWhereClause = " AND a.TokenDate IS NULL";
                }
                else if (strTokenDateOption == "NOT NULL")
                {
                    strWhereClause = " AND a.TokenDate IS NOT NULL";
                }
                else if (strTokenDateOption == "=" || strTokenDateOption == ">" || strTokenDateOption == "<")
                {
                    strWhereClause = " AND CONVERT(Date,a.TokenDate) " + strTokenDateOption + " Convert(date, '" + TokenDate1 + "')";
                }
                else if (strTokenDateOption == "Between")
                {
                    strWhereClause = " AND CONVERT(Date,a.TokenDate) BETWEEN " + " Convert(date, '" + TokenDate1 + "')" + " AND Convert(date, '" + TokenDate2 + "')";
                }
            }

            if (strCardExpDateOption.Trim() != "" && strCardExpDateOption.Trim().ToUpper() != "ALL")
            {
                if (strCardExpDateOption == "NULL")
                {
                    strWhereClause += " AND a.ExpDate IS NULL";
                }
                else if (strCardExpDateOption == "NOT NULL")
                {
                    strWhereClause += " AND a.ExpDate IS NOT NULL";
                }
                else if (strCardExpDateOption == "=" || strCardExpDateOption == ">" || strCardExpDateOption == "<")
                {
                    strWhereClause += " AND CONVERT(Date,a.ExpDate) " + strCardExpDateOption + " Convert(date, '" + CardExpDate1 + "')";
                }
                else if (strCardExpDateOption == "Between")
                {
                    strWhereClause += " AND CONVERT(Date,a.ExpDate) BETWEEN " + " Convert(date, '" + CardExpDate1 + "')" + " AND Convert(date, '" + CardExpDate2 + "')";
                }
            }

            if (CustomerID > 0)
                strWhereClause += " AND a.CustomerID = " + CustomerID;

            if (strProcessor.ToUpper() != "ALL")
                strWhereClause += " AND a.Processor = '" + strProcessor.Replace("'", "''") + "'";

            if (strCardType.ToUpper() != "ALL")
                strWhereClause += " AND a.CardType = '" + strCardType.Replace("'", "''") + "'";

            if (nStatus != 0)
            {
                if (nStatus == 1)
                    strWhereClause += " AND (a.IsActive IS NULL OR a.IsActive = 1) AND a.ExpDate >= GETDATE()";
                else if (nStatus == 2)
                    strWhereClause += " AND a.IsActive = " + 0;
                else if (nStatus == 3)
                {
                    strWhereClause += " AND (a.IsActive IS NULL OR a.IsActive = 1) AND a.ExpDate < GETDATE()";
                }
            }

            if (strLast4DigitsOfCC != "")
                strWhereClause += " AND a.Last4 = '" + strLast4DigitsOfCC + "'";

            sSQL += strWhereClause;

            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, sSQL);
            }
            logger.Trace("GetCreditCardProfiles() - " + clsPOSDBConstants.Log_Exiting);
            return dt;
        }
        #endregion
    }
}