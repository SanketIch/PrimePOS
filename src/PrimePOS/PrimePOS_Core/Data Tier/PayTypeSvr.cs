using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using System.Data.SqlClient;
using POS_Core.Resources.DelegateHandler;
using POS_Core.Resources;
//using POS.Resources;

namespace POS_Core.DataAccess
{


    // Provides data access methods for DeptCode

    public class PayTypeSvr : IDisposable
    {
        #region Persist Methods

        // Inserts, updates or deletes rows in a PayTypeData, within a database transaction.

        public void Persist(PayTypeData updates, SqlTransaction tx)
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
                ErrorLogging.ErrorHandler.throwException(ex, "", "");
            }
        }


        // Inserts, updates or deletes rows in a PayTypeData.

        public void Persist(PayTypeData updates)
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
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                tx.Rollback();
                ErrorHandler.throwException(ex, "", "");
            }

        }
        public bool DeleteRow(string PaytypeID)
        {
            string sSQL;
            try
            {
                DataTable dtTransPayment = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "Select * from POSTransPayment where TransTypeCode = '" + PaytypeID + "'").Tables[0];
                if (dtTransPayment.Rows.Count == 0)
                {
                    sSQL = " DELETE FROM PayType WHERE PayTypeID = '" + PaytypeID + "'";
                    DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                    #region PRIMEPOS-2847 20-May-2020 JY Added
                    try
                    {
                        sSQL = "DELETE FROM Util_PayTypeReceipts WHERE PayTypeID = '" + PaytypeID + "'";
                        DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                    }
                    catch { }
                    #endregion
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

        #endregion

        #region Get Methods
        public string GetNextPayTypeID()
        {
            string retVal = string.Empty;
            string sAlreadyDefineID = string.Empty;
            try
            {
                PayTypeData oPayTypeData = PopulateList(" PayTypeID NOT IN('1','2','3','4','5','6','7','B','C','E','H')");
                Random _random = new Random();
                sAlreadyDefineID = "'B','C','E','H','S','K','A','V','M','D','T','U','R'";

                bool isValidID = false;
                if (oPayTypeData == null)
                {
                    throw new Exception("Payment Options are not set , Please contact MMS Support for Resolution.");
                }
                #region PRIMEPOS-2940 09-Mar-2021 JY Commented
                //else if(oPayTypeData.PayTypes.Rows.Count == 26)
                //{
                //    clsCoreUIHelper.ShowWarningMsg("Reached maximum no. of payment types \"26\", you cannot add more payment types.", "Add Payment Types");
                //    return string.Empty;
                //}
                #endregion

                foreach (PayTypeRow oRow in oPayTypeData.PayTypes.Rows)
                {
                    sAlreadyDefineID += ",'" + oRow.PaytypeID.Trim() + "'";
                }

                #region PRIMEPOS-2940 09-Mar-2021 JY Added
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        string strPayTypeID = string.Empty;
                        if (i == 1)
                            strPayTypeID = "D" + j.ToString();
                        else if (i == 2)
                            strPayTypeID = "E" + j.ToString();
                        else if (i == 3)
                            strPayTypeID = "F" + j.ToString();

                        if (sAlreadyDefineID.Contains("'" + strPayTypeID + "'") == false)
                        {
                            retVal = Configuration.convertNullToString(strPayTypeID);
                            isValidID = true;
                            break;
                        }
                    }
                    if (isValidID == true)
                        break;
                }

                if (retVal == string.Empty)
                {
                    clsCoreUIHelper.ShowWarningMsg("Reached maximum no. of payment types, you cannot add more payment types.", "Add Payment Types");
                }
                #endregion

                #region PRIMEPOS-2940 09-Mar-2021 JY Commented
                // This method returns a random uppercase letter.
                // Between 'A' and 'Z' inclusize.
                //while (isValidID == false)
                //{
                //    int iLetNo = _random.Next(65, 90);
                //    char let = (char)(iLetNo);
                //    if (sAlreadyDefineID.Contains("'" + let + "'") == false)
                //    {
                //        retVal = Configuration.convertNullToString(let);
                //        isValidID = true;
                //    }
                //}
                #endregion
            }
            catch (Exception Ex)
            {
                throw (Ex);
            }
            return retVal;
        }

        public PayTypeData Populate(System.String PayTypeID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                                    + clsPOSDBConstants.PayType_Fld_PayTypeID
                                    + " , " + clsPOSDBConstants.PayType_Fld_PayTypeDescription
                                    + " , " + clsPOSDBConstants.PayType_Fld_PayType
                                    + ", ISNULL(" + clsPOSDBConstants.PayType_Fld_IsHide + ", 0) as IsHide"  //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
                                    + ", ISNULL(" + clsPOSDBConstants.PayType_Fld_StopAtRefNo + ", 0) as StopAtRefNo"    //PRIMEPOS-2309 08-Mar-2019 JY Added
                                    + ", ISNULL(" + clsPOSDBConstants.PayType_Fld_SortOrder + ", 0) as SortOrder" //PRIMEPOS-2966 20-May-2021 JY Added
                                + " FROM "
                                    + clsPOSDBConstants.PayType_tbl
                                + " WHERE "
                                    + clsPOSDBConstants.PayType_Fld_PayTypeID + " = '" + PayTypeID + "'";


                PayTypeData ds = new PayTypeData();
                ds.PayTypes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public PayTypeData PopulateList(string sWhereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(sWhereClause, conn));
            }
        }

        public PayTypeData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select "
                                    + clsPOSDBConstants.PayType_Fld_PayTypeID
                                    + " , " + clsPOSDBConstants.PayType_Fld_PayTypeDescription + " as " + clsPOSDBConstants.PayType_Fld_PayTypeDescription  //+ clsPOSDBConstants.PayType_Fld_PayTypeID + " + ' - ' + "
                                    + " , " + clsPOSDBConstants.PayType_Fld_PayType
                                    + ", ISNULL(" + clsPOSDBConstants.PayType_Fld_IsHide + ", 0) as IsHide"  //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
                                    + ", ISNULL(" + clsPOSDBConstants.PayType_Fld_StopAtRefNo + ", 0) as StopAtRefNo"    //PRIMEPOS-2309 08-Mar-2019 JY Added
                                    + ", ISNULL(" + clsPOSDBConstants.PayType_Fld_SortOrder + ", 0) as SortOrder" //PRIMEPOS-2966 20-May-2021 JY Added
                                    + " FROM "
                                    + clsPOSDBConstants.PayType_tbl;
                if (sWhereClause.Trim() != "")
                    sWhereClause = " where " + sWhereClause;
                sSQL = sSQL + sWhereClause;
                //sSQL += " ORDER BY " + clsPOSDBConstants.PayType_Fld_CustomPayType + ", " + clsPOSDBConstants.PayType_Fld_PayTypeID;    //PRIMEPOS-2940 30-Mar-2021 JY Added  //PRIMEPOS-2966 20-May-2021 JY Commented
                sSQL += " ORDER BY " + clsPOSDBConstants.PayType_Fld_SortOrder + ", " + clsPOSDBConstants.PayType_Fld_CustomPayType + ", " + clsPOSDBConstants.PayType_Fld_PayTypeID;   //PRIMEPOS-2966 20-May-2021 JY Added
                PayTypeData ds = new PayTypeData();
                ds.PayTypes.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
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
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        //PRIMEPOS-2966 20-May-2021 JY Added
        public System.Int32 GetNextSortOrder()
        {
            try
            {
                string strSQL = "SELECT IsNull(Max(SortOrder),0) + 1 FROM PayType";
                object obj = DataHelper.ExecuteScalar(DBConfig.ConnectionString, CommandType.Text, strSQL);
                return (Configuration.convertNullToInt(obj));
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }

        public System.Int32 GetCreditCardSortOrder()
        {
            try
            {
                string strSQL = "SELECT ISNULL(MIN(SortOrder),3) FROM PayType WHERE PayTypeID IN ('3','4','5','6')";
                object obj = DataHelper.ExecuteScalar(DBConfig.ConnectionString, CommandType.Text, strSQL);
                return (Configuration.convertNullToInt(obj));
            }
            catch (Exception Ex)
            {
                return 0;
            }
        }
        #endregion //Get Method

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

        #endregion

        #region Insert, Update, and Delete Methods
        public void Insert(PayTypeData ds, SqlTransaction tx)
        {

            PayTypeTable addedTable = (PayTypeTable)ds.Tables[clsPOSDBConstants.PayType_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (PayTypeRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.PayType_tbl, insParam);
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
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a TaxCodes PayTypeData, within a given database transaction.

        public void Update(PayTypeData ds, SqlTransaction tx)
        {
            PayTypeTable modifiedTable = (PayTypeTable)ds.Tables[clsPOSDBConstants.PayType_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (PayTypeRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.PayType_tbl, updParam);

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
                        ErrorHandler.throwException(ex, "", "");
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        // Delete all rows within a TaxCodes PayTypeData, within a given database transaction.
        public void Delete(PayTypeData ds, SqlTransaction tx)
        {

            PayTypeTable table = (PayTypeTable)ds.Tables[clsPOSDBConstants.PayType_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (PayTypeRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.PayType_tbl, delParam);
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
        #endregion
        #region IDBDataParameter Generator Methods

        private IDbDataParameter[] PKParameters(System.String PayTypeID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@PayTypeID";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = PayTypeID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(PayTypeRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@PayTypeID";
            sqlParams[0].DbType = System.Data.DbType.String;

            sqlParams[0].Value = row.PaytypeID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.PayType_Fld_PayType;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(PayTypeRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7); //PRIMEPOS-2966 20-May-2021 JY changed from 6 to 7

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_PayTypeID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_PayType, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_PayTypeDescription, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_UserID, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_IsHide, System.Data.DbType.Boolean);  //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_StopAtRefNo, System.Data.DbType.Boolean);    //PRIMEPOS-2309 08-Mar-2019 JY Added
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_SortOrder, System.Data.DbType.Int32);    //PRIMEPOS-2966 20-May-2021 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.PayType_Fld_PayTypeID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.PayType_Fld_PayType;
            sqlParams[2].SourceColumn = clsPOSDBConstants.PayType_Fld_PayTypeDescription;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Users_Fld_UserID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.PayType_Fld_IsHide;    //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            sqlParams[5].SourceColumn = clsPOSDBConstants.PayType_Fld_StopAtRefNo;  //PRIMEPOS-2309 08-Mar-2019 JY Added
            sqlParams[6].SourceColumn = clsPOSDBConstants.PayType_Fld_SortOrder;    //PRIMEPOS-2966 20-May-2021 JY Added

            sqlParams[0].Value = GetNextPayTypeID();

            if (row.PayType != System.String.Empty)
                sqlParams[1].Value = row.PayType;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.PayTypeDesc != System.String.Empty)
                sqlParams[2].Value = row.PayTypeDesc;
            else
                sqlParams[2].Value = DBNull.Value;

            sqlParams[3].Value = Configuration.UserName;
            sqlParams[4].Value = Configuration.convertNullToBoolean(row.IsHide); //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            sqlParams[5].Value = Configuration.convertNullToBoolean(row.StopAtRefNo);   //PRIMEPOS-2309 08-Mar-2019 JY Added
            sqlParams[6].Value = Configuration.convertNullToInt(row.SortOrder); //PRIMEPOS-2966 20-May-2021 JY Added
            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(PayTypeRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7); //PRIMEPOS-2966 20-May-2021 JY changed from 6 to 7

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_PayTypeID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_PayType, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_PayTypeDescription, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Users_Fld_UserID, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_IsHide, System.Data.DbType.Boolean);  //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_StopAtRefNo, System.Data.DbType.Boolean);    //PRIMEPOS-2309 08-Mar-2019 JY Added
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.PayType_Fld_SortOrder, System.Data.DbType.Int32);    //PRIMEPOS-2966 20-May-2021 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.PayType_Fld_PayTypeID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.PayType_Fld_PayType;
            sqlParams[2].SourceColumn = clsPOSDBConstants.PayType_Fld_PayTypeDescription;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Users_Fld_UserID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.PayType_Fld_IsHide;    //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            sqlParams[5].SourceColumn = clsPOSDBConstants.PayType_Fld_StopAtRefNo;  //PRIMEPOS-2309 08-Mar-2019 JY Added
            sqlParams[6].SourceColumn = clsPOSDBConstants.PayType_Fld_SortOrder;    //PRIMEPOS-2966 20-May-2021 JY Added

            if (row.PaytypeID != System.String.Empty)
                sqlParams[0].Value = row.PaytypeID;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.PayType != System.String.Empty)
                sqlParams[1].Value = row.PayType;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.PayTypeDesc != System.String.Empty)
                sqlParams[2].Value = row.PayTypeDesc;
            else
                sqlParams[2].Value = DBNull.Value;

            sqlParams[3].Value = Configuration.UserName;
            sqlParams[4].Value = Configuration.convertNullToBoolean(row.IsHide); //Sprint-23 - PRIMEPOS-2255 16-May-2016 JY Added 
            sqlParams[5].Value = Configuration.convertNullToBoolean(row.StopAtRefNo);   //PRIMEPOS-2309 08-Mar-2019 JY Added
            sqlParams[6].Value = Configuration.convertNullToInt(row.SortOrder); //PRIMEPOS-2966 20-May-2021 JY Added
            return (sqlParams);
        }
        #endregion

        public void Dispose() { }
    }
}
