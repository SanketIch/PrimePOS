using System;
using System.Data;
using System.Collections;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using Resources;
using System.Windows.Forms;
using POS_Core.ErrorLogging;
using MMSComponentInfo;
using POS_Core.Resources;
//using POS.Resources;
using NLog;

namespace POS_Core.DataAccess
{


    public class MMSComponentInfoSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        // Inserts, updates or deletes rows in a DataSet, within a database transaction.
        public void Persist(PrimeApplicationList appList, IDbTransaction tx)
        {
            try
            {
                DeleteStationData(Configuration.StationID, tx);
                foreach (PrimeApplication app in appList)
                {
                    if (app.InstalledLocationList.Count > 0)
                    {
                        this.Insert(app, tx);
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
                logger.Fatal(ex, "Persist(PrimeApplicationList appList, IDbTransaction tx) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                                                                                                //ErrorLogging.ErrorHandler.throwException(ex,"","");
            }
        }

        // Inserts, updates or deletes rows in a DataSet.
        public void Persist(PrimeApplicationList appList)
        {

            IDbTransaction tx = null;
            try
            {
                IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = conn.BeginTransaction();
                this.Persist(appList, tx);
                tx.Commit();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(PrimeApplicationList appList) ");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
            finally
            {
                if (tx.Connection != null)// if Condition is Added By Shitaljit on 2 Sept 2011
                {
                    tx.Rollback();
                    tx.Dispose();
                }
            }
        }

        #endregion

        #region Get Methods

        public bool CheckAppRecordExist(PrimeApplication primeApp, IDbTransaction trans)
        {
            bool returnValue = false;
            try
            {
                string sSQL = "Select 1 from AutoUpdateAppVer Where StationId=@StationID And AppName=@AppName";

                IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(2);

                sqlParams[0] = DataFactory.CreateParameter();
                sqlParams[0].ParameterName = "@StationID";
                sqlParams[0].DbType = System.Data.DbType.String;
                sqlParams[0].Value = Configuration.StationID;

                sqlParams[1] = DataFactory.CreateParameter();
                sqlParams[1].ParameterName = "@AppName";
                sqlParams[1].DbType = System.Data.DbType.String;
                sqlParams[1].Value = primeApp.Name;

                object data = DataHelper.ExecuteScalar(trans, CommandType.Text, sSQL, sqlParams);

                if (data != null)
                {
                    returnValue = Configuration.convertNullToBoolean(data);
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
                logger.Fatal(ex, "CheckAppRecordExist(PrimeApplication primeApp, IDbTransaction trans)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }

            return returnValue;
        }

        #endregion //Get Method

        #region Insert, Update, and Delete Methods

        public void Insert(PrimeApplication primeApp, IDbTransaction tx)
        {

            string sSQL;
            IDbDataParameter[] insParam;

            try
            {
                foreach (InstalledLocation installedLocation in primeApp.InstalledLocationList)
                {
                    insParam = InsertParameters(primeApp, installedLocation);
                    sSQL = BuildInsertSQL(clsPOSDBConstants.AutoUpdateAppVer_tbl, insParam);

                    DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL, insParam);
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
                logger.Fatal(ex, "Insert(PrimeApplication primeApp, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void Update(PrimeApplication primeApp, IDbTransaction tx)
        {
            string sSQL;
            IDbDataParameter[] updParam;

            try
            {
                updParam = UpdateParameters(primeApp);
                sSQL = BuildUpdateSQL(clsPOSDBConstants.AutoUpdateAppVer_tbl, updParam);

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
                logger.Fatal(ex, "Update(PrimeApplication primeApp, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
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

        private string BuildUpdateSQL(string tableName, IDbDataParameter[] updParam)
        {
            string sUpdateSQL = "UPDATE " + tableName + " SET ";
            // build where clause
            sUpdateSQL = sUpdateSQL + updParam[2].SourceColumn + "  = " + updParam[2].ParameterName;

            for (int i = 3; i < updParam.Length; i++)
            {
                sUpdateSQL = sUpdateSQL + " , " + updParam[i].SourceColumn + "  = " + updParam[i].ParameterName;
            }

            sUpdateSQL = sUpdateSQL + " WHERE " + updParam[0].SourceColumn + " = " + updParam[0].ParameterName;
            sUpdateSQL = sUpdateSQL + " And " + updParam[1].SourceColumn + " = " + updParam[1].ParameterName;

            return sUpdateSQL;
        }

        public void DeleteStationData(string stationID, IDbTransaction tx)
        {

            try
            {
                string sSQL = "Delete from " + clsPOSDBConstants.AutoUpdateAppVer_tbl +
                    " where StationID='" + stationID.Replace("'", "''") + "'";

                DataHelper.ExecuteNonQuery(tx, CommandType.Text, sSQL);
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
                logger.Fatal(ex, "DeleteStationData(string stationID, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
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

        private IDbDataParameter[] InsertParameters(PrimeApplication primeApp, InstalledLocation installedLocation)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_AppName, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_CurrentVersion, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_LastUpdatedAt, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_MechineName, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_Path, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_StationId, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_UpdateType, System.Data.DbType.String);

            sqlParams[0].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_AppName;
            sqlParams[1].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_CurrentVersion;
            sqlParams[2].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_LastUpdatedAt;
            sqlParams[3].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_MechineName;
            sqlParams[4].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_Path;
            sqlParams[5].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_StationId;
            sqlParams[6].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_UpdateType;

            sqlParams[0].Value = primeApp.Name;
            sqlParams[1].Value = installedLocation.Version;
            sqlParams[2].Value = DateTime.Now;

            sqlParams[3].Value = installedLocation.MachineName;
            sqlParams[4].Value = installedLocation.Location;

            sqlParams[5].Value = Configuration.StationID;
            sqlParams[6].Value = "";

            return (sqlParams);
        }

        private IDbDataParameter[] UpdateParameters(PrimeApplication primeApp)
        {
            IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(7);

            sqlParams[0] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_AppName, System.Data.DbType.String);
            sqlParams[1] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_StationId, System.Data.DbType.String);
            sqlParams[2] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_CurrentVersion, System.Data.DbType.String);
            sqlParams[3] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_LastUpdatedAt, System.Data.DbType.String);
            sqlParams[4] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_MechineName, System.Data.DbType.String);
            sqlParams[5] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_Path, System.Data.DbType.String);
            sqlParams[6] = DataFactory.CreateParameter("@" + clsPOSDBConstants.AutoUpdateAppVer_Fld_UpdateType, System.Data.DbType.String);

            sqlParams[0].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_AppName;
            sqlParams[1].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_StationId;
            sqlParams[2].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_CurrentVersion;
            sqlParams[3].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_LastUpdatedAt;
            sqlParams[4].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_MechineName;
            sqlParams[5].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_Path;
            sqlParams[6].SourceColumn = clsPOSDBConstants.AutoUpdateAppVer_Fld_UpdateType;

            sqlParams[0].Value = primeApp.Name;
            sqlParams[1].Value = Configuration.StationID;
            sqlParams[2].Value = primeApp.InstalledLocationList[0].Version;
            sqlParams[3].Value = DateTime.Now;
            if (primeApp.InstalledLocationList.Count > 0)
            {
                sqlParams[4].Value = primeApp.InstalledLocationList[0].MachineName;
                sqlParams[5].Value = primeApp.InstalledLocationList[0].Location;
            }
            else
            {
                sqlParams[4].Value = string.Empty;
                sqlParams[5].Value = string.Empty;
            }

            sqlParams[6].Value = "";

            return (sqlParams);
        }

        #endregion

        public void Dispose() { }
    }
}
