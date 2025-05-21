using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using DbAcc;

namespace MMSAppUpdater
{
    internal class clsDBConfigHelper
    {
        protected DBAccess db;

        private string m_sConnStr = "";
        SqlConnection Con = null;
        public string ConnStr
        {
            get { return m_sConnStr; }
            set { m_sConnStr = value; }
        }
        public clsDBConfigHelper(string sConnectionString)
        {
            m_sConnStr = sConnectionString;
            db = DBType.GetDBType();
        }

        public string GetLastVersion(string sApplName,string StationId,String UpdateType)
        {

            SqlConnection Connection = new SqlConnection(m_sConnStr);
            string sCurrentversion = "0.0.0.0"; // Default Version 
            string sSQL = @"SELECT 
                            * 
                            FROM AutoUpdateAppVer where ltrim(RTRIm(AppName))='" + sApplName
                             + "' AND MachineName = '" + Environment.MachineName + "' AND StationId = '" + StationId + "'";
            DataSet ds = new DataSet();
            try
            {
                

                if (Connection.State != ConnectionState.Open) Connection.Open();
                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                
                oDa.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sCurrentversion = ds.Tables[0].Rows[0]["CurrentVersion"].ToString();
                                     
                }
                //else if( UpdateType.Trim()=="SP" && GetStationId(sApplName)!="00") // means records exits for other Station
                //{
                //    sCurrentversion = "";
                //}
                //else if (UpdateType.Trim()=="SP" && StationId != "01")
                //{
                //    sCurrentversion = ""; // send the Emptery Verison no Means No update Exites for the current stations
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return sCurrentversion;
        }
        private string GetStationId(string sApplName)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            string StationId = "00";
            string sSQL = @"SELECT 
                            AppName,STATIONID
                            , (SELECT Top 1 CurrentVersion FROM AutoUpdateAppVer 
                            WHERE AppName = AutoUpVer.AppName Order By LastUpdatedAt DESC) AppCurrVersion
                            FROM AutoUpdateAppVer AutoUpVer where ltrim(RTRIm(AppName))='" + sApplName + "'";
            DataSet ds = new DataSet();
            try
            {
                
                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);

                if (Connection.State != ConnectionState.Open)
                    Connection.Open();
                
