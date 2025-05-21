using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using NLog;
using DbAcc;// PRIMEPOS-2522
namespace Resources
{
    public sealed class DBConfig
    {
        private static string m_ConnString = "";

        private static string m_DBUser;

        private static string m_Passward;

        private static string m_ServerName;

        private static string m_DatabaseName;

        private static string m_ConnStringMaster = "";

        private static Dictionary<string, string> m_DBParameters;// PRIMEPOS-2522

        static ILogger logger = LogManager.GetCurrentClassLogger();

        //private static string m_DBUser;
        //private static string m_Passward;
        //private static string m_ServerName;
        //private static string m_DatabaseName;
        // PRIMEPOS-2522
        public static Dictionary<string, string> DBParameters
        {
            get
            {
                return m_DBParameters;
            }
            set
            {
                m_DBParameters = value;
            }
        }

        public static string ConnectionString
        {
            get
            {
                if (DBConfig.m_ConnString == "")
                {
                    DBConfig.buildConnectionString();
                }
                return DBConfig.m_ConnString;
            }
        }

        public static String UserName
        {
            get
            {
                return m_DBUser;
            }
            set
            {
                m_DBUser = value;
            }
        }

        public static String Passward
        {
            get
            {
                return m_Passward;
            }
        }

        public static String ServerName
        {
            get
            {
                return m_ServerName;
            }
        }

        public static String DatabaseName
        {
            get
            {
                return m_DatabaseName;
            }
        }

        private DBConfig()
        {
        }

        private static void buildConnectionString()
        {
            string splitConnection = "";
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            AssemblyName[] referencedAssemblies = executingAssembly.GetReferencedAssemblies();
            if ((DBConfig.m_ConnString == null) || (DBConfig.m_ConnString == string.Empty))// PRIMEPOS-2522
            {
                try
                {
                    AssemblyName[] array = referencedAssemblies;
                    for (int i = 0; i < array.Length; i++)
                    {
                        AssemblyName assemblyName = array[i];
                        if (assemblyName.Name == ConfigurationSettings.AppSettings["assembly"])
                        {
                            /*
                            Assembly assembly = Assembly.Load(assemblyName.FullName);
                            DBConfig.m_DBUser = mmsclDBAccess.GetStringUA();
                            DBConfig.m_Passward = mmsclDBAccess.GetStringData(DBConfig.m_DBUser.ToString());
                            mmsclDBAccess.DBServer = DBConfig.m_ServerName = ConfigurationSettings.AppSettings["ServerName"];
                            mmsclDBAccess.Catalog = DBConfig.m_DatabaseName = ConfigurationSettings.AppSettings["DataBase"];
                            DBAccess.ApplicationName = "PrimePOS";
                            */
                            // PRIMEPOS-2522 :Start- Get Connection String and set as parameter value like UserID, Password, Database Name , Server Name - NileshJ
                            DBAccess.ApplicationName = "PrimePOS";
                            //DBAccess objDB = DbAcc.DBType.GetDBType(m_DBParameters);
                            DBAccess objDB = DbAcc.DBType.GetDBType(m_DBParameters, true, DecisionSupportModuleType.POS);
                            DBConfig.m_ConnString = objDB.ConnectString;
                            splitConnection = objDB.ConnectString;  // Get ConnectionString
                            string[] arrConnection = splitConnection.Split(';'); // Split ConnectionString with Semicolon ';' 

                            DBConfig.m_DBUser = (arrConnection[1].ToString().Split('='))[1].ToString(); // Get User
                            DBConfig.m_Passward = (arrConnection[4].ToString().Split('='))[1].ToString(); // Get Password
                            DBConfig.m_DatabaseName = (arrConnection[2].ToString().Split('='))[1].ToString(); // Get Database Name
                            DBConfig.m_ServerName = (arrConnection[3].ToString().Split('='))[1].ToString(); // Get Server Name
                            // PRIMEPOS-2522 : End
                        }
                    }
                    //DBConfig.m_ConnString = mmsclDBAccess.ConnectString;

                    //DBConfig.m_ConnString = string.Concat(new string[]
                    //{
                    //    "Data Source=",
                    //    DBConfig.m_ServerName,
                    //    ";Initial Catalog=",
                    //    DBConfig.m_DatabaseName,
                    //    ";Persist Security Info=True;User ID=sa;password=MMSPhW110"
                    //    //userCredentials
                    //});

                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An Error occured while Generating Connection string");
                }
            }
        }

        public static int convertBoolToInt(bool bValue)
        {
            try
            {
                if (bValue == true)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return 0;
            }
        }

