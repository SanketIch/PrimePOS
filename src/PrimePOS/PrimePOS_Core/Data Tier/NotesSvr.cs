
// ----------------------------------------------------------------
//Added By Shitaljit(QuicSolv) 0n 5 oct 2011
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
using POS_Core.Resources;
//using POS.Resources;
using NLog;

namespace POS_Core.DataAccess
{
    public class NotesSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist
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
                //POS_Core.ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx)
        {

            NotesTable addedTable = (NotesTable)ds.Tables[clsPOSDBConstants.Notes_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (NotesRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.Notes_tbl, insParam);
                        for (int i = 0; i < insParam.Length; i++)
                        {
                            Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);

                        }
                        DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, sSQL, insParam);
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.Notes_CodeCanNotBeNULL);
                        else
                            throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a Notes DataSet, within a given database transaction.

        public void Update(DataSet ds, IDbTransaction tx)
        {
            NotesTable modifiedTable = (NotesTable)ds.Tables[clsPOSDBConstants.Notes_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (NotesRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.Notes_tbl, updParam);
                        DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, sSQL, updParam);
                    }
                    catch (POSExceptions ex)
                    {
                        throw (ex);
                    }

                    catch (OtherExceptions ex)
                    {
                        throw (ex);
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.Notes_DuplicateCode);
                        else
                            throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        // Delete all rows within a Notes DataSet, within a given database transaction.
        public void Delete(DataSet ds, IDbTransaction tx)
        {

            NotesTable table = (NotesTable)ds.Tables[clsPOSDBConstants.Notes_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (NotesRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.Notes_tbl, delParam);
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
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
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
            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            return sUpdateSQL;
        }

        public bool DeleteRow(System.Int32 CurrentID)
        {

            string sSQL;
            try
            {
                sSQL = "DELETE FROM Notes where NoteId= '" + CurrentID + "'";
                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(System.Int32 CurrentID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        #endregion

        #endregion

        #region Get Methods
        public NotesData PopulateList(string whereClause)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            return (PopulateList(whereClause, conn));
        }
        public DataSet PopulateList(string _Table, string whereClause)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            return (PopulateList(_Table, whereClause, conn));
        }

        public NotesData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("SELECT * FROM ", clsPOSDBConstants.Notes_tbl, sWhereClause);

                NotesData ds = new NotesData();
                ds.Notes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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

        public DataSet PopulateList(string _Table, string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = "";
                if (_Table == clsPOSDBConstants.Department_tbl)
                {
                    sSQL = "SELECT " + clsPOSDBConstants.Department_tbl + "." + clsPOSDBConstants.Department_Fld_DeptCode + " AS Code"
                        + " , " + clsPOSDBConstants.Department_tbl + "." + clsPOSDBConstants.Department_Fld_DeptName + "  AS Name "
                        + " , " + clsPOSDBConstants.Notes_tbl
                        + ".*  FROM " + clsPOSDBConstants.Notes_tbl + "," + clsPOSDBConstants.Department_tbl
                        + "  WHERE  " + clsPOSDBConstants.Notes_tbl + "." + clsPOSDBConstants.Notes_Fld_EntityId + " = CAST(" + clsPOSDBConstants.Department_tbl + "." + clsPOSDBConstants.Department_Fld_DeptID + " AS VARCHAR(20))";  //Sprint-21 - 2234 08-Sep-2015 JY Added cast to resolve the error

                }
                if (_Table == clsPOSDBConstants.Vendor_tbl)
                {
                    sSQL = "SELECT " + clsPOSDBConstants.Vendor_tbl + "." + clsPOSDBConstants.Vendor_Fld_VendorCode + " AS Code"
                        + " , " + clsPOSDBConstants.Vendor_tbl + "." + clsPOSDBConstants.Vendor_Fld_VendorName + "  AS Name "
                        + " , " + clsPOSDBConstants.Notes_tbl
                        + ".*  FROM " + clsPOSDBConstants.Notes_tbl + "," + clsPOSDBConstants.Vendor_tbl
                        + "  WHERE  " + clsPOSDBConstants.Notes_tbl + "." + clsPOSDBConstants.Notes_Fld_EntityId + " = CAST(" + clsPOSDBConstants.Vendor_tbl + "." + clsPOSDBConstants.Vendor_Fld_VendorId + " AS VARCHAR(20))";    //Sprint-21 - 2234 08-Sep-2015 JY Added cast to resolve the error

                }
                if (_Table == clsPOSDBConstants.Item_tbl)
                {
                    sSQL = "SELECT " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID + " AS Code"
                        + " , " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_Description + "  AS Name "
                        + " , " + clsPOSDBConstants.Notes_tbl
                        + ".*  FROM " + clsPOSDBConstants.Notes_tbl + "," + clsPOSDBConstants.Item_tbl
                        + "  WHERE  " + clsPOSDBConstants.Notes_tbl + "." + clsPOSDBConstants.Notes_Fld_EntityId + " = " + clsPOSDBConstants.Item_tbl + "." + clsPOSDBConstants.Item_Fld_ItemID;

                }
                if (_Table == clsPOSDBConstants.Users_tbl)
                {
                    sSQL = "SELECT " + clsPOSDBConstants.Users_tbl + "." + clsPOSDBConstants.Users_Fld_UserID + " AS Code"
                        + " , " + clsPOSDBConstants.Users_tbl + "." + clsPOSDBConstants.Users_Fld_fName + "  AS Name "
                        + " , " + clsPOSDBConstants.Notes_tbl
                        + ".*  FROM " + clsPOSDBConstants.Notes_tbl + "," + clsPOSDBConstants.Users_tbl
                        + "  WHERE  " + clsPOSDBConstants.Notes_tbl + "." + clsPOSDBConstants.Notes_Fld_EntityId + " = " + clsPOSDBConstants.Users_tbl + "." + clsPOSDBConstants.Users_Fld_UserID;

                }
                if (_Table == "")
                {
                    sSQL = "SELECT *  FROM " + clsPOSDBConstants.Notes_tbl;
                }

                if (sWhereClause != "" && _Table != "")
                {
                    sSQL += sWhereClause;
                }
                DataSet ds = new DataSet();
                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
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
                logger.Fatal(ex, "PopulateList(string _Table, string sWhereClause, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                       //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public NotesData Populate(int NoteID)
        {
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            return (Populate(NoteID, conn));
        }

        public NotesData Populate(int NoteID, IDbConnection conn)
        {
            try
            {
                string sSQL = "SELECT * FROM Notes WHERE NoteId = " + NoteID;

                NotesData ds = new NotesData();
                ds.Notes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
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
                logger.Fatal(ex, "Populate(int NoteID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
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

        private IDbDataParameter[] PKParameters(System.String NotesId)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@NotesId";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = NotesId;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(NotesRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@NotesId";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.NoteId;
            sqlParams[0].SourceColumn = clsPOSDBConstants.Notes_Fld_NoteId;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(NotesRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(8);

            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_EntityId, System.Data.SqlDbType.VarChar);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_EntityType, System.Data.SqlDbType.VarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_Note, System.Data.SqlDbType.NVarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_CreatedDate, System.Data.SqlDbType.DateTime);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_CreatedBy, System.Data.SqlDbType.NVarChar);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_UpdatedDate, System.Data.SqlDbType.DateTime);
            sqlParams[6] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_UpdatedBy, System.Data.SqlDbType.NVarChar);
            sqlParams[7] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_POPUPMSG, System.Data.SqlDbType.Bit);


            sqlParams[0].SourceColumn = clsPOSDBConstants.Notes_Fld_EntityId;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Notes_Fld_EntityType;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Notes_Fld_Note;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Notes_Fld_CreatedDate;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Notes_Fld_CreatedBy;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Notes_Fld_UpdatedDate;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Notes_Fld_UpdatedBy;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Notes_Fld_POPUPMSG;

            //if (row.NoteId != 0)
            //    sqlParams[0].Value = row.NoteId;
            //else
            //    sqlParams[0].Value = DBNull.Value;


            if (row.EntityId != System.String.Empty)
                sqlParams[0].Value = row.EntityId;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.EntityType != System.String.Empty)
                sqlParams[1].Value = row.EntityType;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Note != System.String.Empty)
                sqlParams[2].Value = row.Note;
            else
                sqlParams[2].Value = DBNull.Value;


            if (row.CreatedDate != System.DateTime.MinValue)
                sqlParams[3].Value = row.CreatedDate;
            else
                sqlParams[3].Value = DBNull.Value;


            if (row.CreatedBy != System.String.Empty)
                sqlParams[4].Value = row.CreatedBy;
            else
                sqlParams[4].Value = DBNull.Value;


            if (row.UpdatedDate != System.String.Empty)
                sqlParams[5].Value = row.UpdatedDate;
            else
                sqlParams[5].Value = DBNull.Value;


            if (row.UpdatedBy != System.String.Empty)
                sqlParams[6].Value = row.UpdatedBy;
            else
                sqlParams[6].Value = DBNull.Value;

            sqlParams[7].Value = row.POPUPMSG;

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(NotesRow row)
        {

            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(6);
            sqlParams[0] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_NoteId, System.Data.SqlDbType.BigInt);
            sqlParams[1] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_EntityId, System.Data.SqlDbType.VarChar);
            sqlParams[2] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_Note, System.Data.SqlDbType.NVarChar);
            sqlParams[3] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_UpdatedDate, System.Data.SqlDbType.DateTime);
            sqlParams[4] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_UpdatedBy, System.Data.SqlDbType.NVarChar);
            sqlParams[5] = new SqlParameter("@" + clsPOSDBConstants.Notes_Fld_POPUPMSG, System.Data.SqlDbType.Bit);

            sqlParams[0].SourceColumn = clsPOSDBConstants.Notes_Fld_NoteId;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Notes_Fld_EntityId;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Notes_Fld_Note;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Notes_Fld_UpdatedDate;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Notes_Fld_UpdatedBy;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Notes_Fld_POPUPMSG;


            sqlParams[0].Value = row.NoteId;


            if (row.EntityId != System.String.Empty)
                sqlParams[1].Value = row.EntityId;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Note != System.String.Empty)
                sqlParams[2].Value = row.Note;
            else
                sqlParams[2].Value = DBNull.Value;

            sqlParams[3].Value = System.DateTime.Now;

            sqlParams[4].Value = Configuration.UserName;


            sqlParams[5].Value = row.POPUPMSG;


            return (sqlParams);
        }

        #endregion

        public void Dispose() { }
    }
}
