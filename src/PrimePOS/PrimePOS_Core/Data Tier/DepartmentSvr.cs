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
using POS_Core.Resources;
//using POS.Resources;
using NLog;

namespace POS_Core.DataAccess
{


    // Provides data access methods for DeptCode

    public class DepartmentSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

        public void Persist(DataSet updates, IDbTransaction tx, ref System.Int32 DeptID)   //Sprint-22 20-Oct-2015 JY Added DeptID
        {
            try

            {
                this.Delete(updates, tx);
                this.Insert(updates, tx, ref DeptID);   //Sprint-22 20-Oct-2015 JY Added DeptID
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
                logger.Fatal(ex, "Persist(DataSet updates, IDbTransaction tx, ref System.Int32 DeptID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }


        // Inserts, updates or deletes rows in a DataSet.

        public void Persist(DataSet updates, ref System.Int32 DeptID)  //Sprint-22 20-Oct-2015 JY Added DeptID
        {

            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = conn.BeginTransaction();
                this.Persist(updates, tx, ref DeptID);  //Sprint-22 20-Oct-2015 JY Added DeptID
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
                logger.Fatal(ex, "Persist(DataSet updates, ref System.Int32 DeptID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }

        }

        public bool DeleteRow(string CurrentID)
        {
            string sSQL;
            try
            {
                DataTable dt = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "SELECT TOP 1 DepartmentID FROM Item WHERE DepartmentID = '" + CurrentID + "'").Tables[0];
                if (dt != null && dt.Rows.Count == 0)
                {
                    dt = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "SELECT TOP 1 DepartmentID FROM SubDepartment WHERE DepartmentID = '" + CurrentID + "'").Tables[0];   //PRIMEPOS-2937 25-Jan-2021 JY Added
                    if (dt != null && dt.Rows.Count == 0)
                    {
                        sSQL = " delete from Department where DeptID= '" + CurrentID + "'";
                        DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                        #region Sprint-22 20-Oct-2015 JY Added logic to delete the record from ItemTax table
                        try
                        {
                            sSQL = "DELETE FROM ItemTax WHERE EntityType = 'D' AND EntityID = '" + CurrentID + "'";
                            DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                        }
                        catch
                        { }
                        #endregion
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(string CurrentID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        #endregion

        #region Get Methods

        // Looks up a DeptCode based on its primary-key:System.Int32 DeptCode

        public DepartmentData Populate(System.String DeptCode, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                                    + clsPOSDBConstants.Department_Fld_DeptID
                                    + " , " + clsPOSDBConstants.Department_Fld_DeptCode
                                    + " , " + clsPOSDBConstants.Department_Fld_DeptName
                                    + " , " + clsPOSDBConstants.Department_Fld_Discount
                                    + " , " + clsPOSDBConstants.Department_Fld_SaleDiscount
                                    + " , " + clsPOSDBConstants.Department_Fld_IsTaxable
                                    + " , " + clsPOSDBConstants.Department_Fld_SaleStartDate
                                    + " , " + clsPOSDBConstants.Department_Fld_SaleEndDate
                                    + " , dept." + clsPOSDBConstants.Department_Fld_TaxID + " as " + clsPOSDBConstants.Department_Fld_TaxID
                                    + " , " + clsPOSDBConstants.Department_Fld_SalePrice
                                    + " , taxcodes." + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " as " + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                                    + " , " + clsPOSDBConstants.TaxCodes_Fld_Description
                                    + " , dept." + clsPOSDBConstants.Department_Fld_UserID + " as " + clsPOSDBConstants.Department_Fld_UserID
                                    + " , dept." + clsPOSDBConstants.Department_Fld_PointsPerDollar + " as " + clsPOSDBConstants.Department_Fld_PointsPerDollar //Sprint-18 - 2041 26-Oct-2014 JY added
                                 + " FROM "
                                //AddedBy shitaljit FOR JIRA- 938"*=", "=*" operators for LEFT OUTER JOIN and RIGHT OUTER JOIN is no longer supported by SQL Server 2012.
                                + clsPOSDBConstants.Department_tbl + " As Dept "
                                + "  LEFT OUTER JOIN " + clsPOSDBConstants.TaxCodes_tbl + " As TaxCodes ON "
                                + " Dept." + clsPOSDBConstants.Department_Fld_TaxID + " = TaxCodes." + clsPOSDBConstants.TaxCodes_Fld_TaxID
                                + " WHERE "
                                 //+ " Dept." + clsPOSDBConstants.Department_Fld_TaxID + " *= TaxCodes." + clsPOSDBConstants.TaxCodes_Fld_TaxID
                                 + clsPOSDBConstants.Department_Fld_DeptCode + " ='" + DeptCode + "'";

                DepartmentData ds = new DepartmentData();
                ds.Department.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                            , sSQL
                                            , PKParameters(DeptCode)).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.String DeptCode, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DepartmentData Populate(System.Int32 DeptId)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(DeptId, conn));
            }
        }

        public DepartmentData Populate(System.Int32 DeptId, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                    + clsPOSDBConstants.Department_Fld_DeptID
                    + " , " + clsPOSDBConstants.Department_Fld_DeptCode
                    + " , " + clsPOSDBConstants.Department_Fld_DeptName
                    + " , " + clsPOSDBConstants.Department_Fld_Discount
                    + " , " + clsPOSDBConstants.Department_Fld_SaleDiscount
                    + " , " + clsPOSDBConstants.Department_Fld_IsTaxable
                    + " , " + clsPOSDBConstants.Department_Fld_SaleStartDate
                    + " , " + clsPOSDBConstants.Department_Fld_SaleEndDate
                    + " , dept." + clsPOSDBConstants.Department_Fld_TaxID + " as " + clsPOSDBConstants.Department_Fld_TaxID
                    + " , " + clsPOSDBConstants.Department_Fld_SalePrice
                    + " , taxcodes." + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " as " + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                    + " , " + clsPOSDBConstants.TaxCodes_Fld_Description
                    + " , dept." + clsPOSDBConstants.Department_Fld_UserID + " as " + clsPOSDBConstants.Department_Fld_UserID
                     + " , dept." + clsPOSDBConstants.Department_Fld_PointsPerDollar + " as " + clsPOSDBConstants.Department_Fld_PointsPerDollar //Sprint-18 - 2041 26-Oct-2014 JY added
                      + " FROM "
                    //AddedBy shitaljit FOR JIRA- 938"*=", "=*" operators for LEFT OUTER JOIN and RIGHT OUTER JOIN is no longer supported by SQL Server 2012.
                    + clsPOSDBConstants.Department_tbl + " As Dept "
                    + "  LEFT OUTER JOIN " + clsPOSDBConstants.TaxCodes_tbl + " As TaxCodes ON "
                    + " Dept." + clsPOSDBConstants.Department_Fld_TaxID + " = TaxCodes." + clsPOSDBConstants.TaxCodes_Fld_TaxID
                    + " WHERE "
                    //+ " Dept." + clsPOSDBConstants.Department_Fld_TaxID + " *= TaxCodes." + clsPOSDBConstants.TaxCodes_Fld_TaxID
                    + clsPOSDBConstants.Department_Fld_DeptID + " =" + DeptId;


                DepartmentData ds = new DepartmentData();
                ds.Department.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                    , sSQL
                    , PKParameters(DeptId.ToString())).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.Int32 DeptId, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        // Fills a DepartmentData with all DeptCode

        public DepartmentData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                if (sWhereClause.Trim().StartsWith("where"))
                {
                    sWhereClause = " and " + sWhereClause.Trim().Substring(5);
                }

                string sSQL = String.Concat("Select "
                                    + clsPOSDBConstants.Department_Fld_DeptID
                                    + " , " + clsPOSDBConstants.Department_Fld_DeptCode
                                    + " , " + clsPOSDBConstants.Department_Fld_DeptName
                                    + " , " + clsPOSDBConstants.Department_Fld_Discount
                                    + " , " + clsPOSDBConstants.Department_Fld_SaleDiscount
                                    + " , " + clsPOSDBConstants.Department_Fld_IsTaxable
                                    + " , " + clsPOSDBConstants.Department_Fld_SaleStartDate
                                    + " , " + clsPOSDBConstants.Department_Fld_SaleEndDate
                                    + " , dept." + clsPOSDBConstants.Department_Fld_TaxID + " as " + clsPOSDBConstants.Department_Fld_TaxID
                                    + " , " + clsPOSDBConstants.Department_Fld_SalePrice
                                    + " , taxcodes." + clsPOSDBConstants.TaxCodes_Fld_TaxCode + " as " + clsPOSDBConstants.TaxCodes_Fld_TaxCode
                                    + " , " + clsPOSDBConstants.TaxCodes_Fld_Description
                                    + " , dept." + clsPOSDBConstants.Department_Fld_UserID + " as " + clsPOSDBConstants.Department_Fld_UserID
                                    + " , dept." + clsPOSDBConstants.Department_Fld_PointsPerDollar + " as " + clsPOSDBConstants.Department_Fld_PointsPerDollar //Sprint-18 - 2041 26-Oct-2014 JY added
                                   + " FROM "
                                    //AddedBy shitaljit FOR JIRA- 938"*=", "=*" operators for LEFT OUTER JOIN and RIGHT OUTER JOIN is no longer supported by SQL Server 2012.
                                    + clsPOSDBConstants.Department_tbl + " As Dept "
                                    + "  LEFT OUTER JOIN " + clsPOSDBConstants.TaxCodes_tbl + " As TaxCodes ON "
                                    + " Dept." + clsPOSDBConstants.Department_Fld_TaxID + " = TaxCodes." + clsPOSDBConstants.TaxCodes_Fld_TaxID
                                    + " WHERE 1=1 "
                                    //AddedBy shitaljit FOR JIRA- 938"*=", "=*" operators for LEFT OUTER JOIN and RIGHT OUTER JOIN is no longer supported by SQL Server 2012.
                                    //+ " Dept." + clsPOSDBConstants.Department_Fld_TaxID + " *= TaxCodes." + clsPOSDBConstants.TaxCodes_Fld_TaxID
                                    , sWhereClause);

                DepartmentData ds = new DepartmentData();
                ds.Department.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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

        public DepartmentData Populate(System.String DeptCode)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(DeptCode, conn));
            }
        }

        // Fills a DepartmentData with all DeptCode

        // Fills a DepartmentData with all DeptCode

        public DepartmentData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(whereClause, conn));
            }
        }
        // Fills the DataSet with All the DaptName and DeptCode
        public DataSet PopulateList()
        {
            return PopulateListWithIdName(string.Empty);
        }

        // Fills the DataSet with All the DaptName and DeptCode
        public DataSet PopulateListWithIdName(string condition)
        {
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                DataSet ds = new DataSet();
                string sSQL = "";
                sSQL = "SELECT  "
                                    + clsPOSDBConstants.Department_Fld_DeptID
                                    + " , " + clsPOSDBConstants.Department_Fld_DeptCode
                                    + " , " + clsPOSDBConstants.Department_Fld_DeptName
                                    + " , " + clsPOSDBConstants.Department_Fld_EXCLUDEFROMCLCouponPay
                                    + " FROM "
                                    + clsPOSDBConstants.Department_tbl;
                if (!String.IsNullOrEmpty(condition))
                {
                    sSQL += condition;
                }

                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateListWithIdName(string condition)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx, ref System.Int32 DeptID)   //Sprint-22 20-Oct-2015 JY Added DeptID
        {

            DepartmentTable addedTable = (DepartmentTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (DepartmentRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.Department_tbl, insParam);
                        for (int i = 0; i < insParam.Length; i++)
                        {
                            Console.WriteLine(insParam[i].ParameterName + "  " + insParam[i].Value);
                        }
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        DeptID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));  //Sprint-22 20-Oct-2015 JY Added
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
                            ErrorHandler.throwCustomError(POSErrorENUM.Department_DuplicateCode);
                        else
                            throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, IDbTransaction tx, ref System.Int32 DeptID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a DeptCodes DataSet, within a given database transaction.

        public void Update(DataSet ds, IDbTransaction tx)
        {
            DepartmentTable modifiedTable = (DepartmentTable)ds.Tables[0].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (DepartmentRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.Department_tbl, updParam);

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
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.Department_DuplicateCode);
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

        // Delete all rows within a DeptCodes DataSet, within a given database transaction.
        public void Delete(DataSet ds, IDbTransaction tx)
        {

            DepartmentTable table = (DepartmentTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (DepartmentRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);
                        sSQL = BuildDeleteSQL(clsPOSDBConstants.Department_tbl, delParam);
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
        private IDbDataParameter[] PKParameters(System.String DeptCode)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@DeptCode";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = DeptCode;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(DepartmentRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@DeptCode";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.DeptCode;
            sqlParams[0].SourceColumn = clsPOSDBConstants.Department_Fld_DeptCode;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(DepartmentRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(10);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_DeptCode, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_DeptName, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_Discount, System.Data.DbType.Decimal);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_IsTaxable, System.Data.DbType.Boolean);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_SaleEndDate, System.Data.DbType.DateTime);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_SalePrice, System.Data.DbType.Currency);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_SaleStartDate, System.Data.DbType.DateTime);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_TaxID, System.Data.DbType.Int32);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_SaleDiscount, System.Data.DbType.Decimal);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_PointsPerDollar, System.Data.DbType.Int32);   //Sprint-18 - 2041 26-Oct-2014 JY  Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.Department_Fld_DeptCode;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Department_Fld_DeptName;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Department_Fld_Discount;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Department_Fld_IsTaxable;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Department_Fld_SaleEndDate;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Department_Fld_SalePrice;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Department_Fld_SaleStartDate;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Department_Fld_TaxID;
            sqlParams[8].SourceColumn = clsPOSDBConstants.Department_Fld_SaleDiscount;
            sqlParams[9].SourceColumn = clsPOSDBConstants.Department_Fld_PointsPerDollar;   //Sprint-18 - 2041 26-Oct-2014 JY  Added

            if (row.DeptCode != System.String.Empty)
                sqlParams[0].Value = row.DeptCode;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.DeptName != System.String.Empty)
                sqlParams[1].Value = row.DeptName;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Discount != System.Int32.MinValue)
                sqlParams[2].Value = row.Discount;
            else
                sqlParams[2].Value = DBNull.Value;

            sqlParams[3].Value = row.IsTaxable;

            if (row.SaleEndDate != System.DateTime.MinValue)
                sqlParams[4].Value = row.SaleEndDate;
            else
                sqlParams[4].Value = System.DateTime.MinValue;

            if (row.SalePrice != 0)
                sqlParams[5].Value = row.SalePrice;
            else
                sqlParams[5].Value = 0;

            if (row.SaleStartDate != System.DateTime.MinValue)
                sqlParams[6].Value = row.SaleStartDate;
            else
                sqlParams[6].Value = System.DateTime.MinValue;

            if (row.TaxId != 0)
                sqlParams[7].Value = row.TaxId;
            else
                sqlParams[7].Value = 0;

            if (row.SaleDiscount != System.Int32.MinValue)
                sqlParams[8].Value = row.Discount;
            else
                sqlParams[8].Value = DBNull.Value;

            //Sprint-18 - 2041 26-Oct-2014 JY  Added
            if (row.PointsPerDollar != System.Int32.MinValue)
                sqlParams[9].Value = row.PointsPerDollar;
            else
                sqlParams[9].Value = DBNull.Value;


            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(DepartmentRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(11);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_DeptID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_DeptCode, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_DeptName, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_Discount, System.Data.DbType.Decimal);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_IsTaxable, System.Data.DbType.Boolean);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_SaleEndDate, System.Data.DbType.DateTime);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_SalePrice, System.Data.DbType.Currency);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_SaleStartDate, System.Data.DbType.DateTime);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_TaxID, System.Data.DbType.Int32);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_SaleDiscount, System.Data.DbType.Decimal);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Department_Fld_PointsPerDollar, System.Data.DbType.Int32);

            sqlParams[0].SourceColumn = clsPOSDBConstants.Department_Fld_DeptID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Department_Fld_DeptCode;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Department_Fld_DeptName;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Department_Fld_Discount;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Department_Fld_IsTaxable;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Department_Fld_SaleEndDate;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Department_Fld_SalePrice;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Department_Fld_SaleStartDate;
            sqlParams[8].SourceColumn = clsPOSDBConstants.Department_Fld_TaxID;
            sqlParams[9].SourceColumn = clsPOSDBConstants.Department_Fld_SaleDiscount;
            sqlParams[10].SourceColumn = clsPOSDBConstants.Department_Fld_PointsPerDollar;

            if (row.DeptID != 0)
                sqlParams[0].Value = row.DeptID;
            else
                sqlParams[0].Value = 0;

            if (row.DeptCode != System.String.Empty)
                sqlParams[1].Value = row.DeptCode;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.DeptName != System.String.Empty)
                sqlParams[2].Value = row.DeptName;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.Discount != 0)
                sqlParams[3].Value = row.Discount;
            else
                sqlParams[3].Value = 0;

            sqlParams[4].Value = row.IsTaxable;

            if (row.SaleEndDate != System.DateTime.MinValue)
                sqlParams[5].Value = row.SaleEndDate;
            else
                sqlParams[5].Value = System.DateTime.MinValue;

            if (row.SalePrice != 0)
                sqlParams[6].Value = row.SalePrice;
            else
                sqlParams[6].Value = 0;

            if (row.SaleStartDate != System.DateTime.MinValue)
                sqlParams[7].Value = row.SaleStartDate;
            else
                sqlParams[7].Value = System.DateTime.MinValue;

            if (row.TaxId != 0)
                sqlParams[8].Value = row.TaxId;
            else
                sqlParams[8].Value = 0;

            if (row.SaleDiscount != 0)
                sqlParams[9].Value = row.SaleDiscount;
            else
                sqlParams[9].Value = 0;

            if (row.PointsPerDollar != 0)
                sqlParams[10].Value = row.PointsPerDollar;
            else
                sqlParams[10].Value = 0;

            return (sqlParams);
        }
        #endregion

        #region PRIMEPOS-2500 02-Apr-2018 JY Added logic to check taxable department
        public bool IsTaxable(int DepartmentID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection())
            {
                conn.ConnectionString = DBConfig.ConnectionString;
                string sSQL = "select IsTaxable from Department WHERE DeptID =" + DepartmentID;
                object objValue = DataHelper.ExecuteScalar(conn, CommandType.Text, sSQL);
                conn.Close();
                if (Configuration.convertNullToBoolean(objValue) == false)
                    return false;
                else
                    return true;
            }
        }
        #endregion

        public void Dispose() { }
    }
}
