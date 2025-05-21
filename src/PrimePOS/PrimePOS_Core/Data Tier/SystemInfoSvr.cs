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
using System.Data;
using POS_Core.Resources;
//using POS.Resources;
using NLog;

//Sprint-22 - PRIMEPOS-2245 15-Oct-2015 JY Added class
namespace POS_Core.DataAccess
{
	

	public class SystemInfoSvr: IDisposable  
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void Persist(DataSet updates, char ch)
        {

            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = conn.BeginTransaction();
                if (ch == 'I')
                    this.InsertData(updates, tx);
                else
                    this.UpdateData(updates, tx);

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
                logger.Fatal(ex, "Persist(DataSet updates, char ch)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void InsertData(DataSet ds, IDbTransaction tx)
        {

            SystemInfoTable addedTable = (SystemInfoTable)ds.Tables[0].GetChanges(DataRowState.Added);
            string sSQL;
            IDbDataParameter[] insParam;

            if (addedTable != null && addedTable.Rows.Count > 0)
            {
                foreach (SystemInfoRow row in addedTable.Rows)
                {
                    try
                    {
                        insParam = InsertParameters(row);
                        sSQL = BuildInsertSQL(clsPOSDBConstants.SystemInfo_tbl, insParam);
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
                        throw (ex);
                    }

                    catch (Exception ex)
                    {
                        logger.Fatal(ex, "InsertData(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
                addedTable.AcceptChanges();
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
                sInsertSQL = sInsertSQL + " , " + delParam[i].ParameterName;
            }
            sInsertSQL = sInsertSQL + " )";
            return sInsertSQL;
        }

        public void UpdateData(DataSet ds, IDbTransaction tx)
        {
            SystemInfoTable oSystemInfoTable = (SystemInfoTable)ds.Tables[0].GetChanges(DataRowState.Added);

            string sSQL;
            IDbDataParameter[] updParam;

            if (oSystemInfoTable != null && oSystemInfoTable.Rows.Count > 0)
            {
                foreach (SystemInfoRow row in oSystemInfoTable.Rows)
                {
                    try
                    {
                        updParam = UpdateParameters(row);
                        sSQL = BuildUpdateSQL(clsPOSDBConstants.SystemInfo_tbl, updParam);

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
                        logger.Fatal(ex, "UpdateData(DataSet ds, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                        //ErrorHandler.throwException(ex, "", "");
                    }
                }
            }
        }

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[0].SourceColumn + "  = " + updParam[0].ParameterName;

            for (int i = 1; i < updParam.Length; i++)
            {
                sUpdateSQL += " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }
            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[3].SourceColumn + " = " + updParam[3].ParameterName;
            return sUpdateSQL;
        }

        public SystemInfoData GetBySystemName(string SystemName)
        {
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                return PopulateList(" where SystemName = '" + SystemName + "'", conn);
            }
        }

        public SystemInfoData PopulateList(string sWhereClause, IDbConnection conn)
        {
            try
            {
                string sSQL = String.Concat("Select * "
                                    + " FROM "
                                        + clsPOSDBConstants.SystemInfo_tbl
                                    , sWhereClause);

                SystemInfoData ds = new SystemInfoData();
                ds.SystemInfo.MergeTable(DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL, whereParameters(sWhereClause)).Tables[0]);
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

        public DataSet GetApplicationList(string AppType)
        {
            try
            {
                string sSQL = String.Concat("Select * FROM ApplicationMaster WHERE AppType = '" + AppType + "'");
                using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
                {
                    DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
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
                logger.Fatal(ex, "GetApplicationList(string AppType)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetApplicationLog(string SystemName)
        {
            string sSQL = String.Concat("select a.* from ApplicationLog a INNER JOIN ApplicationMaster b on a.AppId = b.AppId WHERE b.AppType = 'exe' AND a.SystemName ='" + SystemName + "'");
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
        }

        public DataSet GetDllLog()
        {
            string sSQL = String.Concat("select a.* from ApplicationLog a INNER JOIN ApplicationMaster b on a.AppId = b.AppId WHERE b.AppType = 'dll'");
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
        }

        public void PersistApplicationLog(DataTable updates)
        {
            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = conn.BeginTransaction();
                this.UpdateApplicationLog(updates, tx);

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
                logger.Fatal(ex, "PersistApplicationLog(DataTable updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void UpdateApplicationLog(DataTable dt, IDbTransaction tx)
        {
            string strSQL = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["UpdStatus"].ToString() == "I")
                {
                    if (strSQL == string.Empty)
                    {
                        strSQL = " INSERT INTO ApplicationLog (PharmNo,SystemName,AppId,StationId,Version,BuildDate,AppPath,UpdatedOn,Status) VALUES ('" +
                            Configuration.convertNullToString(dt.Rows[i]["PharmNo"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["SystemName"]).Replace("'", "''") + "'," +
                            Configuration.convertNullToInt(dt.Rows[i]["AppId"]) + ",'" +
                            Configuration.convertNullToString(dt.Rows[i]["StationId"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["Version"]).Replace("'", "''") + "','" +
                            dt.Rows[i]["BuildDate"].ToString() + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["AppPath"]).Replace("'", "''") + "', GetDate(),0);";
                    }
                    else
                    {
                        strSQL += " INSERT INTO ApplicationLog (PharmNo,SystemName,AppId,StationId,Version,BuildDate,AppPath,UpdatedOn,Status) VALUES ('" +
                            Configuration.convertNullToString(dt.Rows[i]["PharmNo"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["SystemName"]).Replace("'", "''") + "'," +
                            Configuration.convertNullToInt(dt.Rows[i]["AppId"]) + ",'" +
                            Configuration.convertNullToString(dt.Rows[i]["StationId"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["Version"]).Replace("'", "''") + "','" +
                            dt.Rows[i]["BuildDate"].ToString() + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["AppPath"]).Replace("'", "''") + "', GetDate(),0);";
                    }
                }
                else if (dt.Rows[i]["UpdStatus"].ToString() == "U")
                {
                    if (strSQL == string.Empty)
                    {
                        strSQL = " UPDATE ApplicationLog SET " +
                                " Version = '" + Configuration.convertNullToString(dt.Rows[i]["Version"]).Replace("'", "''") + 
                                "', AppPath = '" + Configuration.convertNullToString(dt.Rows[i]["AppPath"]).Replace("'", "''") +
                                "', BuildDate = '" + dt.Rows[i]["BuildDate"].ToString() +
                                " ', UpdatedOn = GetDate(), Status = 0 WHERE SystemName = '" + dt.Rows[i]["SystemName"].ToString().Replace("'", "''") + 
                                "' AND AppId = " + dt.Rows[i]["AppId"].ToString() + " AND StationId = '" + dt.Rows[i]["StationId"].ToString().Replace("'", "''") + "';";
                    }
                    else
                    {
                        strSQL += " UPDATE ApplicationLog SET " +
                                " Version = '" + Configuration.convertNullToString(dt.Rows[i]["Version"]).Replace("'", "''") +
                                "', AppPath = '" + Configuration.convertNullToString(dt.Rows[i]["AppPath"]).Replace("'", "''") +
                                "', BuildDate = '" + dt.Rows[i]["BuildDate"].ToString() +
                                " ', UpdatedOn = GetDate(), Status = 0 WHERE SystemName = '" + dt.Rows[i]["SystemName"].ToString().Replace("'", "''") + 
                                "' AND AppId = " + dt.Rows[i]["AppId"].ToString() + " AND StationId = '" + dt.Rows[i]["StationId"].ToString().Replace("'", "''") + "';";
                    }
                }
            }
             
            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL);
        }

        #region Sprint-25 - PRIMEPOS-2245 06-Mar-2017 JY Added logic to log POS settings
        public DataSet GetSystemLevelSettings()
        {
            string sSQL = "select StoreID, CompanyName, MerchantNo from Util_Company_Info; select User_ID,Merchant,Processor_ID,Payment_Server,Port_No,Payment_Client,Payment_ResultFile,Application_Name,XCClientUITitle,LicenseID,SiteID,DeviceID,URL,VCBin,MCBin from MerchantConfig";
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
        }

        public DataSet GetSystemLevelSettingsLog()
        {
            string sSQL = "SELECT PharmNo, StoreID, CompanyName, MerchantNo, MerchantConfig_User_ID, MerchantConfig_Merchant, MerchantConfig_Processor_ID, MerchantConfig_Payment_Server, MerchantConfig_Port_No, MerchantConfig_Payment_Client, MerchantConfig_Payment_ResultFile, MerchantConfig_Application_Name, MerchantConfig_XCClientUITitle, MerchantConfig_LicenseID, MerchantConfig_SiteID, MerchantConfig_DeviceID, MerchantConfig_URL, MerchantConfig_VCBin, MerchantConfig_MCBin, UpdatedOn, Status FROM SystemLevelSettingsLog";
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
        }

        public void PersistSystemLevelSettingsLog(DataTable dtSystemLevelSettingsLog, string sRecordStatus)
        {
            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = conn.BeginTransaction();
                this.UpdateSystemLevelSettingsLog(dtSystemLevelSettingsLog, tx, sRecordStatus);

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
                logger.Fatal(ex, "PersistSystemLevelSettingsLog(DataTable dtSystemLevelSettingsLog, string sRecordStatus)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void UpdateSystemLevelSettingsLog(DataTable dt, IDbTransaction tx, string sRecordStatus)
        {
            string strSQL = string.Empty;
            
            if (sRecordStatus == "I")
            {
                strSQL = " INSERT INTO SystemLevelSettingsLog (PharmNo, StoreID, CompanyName, MerchantNo, MerchantConfig_User_ID, MerchantConfig_Merchant, MerchantConfig_Processor_ID, MerchantConfig_Payment_Server, MerchantConfig_Port_No, MerchantConfig_Payment_Client, MerchantConfig_Payment_ResultFile, MerchantConfig_Application_Name, MerchantConfig_XCClientUITitle, MerchantConfig_LicenseID, MerchantConfig_SiteID, MerchantConfig_DeviceID, MerchantConfig_URL, MerchantConfig_VCBin, MerchantConfig_MCBin, UpdatedOn, Status) VALUES ('" +
                        Configuration.convertNullToString(dt.Rows[0]["PharmNo"]).Replace("'","''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["StoreID"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["CompanyName"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantNo"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_User_ID"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Merchant"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Processor_ID"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Payment_Server"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Port_No"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Payment_Client"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Payment_ResultFile"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Application_Name"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_XCClientUITitle"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_LicenseID"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_SiteID"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_DeviceID"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_URL"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_VCBin"]).Replace("'", "''") + "','" +
                        Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_MCBin"]).Replace("'", "''") + "', GetDate(),0);";
                  
            }
            else if (sRecordStatus == "U")
            {
                strSQL = " UPDATE SystemLevelSettingsLog SET " +
                        "StoreID = '" + Configuration.convertNullToString(dt.Rows[0]["StoreID"]).Replace("'", "''") + "'," +
                        "CompanyName = '" + Configuration.convertNullToString(dt.Rows[0]["CompanyName"]).Replace("'", "''") + "'," +
                        "MerchantNo = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantNo"]).Replace("'", "''") + "'," +
                        "MerchantConfig_User_ID = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_User_ID"]).Replace("'", "''") + "'," +
                        "MerchantConfig_Merchant = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Merchant"]).Replace("'", "''") + "'," +
                        "MerchantConfig_Processor_ID = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Processor_ID"]).Replace("'", "''") + "'," +
                        "MerchantConfig_Payment_Server = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Payment_Server"]).Replace("'", "''") + "'," +
                        "MerchantConfig_Port_No = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Port_No"]).Replace("'", "''") + "'," +
                        "MerchantConfig_Payment_Client = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Payment_Client"]).Replace("'", "''") + "'," +
                        "MerchantConfig_Payment_ResultFile = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Payment_ResultFile"]).Replace("'", "''") + "'," +
                        "MerchantConfig_Application_Name = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_Application_Name"]).Replace("'", "''") + "'," +
                        "MerchantConfig_XCClientUITitle = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_XCClientUITitle"]).Replace("'", "''") + "'," +
                        "MerchantConfig_LicenseID = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_LicenseID"]).Replace("'", "''") + "'," +
                        "MerchantConfig_SiteID = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_SiteID"]).Replace("'", "''") + "'," +
                        "MerchantConfig_DeviceID = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_DeviceID"]).Replace("'", "''") + "'," +
                        "MerchantConfig_URL = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_URL"]).Replace("'", "''") + "'," +
                        "MerchantConfig_VCBin = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_VCBin"]).Replace("'", "''") + "'," +
                        "MerchantConfig_MCBin = '" + Configuration.convertNullToString(dt.Rows[0]["MerchantConfig_MCBin"]).Replace("'", "''") + "', UpdatedOn = GetDate(), Status = 0";
            }

            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL);
        }

        public DataSet GetStationLevelSettings()
        {
            string sSQL = "select " + Configuration.CInfo.StoreID + " AS PharmNo, StationId, StationName, UsePoleDSP, UseCashDRW, USEPINPAD, USESigPad, SigPadHostAddr, PINPADMODEL, PAYMENTPROCESSOR, HPS_USERNAME, HPS_PASSWORD, USEPRIMEPO, DefaultVendor, 'I' AS UpdStatus from Util_POSSET";
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
        }

        public DataSet GetStationLevelSettingsLog()
        {
            string sSQL = "SELECT LogId, PharmNo, StationId, StationName, UsePoleDSP, UseCashDRW, UsePinPad, USESigPad, SigPadHostAddr, PinPadModel, PaymentProcessor, HPS_UserName, HPS_Password, UsePrimePO, DefaultVendor, UpdatedOn, Status FROM StationLevelSettingsLog";
            using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
            {
                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                return ds;
            }
        }

        public void PersistStationLevelSettingsLog(DataTable updates)
        {
            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = conn.BeginTransaction();
                this.UpdateStationLevelSettingsLog(updates, tx);

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
                logger.Fatal(ex, "PersistStationLevelSettingsLog(DataTable updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void UpdateStationLevelSettingsLog(DataTable dt, IDbTransaction tx)
        {
            string strSQL = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["UpdStatus"].ToString() == "I")
                {
                    if (strSQL == string.Empty)
                    {
                        strSQL = " INSERT INTO StationLevelSettingsLog (PharmNo, StationId, StationName, UsePoleDSP, UseCashDRW, UsePinPad, USESigPad, SigPadHostAddr, PinPadModel, PaymentProcessor, HPS_UserName, HPS_Password, UsePrimePO, DefaultVendor, UpdatedOn, Status) VALUES ('" +
                            Configuration.convertNullToString(dt.Rows[i]["PharmNo"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["StationId"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["StationName"]).Replace("'","''") + "'," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePoleDSP"])) + "," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UseCashDRW"])) + "," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePinPad"])) + "," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["USESigPad"])) + ",'" +
                            Configuration.convertNullToString(dt.Rows[i]["SigPadHostAddr"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["PinPadModel"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["PaymentProcessor"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["HPS_UserName"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["HPS_Password"]).Replace("'", "''") + "'," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePrimePO"])) + ",'" +
                            Configuration.convertNullToString(dt.Rows[i]["DefaultVendor"]).Replace("'", "''") + "', GetDate(),0);";
                    }
                    else
                    {
                        strSQL += " INSERT INTO StationLevelSettingsLog (PharmNo, StationId, StationName, UsePoleDSP, UseCashDRW, UsePinPad, USESigPad, SigPadHostAddr, PinPadModel, PaymentProcessor, HPS_UserName, HPS_Password, UsePrimePO, DefaultVendor, UpdatedOn, Status) VALUES ('" +
                            Configuration.convertNullToString(dt.Rows[i]["PharmNo"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["StationId"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["StationName"]).Replace("'", "''") + "'," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePoleDSP"])) + "," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UseCashDRW"])) + "," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePinPad"])) + "," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["USESigPad"])) + ",'" +
                            Configuration.convertNullToString(dt.Rows[i]["SigPadHostAddr"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["PinPadModel"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["PaymentProcessor"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["HPS_UserName"]).Replace("'", "''") + "','" +
                            Configuration.convertNullToString(dt.Rows[i]["HPS_Password"]).Replace("'", "''") + "'," +
                            Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePrimePO"])) + ",'" +
                            Configuration.convertNullToString(dt.Rows[i]["DefaultVendor"]).Replace("'", "''") + "', GetDate(),0);";
                    }
                }
                else if (dt.Rows[i]["UpdStatus"].ToString() == "U")
                {
                    if (strSQL == string.Empty)
                    {
                        strSQL = " UPDATE StationLevelSettingsLog SET " +
                            " StationName = '" + Configuration.convertNullToString(dt.Rows[i]["StationName"]).Replace("'", "''") + "'" +
                            ", UsePoleDSP = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePoleDSP"])) +
                            ", UseCashDRW = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UseCashDRW"])) +
                            ", UsePinPad = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePinPad"])) +
                            ", USESigPad = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["USESigPad"])) +
                            ", SigPadHostAddr = '" + Configuration.convertNullToString(dt.Rows[i]["SigPadHostAddr"]).Replace("'", "''") + "'" +
                            ", PinPadModel = '" + Configuration.convertNullToString(dt.Rows[i]["PinPadModel"]).Replace("'", "''") + "'" +
                            ", PaymentProcessor = '" + Configuration.convertNullToString(dt.Rows[i]["PaymentProcessor"]).Replace("'", "''") + "'" +
                            ", HPS_UserName = '" + Configuration.convertNullToString(dt.Rows[i]["HPS_UserName"]).Replace("'", "''") + "'" +
                            ", HPS_Password = '" + Configuration.convertNullToString(dt.Rows[i]["HPS_Password"]).Replace("'", "''") + "'" +
                            ", UsePrimePO = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePrimePO"])) +
                            ", DefaultVendor = '" + Configuration.convertNullToString(dt.Rows[i]["DefaultVendor"]).Replace("'", "''") + "'" +
                            ", UpdatedOn = GetDate(), Status = 0 WHERE StationId = '" + dt.Rows[i]["StationId"].ToString() + "';";
                    }
                    else
                    {
                        strSQL += " UPDATE StationLevelSettingsLog SET " +
                            " StationName = '" + Configuration.convertNullToString(dt.Rows[i]["StationName"]).Replace("'", "''") + "'" +
                            ", UsePoleDSP = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePoleDSP"])) +
                            ", UseCashDRW = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UseCashDRW"])) +
                            ", UsePinPad = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePinPad"])) +
                            ", USESigPad = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["USESigPad"])) +
                            ", SigPadHostAddr = '" + Configuration.convertNullToString(dt.Rows[i]["SigPadHostAddr"]).Replace("'", "''") + "'" +
                            ", PinPadModel = '" + Configuration.convertNullToString(dt.Rows[i]["PinPadModel"]).Replace("'", "''") + "'" +
                            ", PaymentProcessor = '" + Configuration.convertNullToString(dt.Rows[i]["PaymentProcessor"]).Replace("'", "''") + "'" +
                            ", HPS_UserName = '" + Configuration.convertNullToString(dt.Rows[i]["HPS_UserName"]).Replace("'", "''") + "'" +
                            ", HPS_Password = '" + Configuration.convertNullToString(dt.Rows[i]["HPS_Password"]).Replace("'", "''") + "'" +
                            ", UsePrimePO = " + Configuration.convertBoolToInt(Configuration.convertNullToBoolean(dt.Rows[i]["UsePrimePO"])) +
                            ", DefaultVendor = '" + Configuration.convertNullToString(dt.Rows[i]["DefaultVendor"]).Replace("'", "''") + "'" +
                            ", UpdatedOn = GetDate(), Status = 0 WHERE StationId = '" + dt.Rows[i]["StationId"].ToString() + "';";
                    }
                }
            }

            DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL);
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

        private IDbDataParameter[] PKParameters(SystemInfoRow row)
        {
            //return a SqlParameterCollection
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(1);


            sqlParams[0] = DataFactory.CreateParameter();
            sqlParams[0].ParameterName = "@ID";
            sqlParams[0].DbType = System.Data.DbType.Int32;

            sqlParams[0].Value = row.Id;
            sqlParams[0].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_Id;

            return (sqlParams);
        }

        private IDbDataParameter[] InsertParameters(SystemInfoRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(12);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_PharmNo, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_OSName, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_Version, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_SystemName, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_SystemManufacturer, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_SystemModel, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_SystemType, System.Data.DbType.String);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_Processor, System.Data.DbType.String);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_RAM, System.Data.DbType.String);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_DriveInfo, System.Data.DbType.String);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_UpdatedOn, System.Data.DbType.Date);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_Status, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_PharmNo;
            sqlParams[1].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_OSName;
            sqlParams[2].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_Version;
            sqlParams[3].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_SystemName;
            sqlParams[4].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_SystemManufacturer;
            sqlParams[5].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_SystemModel;
            sqlParams[6].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_SystemType;
            sqlParams[7].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_Processor;
            sqlParams[8].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_RAM;
            sqlParams[9].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_DriveInfo;
            sqlParams[10].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_UpdatedOn;
            sqlParams[11].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_Status;

            sqlParams[0].Value = row.PharmNo;
            sqlParams[1].Value = row.OSName;
            sqlParams[2].Value = row.Version;
            sqlParams[3].Value = row.SystemName;
            sqlParams[4].Value = row.SystemManufacturer;
            sqlParams[5].Value = row.SystemModel;
            sqlParams[6].Value = row.SystemType;
            sqlParams[7].Value = row.Processor;
            sqlParams[8].Value = row.RAM;
            sqlParams[9].Value = row.DriveInfo;
            sqlParams[10].Value = System.DateTime.Now;
            sqlParams[11].Value = false;
            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(SystemInfoRow row)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(12);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_PharmNo, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_OSName, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_Version, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_SystemName, System.Data.DbType.String);//where clause applied on this
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_SystemManufacturer, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_SystemModel, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_SystemType, System.Data.DbType.String);
            sqlParams[7] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_Processor, System.Data.DbType.String);
            sqlParams[8] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_RAM, System.Data.DbType.String);
            sqlParams[9] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_DriveInfo, System.Data.DbType.String);
            sqlParams[10] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_UpdatedOn, System.Data.DbType.Date);
            sqlParams[11] = DataFactory.CreateParameter("@" + clsPOSDBConstants.SystemInfo_Fld_Status, System.Data.DbType.Boolean);

            sqlParams[0].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_PharmNo;
            sqlParams[1].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_OSName;
            sqlParams[2].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_Version;
            sqlParams[3].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_SystemName;
            sqlParams[4].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_SystemManufacturer;
            sqlParams[5].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_SystemModel;
            sqlParams[6].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_SystemType;
            sqlParams[7].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_Processor;
            sqlParams[8].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_RAM;
            sqlParams[9].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_DriveInfo;
            sqlParams[10].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_UpdatedOn;
            sqlParams[11].SourceColumn = clsPOSDBConstants.SystemInfo_Fld_Status;

            sqlParams[0].Value = row.PharmNo;
            sqlParams[1].Value = row.OSName;
            sqlParams[2].Value = row.Version;
            sqlParams[3].Value = row.SystemName;
            sqlParams[4].Value = row.SystemManufacturer;
            sqlParams[5].Value = row.SystemModel;
            sqlParams[6].Value = row.SystemType;
            sqlParams[7].Value = row.Processor;
            sqlParams[8].Value = row.RAM;
            sqlParams[9].Value = row.DriveInfo;
            sqlParams[10].Value = System.DateTime.Now;
            sqlParams[11].Value = false;
            return (sqlParams);
        }
        #endregion

        public void Dispose() { }   
    }
}
