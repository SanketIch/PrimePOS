using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Resources
{
    public sealed class DataFactory
    {

        private static IDbDataAdapter idbDbDataAdapter = null;
        private static Assembly m_assembly = null;

        public static IDbDataParameter[] CreateParameterArray(int size)
        {

            Type t = null;

            if (m_assembly == null)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();
                AssemblyName[] b = a.GetReferencedAssemblies();
                foreach (AssemblyName obj in b)
                {
                    if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
                    {
                        m_assembly = Assembly.Load(obj.FullName);
                        break;
                    }
                }
            }

            t = m_assembly.GetType(ConfigurationSettings.AppSettings["parameter"]);
            return (IDbDataParameter[])Array.CreateInstance(t, size);

            //return (IDbDataParameter)Activator.CreateInstance(t,);

        }


        public static IDbConnection CreateConnection()
        {
            Type t = null;
            if (m_assembly == null)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();
                AssemblyName[] b = a.GetReferencedAssemblies();
                foreach (AssemblyName obj in b)
                {
                    if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
                    {

                        m_assembly = Assembly.Load(obj.FullName);
                        break;
                    }
                }
            }
            t = m_assembly.GetType(ConfigurationSettings.AppSettings["connection"]);
            return (IDbConnection)Activator.CreateInstance(t);
        }

        public static IDbConnection CreateConnection(string ConnectionString)
        {
            Type t = null;
            if (m_assembly == null)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();
                AssemblyName[] b = a.GetReferencedAssemblies();
                foreach (AssemblyName obj in b)
                {
                    if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
                    {
                        m_assembly = Assembly.Load(obj.FullName);
                        break;
                    }
                }
            }
            t = m_assembly.GetType(ConfigurationSettings.AppSettings["connection"]);
            IDbConnection oConn = (IDbConnection)Activator.CreateInstance(t);
            oConn.ConnectionString = ConnectionString;
            try
            {
                oConn.Open();
                if (oConn.State != ConnectionState.Open)
                    throw (new Exception("Database is closed."));
            }
            catch (Exception)
            {
                //ErrorHandler.throwCustomError(POSErrorENUM.Database_ConnectionFailed);
                return null;
            }
            return oConn;
        }

        public static IDbCommand CreateCommand()
        {

            Type t = null;

            if (m_assembly == null)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();
                AssemblyName[] b = a.GetReferencedAssemblies();
                foreach (AssemblyName obj in b)
                {
                    if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
                    {
                        m_assembly = Assembly.Load(obj.FullName);
                        break;
                    }
                }
            }

            t = m_assembly.GetType(ConfigurationSettings.AppSettings["command"]);

            return (IDbCommand)Activator.CreateInstance(t);
        }

        /// <summary>
        /// Added By Shitaljit to  destroy the command and connection after use.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static bool DisposeCommand(IDbCommand cmd)
        {
            bool RetVal = false;
            if (cmd != null)
            {
                IDbConnection conn = cmd.Connection;
                if (cmd.Transaction == null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    cmd.Dispose();
                    cmd = null;
                    RetVal = true;
                }
            }
            return RetVal;
        }
        public static IDbDataAdapter CreateDataAdapter()
        {
            if (idbDbDataAdapter != null)
                return idbDbDataAdapter;

            Type t = null;
            Assembly a;
            a = Assembly.GetExecutingAssembly();
            AssemblyName[] b = a.GetReferencedAssemblies();
            foreach (AssemblyName obj in b)
            {
                if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
                {
                    Assembly an;
                    an = Assembly.Load(obj.FullName);
                    t = an.GetType(ConfigurationSettings.AppSettings["dataadapter"]);
                    break;
                }
            }
            idbDbDataAdapter = (IDbDataAdapter)Activator.CreateInstance(t);
            return idbDbDataAdapter;
        }
        public static IDbDataParameter CreateParameter(string ParamName, System.Data.DbType type)
        {
            Type t = null;

            string sourceCol = "";

            if (ParamName.Substring(0, 1) == "@")
            {
                sourceCol = ParamName.Substring(1, ParamName.Length - 1);
            }
            else
            {
                sourceCol = ParamName;
                ParamName = "@" + ParamName;
            }

            Object[] arr = { ParamName, type };

            if (m_assembly == null)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();
                AssemblyName[] b = a.GetReferencedAssemblies();
                foreach (AssemblyName obj in b)
                {
                    if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
                    {
                        m_assembly = Assembly.Load(obj.FullName);
                        break;
                    }
                }
            }
            t = m_assembly.GetType(ConfigurationSettings.AppSettings["parameter"]);

            IDbDataParameter oParam = (IDbDataParameter)Activator.CreateInstance(t, arr);
            oParam.SourceColumn = sourceCol;
            return oParam;
        }

        public static IDbDataParameter CreateParameter(string ParamName, System.Data.DbType type, string sourceColumn, object value)
        {
            Type t = null;

            string sourceCol = "";

            if (ParamName.Substring(0, 1) == "@")
            {
                sourceCol = ParamName.Substring(1, ParamName.Length - 1);
            }
            else
            {
                sourceCol = ParamName;
                ParamName = "@" + ParamName;
            }

            Object[] arr = { ParamName, type };

            if (m_assembly == null)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();
                AssemblyName[] b = a.GetReferencedAssemblies();
                foreach (AssemblyName obj in b)
                {
                    if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
                    {
                        m_assembly = Assembly.Load(obj.FullName);
                        break;
                    }
                }
            }
            t = m_assembly.GetType(ConfigurationSettings.AppSettings["parameter"]);
            IDbDataParameter oParam = (IDbDataParameter)Activator.CreateInstance(t, arr);
            oParam.SourceColumn = sourceColumn;
            oParam.Value = value;
            return oParam;
        }

        public static IDbDataParameter CreateParameter()
        {
            Type t = null;
            if (m_assembly == null)
            {
                Assembly a;
                a = Assembly.GetExecutingAssembly();
                AssemblyName[] b = a.GetReferencedAssemblies();
                foreach (AssemblyName obj in b)
                {
                    if (obj.Name == ConfigurationSettings.AppSettings["assembly"])
                    {
                        m_assembly = Assembly.Load(obj.FullName);

                        break;
                    }
                }
            }
            t = m_assembly.GetType(ConfigurationSettings.AppSettings["parameter"]);
            return (IDbDataParameter)Activator.CreateInstance(t);
        }
    }

}