                oDa.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    StationId = ds.Tables[0].Rows[0]["STATIONID"].ToString();
                                     
                }
               
            }
            catch (Exception ex)
            {
                StationId = "00";
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return StationId;
        }

        public string getPathFromDB(string sApplicationName, string StationdId, string sMachineName)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            string sPath = "";
            string sSQL = @" SELECT [Path] FROM [AutoUpdateAppVer] WHERE StationId = '"+ StationdId +"' AND MachineName = '"+ sMachineName +"' AND AppName = '"+ sApplicationName +"' ";
            DataSet ds = new DataSet();
            try
            {

                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);

                if (Connection.State != ConnectionState.Open)
                    Connection.Open();

                oDa.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    sPath = ds.Tables[0].Rows[0]["Path"].ToString();

                }

            }
            catch (Exception ex)
            {
                sPath = "";
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return sPath;
        }

        public void SaveAppVersion(string sApplName, string sNewVersion, string StationId, string sDownloadStatus)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataSet ds = new DataSet();

            try
            {
                string sSQL = @"select * from AutoUpdateAppVer where LTRIM(RTRIM(APPNAME))='" + sApplName.Trim() + "' AND STATIONID='" + StationId + "' and  isnull([MachineName],'')='" + Environment.MachineName + "'";
                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(ds);
               
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        sSQL = @" UPDATE AutoUpdateAppVer SET " + (sNewVersion == "" ? "" : " CurrentVersion= '" + sNewVersion + "',") + " DownloadStatus = '" + sDownloadStatus.Replace("_", " ") + "'    WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "'  AND MachineName = '" + Environment.MachineName + "'";
                    }
                    else
                    {
                        sSQL = @" INSERT INTO AutoUpdateAppVer (AppName,STATIONID,MachineName,DownloadStatus) VALUES ('" + sApplName + "','" + StationId + "','" + Environment.MachineName + "','" + enmDownloadAndIntsallStatus.Latest_Version.ToString().Replace("_", " ") + "' )";
                    }
                
                SqlCommand oCmd = new SqlCommand(sSQL, Connection);

                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
        }

        public void SaveAppVersion(string sApplName, string sNewVersion, string StationId, string sPath, bool IsAlwaysRuning, bool LocalMultiInstanceAllowed, bool NetworkMultiinstanceAllowed, bool ReportRunningTime, bool IsUpdateVersion, string sDownloadStatus)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataSet ds = new DataSet();

            try
            {
                string sSQL = @"select * from AutoUpdateAppVer where LTRIM(RTRIM(APPNAME))='" + sApplName.Trim() + "' AND STATIONID='" + StationId + "' and  isnull([MachineName],'')='" + Environment.MachineName + "'";
                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(ds);
                if (ReportRunningTime == true)
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (IsUpdateVersion)
                        {
                            sSQL = @" UPDATE AutoUpdateAppVer SET CurrentVersion= '" + sNewVersion + "', Path='" + sPath + "'"
                                + ",MachineName ='" + Environment.MachineName + "' , DownloadStatus = '" + sDownloadStatus.Replace("_", " ") + "', AlwaysRunning =" + (IsAlwaysRuning == true ? "1" : "0") + ", LocalMultiInstanceAllowed=" + (LocalMultiInstanceAllowed == true ? "1" : "0") + ", NetworkMultiinstanceAllowed=" + (NetworkMultiinstanceAllowed == true ? "1" : "0") + " ,LastRecordedRunning=getdate()   WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "' AND MachineName = '" + Environment.MachineName + "'";
                        }
                        else
                        {
                            sSQL = @" UPDATE AutoUpdateAppVer SET  Path='" + sPath + "'"
                               + ",MachineName ='" + Environment.MachineName + "', AlwaysRunning =" + (IsAlwaysRuning == true ? "1" : "0") + " , DownloadStatus = '" + sDownloadStatus.Replace("_", " ") + "', LocalMultiInstanceAllowed=" + (LocalMultiInstanceAllowed == true ? "1" : "0") + ", NetworkMultiinstanceAllowed=" + (NetworkMultiinstanceAllowed == true ? "1" : "0") + " ,LastRecordedRunning=getdate()   WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "' AND MachineName = '" + Environment.MachineName + "'";
                        }
                    }
                    else
                    {
                        sSQL = @" INSERT INTO AutoUpdateAppVer (AppName,CurrentVersion,LastUpdatedAt,STATIONID,[Path],MachineName,DownloadStatus,AlwaysRunning,LocalMultiInstanceAllowed,NetworkMultiinstanceAllowed,LastRecordedRunning) VALUES ('" + sApplName + "','" + sNewVersion + "', getdate(),'" + StationId + "','" + sPath + "','" + Environment.MachineName + "','" + enmDownloadAndIntsallStatus.Latest_Version.ToString().Replace("_", " ") + "'," + (IsAlwaysRuning == true ? "1" : "0") + "," + (LocalMultiInstanceAllowed == true ? "1" : "0") + "," + (NetworkMultiinstanceAllowed == true ? "1" : "0") + ",getdate())";
                    }
                }
                else
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (IsUpdateVersion)
                        {
                            sSQL = @" UPDATE AutoUpdateAppVer SET CurrentVersion= '" + sNewVersion + "', Path='" + sPath + "'"
                                + ",MachineName ='" + Environment.MachineName + "', DownloadStatus = '" + sDownloadStatus.Replace("_", " ") + "', AlwaysRunning =" + (IsAlwaysRuning == true ? "1" : "0") + ", LocalMultiInstanceAllowed=" + (LocalMultiInstanceAllowed == true ? "1" : "0") + ", NetworkMultiinstanceAllowed=" + (NetworkMultiinstanceAllowed == true ? "1" : "0") + "    WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "' AND MachineName = '" + Environment.MachineName + "'";
                        }
                        else
                        {
                            sSQL = @" UPDATE AutoUpdateAppVer SET  Path='" + sPath + "'"
                                + ",MachineName ='" + Environment.MachineName + "', DownloadStatus = '" + sDownloadStatus.Replace("_", " ") + "', AlwaysRunning =" + (IsAlwaysRuning == true ? "1" : "0") + ", LocalMultiInstanceAllowed=" + (LocalMultiInstanceAllowed == true ? "1" : "0") + ", NetworkMultiinstanceAllowed=" + (NetworkMultiinstanceAllowed == true ? "1" : "0") + "    WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "' AND MachineName = '" + Environment.MachineName + "'";
                        }
                    }
                    else
                    {
                        sSQL = @" INSERT INTO AutoUpdateAppVer (AppName,CurrentVersion,LastUpdatedAt,STATIONID,[Path],MachineName,DownloadStatus,AlwaysRunning,LocalMultiInstanceAllowed,NetworkMultiinstanceAllowed) VALUES ('" + sApplName + "','" + sNewVersion + "', getdate(),'" + StationId + "','" + sPath + "','" + Environment.MachineName + "','" + enmDownloadAndIntsallStatus.Latest_Version.ToString().Replace("_", " ") + "'," + (IsAlwaysRuning == true ? "1" : "0") + "," + (LocalMultiInstanceAllowed == true ? "1" : "0") + "," + (NetworkMultiinstanceAllowed == true ? "1" : "0") + ")";
                    }
                }
                SqlCommand oCmd = new SqlCommand(sSQL, Connection);

                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
        }

        public void SaveAppVersion(string sApplName, string sNewVersion, string StationId, string sPath, bool IsAlwaysRuning, bool LocalMultiInstanceAllowed, bool NetworkMultiinstanceAllowed, bool ReportRunningTime, bool IsUpdateVersion, bool isRemoteOrCitrix = false, string clientMachineName = "")
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataSet ds = new DataSet();
            string strMachineName = string.Empty;

            try
            {
                if (isRemoteOrCitrix && !string.IsNullOrWhiteSpace(clientMachineName) && !string.IsNullOrWhiteSpace(sApplName) && sApplName.ToLower() == "pos")
                    strMachineName = clientMachineName;
                else
                    strMachineName = Environment.MachineName;

                string sSQL = @"select * from AutoUpdateAppVer where LTRIM(RTRIM(APPNAME))='" + sApplName.Trim() + "' AND STATIONID='" + StationId + "' and  isnull([MachineName],'')='" + strMachineName + "'";
                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                if (Connection.State!= ConnectionState.Open) Connection.Open();
                oDa.Fill(ds);
                if (ReportRunningTime == true)
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (IsUpdateVersion)
                        {
                            sSQL = @" UPDATE AutoUpdateAppVer SET CurrentVersion= '" + sNewVersion + "', Path='" + sPath + "'"
                                + ",MachineName ='" + strMachineName + "' , AlwaysRunning =" + (IsAlwaysRuning == true ? "1" : "0") + ", LocalMultiInstanceAllowed=" + (LocalMultiInstanceAllowed == true ? "1" : "0") + ", NetworkMultiinstanceAllowed=" + (NetworkMultiinstanceAllowed == true ? "1" : "0") + " ,LastRecordedRunning=getdate()   WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "' AND MachineName = '" + strMachineName + "' ";
                        }
                        else
                        {
                            sSQL = @" UPDATE AutoUpdateAppVer SET  Path='" + sPath + "'"
                               + ",MachineName ='" + strMachineName + "', AlwaysRunning =" + (IsAlwaysRuning == true ? "1" : "0") + " , LocalMultiInstanceAllowed=" + (LocalMultiInstanceAllowed == true ? "1" : "0") + ", NetworkMultiinstanceAllowed=" + (NetworkMultiinstanceAllowed == true ? "1" : "0") + " ,LastRecordedRunning=getdate()   WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "' AND MachineName = '" + strMachineName + "'";
                        }
                    }
                    else
                    {
                        sSQL = @" INSERT INTO AutoUpdateAppVer (AppName,CurrentVersion,LastUpdatedAt,STATIONID,[Path],MachineName,AlwaysRunning,LocalMultiInstanceAllowed,NetworkMultiinstanceAllowed,LastRecordedRunning) VALUES ('" + sApplName + "','" + sNewVersion + "', getdate(),'" + StationId + "','" + sPath + "','" + strMachineName + "'," + (IsAlwaysRuning == true ? "1" : "0") + "," + (LocalMultiInstanceAllowed == true ? "1" : "0") + "," + (NetworkMultiinstanceAllowed == true ? "1" : "0") + ",getdate())";
                    }
                }
                else
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (IsUpdateVersion)
                        {
                            sSQL = @" UPDATE AutoUpdateAppVer SET CurrentVersion= '" + sNewVersion + "', Path='" + sPath + "'"
                                + ",MachineName ='" + strMachineName + "', AlwaysRunning =" + (IsAlwaysRuning == true ? "1" : "0") + ", LocalMultiInstanceAllowed=" + (LocalMultiInstanceAllowed == true ? "1" : "0") + ", NetworkMultiinstanceAllowed=" + (NetworkMultiinstanceAllowed == true ? "1" : "0") + "    WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "' AND MachineName = '" + strMachineName + "'";
                        }
                        else
                        {
                            sSQL = @" UPDATE AutoUpdateAppVer SET  Path='" + sPath + "'"
                                + ",MachineName ='" + strMachineName + "', AlwaysRunning =" + (IsAlwaysRuning == true ? "1" : "0") + ", LocalMultiInstanceAllowed=" + (LocalMultiInstanceAllowed == true ? "1" : "0") + ", NetworkMultiinstanceAllowed=" + (NetworkMultiinstanceAllowed == true ? "1" : "0") + "    WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "' AND MachineName = '" + strMachineName + "'";
                        }
                    }
                    else
                    {
                        sSQL = @" INSERT INTO AutoUpdateAppVer (AppName,CurrentVersion,LastUpdatedAt,STATIONID,[Path],MachineName,AlwaysRunning,LocalMultiInstanceAllowed,NetworkMultiinstanceAllowed) VALUES ('" + sApplName + "','" + sNewVersion + "', getdate(),'" + StationId + "','" + sPath + "','" + strMachineName + "'," + (IsAlwaysRuning == true ? "1" : "0") + "," + (LocalMultiInstanceAllowed == true ? "1" : "0") + "," + (NetworkMultiinstanceAllowed == true ? "1" : "0") + ")";
                    }
                }
                SqlCommand oCmd = new SqlCommand(sSQL, Connection);

                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
        }

        public DataTable GetAppRecordForMachine(string AppName, string MachinerName)
        {
            string sSQL = "Select AppName, CurrentVersion, LastUpdatedAt, StationId, UpdateType, Path, MachineName, AlwaysRunning, LocalMultiInstanceAllowed, NetworkMultiInstanceAllowed, LastRecordedRunning, DownloadStatus from AutoUpdateAppVer where LTRIM(RTRIM(APPNAME))='" + AppName.Trim() + "' AND MachineName='" + Environment.MachineName + "' ";
            DataSet oDs;
            this.db.Fill(sSQL, "AutoUpdateAppVer", out oDs);
            if (oDs != null && oDs.Tables.Count > 0)
                return oDs.Tables[0];
            else
                return null;
        }

        public string GetStatus(string sApplName, string StationId)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();
            string sDownloadStatus = string.Empty;

            try
            {

                string sSQL = @" select IsNull(DownloadStatus,'') As DownloadStatus
                                 from AutoUpdateAppVer 
                                    where LTRIM(RTRIM(APPNAME))='" + sApplName.Trim() + "' AND STATIONID='" + StationId + "' AND MachineName='" + Environment.MachineName + "' ";
                    
                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(Dt);
                if (Dt.Rows.Count > 0)
                {
                    sDownloadStatus =  Dt.Rows[0]["DownloadStatus"].ToString().Replace(" ","_");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return sDownloadStatus;
        }
        public string GetDbVer(string sApplName, string StationId)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();
            string sDbver = string.Empty;

            try
            {

                string sSQL = @" select IsNull(CurrentVersion,'') As CurrentVersion
                                 from AutoUpdateAppVer 
                                    where LTRIM(RTRIM(APPNAME))='" + sApplName.Trim() + "' AND STATIONID='" + StationId + "' AND MachineName='" + Environment.MachineName + "' ";

                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(Dt);
                if (Dt.Rows.Count > 0)
                {
                    sDbver = Dt.Rows[0]["CurrentVersion"].ToString();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return sDbver;
        }
        public DataTable GetStationsList()
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();
         
            try
            {
                string sSQL = @" select LTRIM(RTRIM(APPNAME)) APPNAME ,STATIONID ,UPDATETYPE, MachineName, LastRecordedRunning, Path
                                 from AutoUpdateAppVer ";
              SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
              if (Connection.State != ConnectionState.Open) Connection.Open();
              oDa.Fill(Dt);
              }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return Dt;
        }
        public DataTable GetAppList()
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();

            try
            {
                string sSQL = @" select Distinct LTRIM(RTRIM(APPNAME)) APPNAME from AutoUpdateAppVer ";
                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return Dt;
        }
        public bool RunningOnAnyOtherMachine(string appname)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            try
            {
                DataTable Dt = new DataTable();
                string sql = @"SELECT isnull(LastRecordedRunning,'1/1/1900') as LastRecordedRunning
                                FROM [AutoUpdateAppVer] where rtrim(ltrim(AppName))='" + appname.Trim() + "' and isnull([MachineName],'')<>'" + Environment.MachineName + "' ";
                SqlDataAdapter oDa = new SqlDataAdapter(sql, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(Dt);

                foreach (DataRow oDr in Dt.Rows)
                {
                    if (DateTime.Parse(oDr["LastRecordedRunning"].ToString()).AddHours(1) > DateTime.Now)
                        return true;
                }

                return false;
            }
            catch (Exception exp)
            {
                return false;
            }
            finally
            {
                if (Connection.State== ConnectionState.Open) Connection.Close();
            }
        }

        // Added by M. Ali on 10 Feb, 2010 for primerx-1748
        public DataTable GetMachinesRunningPOSOnStation(string Stationid)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();
            try
            {
                string sql = @"SELECT top 1 *
                                FROM [AutoUpdateAppVer] where rtrim(ltrim(AppName))='POS' and MachineName IS Not NULL AND (Path IS NOT NULL) AND  LEN(Path)>0 AND StationId='" + Stationid + "' AND AutoUpdateAppVer.Path NOT LIKE('%\\bin\\Debug\\POS.exe%') order by LastRecordedRunning Desc";
                SqlDataAdapter oDa = new SqlDataAdapter(sql, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(Dt);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
                
            }
            return Dt;
        }

        //Added By M. Ali on 10 Feb, 2014 for primerx-1748
        public DataTable GetAvailableStationsId()
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();
            try
            {
                string sql = @"DECLARE @MaxID as INT;
                            (SELECT @MaxID=99)
                            SELECT top 6 right('0' +CONVERT(VARCHAR, SeqID, 2),2) AS StationId
                             FROM 
                              (SELECT ROW_NUMBER() OVER (ORDER BY column_id) SeqID from sys.columns) LkUp
                               LEFT JOIN syshard t ON t.userID = LkUp.SeqID
                                WHERE t.userID is null and SeqID < @MaxID AND SeqID NOT IN(select distinct STationId from AutoUpdateAppVer)";
                SqlDataAdapter oDa = new SqlDataAdapter(sql, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(Dt);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();

            }
            return Dt;
        }

        public DataTable GetLastestRecordedRunningOnDifferentMachine(string appname,string Stationid)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();
            DataTable Dt1 = new DataTable();
            try
            {
                string sql=@"SELECT isnull(LastRecordedRunning,'1/1/1900') as LastRecordedRunning
                                FROM [AutoUpdateAppVer] where rtrim(ltrim(AppName))='"+ appname.Trim() +"' and isnull([MachineName],'')='" + Environment.MachineName + "' ";
                SqlDataAdapter oDa = new SqlDataAdapter(sql, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(Dt1);
                if (Dt1.Rows.Count > 0)
                {
                    string sSQL = @" SELECT isnull(LastRecordedRunning,'1/1/1900') as LastRecordedRunning
                                FROM [AutoUpdateAppVer] where rtrim(ltrim(AppName))='" + appname.Trim() + "' and isnull([MachineName],'')<>'" + Environment.MachineName + "' ";
                     oDa = new SqlDataAdapter(sSQL, Connection);
                     oDa.Fill(Dt);
                }
               
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                if (Connection.State== ConnectionState.Open) Connection.Close();
            }

            return Dt;
        }

        public DataTable GetAutoUpdateAppVerTableData()
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();

            try
            {
                string sSQL = @" SELECT *
                                FROM [AutoUpdateAppVer] where isnull([MachineName],'')='" + Environment.MachineName  + "'";
                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return Dt;
        }

        public DataTable GetInstalledAlwaysRunningProductList(string StationId)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();

            try
            {
                string sSQL = @" SELECT [AppName]
                              ,[CurrentVersion],[LastUpdatedAt],[StationId],[UpdateType],[Path],[MachineName],isnull(AlwaysRunning,0) as AlwaysRunning,isnull(LocalMultiInstanceAllowed,0) as LocalMultiInstanceAllowed ,isnull(NetworkMultiinstanceAllowed,0) as NetworkMultiinstanceAllowed ,isnull(LastRecordedRunning,'1/1/1900') as LastRecordedRunning
                                FROM [AutoUpdateAppVer] where isnull([Path],'') <>'' and  isnull([MachineName],'')='" + Environment.MachineName + "'  and  isnull([StationId],'')='" + StationId + "'";
                 SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                 if (Connection.State != ConnectionState.Open)  Connection.Open();
                oDa.Fill(Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return Dt;
        }

        public DataTable GetAutoUpdateAppVerTable()
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            DataTable Dt = new DataTable();

            try
            {
                string sSQL = @" SELECT [AppName]
                              ,[CurrentVersion],[LastUpdatedAt],[StationId],[UpdateType],[Path],[MachineName],isnull(AlwaysRunning,0) as AlwaysRunning,isnull(LocalMultiInstanceAllowed,0) as LocalMultiInstanceAllowed ,isnull(NetworkMultiinstanceAllowed,0) as NetworkMultiinstanceAllowed ,isnull(LastRecordedRunning,'1/1/1900') as LastRecordedRunning
                                FROM [AutoUpdateAppVer] where isnull([Path],'') <>'' and  isnull([MachineName],'')='" + Environment.MachineName + "'";
                SqlDataAdapter oDa = new SqlDataAdapter(sSQL, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                oDa.Fill(Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return Dt;
        }
        public bool UpdateSystemStation(string PrvStationId, string StationId, string sAppName)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            int Result = 0;
           
            try
            {
                string sSQL = @" UPdate AutoUpdateAppVer  SET STATIONID='" + StationId.ToString()+"'"
                                + " WHERE LTRIM(RTRIM(APPNAME))='" + sAppName + "' AND STATIONID ='" + PrvStationId.ToString()+"'";
                SqlCommand oCmd = new SqlCommand(sSQL, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
               Result=oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return Convert.ToBoolean(Result);
        }
        public bool DeleteStation(string PrvStationId, string StationId, string sAppName)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            int Result = 0;

            try
            {
                string sSQL = @" Delete From AutoUpdateAppVer "
                                + " WHERE LTRIM(RTRIM(APPNAME))='" + sAppName + "' AND STATIONID ='" + PrvStationId.ToString() + "'";
                SqlCommand oCmd = new SqlCommand(sSQL, Connection);
                if (Connection.State != ConnectionState.Open) Connection.Open();
                Result = oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
            return Convert.ToBoolean(Result);
        }
        public void SaveAppVersion(string StationId)
        {
            SqlConnection Connection = new SqlConnection(m_sConnStr);
            try
            {
              string sNewVersion =FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion;
              string sApplName = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
                string    sSQL = @" UPDATE AutoUpdateAppVer SET 
                                    CurrentVersion= '" + sNewVersion + "'" + " WHERE LTRIM(RTRIM(APPNAME)) ='" + sApplName + "' AND STATIONID = '" + StationId + "'";
                         SqlCommand oCmd = new SqlCommand(sSQL, Connection);
                         if (Connection.State != ConnectionState.Open) Connection.Open();
                         oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
            }
        }
    }
}
