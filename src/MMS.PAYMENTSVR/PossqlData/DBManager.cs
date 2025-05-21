using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.EntityClient;
using NLog;
using System.Reflection;
using DbAcc;

namespace PossqlData
{
    internal static class DBManager
    {
        static ILogger logger = LogManager.GetCurrentClassLogger();

        private static string dataSource = string.Empty;
        private static string catalog = string.Empty;
        private static string connString = string.Empty;
        private static string dBUser = string.Empty;
        private static string password = string.Empty;
        private static Dictionary<string, string> dbParameters = new Dictionary<string, string>();
        private static bool bExecuteOnce = false;   //PRIMEPOS-2723 21-Aug-2019 JY Added

        private static void GetMappedExeConfig()
        {
            string exePath = Path.Combine(Environment.CurrentDirectory, System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName + ".config");// NileshJ - replace POS.exe to System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = exePath;

            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            try
            {
                catalog = config.AppSettings.Settings["DataBase"].Value;
                dataSource = config.AppSettings.Settings["ServerName"].Value;
                if (bExecuteOnce == false)
                {
                    bExecuteOnce = true;
                    dbParameters.Add("CONNECTSTRING", "CONNECTSTRING");
                    //dbParameters.Add("CATALOG", "DataBase");
                    //dbParameters.Add("DBSERVER", "ServerName");
                    dbParameters.Add("CATALOG", "CATALOG"); //PRIMEPOS-2723 21-Aug-2019 JY set key to PrimeRx database, as we just need to fetch the sql user credentials
                    dbParameters.Add("DBSERVER", "DBSERVER");   //PRIMEPOS-2723 21-Aug-2019 JY set key to PrimeRx server, as we just need to fetch the sql user credentials
                    dbParameters.Add("DBTYPE", "DBTYPE");
                    dbParameters.Add("GSDDDB", "");
                    dbParameters.Add("USEINI", "True");
                    buildConnectionString();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, ex.Message);
            }

        }

        #region PRIMEPOS-2742 09-Oct-2019 JY Added
        private static void GetMappedExeIni()
        {
            try
            {
                if (bExecuteOnce == false)
                {
                    bExecuteOnce = true;
                    dbParameters.Add("CONNECTSTRING", "CONNECTSTRING");
                    dbParameters.Add("CATALOG", "CATALOG"); //PRIMEPOS-2723 21-Aug-2019 JY set key to PrimeRx database, as we just need to fetch the sql user credentials
                    dbParameters.Add("DBSERVER", "DBSERVER");   //PRIMEPOS-2723 21-Aug-2019 JY set key to PrimeRx server, as we just need to fetch the sql user credentials
                    dbParameters.Add("DBTYPE", "DBTYPE");
                    dbParameters.Add("GSDDDB", "");
                    dbParameters.Add("USEINI", "True");
                    buildConnectionString();
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, ex.Message);
            }
        }
        #endregion

        //Added by Arvind for the sa Password of CreditCard Transaction
        //public static void buildConnectionString()
        //{
        //    string splitConnection = "";
        //    Assembly executingAssembly = Assembly.GetExecutingAssembly();
        //    AssemblyName[] referencedAssemblies = executingAssembly.GetReferencedAssemblies();
        //    if ((DBManager.connString == null) || (DBManager.connString == string.Empty))
        //    {
        //        try
        //        {
        //            AssemblyName[] array = referencedAssemblies;
        //            for (int i = 0; i < array.Length; i++)
        //            {
        //                AssemblyName assemblyName = array[i];
        //                if (assemblyName.Name == ConfigurationSettings.AppSettings["assembly"])
        //                {
        //                    DBAccess.ApplicationName = "PrimePOS";
        //                    DBAccess objDB = DbAcc.DBType.GetDBType(dbParameters, false);
        //                    DBManager.connString = objDB.ConnectString;
        //                    splitConnection = objDB.ConnectString;  // Get ConnectionString
        //                    string[] arrConnection = splitConnection.Split(';'); // Split ConnectionString with Semicolon ';' 

        //                    DBManager.dBUser = (arrConnection[1].ToString().Split('='))[1].ToString(); // Get User
        //                    DBManager.password = (arrConnection[4].ToString().Split('='))[1].ToString(); // Get Password
        //                    //DBManager.catalog = (arrConnection[2].ToString().Split('='))[1].ToString(); // Get Database Name  //PRIMEPOS-2723 21-Aug-2019 JY Commented
        //                    //DBManager.dataSource = (arrConnection[3].ToString().Split('='))[1].ToString(); // Get Server Name //PRIMEPOS-2723 21-Aug-2019 JY Commented                            
        //                    break;  //PRIMEPOS-2723 21-Aug-2019 JY Added
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            logger.Error(ex, "An Error occured while Generating Connection string");
        //        }
        //    }
        //}

        public static void buildConnectionString()
        {
            string splitConnection = "";
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName[] referencedAssemblies = executingAssembly.GetReferencedAssemblies();
            if ((DBManager.connString == null) || (DBManager.connString == string.Empty))
            {
                try
                {
                    AssemblyName[] array = referencedAssemblies;
                    for (int i = 0; i < array.Length; i++)
                    {
                        AssemblyName assemblyName = array[i];
                        if (assemblyName.Name == ConfigurationSettings.AppSettings["assembly"])
                        {
                            DBAccess.ApplicationName = "PrimePOS";
                            //DBAccess objDB = DbAcc.DBType.GetDBType(dbParameters, false); //PRIMEPOS-2742 09-Oct-2019 JY Commented     
                            DBAccess objDB = DbAcc.DBType.GetDBType(dbParameters, true, DecisionSupportModuleType.CDS);    //PRIMEPOS-2742 09-Oct-2019 JY Added
                            DBManager.connString = objDB.ConnectString;
                            splitConnection = objDB.ConnectString;  // Get ConnectionString
                            string[] arrConnection = splitConnection.Split(';'); // Split ConnectionString with Semicolon ';' 

                            DBManager.dBUser = (arrConnection[1].ToString().Split('='))[1].ToString(); // Get User
                            DBManager.password = (arrConnection[4].ToString().Split('='))[1].ToString(); // Get Password
                            DBManager.catalog = objDB.POSDB;    //PRIMEPOS-2742 09-Oct-2019 JY Added to get Database Name    
                            DBManager.dataSource = objDB.POSServer; //PRIMEPOS-2742 09-Oct-2019 JY Added to get Server Name   
                            break;  //PRIMEPOS-2723 21-Aug-2019 JY Added
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An Error occured while Generating Connection string");
                }
            }
        }

        public static string CreateConnection()
        {
            //GetMappedExeConfig(); //PRIMEPOS-2742 09-Oct-2019 JY Commented
            GetMappedExeIni();  //PRIMEPOS-2742 09-Oct-2019 JY Added
            try
            {
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = dataSource,
                    InitialCatalog = catalog,
                    IntegratedSecurity = false,
                    UserID = dBUser,
                    Password = password,
                    MultipleActiveResultSets = true,
                    PersistSecurityInfo = true


                };

                EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                entityBuilder.Provider = "System.Data.SqlClient";

                entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
                return entityBuilder.ProviderConnectionString;

            }
            catch (Exception ex)
            {
                logger.Fatal(ex, ex.Message);

                return string.Empty;
            }



        }
    }
}
