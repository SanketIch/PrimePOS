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

    public class CLCouponsSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.

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
                //ErrorLogging.ErrorHandler.throwException(ex, "", "");
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
                logger.Fatal(ex, "Persist(DataSet updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }

        }

        public bool DeleteRow(string CurrentID)
        {
            string sSQL;
            try
            {
                DataTable dtItem = DataHelper.ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, "Select * from Item where CLCouponsID = '" + CurrentID + "'").Tables[0];
                if (dtItem.Rows.Count == 0)
                {
                    //sSQL = " update CL_Coupons set isActive=0 where ID= '" + CurrentID + "'"; //Sprint-18 - 2090 22-Oct-2014 JY commented
                    sSQL = " update CL_Coupons set isActive=0 where isActive<>0 AND ID= '" + CurrentID + "'";   //Sprint-18 - 2090 22-Oct-2014 JY Added optimized SQL
                    DataHelper.ExecuteNonQuery(DBConfig.ConnectionString, CommandType.Text, sSQL);
                    return true;
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

        public bool DeleteCoupons(Int64 iCLCardID,IDbTransaction oTrans)
        {
            string sSQL;
            try
            {
                //sSQL = " update CL_Coupons set isActive=0 where CardID= " + iCLCardID.ToString(); //Sprint-18 - 2090 22-Oct-2014 JY Commented
                sSQL = " update CL_Coupons set isActive=0 where isActive<>0 AND CardID= " + iCLCardID.ToString();   //Sprint-18 - 2090 22-Oct-2014 JY Added optimized sql
                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteCoupons(Int64 iCLCardID,IDbTransaction oTrans)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }

        public bool DeleteUnusedCoupons(Int64 iCLCardID, IDbTransaction oTrans)
        {
            string sSQL;
            try
            {
                //sSQL = " update CL_Coupons set isActive=0 where CardID= " + iCLCardID.ToString() +
                //    " and (IsCouponUsed=0 And DateAdd(d,ExpiryDays,CreatedOn)>GetDate())";    //Sprint-18 - 2090 22-Oct-2014 JY Commented
                sSQL = " update CL_Coupons set isActive=0 where isActive<>0 AND CardID= " + iCLCardID.ToString() +
                    " and (IsCouponUsed=0 And DateAdd(d,ExpiryDays,CreatedOn)>GetDate())";  //Sprint-18 - 2090 22-Oct-2014 JY Added optimized sql
                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteUnusedCoupons(Int64 iCLCardID, IDbTransaction oTrans)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return false;
            }
        }
        #endregion

        #region Get Methods

        // Looks up a DeptCode based on its primary-key:System.Int32 DeptCode

        public CLCouponsData Populate(System.Int64 ID, IDbConnection conn)
        {
            try
            {
                string sSQL = "Select * "
                                + " FROM "
                                    + clsPOSDBConstants.CLCoupons_tbl
                                + " WHERE " + clsPOSDBConstants.CLCoupons_Fld_ID + " =" + ID + " And isActive=1 ";


                CLCouponsData ds = new CLCouponsData();
                ds.CLCoupons.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text
                                            , sSQL
                                            , PKParameters(ID)).Tables[0]);
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
                logger.Fatal(ex, "Populate(System.Int64 ID, IDbConnection conn)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public CLCouponsData Populate(System.Int64 ID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (Populate(ID, conn));
            }
        }

        public CLCouponsData GetByCLCardID(System.Int64 iCLCardID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where CardID=" + iCLCardID.ToString() + " And isActive=1 ORDER BY ID DESC", conn));
            }
        }

        //PRIMEPOS-2794 SAJID DHUKKA
        public CLCouponsData GetByCLCardID(string iCLCardID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where CardID IN(" + iCLCardID.ToString() + ") And isActive=1 ORDER BY ID DESC", conn));
            }
        }

        public CLCouponsData GetByCLCardID(System.Int64 iCLCardID, string strCalledFrom)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where CardID=" + iCLCardID.ToString() + " And isActive=1 ORDER BY ID DESC", conn, strCalledFrom));
            }
        }

        /// <summary>
        /// Author: Shitaljit
        /// CreatedvDate : 2/4/2014
        /// Added to check whether customer has alrady availed coupon for the give reward tier or not
        /// <summary>
        /// <param name="iCLCardID"></param>
        /// <param name="iCLTierID"></param>
        /// <returns></returns>
        public CLCouponsData IsCouponGeneratedForCLTier(System.Int64 iCLCardID, Int64 iCLTierID, IDbTransaction oTrans)
        {
            return (PopulateList(" WHERE CardID=" + iCLCardID.ToString() + " AND CLTierID = "+iCLTierID.ToString() , oTrans));
            
        }
        
        public CLCouponsData GetValidUnusedCoupons(System.Int64 iCLCardID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where CardID=" + iCLCardID.ToString() + " And isActive=1 ORDER BY ID DESC", conn));  //Sprint-19 - 01-Apr-2015 JY Added order by
            }
        }

        public CLCouponsData GetByCLCouponID(System.Int64 iCLCouponID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(" where ID=" + iCLCouponID.ToString() + " And isActive=1 ORDER BY ID DESC", conn));    //Sprint-19 - 01-Apr-2015 JY Added 
            }
        }

        public CLCouponsData GetByCLCouponID(System.Int64 iCLCouponID, System.Data.IDbTransaction tx)
        {
            return (PopulateList(" where ID=" + iCLCouponID.ToString() + " And isActive=1", tx));
        }

        public CLCouponsData GetUnUsedCLCoupons(Int64 iCLCardID)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {

                return (PopulateList(" where CardID=" + iCLCardID.ToString() + " And (IsCouponUsed=0 And isActive=1 And DateAdd(d,ExpiryDays,CreatedOn)>GetDate())", conn));
            }
        }

        public CLCouponsData GetUnUsedCLCoupons(IDbTransaction tx, Int64 iCLCardID)
        {
            return (PopulateList(" where CardID=" + iCLCardID.ToString() + " And  (IsCouponUsed=0  And isActive=1 And DateAdd(d,ExpiryDays,CreatedOn)>GetDate())", tx));
        }

        public CLCouponsData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select * "   
                                    + " FROM "
                                        + clsPOSDBConstants.CLCoupons_tbl
                                    , sWhereClause);

                CLCouponsData ds = new CLCouponsData();
                ds.CLCoupons.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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

        //Sprint-19 - 01-Apr-2015 JY Added to resolved duplicate active coupon issue
        public CLCouponsData PopulateList(string sWhereClause, IDbConnection conn, string strCalledFrom)
        {
            try
            {
                string sSQL = String.Concat("Select TOP 1 * "
                                    + " FROM "
                                        + clsPOSDBConstants.CLCoupons_tbl
                                    , sWhereClause);

                CLCouponsData ds = new CLCouponsData();
                ds.CLCoupons.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbConnection conn, string strCalledFrom)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public CLCouponsData PopulateList(string sWhereClause, IDbTransaction tx)
        {
            try
            {
                string sSQL = String.Concat("Select * "
                                    + " FROM "
                                        + clsPOSDBConstants.CLCoupons_tbl
                                    , sWhereClause);

                CLCouponsData ds = new CLCouponsData();
                ds.CLCoupons.MergeTable(DataHelper.ExecuteDataset(tx, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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
                logger.Fatal(ex, "PopulateList(string sWhereClause, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public CLCouponsData PopulateList(string whereClause)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return (PopulateList(whereClause, conn));
            }
        }

        #endregion //Get Method

        #region Insert, Update, and Delete Methods
        public void Insert(DataSet ds, IDbTransaction tx)
        {

            CLCouponsTable addedTable = (CLCouponsTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (CLCouponsRow row in addedTable.Rows)
                {
                    try
                    {

                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.CLCoupons_tbl, insParam);
                        
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

        // Update all rows in a DeptCodes DataSet, within a given database transaction.

        public void Update(DataSet ds, IDbTransaction tx)
        {
            CLCouponsTable modifiedTable = (CLCouponsTable)ds.Tables[0].GetChanges(DataRowState.Modified);

            string sSQL;
            IDbDataParameter[] updParam;

            if (modifiedTable != null && modifiedTable.Rows.Count > 0)
            {
                foreach (CLCouponsRow row in modifiedTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.CLCoupons_tbl, updParam);

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

            CLCouponsTable table = (CLCouponsTable)ds.Tables[0].GetChanges(DataRowState.Deleted);
            string sSQL;
            IDbDataParameter[] delParam;

            if (table != null && table.Rows.Count > 0)
            {
                table.RejectChanges(); //so we can access the rows
                foreach (CLCouponsRow row in table.Rows)
                {
                    try
                    {
                        delParam = PKParameters(row);

                        sSQL = BuildDeleteSQL(clsPOSDBConstants.CLCoupons_tbl, delParam);
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
            //string sDeleteSQL = "Update " + tableName + " Set IsActive=0  WHERE ";    //Sprint-18 - 2090 22-Oct-2014 JY Commented
            string sDeleteSQL = "Update " + tableName + " Set IsActive=0  WHERE IsActive<>0 AND ";   //Sprint-18 - 2090 22-Oct-2014 JY Added optimized sql
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
            sInsertSQL = sInsertSQL + " , CreatedBy, CreatedOn ";
            sInsertSQL = sInsertSQL + " ) Values (" + delParam[0].ParameterName;

            for (int i = 1; i < delParam.Length; i++)
            {
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " , '" + Configuration.UserName + "', GetDate()";
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

        public void ConsumeCoupon(POSTransPaymentRow oTransPRow, System.Data.IDbTransaction oDBTrans)
        {

            try
            {
                CLCouponsData oCLCouponData = this.GetByCLCouponID(oTransPRow.CLCouponID,oDBTrans);
                if (oCLCouponData.CLCoupons.Rows.Count > 0)
                {
                    Decimal remainingPoints = 0;
                    if (oTransPRow.Amount != oCLCouponData.CLCoupons[0].CouponValue && oCLCouponData.CLCoupons[0].IsCouponValueInPercentage==false)
                    {

                        decimal perDollarPoints = oCLCouponData.CLCoupons[0].Points / oCLCouponData.CLCoupons[0].CouponValue;

                        decimal consumedPoints = perDollarPoints * (oTransPRow.Amount);  //Sprint-18 - 2090 17-Oct-2014 JY change int to decimal
                        remainingPoints = oCLCouponData.CLCoupons[0].Points - consumedPoints;
                        oCLCouponData.CLCoupons[0].Points = consumedPoints;
                        oCLCouponData.CLCoupons[0].CouponValue = oTransPRow.Amount;

                    }
                    oCLCouponData.CLCoupons[0].IsCouponUsed = true;
                    oCLCouponData.CLCoupons[0].IsActive = false; //Added By Shitaljit on 24Jan14 to set used coupon as inactive
                    oCLCouponData.CLCoupons[0].UsedInTransID = oTransPRow.TransID;

                    this.Persist(oCLCouponData, oDBTrans);

                    if (remainingPoints > 0)
                    {
                        string sSQL = "update CL_Cards set CurrentPoints=currentpoints+" + remainingPoints.ToString() + " where CardID=" + oCLCouponData.CLCoupons[0].CLCardID.ToString();
                        DataHelper.ExecuteNonQuery(oDBTrans, CommandType.Text, sSQL);
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
                logger.Fatal(ex, "ConsumeCoupon(POSTransPaymentRow oTransPRow, System.Data.IDbTransaction oDBTrans)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        /// <summary>
        /// Author:Shitaljit
        /// Date: 2/6/2014
        /// Created to reset single coupon per reawrd tier logic for specific card.
        /// </summary>
        /// <param name="iCLCardID"></param>
        public bool ResetCouponGenarationCycle(Int64 iCLCardID, IDbTransaction oTrans)
        {
            string sSQL = string.Empty;
            try
            {
                sSQL = " UPDATE CL_Coupons SET " + clsPOSDBConstants.CLCoupons_Fld_CLTierID + " = 0 WHERE "+ 
                        clsPOSDBConstants.CLCoupons_Fld_CLCardID+"= '" + iCLCardID + "'";
                DataHelper.ExecuteNonQuery(oTrans, CommandType.Text, sSQL);
                return true;
            }
            catch (Exception ex)
            {
                throw (ex);
                return false;
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

        private IDbDataParameter[] PKParameters(System.Int64 ID)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);

            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int64;
            sqlParams[0].Value = ID;

            return (sqlParams);
        }

        private IDbDataParameter[] PKParameters(CLCouponsRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int64;

            sqlParams[0].Value = row.ID;
            sqlParams[0].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_ID;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(CLCouponsRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(10);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed, System.Data.DbType.Boolean);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_ExpiryDays, System.Data.DbType.Int32);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_CLCardID, System.Data.DbType.Int64);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_Points, System.Data.DbType.Decimal);   //Sprint-18 - 2090 22-Oct-2014 JY int to decimal
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_CouponValue, System.Data.DbType.Decimal);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_CreatedInTransID, System.Data.DbType.Int64);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_UsedInTransID, System.Data.DbType.Int64);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage, System.Data.DbType.Boolean);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCards_Fld_IsActive, System.Data.DbType.Boolean);//Added By Shitaljit on 24Jan14 to set used coupon as inactive
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_CLTierID, System.Data.DbType.Int64); //Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon //Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon

            sqlParams[0].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_ExpiryDays;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_CLCardID;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_Points;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_CouponValue;
            sqlParams[5].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_CreatedInTransID;
            sqlParams[6].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_UsedInTransID;
            sqlParams[7].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage;
            sqlParams[8].SourceColumn = clsPOSDBConstants.CLCards_Fld_IsActive;//Added By Shitaljit on 24Jan14 to set used coupon as inactive
            sqlParams[9].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_CLTierID; //Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon

            sqlParams[0].Value = row.IsCouponUsed;
            sqlParams[1].Value = row.ExpiryDays;
            sqlParams[2].Value = row.CLCardID;
            sqlParams[3].Value = row.Points;
            sqlParams[4].Value = row.CouponValue;
            sqlParams[5].Value = row.CreatedInTransID;
            sqlParams[6].Value = row.UsedInTransID;
            sqlParams[7].Value = row.IsCouponValueInPercentage;
            sqlParams[8].Value = true;//Added By Shitaljit on 24Jan14 to set used coupon as inactive
            sqlParams[9].Value = row.CLTierID;//Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon
            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(CLCouponsRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(11);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_ID, System.Data.DbType.Int32);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed, System.Data.DbType.Boolean);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_ExpiryDays, System.Data.DbType.Int32);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_CLCardID, System.Data.DbType.Int64);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_Points, System.Data.DbType.Decimal);   //Sprint-18 - 2090 22-Oct-2014 JY int to decimal
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_CouponValue, System.Data.DbType.Decimal);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_CreatedInTransID, System.Data.DbType.Int64);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_UsedInTransID, System.Data.DbType.Int64);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage, System.Data.DbType.Boolean);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCards_Fld_IsActive, System.Data.DbType.Boolean);//Added By Shitaljit on 24Jan14 to set used coupon as inactive
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.CLCoupons_Fld_CLTierID, System.Data.DbType.Int64); //Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon

            sqlParams[0].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_ID;
            sqlParams[1].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_IsCouponUsed;
            sqlParams[2].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_ExpiryDays;
            sqlParams[3].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_CLCardID;
            sqlParams[4].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_Points;
            sqlParams[5].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_CouponValue;
            sqlParams[6].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_CreatedInTransID;
            sqlParams[7].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_UsedInTransID;
            sqlParams[8].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_IsCouponValueInPercentage;
            sqlParams[9].SourceColumn = clsPOSDBConstants.CLCards_Fld_IsActive;//Added By Shitaljit on 24Jan14 to set used coupon as inactive
            sqlParams[10].SourceColumn = clsPOSDBConstants.CLCoupons_Fld_CLTierID; //Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon

            sqlParams[0].Value = row.ID;
            sqlParams[1].Value = row.IsCouponUsed;
            sqlParams[2].Value = row.ExpiryDays;
            sqlParams[3].Value = row.CLCardID;
            sqlParams[4].Value = row.Points;
            sqlParams[5].Value = row.CouponValue;
            sqlParams[6].Value = row.CreatedInTransID;
            sqlParams[7].Value = row.UsedInTransID;
            sqlParams[8].Value = row.IsCouponValueInPercentage;
            sqlParams[9].Value = row.IsActive;//Added By Shitaljit on 24Jan14 to set used coupon as inactive
            sqlParams[10].Value = row.CLTierID;//Added By Shitaljit on 2/4/2014 to save Tier ID while creating coupon
            return (sqlParams);
        }
        #endregion

        public void Dispose() { }

    }
}
