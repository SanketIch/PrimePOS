using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
//using POS.Resources;
using Resources;
using NLog;
using POS_Core.Resources;

namespace POS_Core.DataAccess
{
    class ColorSchemeForViewPOSTransSvr : IDisposable 
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

        public void Persist(DataSet updates, SqlTransaction tx)
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
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");
                //ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }


        // Inserts, updates or deletes rows in a DataSet.

        public void Persist(DataSet updates)
        {

            SqlTransaction tx;
            SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString);

            conn.Open();
            tx = conn.BeginTransaction();
            try
            {
                this.Persist(updates, tx);
                tx.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                //ErrorHandler.throwException(ex, "", "");
            }

        }
        #endregion

        #region Get Methods

        

        public ColorSchemeForViewPOSTransData Populate(System.Int32 Id, SqlConnection conn)
        {
            try
            {
                ColorSchemeForViewPOSTransData ds = new ColorSchemeForViewPOSTransData();
                ds.ColorSchemeForViewPOSTrans.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select * FROM "+clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl + " WHERE " + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID + " ='" + Id + "'", PKParameters(Id)).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 Id, SqlConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 Id, SqlConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.Int32 Id, SqlConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public ColorSchemeForViewPOSTransData Populate(System.Int32 Id)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(Id, conn));
            }
        }

        // Fills a ColorSchemeForViewPOSTransData with all AmountRange

        public ColorSchemeForViewPOSTransData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select " + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID + " , " + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount +  "," +clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount + " , " + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor + " , " + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor + " FROM " + clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl, sWhereClause);

                ColorSchemeForViewPOSTransData ds = new ColorSchemeForViewPOSTransData();
                ds.ColorSchemeForViewPOSTrans.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn)");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        // Fills a ColorSchemeForViewPOSTransData with all AmountRange

        public ColorSchemeForViewPOSTransData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                return (PopulateList(whereClause, conn));
            }
        }

        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, SqlTransaction tx)
        {

            ColorSchemeForViewPOSTransTable addedTable = (ColorSchemeForViewPOSTransTable)ds.Tables[clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (ColorSchemeForViewPOSTransRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl, insParam);
                        for (int i = 0; i < insParam.Length; i++)
                        {
                            Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
                        }
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);


                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a ColorSchemeForViewPOSTrans DataSet, within a given database transaction.

        public void Update(DataSet ds, SqlTransaction tx)
        {
            ColorSchemeForViewPOSTransTable modifiedTable = (ColorSchemeForViewPOSTransTable)ds.Tables[clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (ColorSchemeForViewPOSTransRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        // Delete all rows within a ColorSchemeForViewPOSTrans DataSet, within a given database transaction.
        public void Delete(DataSet ds, SqlTransaction tx)
        {

            ColorSchemeForViewPOSTransTable table = (ColorSchemeForViewPOSTransTable)ds.Tables[clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (ColorSchemeForViewPOSTransRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl, delParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, delParam);
                    }
                    catch (POSExceptions ex)
                    {
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");
                        //ErrorHandler.throwException(ex, "", "");
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
            sInsertSQL = sInsertSQL + " , UserId ";
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " , '" + Configuration.UserName + "'";
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

            sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'";

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
        private IDbDataParameter[] PKParameters(System.Int32 ID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int64;
            sqlParams[0].Value = ID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(ColorSchemeForViewPOSTransRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int64;

            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(ColorSchemeForViewPOSTransRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(4);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount, System.Data.DbType.Decimal);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount, System.Data.DbType.Decimal);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor, System.Data.DbType.String);

            sqlParams[0].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor;

            if (row.FromAmount >= 0)
                sqlParams[0].Value = row.FromAmount;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.ToAmount >= 0)
                sqlParams[1].Value = row.ToAmount;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.BackColor != System.String.Empty)
                sqlParams[2].Value = row.BackColor;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.ForeColor != System.String.Empty)
                sqlParams[3].Value = row.ForeColor;
            else
                sqlParams[3].Value = DBNull.Value;

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(ColorSchemeForViewPOSTransRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(5);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount, System.Data.DbType.Decimal);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount, System.Data.DbType.Decimal);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor, System.Data.DbType.String);


            sqlParams[0].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_FromAmount;
            sqlParams[2].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ToAmount;
            sqlParams[3].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_BackColor;
            sqlParams[4].SourceColumn = clsPOSDBConstants.ColorSchemeForViewPOSTrans_Fld_ForeColor;

            if (row.ID != 0)
                sqlParams[0].Value = row.ID;
            else
                sqlParams[0].Value = 0;

            if (row.FromAmount >= 0)
                sqlParams[1].Value = row.FromAmount;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.ToAmount >= 0)
                sqlParams[2].Value = row.ToAmount;
            else
                sqlParams[2].Value = DBNull.Value;


            if (row.BackColor != System.String.Empty)
                sqlParams[3].Value = row.BackColor;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.ForeColor != System.String.Empty)
                sqlParams[4].Value = row.ForeColor;
            else
                sqlParams[4].Value = DBNull.Value;

            return (sqlParams);
        }
        #endregion


        public void Dispose() { }    
    }
}
