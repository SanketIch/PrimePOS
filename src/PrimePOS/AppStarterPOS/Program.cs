using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using AppStarter.MMSUpdate;
using System.Configuration;
using System.Windows.Forms;

namespace AppStarter
{
    class Program
    {
        static clsLogService Log = new clsLogService("AutoUpdatAppStarter.log");
        static string sInstallationMode = "";
        static string sFilePath = "";

        static void Main(string[] args)
        {
            string[] Arrguments = args; // get the application path 
            String ApplicationName = "";
            string sPharmNo = "";
            string AppServerVersion = "";

            try
            {
                if (Arrguments.Length > 1)
                {
                    sFilePath=System.IO.Path.GetDirectoryName(Arrguments[1].ToString()) + "\\POS.exe";
                    System.Threading.Thread.Sleep(1000); //Create a 3 secound pause
                    Process oProcess = new Process();
                    ProcessStartInfo oProcessInformation = new ProcessStartInfo();
                    StopService(Arrguments[0].ToString());
                    Console.WriteLine("Running Set up {0}", Arrguments[0].ToString());
                    System.Threading.Thread.Sleep(5000);
                  
                    oProcessInformation.FileName = Arrguments[0].ToString(); // set the Setup file path
                    string Arrgumetns = "\"" + System.IO.Path.GetDirectoryName(Arrguments[1].ToString()) + "\""+" "+"\"" + Arrguments[2].ToString().Trim()+ "\"";;
                    oProcessInformation.Arguments = Arrgumetns;
                    oProcess.StartInfo = oProcessInformation; // get the starting information

                    oProcess.Start();
                    oProcess.WaitForExit();
                    // update the Log by inserting log to webservice

                    string Message = string.Format("Application {0} update Successfully ", Arrguments[0].ToString());
                    Log.AddMessage(Message);
                    if (Arrguments.Length > 5)
                    {
                        MmsUPdateService oMmsUpdateService = new MmsUPdateService();
                        oMmsUpdateService.Url = Arrguments[2].ToString();
                        ApplicationName = Arrguments[3].ToString();
                        sPharmNo = Arrguments[4].ToString();
                        AppServerVersion = Arrguments[5].ToString();
                        string LogMessage = Log.ReadLog(System.IO.Path.GetDirectoryName(Arrguments[0].ToString()) + "\\setup.log");
                        try
                        {
                            oMmsUpdateService.InSertLog(ApplicationName, sPharmNo, AppServerVersion, DateTime.Now, true, LogMessage);
                        }
                        catch (Exception ex)
                        {
                            Log.AddMessage(ex.Message);
                        }
                    }
                    Revoke();
                    oProcess = new Process();
                    oProcessInformation = new ProcessStartInfo();
                    oProcessInformation.FileName = Arrguments[1].ToString(); // run the application again 
                    oProcess.StartInfo = oProcessInformation; // get the starting information
                    oProcess.Start();
                }
                else
                {
                    Log.AddError(@"Installation fail Please Provide the Application Path Space Applcation Setup file path\n e.g d:\POSW\POS.exe d:\POSW\TempFolder\setup.exe");

                    if (Arrguments.Length > 5)
                    {
                        MmsUPdateService oMmsUpdateService = new MmsUPdateService();
                        oMmsUpdateService.Url = Arrguments[2].ToString();
                        ApplicationName = Arrguments[3].ToString();
                        sPharmNo = Arrguments[4].ToString();
                        AppServerVersion = Arrguments[5].ToString();
                        string LogMessage = Log.ReadLog(System.IO.Path.GetDirectoryName(Arrguments[0].ToString()) + "\\setup.log");

                        try
                        {
                            oMmsUpdateService.InSertLog(ApplicationName, sPharmNo, AppServerVersion, DateTime.Now, false, LogMessage);
                        }
                        catch (Exception ex)
                        {
                            Log.AddMessage(ex.Message);
                        }

                    }
                }
                if (Arrguments[0] != null && Arrguments[0].ToString() != "")
                {
                    System.IO.Directory.Delete(System.IO.Path.GetDirectoryName(Arrguments[0].ToString()), true);
                }
            }
            catch (Exception ex)
            {
                Log.AddError(String.Format("ERROR: {0} " + Environment.NewLine + "STACK TRACE:{1} ", ex.Message, ex.StackTrace));

                if (Arrguments.Length > 5)
                {
                    MmsUPdateService oMmsUpdateService = new MmsUPdateService();
                    oMmsUpdateService.Url = Arrguments[2].ToString();
                    ApplicationName = Arrguments[3].ToString();
                    sPharmNo = Arrguments[4].ToString();
                    AppServerVersion = Arrguments[5].ToString();
                    string LogMessage = Log.ReadLog(System.IO.Path.GetDirectoryName(Arrguments[0].ToString()) + "\\setup.log");
                    try
                    {
                        oMmsUpdateService.InSertLog(ApplicationName,
                       sPharmNo, AppServerVersion, DateTime.Now, false, LogMessage);
                    }
                    catch (Exception exp)
                    {
                        Log.AddMessage(exp.Message);
                    }

                }
            }
            finally
            {
                Log.Close();
                Revoke();
            }
          
        }

