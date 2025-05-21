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
////using POS.Resources;
using POS_Core.Resources;
using NLog;

namespace POS_Core.DataAccess
{
    // Provides data access methods for Vendor
    public class VendorSvr : IDisposable
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
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(DataSet updates, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }

        // Inserts, updates or deletes rows in a DataSet.
        public void Persist(DataSet updates)
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                conn.Open();
                SqlTransaction tx = conn.BeginTransaction();
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
                    logger.Fatal(ex, "Persist(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                    //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                }
            }
        }

        public bool DeleteRow(string CurrentID)
        {
            string sSQL;
            try
            {
                DataTable dtVendor = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "Select * from ItemVendor where VendorID = '" + CurrentID + "'").Tables[0];
                DataTable dtPO = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "Select * from PurchaseOrder where VendorID = '" + CurrentID + "'").Tables[0];
                if ((dtVendor.Rows.Count == 0) && (dtPO.Rows.Count == 0))
                {
                    sSQL = " delete from Vendor where VendorID= '" + CurrentID + "'";
                    DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow(string CurrentID)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return false;
            }
        }
        #endregion

        #region Get Methods
        // Looks up a Vendor based on its primary-key:System.Int32 vendorcode
        public virtual VendorData Populate(System.String vendorcode, SqlConnection conn)
        {
            try
            {
                using (VendorData oVendorData = new VendorData())
                {
                    using (DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, "Select * FROM " + clsPOSDBConstants.Vendor_tbl + " WHERE " + clsPOSDBConstants.Vendor_Fld_VendorCode + " ='" + vendorcode.Replace("'", "''") + "'", PKParameters(vendorcode)))   //20-Nov-2015 JY replaced "'" with "''" to avoid exception
                    {
                        oVendorData.Vendor.MergeTable(ds.Tables[0]);
                        return oVendorData;
                    }
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
                logger.Fatal(ex, "Populate(System.String vendorcode, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public virtual VendorData Populate(System.Int32 vendorId, SqlConnection conn)
        {
            try
            {
                VendorData ds = new VendorData();
                ds.Vendor.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, "Select * FROM " + clsPOSDBConstants.Vendor_tbl + " WHERE " + clsPOSDBConstants.Vendor_Fld_VendorId + " =" + vendorId, PKParameters(vendorId.ToString())).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.Int32 vendorId, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public VendorData Populate(System.String vendorcode)
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                return (Populate(vendorcode, conn));
            }
        }

        public VendorData Populate(System.Int32 vendorId)
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                return (Populate(vendorId, conn));
            }
        }

        // Fills a VendorData with all Vendor
        public VendorData PopulateList(string sWhereClause, SqlConnection conn)
        {
            try
            {
                string sSQL = String.Concat("SELECT * FROM ", clsPOSDBConstants.Vendor_tbl, sWhereClause);
                using (VendorData ds = new VendorData())
                {
                    ds.Vendor.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
                    return ds;
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
                logger.Fatal(ex, "PopulateList(string sWhereClause, SqlConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        // Fills a VendorData with all Vendor
        public VendorData PopulateList(string whereClause)
        {
            using (SqlConnection conn = new SqlConnection(DBConfig.ConnectionString))
            {
                return (PopulateList(whereClause, conn));
            }
        }

        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, SqlTransaction tx)
        {
            VendorTable addedTable = (VendorTable)ds.Tables[clsPOSDBConstants.Vendor_tbl].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (VendorRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.Vendor_tbl, insParam);
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
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                            ErrorHandler.throwCustomError(POSErrorENUM.Vendor_DuplicateCode);
                        else
                            throw (ex);
                    }
                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Insert(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                    }
                }
                addedTable.AcceptChanges();
            }
        }

        // Update all rows in a Vendor DataSet, within a given database transaction.
        public void Update(DataSet ds, SqlTransaction tx)
        {
            VendorTable modifiedTable = (VendorTable)ds.Tables[clsPOSDBConstants.Vendor_tbl].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (VendorRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.Vendor_tbl, updParam);
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
                            ErrorHandler.throwCustomError(POSErrorENUM.Vendor_DuplicateCode);
                        else
                            throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "Update(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                    }
                }
                modifiedTable.AcceptChanges();
            }
        }

        // Delete all rows within a Vendor DataSet, within a given database transaction.
        public void Delete(DataSet ds, SqlTransaction tx)
        {
            VendorTable table = (VendorTable)ds.Tables[clsPOSDBConstants.Vendor_tbl].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (VendorRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.Vendor_tbl, delParam);
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
                        logger.Fatal(ex, "Delete(DataSet ds, SqlTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
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
            sUpdateSQL = sUpdateSQL + updParam[2].SourceColumn + "  = " + updParam[2].ParameterName;

            for (int i = 3; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }
            sUpdateSQL = sUpdateSQL + " , UserID  = '" + Configuration.UserName + "'";
            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[1].SourceColumn + " = " + updParam[1].ParameterName;
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

        private IDbDataParameter[] PKParameters(System.String vendorCode)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@VendorCode";
            sqlParams[0].DbType = System.Data.DbType.String;
            sqlParams[0].Value = vendorCode;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(VendorRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@Vendorcode";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.Vendorcode;
            sqlParams[0].SourceColumn = clsPOSDBConstants.Vendor_Fld_VendorCode;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(VendorRow row)
        {
            //Added By SRT(Abhishek) Date : 01/07/2009 
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(27);//DataFactory.CreateParameterArray(23);//Commented By by Ravindra(QuicSolv) on 20 feb 2013 for AckPriceUpdate//4-Nov-2014 Ravindra added For SalePriceQualifier;  //Sprint-21 - 2208 24-Jul-2015 JY Added increment index by 1 as added new column
                                                                                // End of Added By SRT(Abhishek) Date : 01/07/2009 

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_VendorId, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_VendorCode, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_VendorName, System.Data.DbType.StringFixedLength);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_Address1, System.Data.DbType.StringFixedLength);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_Address2, System.Data.DbType.StringFixedLength);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_City, System.Data.DbType.StringFixedLength);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_State, System.Data.DbType.StringFixedLength);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_Zip, System.Data.DbType.StringFixedLength);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_TelephoneNo, System.Data.DbType.StringFixedLength);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_FaxNo, System.Data.DbType.StringFixedLength);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_CellNo, System.Data.DbType.StringFixedLength);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_URL, System.Data.DbType.String);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_Email, System.Data.DbType.String);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode, System.Data.DbType.String);
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_USEVICForEPO, System.Data.DbType.Boolean);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_PriceQualifier, System.Data.DbType.String);
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_CostQualifier, System.Data.DbType.String);

            //Added By SRT(Abhishek) Date : 04/16/2009 
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_UpdatePrice, System.Data.DbType.Boolean);
            // End of Added By SRT(Abhishek) Date : 04/16/2009
            //Added By SRT(Prashant) Date : 01/06/2009 
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_TimeToOrder, System.Data.DbType.String);
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_IsAutoClose, System.Data.DbType.Boolean);
            // End of Added By SRT(Prashant) Date : 01/06/2009
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice, System.Data.DbType.Boolean);
            //Added By SRT(Abhishek) Date : 07/3/2009 
            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_PrimePOVendorID, System.Data.DbType.Int32);
            // End of Added By SRT(Abhishek) Date : 04/16/2009
            //AckPriceUpdate Added by Ravindra on 20 Feb 2013
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_AckPriceUpdate, System.Data.DbType.Boolean);
            sqlParams[24] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_SalePriceUpdate, System.Data.DbType.Boolean);   //12-Nov-2014 JY added new field IsSalePriceUpdate
            sqlParams[25] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_SalePriceQualifier, System.Data.DbType.Boolean);   //12-Nov-2014 JY added new field IsSalePriceUpdate
            sqlParams[26] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice, System.Data.DbType.Boolean);   //Sprint-21 - 2208 24-Jul-2015 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.Vendor_Fld_VendorId;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Vendor_Fld_VendorCode;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Vendor_Fld_VendorName;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Vendor_Fld_Address1;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Vendor_Fld_Address2;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Vendor_Fld_City;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Vendor_Fld_State;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Vendor_Fld_Zip;
            sqlParams[8].SourceColumn = clsPOSDBConstants.Vendor_Fld_TelephoneNo;
            sqlParams[9].SourceColumn = clsPOSDBConstants.Vendor_Fld_FaxNo;
            sqlParams[10].SourceColumn = clsPOSDBConstants.Vendor_Fld_CellNo;
            sqlParams[11].SourceColumn = clsPOSDBConstants.Vendor_Fld_URL;
            sqlParams[12].SourceColumn = clsPOSDBConstants.Vendor_Fld_Email;
            sqlParams[13].SourceColumn = clsPOSDBConstants.Vendor_Fld_IsActive;
            sqlParams[14].SourceColumn = clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode;
            sqlParams[15].SourceColumn = clsPOSDBConstants.Vendor_Fld_USEVICForEPO;
            sqlParams[16].SourceColumn = clsPOSDBConstants.Vendor_Fld_PriceQualifier;
            sqlParams[17].SourceColumn = clsPOSDBConstants.Vendor_Fld_CostQualifier;

            //Added By SRT(Abhishek) Date : 04/16/2009 
            sqlParams[18].SourceColumn = clsPOSDBConstants.Vendor_Fld_UpdatePrice;
            // End of Added By SRT(Abhishek) Date : 04/16/2009    

            //Added By SRT(Prashant) Date : 01/06/2009 
            sqlParams[19].SourceColumn = clsPOSDBConstants.Vendor_Fld_TimeToOrder;
            sqlParams[20].SourceColumn = clsPOSDBConstants.Vendor_Fld_IsAutoClose;
            // End of Added By SRT(Prashant) Date : 01/06/2009
            sqlParams[21].SourceColumn = clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice;
            //Added By SRT(Abhishek) Date : 07/3/2009 
            sqlParams[22].SourceColumn = clsPOSDBConstants.Vendor_Fld_PrimePOVendorID;
            // End of Added By SRT(Abhishek) Date : 07/3/2009 
            //AckPriceUpdate Added by Ravindra on 20 Feb 2013
            sqlParams[23].SourceColumn = clsPOSDBConstants.Vendor_Fld_AckPriceUpdate;
            sqlParams[24].SourceColumn = clsPOSDBConstants.Vendor_Fld_SalePriceUpdate; //12-Nov-2014 JY added new field IsSalePriceUpdate
            sqlParams[25].SourceColumn = clsPOSDBConstants.Vendor_Fld_SalePriceQualifier; //4-Nov-2014 Ravindra added For SalePriceQualifier;
            sqlParams[26].SourceColumn = clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice; //Sprint-21 - 2208 24-Jul-2015 JY Added

            if (row.VendorId != System.Int32.MinValue)
                sqlParams[0].Value = row.VendorId;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.Vendorcode != System.String.Empty)
                sqlParams[1].Value = row.Vendorcode;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Vendorname != System.String.Empty)
                sqlParams[2].Value = row.Vendorname;
            else
                sqlParams[2].Value = DBNull.Value;
            if (row.Address1 != System.String.Empty)
                sqlParams[3].Value = row.Address1;
            else
                sqlParams[3].Value = DBNull.Value;
            if (row.Address2 != System.String.Empty)
                sqlParams[4].Value = row.Address2;
            else
                sqlParams[4].Value = DBNull.Value;
            if (row.City != System.String.Empty)
                sqlParams[5].Value = row.City;
            else
                sqlParams[5].Value = DBNull.Value;
            if (row.State != System.String.Empty)
                sqlParams[6].Value = row.State;
            else
                sqlParams[6].Value = DBNull.Value;
            if (row.Zip != System.String.Empty)
                sqlParams[7].Value = row.Zip;
            else
                sqlParams[7].Value = DBNull.Value;
            if (row.Telephoneno != System.String.Empty)
                sqlParams[8].Value = row.Telephoneno;
            else
                sqlParams[8].Value = DBNull.Value;
            if (row.Faxno != System.String.Empty)
                sqlParams[9].Value = row.Faxno;
            else
                sqlParams[9].Value = DBNull.Value;
            if (row.Cellno != System.String.Empty)
                sqlParams[10].Value = row.Cellno;
            else
                sqlParams[10].Value = DBNull.Value;
            if (row.Url != System.String.Empty)
                sqlParams[11].Value = row.Url;
            else
                sqlParams[11].Value = DBNull.Value;
            if (row.Email != System.String.Empty)
                sqlParams[12].Value = row.Email;
            else
                sqlParams[12].Value = DBNull.Value;

            sqlParams[13].Value = row.IsActive;

            if (row.PrimePOVendorCode != System.String.Empty)
                sqlParams[14].Value = row.PrimePOVendorCode;
            else
                sqlParams[14].Value = DBNull.Value;

            sqlParams[15].Value = row.USEVICForEPO;

            sqlParams[16].Value = row.PriceQualifier;

            sqlParams[17].Value = row.CostQualifier;

            //Added By SRT(Abhishek) Date : 04/16/2009 
            sqlParams[18].Value = row.UpdatePrice;
            // End of Added By SRT(Abhishek) Date : 04/16/2009    
            //Added By SRT(Prashant) Date : 01/06/2009 
            sqlParams[19].Value = row.TimeToOrder;
            sqlParams[20].Value = row.IsAutoClose;
            //End of Added By SRT(Prashant) Date : 01/06/2009
            sqlParams[21].Value = row.SendVendCostPrice;
            //Added By SRT(Abhishek) Date : 07/03/2009 
            sqlParams[22].Value = 0;
            // End of Added By SRT(Abhishek) Date : 07/03/2009  
            //AckPriceUpdate Added by Ravindra on 20 Feb 2013
            sqlParams[23].Value = row.AckPriceUpdate;
            sqlParams[24].Value = row.SalePriceUpdate; //12-Nov-2014 JY added new field IsSalePriceUpdate
            sqlParams[25].Value = row.SalePriceQualifier; //12-Nov-2014 JY added new field IsSalePriceUpdate
            sqlParams[26].Value = row.ReduceSellingPrice; //Sprint-21 - 2208 24-Jul-2015 JY Added

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(VendorRow row)
        {
            //Added By SRT(Abhishek) Date : 04/16/2009 
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(27);//DataFactory.CreateParameterArray(23);//Commented By by Ravindra(QuicSolv) on 20 feb 2013 for AckPriceUpdate//4-Nov-2014 Ravindra added For SalePriceQualifier;    //Sprint-21 - 2208 24-Jul-2015 JY Added increment index by 1 as added new column
                                                                                // End of Added By SRT(Abhishek) Date : 04/16/2009 

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_VendorId, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_VendorCode, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_VendorName, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_Address1, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_Address2, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_City, System.Data.DbType.StringFixedLength);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_State, System.Data.DbType.StringFixedLength);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_Zip, System.Data.DbType.StringFixedLength);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_TelephoneNo, System.Data.DbType.StringFixedLength);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_FaxNo, System.Data.DbType.StringFixedLength);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_CellNo, System.Data.DbType.StringFixedLength);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_URL, System.Data.DbType.StringFixedLength);
            sqlParams[12] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_Email, System.Data.DbType.String);
            sqlParams[13] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_IsActive, System.Data.DbType.Boolean);
            sqlParams[14] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode, System.Data.DbType.String);
            sqlParams[15] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_USEVICForEPO, System.Data.DbType.Boolean);
            sqlParams[16] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_PriceQualifier, System.Data.DbType.String);
            sqlParams[17] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_CostQualifier, System.Data.DbType.String);
            //Added By SRT(Abhishek) Date : 04/16/2009 
            sqlParams[18] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_UpdatePrice, System.Data.DbType.Boolean);
            // End of Added By SRT(Abhishek) Date : 04/16/2009
            //Added By SRT(Prashant) Date : 01/06/2009 
            sqlParams[19] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_TimeToOrder, System.Data.DbType.String);
            sqlParams[20] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_IsAutoClose, System.Data.DbType.Boolean);
            // End of Added By SRT(Prashant) Date : 01/06/2009 
            sqlParams[21] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice, System.Data.DbType.Boolean);
            sqlParams[22] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_Process810, System.Data.DbType.Boolean);
            //AckPriceUpdate Add by Ravindra(QuicSolv) on 20 feb 2013
            sqlParams[23] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_AckPriceUpdate, System.Data.DbType.Boolean);
            //End Of Added By   Ravindra(QuicSolv)  on 20 feb 2013
            sqlParams[24] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_SalePriceUpdate, System.Data.DbType.Boolean);    //12-Nov-2014 JY added new field IsSalePriceUpdate

            sqlParams[25] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_SalePriceQualifier, System.Data.DbType.String);    //4-Nov-2014 Ravindra added For SalePriceQualifier;
            sqlParams[26] = DataFactory.CreateParameter("@" + clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice, System.Data.DbType.String);  //Sprint-21 - 2208 24-Jul-2015 JY Added

            sqlParams[0].SourceColumn = clsPOSDBConstants.Vendor_Fld_VendorId;
            sqlParams[1].SourceColumn = clsPOSDBConstants.Vendor_Fld_VendorCode;
            sqlParams[2].SourceColumn = clsPOSDBConstants.Vendor_Fld_VendorName;
            sqlParams[3].SourceColumn = clsPOSDBConstants.Vendor_Fld_Address1;
            sqlParams[4].SourceColumn = clsPOSDBConstants.Vendor_Fld_Address2;
            sqlParams[5].SourceColumn = clsPOSDBConstants.Vendor_Fld_City;
            sqlParams[6].SourceColumn = clsPOSDBConstants.Vendor_Fld_State;
            sqlParams[7].SourceColumn = clsPOSDBConstants.Vendor_Fld_Zip;
            sqlParams[8].SourceColumn = clsPOSDBConstants.Vendor_Fld_TelephoneNo;
            sqlParams[9].SourceColumn = clsPOSDBConstants.Vendor_Fld_FaxNo;
            sqlParams[10].SourceColumn = clsPOSDBConstants.Vendor_Fld_CellNo;
            sqlParams[11].SourceColumn = clsPOSDBConstants.Vendor_Fld_URL;
            sqlParams[12].SourceColumn = clsPOSDBConstants.Vendor_Fld_Email;
            sqlParams[13].SourceColumn = clsPOSDBConstants.Vendor_Fld_IsActive;
            sqlParams[14].SourceColumn = clsPOSDBConstants.Vendor_Fld_PrimePOVendorCode;
            sqlParams[15].SourceColumn = clsPOSDBConstants.Vendor_Fld_USEVICForEPO;
            sqlParams[16].SourceColumn = clsPOSDBConstants.Vendor_Fld_PriceQualifier;
            sqlParams[17].SourceColumn = clsPOSDBConstants.Vendor_Fld_CostQualifier;

            //Added By SRT(Abhishek) Date : 04/16/2009 
            sqlParams[18].SourceColumn = clsPOSDBConstants.Vendor_Fld_UpdatePrice;
            // End of Added By SRT(Abhishek) Date : 04/16/2009

            //Added By SRT(Prashant) Date : 01/06/2009 
            sqlParams[19].SourceColumn = clsPOSDBConstants.Vendor_Fld_TimeToOrder;
            sqlParams[20].SourceColumn = clsPOSDBConstants.Vendor_Fld_IsAutoClose;
            // End of Added By SRT(Prashant) Date : 01/06/2009
            sqlParams[21].SourceColumn = clsPOSDBConstants.Vendor_Fld_SendVendorCostPrice;
            sqlParams[22].SourceColumn = clsPOSDBConstants.Vendor_Fld_Process810;
            //AckPriceUpdate Add by Ravindra(QuicSolv) on 20 feb 2013
            sqlParams[23].SourceColumn = clsPOSDBConstants.Vendor_Fld_AckPriceUpdate;
            //End of Add by Ravindra(QuicSolv) on 20 feb 2013
            sqlParams[24].SourceColumn = clsPOSDBConstants.Vendor_Fld_SalePriceUpdate;   //12-Nov-2014 JY added new field IsSalePriceUpdate
            sqlParams[25].SourceColumn = clsPOSDBConstants.Vendor_Fld_SalePriceQualifier;  //4-Nov-2014 Ravindra added For SalePriceQualifier; 
            sqlParams[26].SourceColumn = clsPOSDBConstants.Vendor_Fld_ReduceSellingPrice;    //Sprint-21 - 2208 24-Jul-2015 JY Added

            if (row.VendorId != System.Int32.MinValue)
                sqlParams[0].Value = row.VendorId;
            else
                sqlParams[0].Value = DBNull.Value;

            if (row.Vendorcode != System.String.Empty)
                sqlParams[1].Value = row.Vendorcode;
            else
                sqlParams[1].Value = DBNull.Value;

            if (row.Vendorname != System.String.Empty)
                sqlParams[2].Value = row.Vendorname;
            else
                sqlParams[2].Value = DBNull.Value;

            if (row.Address1 != System.String.Empty)
                sqlParams[3].Value = row.Address1;
            else
                sqlParams[3].Value = DBNull.Value;

            if (row.Address2 != System.String.Empty)
                sqlParams[4].Value = row.Address2;
            else
                sqlParams[4].Value = DBNull.Value;

            if (row.City != System.String.Empty)
                sqlParams[5].Value = row.City;
            else
                sqlParams[5].Value = DBNull.Value;

            if (row.State != System.String.Empty)
                sqlParams[6].Value = row.State;
            else
                sqlParams[6].Value = DBNull.Value;

            if (row.Zip != System.String.Empty)
                sqlParams[7].Value = row.Zip;
            else
                sqlParams[7].Value = DBNull.Value;

            if (row.Telephoneno != System.String.Empty)
                sqlParams[8].Value = row.Telephoneno;
            else
                sqlParams[8].Value = DBNull.Value;

            if (row.Faxno != System.String.Empty)
                sqlParams[9].Value = row.Faxno;
            else
                sqlParams[9].Value = DBNull.Value;

            if (row.Cellno != System.String.Empty)
                sqlParams[10].Value = row.Cellno;
            else
                sqlParams[10].Value = DBNull.Value;

            if (row.Url != System.String.Empty)
                sqlParams[11].Value = row.Url;
            else
                sqlParams[11].Value = DBNull.Value;

            if (row.Email != System.String.Empty)
                sqlParams[12].Value = row.Email;
            else
                sqlParams[12].Value = DBNull.Value;

            sqlParams[13].Value = row.IsActive;

            if (row.PrimePOVendorCode != System.String.Empty)
                sqlParams[14].Value = row.PrimePOVendorCode;
            else
                sqlParams[14].Value = DBNull.Value;

            sqlParams[15].Value = row.USEVICForEPO;
            sqlParams[16].Value = row.PriceQualifier;
            sqlParams[17].Value = row.CostQualifier;

            //Added By SRT(Abhishek) Date : 04/16/2009 
            sqlParams[18].Value = row.UpdatePrice;
            // End of Added By SRT(Abhishek) Date : 04/16/2009

            //Added By SRT(Prashant) Date : 01/06/2009(dd/mm/yyyy) 
            sqlParams[19].Value = row.TimeToOrder;
            sqlParams[20].Value = row.IsAutoClose;
            // End of Added By SRT(Prashant) Date : 01/06/2009
            sqlParams[21].Value = row.SendVendCostPrice;
            sqlParams[22].Value = row.Process810;

            sqlParams[23].Value = row.AckPriceUpdate;
            sqlParams[24].Value = row.SalePriceUpdate;    //12-Nov-2014 JY added new field IsSalePriceUpdate
            sqlParams[25].Value = row.SalePriceQualifier;    //4-Nov-2014 Ravindra added For SalePriceQualifier;
            sqlParams[26].Value = row.ReduceSellingPrice;   //Sprint-21 - 2208 24-Jul-2015 JY Added

            return (sqlParams);
        }
        #endregion

        public void Dispose() { }
    }

}
