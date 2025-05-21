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
    

    // Provides data access methods for Customer
    public class CustomerSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public delegate void DataRowSavedHandler();
        public event DataRowSavedHandler DataRowSaved;

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
                ErrorLogging.ErrorHandler.throwException(ex, "", "");
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
                tx.Rollback();
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates)");
                tx.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        public bool DeleteRow(string CurrentID)
        {
            string sSQL;
            try
            {
                sSQL = " delete from Customer where CustomerID= '" + CurrentID + "'";
                DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(string CurrentID)");
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

        private void RaiseDataRowSaved()
        {
            if (DataRowSaved != null)
            {
                DataRowSaved();
            }
        }

        #region Get Methods
        // Looks up a Customer based on its primary-key:System.Int32 Customercode
        public virtual CustomerData Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn)
        {
            try
            {
                //ErrorLogging.Logs.Logger("Customer ", "  Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn)", " About to execute query to database for Customer: " + Key);
                logger.Trace("Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn) - " + clsPOSDBConstants.Log_Entering);
                string sSQL = "";
                if (SrhByName == false)
                {
                    Int32 iID = Configuration.convertNullToInt(Key);
                    //Updated By SRT(Gaurav) Date: 20/07/2009
                    //sSQL = "Select * FROM " + clsPOSDBConstants.Customer_tbl + " WHERE (" + clsPOSDBConstants.Customer_Fld_CustomerCode + " ='" + iID.ToString() + "' " 
                    //+ " Or " + clsPOSDBConstants.Customer_Fld_CustomerCode + "= NULL Or " + clsPOSDBConstants.Customer_Fld_AcctNumber + " =" + iID.ToString() + ") ";
                    //End Of Updated By SRT(Gaurav)

                    sSQL = "Select * FROM " + clsPOSDBConstants.Customer_tbl + " WHERE " + clsPOSDBConstants.Customer_Fld_AcctNumber + " =" + iID.ToString() + " ";
                }
                else
                    sSQL = "Select * FROM " + clsPOSDBConstants.Customer_tbl + " WHERE CustomerName + ', ' + FirstName Like '" + Key + "%'";

                if (isActive == true)
                {
                    sSQL += " And isActive=1 ";
                }

                CustomerData ds = new CustomerData();
                //ErrorLogging.Logs.Logger("Customer ", "  Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn)", " Executing query to database for Customer: " + Key);
                ds.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(Key)).Tables[0]);
                //ErrorLogging.Logs.Logger("Customer ", "  Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn)", "Completed Executing query to database for Customer: " + Key);
                logger.Trace("Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn) - " + clsPOSDBConstants.Log_Exiting);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn)");
                //ErrorLogging.Logs.Logger("Customer ", "  Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn)", 
                //    " Executing query to database for Customer: " + Key+"\t Exception ocured"+ex.StackTrace.ToString());
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.String Key, bool SrhByName, bool isActive, SqlConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public CustomerData Populate(System.String Key, bool SearchByName, bool isActive)
        {
            //ErrorLogging.Logs.Logger("Customer Server Class ", " Populate(System.String Customercode, bool isActive)", " About to Populate Customer: " + Key);
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(Key, SearchByName, isActive, conn));
            }
        }        

        public CustomerData GetCustomerByID(System.Int32 CustomerID)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string sSQL = "Select * FROM " + clsPOSDBConstants.Customer_tbl + " WHERE " + clsPOSDBConstants.Customer_Fld_CustomerId + " =" + CustomerID.ToString() + " ";

                CustomerData ds = new CustomerData();
                ds.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(CustomerID)).Tables[0]);
                return ds;
            }
        }       

        public CustomerData GetCustomerByPatientNo(System.Int32 PatientNo)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                string sSQL = "Select * FROM " + clsPOSDBConstants.Customer_tbl + " WHERE " + clsPOSDBConstants.Customer_Fld_PatientNo + " =" + PatientNo.ToString() + " ORDER BY CustomerID DESC ";

                CustomerData ds = new CustomerData();
                ds.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, PKParameters(PatientNo)).Tables[0]);
                return ds;
            }
        }

        #region 01-Dec-2017 JY added logic to get customer w.r.t. specific filter
        //public CustomerData GetCustomer(System.Int32? CustomerID, System.Int32? AcctNumber, System.Int32? PatientNo)
        //{
        //    using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
        //    {
        //        IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);
        //        sqlParams[0] = DataFactory.CreateParameter();
        //        sqlParams[0].DbType = System.Data.DbType.Int32;
        //        sqlParams[0].Direction = ParameterDirection.Input;
        //        sqlParams[0].ParameterName = "@CustomerID";
        //        sqlParams[0].Value = CustomerID;

        //        sqlParams[1] = DataFactory.CreateParameter();
        //        sqlParams[1].DbType = System.Data.DbType.Int32;
        //        sqlParams[1].Direction = ParameterDirection.Input;
        //        sqlParams[1].ParameterName = "@ACCTNUMBER";
        //        sqlParams[1].Value = AcctNumber;

        //        sqlParams[2] = DataFactory.CreateParameter();
        //        sqlParams[2].DbType = System.Data.DbType.Int32;
        //        sqlParams[2].Direction = ParameterDirection.Input;
        //        sqlParams[2].ParameterName = "@PatientNo";
        //        sqlParams[2].Value = PatientNo;

        //        CustomerData ds = new CustomerData();
        //        ds.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "POS_GetCustomer", sqlParams).Tables[0]);
        //        return ds;
        //    }
        //}
        #endregion

        // Fills a CustomerData with all Customer
        public CustomerData PopulateList(string sWhereClause, SqlConnection conn)
        {
            try
            {
                string sSQL = String.Concat("SELECT * FROM ", clsPOSDBConstants.Customer_tbl, sWhereClause);

                CustomerData ds = new CustomerData();
                ds.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, SqlConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, SqlConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string sWhereClause, SqlConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public CustomerData PopulateListByCotactNumber(string sContactNumber, SqlConnection conn)
        {
            try
            {
                //string sSQL = " Select Customer.* from Customer ,(Select Case IsNull(PrimaryContact,0) When 0 Then Isnull(CellNo,'')  "
                //         + " When 1 Then IsNull(PhoneOff,'') Else '' End as PrimaryContact,CustomerID "
                //         + " From Customer) as PContactTab Where PContactTab.CustomerID=Customer.CustomerID "
                //         + " And PContactTab.PrimaryContact='" + sContactNumber.Replace("'", "''") + "'";
                
                string sSQL = " Select TOP 1 Customer.* from Customer "
                         + " Where CellNo='" + sContactNumber.Replace("'", "''") + "' "
                         + " Or PhoneOff='" + sContactNumber.Replace("'", "''") + "' "
                         + "  Or PhoneHome='" + sContactNumber.Replace("'", "''") + "' ORDER BY CustomerID DESC";   //PRIMEPOS-2426 06-Jun-2019 JY Added logic to return recently added customer

                CustomerData ds = new CustomerData();
                ds.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL).Tables[0]);
                return ds;
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateListByCotactNumber(string sContactNumber, SqlConnection conn)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateListByCotactNumber(string sContactNumber, SqlConnection conn)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateListByCotactNumber(string sContactNumber, SqlConnection conn)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        // Fills a CustomerData with all Customer
        public CustomerData PopulateList(string whereClause)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(whereClause, conn));
            }
        }

        public CustomerData PopulateListByCotactNumber(string sContactNumber)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateListByCotactNumber(sContactNumber, conn));
            }
        }

        #region  PRIMEPOS-2598 12-Oct-2018 JY Added
        public bool IsCustomerAssociatedWithTransaction(string CustomerID)
        {
            string sSQL;
            try
            {
                sSQL = "SELECT 1 FROM POSTransaction PT INNER JOIN Customer C ON C.CustomerID = PT.CustomerID WHERE C.CustomerID = " + CustomerID;
                object obj = DataHelper.ExecuteScalar(DBConfig.ConnectionString, CommandType.Text, sSQL);
                return (Configuration.convertNullToBoolean(obj));
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "IsCustomerAssociatedWithTransaction(string CustomerID)");
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        #endregion
        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, SqlTransaction tx)
        {
            //DataTable addedTable = ds.Tables[clsPOSDBConstants.Customer_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            //if (addedTable != null && addedTable.Rows.Count > 0) 
            //{
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                try
                {
                    if (row.RowState == DataRowState.Added)
                    {
                        insParam = InsertParameters((CustomerRow)row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.Customer_tbl, insParam);
                        
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
                        System.Int32 CustomerID = Convert.ToInt32(DataHelper.ExecuteScalar(tx, CommandType.Text, "select @@identity"));

                        sSQL = "Update Customer Set AcctNumber=" + CustomerID.ToString() + " where CustomerID=" + CustomerID.ToString();

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
                        row["AcctNumber"] = CustomerID;
                        row["CustomerId"] = CustomerID;
                        row.AcceptChanges();
                        RaiseDataRowSaved();
                    }
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
                catch (SqlException ex)
                {
                    logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                    if (ex.Number == 2627)
                        ErrorHandler.throwCustomError(POSErrorENUM.Customer_DuplicateCode);
                    else
                        throw (ex);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");
                    ErrorHandler.throwException(ex, "", "");
                }
            }
            //addedTable.AcceptChanges();
            //}		
        }

        // Update all rows in a Customer DataSet, within a given database transaction.
        public void Update(DataSet ds, SqlTransaction tx)
        {
            CustomerTable modifiedTable = (CustomerTable)ds.Tables[clsPOSDBConstants.Customer_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (CustomerRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.Customer_tbl, updParam);

                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, updParam);
                        RaiseDataRowSaved();
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
                    catch (SqlException ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.Customer_DuplicateCode);
                        else
                            throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        #region Sprint-25 - PRIMEPOS-2371 22-Mar-2017 JY Added functionality to turn on the "Save Card Profile" flag
        public Int32 UpdateSaveCardProfileFlag(System.Int32 CustomerID)
        {
            int retVal = 0;
            string strSQL = "UPDATE Customer SET SaveCardProfile = 1 WHERE ISNULL(SaveCardProfile,0) <> 1 AND CustomerID = " + CustomerID;
            retVal = DataHelper.ExecuteNonQuery(strSQL);
            return retVal;
        }
        #endregion

        // Delete all rows within a Customer DataSet, within a given database transaction.
        public void Delete(DataSet ds, SqlTransaction tx)
        {
            CustomerTable table = (CustomerTable)ds.Tables[clsPOSDBConstants.Customer_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (CustomerRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.Customer_tbl, delParam);
                        DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, delParam);
                        RaiseDataRowSaved();
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
                        ErrorHandler.throwException(ex, "", "");
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
            string sInsertSQL = "Insert Into " + tableName + " ( ";
            // build where clause
            sInsertSQL = sInsertSQL + delParam[1].SourceColumn;

            for (int i = 2; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].SourceColumn;
            }
            sInsertSQL = sInsertSQL + " , UserId ";

            sInsertSQL = sInsertSQL + " ) Values (" + delParam[1].ParameterName;

            for (int i = 2; i < delParam.Length; i++)
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

        #region PRIMEPOS-2886 24-Aug-2020 JY Added
        public CustomerData GetExactCustomerByMultiplePatientsPatameters(DataSet dsPatient)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                try
                {
                    string LNAME = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["LNAME"].ToString()).Trim().Replace("'","''");
                    string FNAME = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["FNAME"].ToString()).Trim().Replace("'", "''");
                    DateTime oDOB;
                    DateTime.TryParse(Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["DOB"]).Trim(), out oDOB);

                    string PHONE = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["PHONE"]).Trim().Replace("'", "''");
                    string MOBILENO = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["MOBILENO"]).Trim().Replace("'", "''");
                    string WORKNO = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["WORKNO"]).Trim().Replace("'", "''");

                    #region level 1  - get exact match with FirstName, LastName, DOB, and one of the phone#
                    string strSQL = "SELECT * FROM Customer WHERE CustomerName = '" + LNAME + "' AND FIRSTNAME = '" + FNAME + "' AND CONVERT(DATE,DATEOFBIRTH) = CONVERT(DATE, '" + oDOB.Date + "')";

                    string strPhoneNo = "";
                    if (WORKNO != "")
                    {
                        if (strPhoneNo == "")
                        {
                            strPhoneNo = " CellNo = '" + WORKNO + "' OR PhoneOff = '" + WORKNO + "' OR PHONEHOME = '" + WORKNO + "'";
                        }
                        else
                        {
                            strPhoneNo += " OR CellNo = '" + WORKNO + "' OR PhoneOff = '" + WORKNO + "' OR PHONEHOME = '" + WORKNO + "'";
                        }
                    }
                    if (PHONE != "")
                    {
                        if (strPhoneNo == "")
                        {
                            strPhoneNo = " CellNo = '" + PHONE + "' OR PhoneOff = '" + PHONE + "' OR PHONEHOME = '" + PHONE + "'";
                        }
                        else
                        {
                            strPhoneNo += " OR CellNo = '" + PHONE + "' OR PhoneOff = '" + PHONE + "' OR PHONEHOME = '" + PHONE + "'";
                        }
                    }
                    if (MOBILENO != "")
                    {
                        if (strPhoneNo == "")
                        {
                            strPhoneNo = " CellNo = '" + MOBILENO + "' OR PhoneOff = '" + MOBILENO + "' OR PHONEHOME = '" + MOBILENO + "'";
                        }
                        else
                        {
                            strPhoneNo += " OR CellNo = '" + MOBILENO + "' OR PhoneOff = '" + MOBILENO + "' OR PHONEHOME = '" + MOBILENO + "'";
                        }
                    }

                    if (strPhoneNo != "")
                        strSQL += " AND (" + strPhoneNo + ")";

                    CustomerData oCustomerData = new CustomerData();
                    oCustomerData.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL).Tables[0]);
                    #endregion

                    //if (oCustomerData == null || oCustomerData.Tables.Count == 0 || oCustomerData.Tables[0].Rows.Count == 0)
                    //{
                    //    #region level 2
                    //    strSQL = "SELECT * FROM Customer WHERE CustomerName = '" + LNAME + "' AND FIRSTNAME = '" + FNAME + "' AND CONVERT(DATE,DATEOFBIRTH) = CONVERT(DATE, '" + oDOB.Date + "')";

                    //    oCustomerData = new CustomerData();
                    //    oCustomerData.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL).Tables[0]);
                    //    #endregion
                    //}
                    return oCustomerData;
                }
                catch (POSExceptions ex)
                {
                    logger.Fatal(ex, "GetExactCustomerByMultiplePatientsPatameters(DataSet dsPatient)");
                    throw (ex);
                }
                catch (OtherExceptions ex)
                {
                    logger.Fatal(ex, "GetExactCustomerByMultiplePatientsPatameters(DataSet dsPatient)");
                    throw (ex);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "GetExactCustomerByMultiplePatientsPatameters(DataSet dsPatient)");
                    ErrorHandler.throwException(ex, "", "");
                    return null;
                }
            }
        }

        public CustomerData GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                try
                {
                    //int PATIENTNO = Configuration.convertNullToInt(dsPatient.Tables[0].Rows[0]["PATIENTNO"]);
                    string LNAME = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["LNAME"].ToString()).Trim().Replace("'", "''");
                    string FNAME = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["FNAME"].ToString()).Trim().Replace("'", "''");
                    DateTime oDOB;
                    DateTime.TryParse(Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["DOB"]).Trim(), out oDOB);

                    string PHONE = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["PHONE"]).Trim().Replace("'", "''");
                    string MOBILENO = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["MOBILENO"]).Trim().Replace("'", "''");
                    string WORKNO = Configuration.convertNullToString(dsPatient.Tables[0].Rows[0]["WORKNO"]).Trim().Replace("'", "''");

                    #region level 1  - get exact match with FirstName, LastName, DOB, and one of the phone#
                    string strSQL = "SELECT * FROM Customer WHERE CustomerName = '" + LNAME + "' AND FIRSTNAME = '" + FNAME + "' AND CONVERT(DATE,DATEOFBIRTH) = CONVERT(DATE, '" + oDOB.Date + "')";

                    string strPhoneNo = "";
                    if (WORKNO != "")
                    {
                        if (strPhoneNo == "")
                        {
                            strPhoneNo = " CellNo = '" + WORKNO + "' OR PhoneOff = '" + WORKNO + "' OR PHONEHOME = '" + WORKNO + "'";
                        }
                        else
                        {
                            strPhoneNo += " OR CellNo = '" + WORKNO + "' OR PhoneOff = '" + WORKNO + "' OR PHONEHOME = '" + WORKNO + "'";
                        }
                    }
                    if (PHONE != "")
                    {
                        if (strPhoneNo == "")
                        {
                            strPhoneNo = " CellNo = '" + PHONE + "' OR PhoneOff = '" + PHONE + "' OR PHONEHOME = '" + PHONE + "'";
                        }
                        else
                        {
                            strPhoneNo += " OR CellNo = '" + PHONE + "' OR PhoneOff = '" + PHONE + "' OR PHONEHOME = '" + PHONE + "'";
                        }
                    }
                    if (MOBILENO != "")
                    {
                        if (strPhoneNo == "")
                        {
                            strPhoneNo = " CellNo = '" + MOBILENO + "' OR PhoneOff = '" + MOBILENO + "' OR PHONEHOME = '" + MOBILENO + "'";
                        }
                        else
                        {
                            strPhoneNo += " OR CellNo = '" + MOBILENO + "' OR PhoneOff = '" + MOBILENO + "' OR PHONEHOME = '" + MOBILENO + "'";
                        }
                    }

                    if (strPhoneNo != "")
                        strSQL += " AND (" + strPhoneNo + ")";
                    
                    CustomerData oCustomerData = new CustomerData();
                    oCustomerData.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL).Tables[0]);
                    LevelId = 1;
                    #endregion

                    if (oCustomerData == null || oCustomerData.Tables.Count == 0 || oCustomerData.Tables[0].Rows.Count == 0)
                    {
                        //#region level 2
                        //strSQL = "SELECT * FROM Customer WHERE CustomerName = '" + LNAME + "' AND FIRSTNAME = '" + FNAME + "' AND CONVERT(DATE,DATEOFBIRTH) = CONVERT(DATE, '" + oDOB.Date + "')";

                        //oCustomerData = new CustomerData();
                        //oCustomerData.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL).Tables[0]);
                        //LevelId = 2;
                        //#endregion
                        //if (oCustomerData == null || oCustomerData.Tables.Count == 0 || oCustomerData.Tables[0].Rows.Count == 0)
                        //{
                        #region level 3
                        // "DOB + Last Name", "DOB + FirstName", " WORKNO OR PHONE OR MOBILENO"
                        strSQL = "SELECT * FROM Customer WHERE CustomerName = '" + LNAME + "' AND CONVERT(DATE,DATEOFBIRTH) = CONVERT(DATE, '" + oDOB.Date + "')" +
                                " UNION SELECT * FROM Customer WHERE FIRSTNAME = '" + FNAME + "' AND CONVERT(DATE,DATEOFBIRTH) = CONVERT(DATE, '" + oDOB.Date + "')";

                        if (strPhoneNo != "")
                            strSQL += " UNION SELECT * FROM Customer WHERE " + strPhoneNo;

                        strSQL += " ORDER BY DATEOFBIRTH, CustomerName, FIRSTNAME";

                        oCustomerData = new CustomerData();
                        oCustomerData.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL).Tables[0]);
                        LevelId = 3;
                        #endregion

                        //if (oCustomerData == null || oCustomerData.Tables.Count == 0 || oCustomerData.Tables[0].Rows.Count == 0)
                        //{
                        //    #region level 4
                        //    //DOB OR Last Name OR FirstName
                        //    strSQL = "SELECT * FROM Customer WHERE CONVERT(DATE,DATEOFBIRTH) = CONVERT(DATE, '" + oDOB.Date + "') OR CustomerName = '" + LNAME + "' OR FIRSTNAME = '" + FNAME + "' ORDER BY DATEOFBIRTH, CustomerName, FIRSTNAME";
                        //    oCustomerData = new CustomerData();
                        //    oCustomerData.Customer.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL).Tables[0]);
                        //    LevelId = 4;
                        //    #endregion
                        //}
                        //}
                    }
                    else
                    {
                    }
                    return oCustomerData;
                }
                catch (POSExceptions ex)
                {
                    logger.Fatal(ex, "GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)");
                    throw (ex);
                }
                catch (OtherExceptions ex)
                {
                    logger.Fatal(ex, "GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)");
                    throw (ex);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)");
                    ErrorHandler.throwException(ex, "", "");
                    return null;
                }
            }
        }

        public DataTable GetCustomersForMapping(string strLastName, string strFirstName, object dtpDOB, string strPhoneNo, Boolean bIncludeLinkedCustomers)
        {
            using (SqlConnection conn = new SqlConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                try
                {
                    string strSQL = "SELECT CustomerName, FIRSTNAME, DATEOFBIRTH, PHONEHOME, PhoneOff, CellNo, PatientNo, CustomerID FROM Customer WHERE 1=1";
                    if (!bIncludeLinkedCustomers)
                    {
                        strSQL += " AND ISNULL(PatientNo,0) = 0";
                    }
                    if (strLastName != "")
                    {
                        strSQL += " AND CustomerName LIKE '" + strLastName.Replace("'", "''") + "%'";
                    }
                    if (strFirstName != "")
                    {
                        strSQL += " AND FIRSTNAME LIKE '" + strFirstName.Replace("'", "''") + "%'";
                    }

                    DateTime oDOB;
                    if (DateTime.TryParse(dtpDOB.ToString(), out oDOB) == true)
                    {
                        strSQL += " AND CONVERT(DATE,DATEOFBIRTH) = CONVERT(DATE, CAST('" + oDOB.Date + "' AS DATETIME))";
                    }

                    if (strPhoneNo != "")
                    {
                        strSQL += " AND (PHONEHOME LIKE '" + strPhoneNo.Replace("'", "''") + "%' OR PhoneOff LIKE '" +
                                    strPhoneNo.Replace("'", "''") + "%' OR CellNo LIKE '"
                                    + strPhoneNo.Replace("'", "''") + "%')";
                    }

                    DataTable dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL);
                    return dt;
                }
                catch (POSExceptions ex)
                {
                    logger.Fatal(ex, "GetCustomersForMapping(string strLastName, string strFirstName, DateTime dtpDOB, string strPhoneNo)");
                    throw (ex);
                }
                catch (OtherExceptions ex)
                {
                    logger.Fatal(ex, "GetCustomersForMapping(string strLastName, string strFirstName, DateTime dtpDOB, string strPhoneNo)");
                    throw (ex);
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "GetCustomersForMapping(string strLastName, string strFirstName, DateTime dtpDOB, string strPhoneNo)");
                    ErrorHandler.throwException(ex, "", "");
                    return null;
                }
            }
        }

        public Int32 UpdatePatientNo(int CustomerID, int PatientNo)
        {
            int retVal = 0;
            string strSQL = "UPDATE Customer SET PatientNo = " + PatientNo + " WHERE CustomerID = " + CustomerID;
            retVal = DataHelper.ExecuteNonQuery(strSQL);
            return retVal;
        }
        #endregion
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

        private IDbDataParameter[] PKParameters(System.Int32 CustomerID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@CustomerID";
            sqlParams[0].DbType = System.Data.DbType.Int32;
            sqlParams[0].Value = CustomerID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(System.String CustomerCode)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);
            
            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@CustomerCode";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = CustomerCode.Trim();

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(CustomerRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@Customerid";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.CustomerId.ToString().Trim();
            sqlParams[0].SourceColumn = clsPOSDBConstants.Customer_Fld_CustomerId;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(CustomerRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(27);    //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_CustomerId, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_CustomerName, System.Data.DbType.StringFixedLength);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Address1, System.Data.DbType.StringFixedLength);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Address2, System.Data.DbType.StringFixedLength);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_City, System.Data.DbType.StringFixedLength);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_State, System.Data.DbType.StringFixedLength);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Zip, System.Data.DbType.StringFixedLength);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_PhoneOffice, System.Data.DbType.StringFixedLength);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_FaxNo, System.Data.DbType.StringFixedLength);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_CellNo, System.Data.DbType.StringFixedLength);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_PhoneHome, System.Data.DbType.String);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Email, System.Data.DbType.String);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_PrimaryContact, System.Data.DbType.Int32);
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_DateOfBirth, System.Data.DbType.DateTime);
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Comments, System.Data.DbType.String);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Gender, System.Data.DbType.Int32);
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_UseForCustomerLoyalty, System.Data.DbType.Boolean);
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_CustomerCode, System.Data.DbType.String);
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_FirstName, System.Data.DbType.String);
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_DriveLicNo, System.Data.DbType.String);
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_DriveLicState, System.Data.DbType.String);
            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_HouseChargeAcctID, System.Data.DbType.Int32);
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_PatientNo, System.Data.DbType.Int32);
            sqlParams[24] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Discount, System.Data.DbType.Decimal);//Added By Shitaljit 0n 17 Feb 2012
            sqlParams[25] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_LanguageId, System.Data.DbType.Int32);//Added By Shitaljit 0n 10 Oct 2013
            sqlParams[26] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_SaveCardProfile, System.Data.DbType.Boolean);  //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.Customer_Fld_CustomerId;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Customer_Fld_CustomerName;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Customer_Fld_Address1;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Customer_Fld_Address2;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Customer_Fld_City;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Customer_Fld_State;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Customer_Fld_Zip;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Customer_Fld_PhoneOffice;
            sqlParams[8].SourceColumn = clsPOSDBConstants.Customer_Fld_FaxNo;
            sqlParams[9].SourceColumn = clsPOSDBConstants.Customer_Fld_CellNo;
            sqlParams[10].SourceColumn = clsPOSDBConstants.Customer_Fld_PhoneHome;
            sqlParams[11].SourceColumn = clsPOSDBConstants.Customer_Fld_Email;
            sqlParams[12].SourceColumn = clsPOSDBConstants.Customer_Fld_IsActive;
            sqlParams[13].SourceColumn = clsPOSDBConstants.Customer_Fld_PrimaryContact;
            sqlParams[14].SourceColumn = clsPOSDBConstants.Customer_Fld_DateOfBirth;
            sqlParams[15].SourceColumn = clsPOSDBConstants.Customer_Fld_Comments;
            sqlParams[16].SourceColumn = clsPOSDBConstants.Customer_Fld_Gender;
            sqlParams[17].SourceColumn = clsPOSDBConstants.Customer_Fld_UseForCustomerLoyalty;
            sqlParams[18].SourceColumn = clsPOSDBConstants.Customer_Fld_CustomerCode;
            sqlParams[19].SourceColumn = clsPOSDBConstants.Customer_Fld_FirstName;
            sqlParams[20].SourceColumn = clsPOSDBConstants.Customer_Fld_DriveLicNo;
            sqlParams[21].SourceColumn = clsPOSDBConstants.Customer_Fld_DriveLicState;
            sqlParams[22].SourceColumn = clsPOSDBConstants.Customer_Fld_HouseChargeAcctID;
            sqlParams[23].SourceColumn = clsPOSDBConstants.Customer_Fld_PatientNo;
            sqlParams[24].SourceColumn = clsPOSDBConstants.Customer_Fld_Discount;//Added By shitaljit 0n 17 Feb 2012
            sqlParams[25].SourceColumn = clsPOSDBConstants.Customer_Fld_LanguageId;//Added By Shitaljit 0n 10 Oct 2013
            sqlParams[26].SourceColumn = clsPOSDBConstants.Customer_Fld_SaveCardProfile;    //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added

            if (row.CustomerId != System.Int32.MinValue)
                sqlParams[0].Value = row.CustomerId;
            else
                sqlParams[0].Value = DBNull.Value;

            sqlParams[1].Value = row.CustomerName.Trim();
            sqlParams[2].Value = row.Address1.Trim();
            sqlParams[3].Value = row.Address2.Trim();
            sqlParams[4].Value = row.City.Trim();
            sqlParams[5].Value = row.State.Trim();
            sqlParams[6].Value = row.Zip.Trim();
            sqlParams[7].Value = row.PhoneOffice.Trim();
            sqlParams[8].Value = row.FaxNo.Trim();
            sqlParams[9].Value = row.CellNo.Trim();
            sqlParams[10].Value = row.PhoneHome.Trim();
            sqlParams[11].Value = row.Email.Trim();
            sqlParams[12].Value = row.IsActive;

            if (row.PrimaryContact != System.Int32.MinValue)
                sqlParams[13].Value = row.PrimaryContact;
            else
                sqlParams[13].Value = 0;

            if (row.DateOfBirth != DBNull.Value)
                sqlParams[14].Value = row.DateOfBirth;
            else
                sqlParams[14].Value = DBNull.Value;

            if (row.Comments != System.String.Empty)
                sqlParams[15].Value = row.Comments.Trim();
            else
                sqlParams[15].Value = DBNull.Value;

            if (row.Gender != System.Int32.MinValue)
                sqlParams[16].Value = row.Gender;
            else
                sqlParams[16].Value = 0;

            sqlParams[17].Value = row.UseForCustomerLoyalty;

            sqlParams[18].Value = row.CustomerCode;
            sqlParams[19].Value = row.FirstName;

            sqlParams[20].Value = row.DriveLicNo;
            sqlParams[21].Value = row.DriveLicState;

            if (row.HouseChargeAcctID != System.Int32.MinValue)
                sqlParams[22].Value = row.HouseChargeAcctID;
            else
                sqlParams[22].Value = DBNull.Value;

            if (row.PatientNo>= System.Int32.MinValue)
                sqlParams[23].Value = row.PatientNo;
            else
                sqlParams[23].Value = DBNull.Value;
            
            //Added By Shitaljit 0n 17 Feb 2012
            if (row.Discount >= System.Decimal.MinValue)
                sqlParams[24].Value = row.Discount;
            else
                sqlParams[24].Value = DBNull.Value;
            sqlParams[25].Value = row.LanguageId;
            sqlParams[26].Value = row.SaveCardProfile;  //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added

            return (sqlParams);
        }
        
        private IDbDataParameter[] UpdateParameters(CustomerRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(28);    //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_CustomerId, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Gender, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_CustomerName, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Address1, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Address2, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_City, System.Data.DbType.StringFixedLength);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_State, System.Data.DbType.StringFixedLength);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Zip, System.Data.DbType.StringFixedLength);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_PhoneOffice, System.Data.DbType.StringFixedLength);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_FaxNo, System.Data.DbType.StringFixedLength);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_CellNo, System.Data.DbType.StringFixedLength);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_PhoneHome, System.Data.DbType.StringFixedLength);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Email, System.Data.DbType.String);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_PrimaryContact, System.Data.DbType.Int32);
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_AcctNumber, System.Data.DbType.Int32);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_DateOfBirth, System.Data.DbType.Date);
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Comments, System.Data.DbType.String);

            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_UseForCustomerLoyalty, System.Data.DbType.Boolean);
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_CustomerCode, System.Data.DbType.String);
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_FirstName, System.Data.DbType.String);
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_DriveLicNo, System.Data.DbType.String);
            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_DriveLicState, System.Data.DbType.String);
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_HouseChargeAcctID, System.Data.DbType.Int32);
            sqlParams[24] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_PatientNo, System.Data.DbType.Int32);
            sqlParams[25] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_Discount, System.Data.DbType.Decimal);//Added By Shitaljit 0n 17 Feb 2012
            sqlParams[26] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_LanguageId, System.Data.DbType.Int32);//Added By Shitaljit 0n 10 Oct 2013
            sqlParams[27] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Customer_Fld_SaveCardProfile, System.Data.DbType.Boolean);  //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.Customer_Fld_CustomerId;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Customer_Fld_Gender;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Customer_Fld_CustomerName;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Customer_Fld_Address1;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Customer_Fld_Address2;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Customer_Fld_City;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Customer_Fld_State;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Customer_Fld_Zip;
            sqlParams[8].SourceColumn = clsPOSDBConstants.Customer_Fld_PhoneOffice;
            sqlParams[9].SourceColumn = clsPOSDBConstants.Customer_Fld_FaxNo;
            sqlParams[10].SourceColumn = clsPOSDBConstants.Customer_Fld_CellNo;
            sqlParams[11].SourceColumn = clsPOSDBConstants.Customer_Fld_PhoneHome;
            sqlParams[12].SourceColumn = clsPOSDBConstants.Customer_Fld_Email;
            sqlParams[13].SourceColumn = clsPOSDBConstants.Customer_Fld_IsActive;
            sqlParams[14].SourceColumn = clsPOSDBConstants.Customer_Fld_PrimaryContact;
            sqlParams[15].SourceColumn = clsPOSDBConstants.Customer_Fld_AcctNumber;
            sqlParams[16].SourceColumn = clsPOSDBConstants.Customer_Fld_DateOfBirth;
            sqlParams[17].SourceColumn = clsPOSDBConstants.Customer_Fld_Comments;
            sqlParams[18].SourceColumn = clsPOSDBConstants.Customer_Fld_UseForCustomerLoyalty;
            sqlParams[19].SourceColumn = clsPOSDBConstants.Customer_Fld_CustomerCode;
            sqlParams[20].SourceColumn = clsPOSDBConstants.Customer_Fld_FirstName;
            sqlParams[21].SourceColumn = clsPOSDBConstants.Customer_Fld_DriveLicNo;
            sqlParams[22].SourceColumn = clsPOSDBConstants.Customer_Fld_DriveLicState;
            sqlParams[23].SourceColumn = clsPOSDBConstants.Customer_Fld_HouseChargeAcctID;
            sqlParams[24].SourceColumn = clsPOSDBConstants.Customer_Fld_PatientNo;
            sqlParams[25].SourceColumn = clsPOSDBConstants.Customer_Fld_Discount;//Added By shitaljit 0n 17 Feb 2012
            sqlParams[26].SourceColumn = clsPOSDBConstants.Customer_Fld_LanguageId;//Added By Shitaljit 0n 10 Oct 2013
            sqlParams[27].SourceColumn = clsPOSDBConstants.Customer_Fld_SaveCardProfile;    //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added

            if (row.CustomerId != System.Int32.MinValue)
                sqlParams[0].Value = row.CustomerId;
            else
                sqlParams[0].Value = DBNull.Value;


            if (row.Gender != System.Int32.MinValue)
                sqlParams[1].Value = row.Gender;
            else
                sqlParams[1].Value = 0;

            sqlParams[2].Value = row.CustomerName.Trim();
            sqlParams[3].Value = row.Address1.Trim();
            sqlParams[4].Value = row.Address2.Trim();
            sqlParams[5].Value = row.City.Trim();
            sqlParams[6].Value = row.State.Trim();
            sqlParams[7].Value = row.Zip.Trim();
            sqlParams[8].Value = row.PhoneOffice.Trim();
            sqlParams[9].Value = row.FaxNo.Trim();
            sqlParams[10].Value = row.CellNo.Trim();
            sqlParams[11].Value = row.PhoneHome.Trim();
            sqlParams[12].Value = row.Email.Trim();
            sqlParams[13].Value = row.IsActive;

            if (row.PrimaryContact != System.Int32.MinValue)
                sqlParams[14].Value = row.PrimaryContact;
            else
                sqlParams[14].Value = 0;

            if (row.AccountNumber != System.Int32.MinValue)
                sqlParams[15].Value = row.AccountNumber;
            else
                sqlParams[15].Value = row.CustomerId;

            if (row.DateOfBirth != DBNull.Value)
                sqlParams[16].Value = row.DateOfBirth;
            else
                sqlParams[16].Value = DBNull.Value;

            sqlParams[17].Value = row.Comments;

            sqlParams[18].Value = row.UseForCustomerLoyalty;

            sqlParams[19].Value = row.CustomerCode;

            sqlParams[20].Value = row.FirstName;

            sqlParams[21].Value = row.DriveLicNo;
            sqlParams[22].Value = row.DriveLicState;

            if (row.HouseChargeAcctID != System.Int32.MinValue)
                sqlParams[23].Value = row.HouseChargeAcctID;
            else
                sqlParams[23].Value = DBNull.Value;

            if (row.PatientNo >= System.Int32.MinValue)
                sqlParams[24].Value = row.PatientNo;
            else
                sqlParams[24].Value = DBNull.Value;

            //Added By Shitaljit 0n 17 Feb 2012
            if (row.Discount >= System.Decimal.MinValue)
                sqlParams[25].Value = row.Discount;
            else
                sqlParams[25].Value = DBNull.Value;
            sqlParams[26].Value = row.LanguageId;
            sqlParams[27].Value = row.SaveCardProfile;  //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added

            return (sqlParams);
        }
        #endregion      

        public void Dispose() { }
    }
}