        public static void StopService(string ExeName)
        {
            try
            {
                String ServiceName = ExeName.Substring(0, ExeName.IndexOf('.'));
                Process[] ProcessList = Process.GetProcessesByName(ServiceName);
                if (ProcessList != null && ProcessList.Length > 0)
                {
                    foreach (Process pro in ProcessList)
                    {
                        pro.Kill();
                    }
                }

            }
            catch (Exception ex)
            {
                Log.AddError(String.Format("ERROR: {0} " + Environment.NewLine + "STACK TRACE:{1} ", ex.Message, ex.StackTrace));

            }
        }
        //static bool CheckExeAccess(string ExeFilePaht, string sUtilityPath,clsLogService Log)
        //{
            
        //    bool Result = true;
        //    DialogResult oDialogResult=DialogResult.Cancel;
        //    try
        //    {
               
        //        ExeConfigurationFileMap oConfigFile = new ExeConfigurationFileMap();
        //        oConfigFile.ExeConfigFilename = System.IO.Path.GetDirectoryName(sUtilityPath) + "\\SetupControls.exe.config";
        //        Configuration oConfigurationFile = ConfigurationManager.OpenMappedExeConfiguration(oConfigFile,ConfigurationUserLevel.None);
        //        sInstallationMode = oConfigurationFile.AppSettings.Settings["INSTALLATION_MODE"].Value;

               
        //        bool isPPDeliveryRunning = false;

        //        string DestinationFilePah = System.IO.Path.GetDirectoryName(ExeFilePaht) + "\\" + DateTime.Now.ToString("MMDDyyyyHHmmss");
        //        System.IO.Directory.CreateDirectory(DestinationFilePah);
        //        string sDestinationFileName = DestinationFilePah + "\\" + System.IO.Path.GetFileName(ExeFilePaht);
        //        System.IO.File.Copy(ExeFilePaht, sDestinationFileName);

        //        do
        //        {
        //            try
        //            {
        //                System.IO.File.Copy(sDestinationFileName, ExeFilePaht,true);
        //                oDialogResult = DialogResult.Cancel;
        //                Result = true;// successfull the file is not being accessed by any body
        //                isPPDeliveryRunning = false;
        //            }
        //            catch
        //            {
        //                isPPDeliveryRunning = true;
        //            }

        //            if (isPPDeliveryRunning == true)
        //            {

        //                DialogResult oDgResult=  (FrmMessageBox.Show(ExeFilePaht, sInstallationMode,Log,new System.Windows.Forms.Form()));
        //                if (oDgResult== DialogResult.Retry)
        //                    {
        //                        oDialogResult = DialogResult.Retry;
        //                        continue;
        //                    }
        //               if(oDgResult==DialogResult.OK)
        //               {
        //                   oDialogResult = DialogResult.Cancel;
        //                   Result = true;
        //               }
        //              else
        //                {
        //                    oDialogResult = DialogResult.Cancel;
        //                    Result = false;
        //                }
                   
        //            }
        //        } while (oDialogResult != DialogResult.Cancel);

        //        if (DestinationFilePah.Trim() != "")
        //            System.IO.Directory.Delete(DestinationFilePah,true);
        //    }
        //    catch
        //    {
        //        Result = false;
        //    }
        //    return Result;

        //}

        private static void Revoke()
        {
             
            System.Data.SqlClient.SqlConnection Con = null;
            try
            {
                string sSqlConnection = ReadPOSConfig();
                Con = new System.Data.SqlClient.SqlConnection(sSqlConnection);
                Con.Open();
                string Squery = @"UPDATE [Util_Company_Info]
                                SET 
                                  [ForceClosed] =0, 
                                  [TimeStemp] ='" + DateTime.Now.ToString() + "'";
                System.Data.SqlClient.SqlCommand oCmd = new System.Data.SqlClient.SqlCommand(Squery, Con);
                oCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (AppMode == "silent")
                {
                    Log.AddError(ex.Message);
                }
                else
                {
                    MessageBox.Show(ex.Message, "AppStarter");
                }
            }
            finally
            {
                if (Con != null && Con.State == System.Data.ConnectionState.Open)
                {
                    Con.Close();
                }
            }
        }

        public static string ReadPOSConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(sFilePath);
            string strServerName = config.AppSettings.Settings["ServerName"].Value;
            string strDatabaseName = config.AppSettings.Settings["DataBase"].Value;
            
            string ConnectionString = "SERVER=" + strServerName + ";DATABASE=" + strDatabaseName + ";User ID=sa;Password=MMSPhW110;";
            
            return ConnectionString;
        }

        public static string AppMode
         {
             get { return sInstallationMode; }               
        }

        public static clsLogService Logobj
        {
            get { return Log; }
        }
    }
}