        /// <summary>
        /// Added By shitaljit to check that datatable is null or empty
        /// return true is null or empty and if has any rows return false
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool isNullOrEmptyDataTable(System.Data.DataTable dt)
        {
            try
            {
                if (dt == null)
                    return true;
                else if (dt != null && dt.Rows.Count == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return true;
            }
        }

        /// <summary>
        /// Added By shitaljit to check that datatable is null or empty
        /// return true is null or empty and if has any rows return false
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool isNullOrEmptyDataSet(System.Data.DataSet ds)
        {
            try
            {
                if (ds == null)
                    return true;
                else if (ds != null && ds.Tables[0].Rows.Count == 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return true;

            }
        }
        public static string convertNullToString(object strValue)
        {
            string result;
            if (strValue == null)
            {
                result = "";
            }
            else
            {
                result = strValue.ToString();
            }
            return result;
        }

        public static bool convertNullToBoolean(string strValue)
        {
            bool result;
            if (strValue == null)
            {
                result = false;
            }
            else if (strValue.Trim() == "")
            {
                result = false;
            }
            else
            {
                try
                {
                    result = Convert.ToBoolean(strValue);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    result = false;
                }
            }
            return result;
        }



        public static int convertNullToInt(string strValue)
        {
            int result;
            if (strValue == null)
            {
                result = 0;
            }
            else if (strValue.Trim() == "")
            {
                result = 0;
            }
            else
            {
                try
                {
                    result = Convert.ToInt32(strValue);
                }
                catch (Exception ex )
                {
                    logger.Error(ex);
                    result = 0;
                }
            }
            return result;
        }

        public static Int64 convertNullToInt64(object strValue)
        {
            if (strValue == null)
            {
                return 0;
            }
            else if (strValue.ToString().Trim() == "")
            {
                return 0;
            }
            else
                try
                {
                    return Convert.ToInt64(strValue.ToString().Trim());
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    return 0;
                }
        }

        public static decimal convertNullToDecimal(string strValue)
        {
            decimal result;
            if (strValue == null)
            {
                result = 0m;
            }
            else if (strValue.Trim() == "")
            {
                result = 0m;
            }
            else
            {
                try
                {
                    result = Convert.ToDecimal(strValue);
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    result = 0m;
                }
            }
            return result;
        }


        //public static String ConnectionStringMaster
        //{
        //    get
        //    {
        //        if (m_ConnStringMaster == "")
        //        {
        //            ConnectionStringType = "MasterDatabase";
        //            buildConnectionString();
        //            ConnectionStringType = "";
        //        }
        //        return m_ConnStringMaster;
        //    }
        //}

        //public static string ConnectionStringType
        //{
        //    get
        //    {
        //        return S_ConnectionStringType;
        //    }
        //    set
        //    {
        //        S_ConnectionStringType = value;
        //    }
        //}

        //private static void buildConnectionString()
        //{
        //    Assembly a;
        //    a = Assembly.GetExecutingAssembly();
        //    AssemblyName[] b = a.GetReferencedAssemblies();
        //    foreach (AssemblyName obj in b)
        //    {
        //        if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
        //        {
        //            Assembly an;
        //            an = Assembly.Load(obj.FullName);

        //            if (ConnectionStringType == "MasterDatabase")
        //            {
        //                m_DBUser = "MMSI";
        //                m_Passward = "MMSPhW110";
        //            }
        //            else
        //            {
        //                if (ConnectionStringType == "sa")
        //                {
        //                    m_DBUser = "sa";
        //                    m_Passward = "MMSPhW110";
        //                }
        //                else
        //                {
        //                    m_DBUser = SQLUserID;
        //                    m_Passward = SQLUserPassword;
        //                }
        //            }
        //            m_ServerName = ConfigurationSettings.AppSettings["ServerName"];
        //            m_DatabaseName = ConfigurationSettings.AppSettings["DataBase"];
        //            //m_StationID= ConfigurationSettings.AppSettings["StationID"];
        //            m_LabelPerSheet = Convert.ToInt32(convertNullToInt(ConfigurationSettings.AppSettings["LabelPerSheet"]));
        //            m_CCProcessDB = ConfigurationSettings.AppSettings["CCProcessDB"];
        //            if (ConfigurationSettings.AppSettings["HostIPAddress"] != null)
        //            {
        //                m_HostIPAddress = ConfigurationSettings.AppSettings["HostIPAddress"];
        //            }
        //            break;
        //        }
        //    }

        //    m_ConnString = String.Concat("server=", m_ServerName, ";Database=", m_DatabaseName, ";User ID =", m_DBUser, ";Password =", m_Passward, ";");
        //    m_ConnString = m_ConnString + ";Max Pool Size=60;Min Pool Size=5;Pooling=True;";

        //    if (ConnectionStringType == "MasterDatabase")
        //        m_ConnStringMaster = m_ConnString;

        //    //GetUserSettings("");
        //}
        // PRIMEPOS-2522 -  Migrate MMSUPUser Method from clsMain 
        public static void CreateMMSSUPUser()
        {
            DBAccess.CreateMMSSUPUser(DBConfig.m_DBParameters); // Create method for MMSSUP user and take parameter from appconfig through clsMain - NileshJ
        }
    }

}
