using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Diagnostics;
using NLog;

namespace Resources
{
    public sealed class DataHelper
    {
        private DataHelper() { }
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        private static void AttachParameters(IDbCommand command, IDbDataParameter[] commandParameters)
        {
            foreach (IDbDataParameter p in commandParameters)
            {
                if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                {
                    p.Value = DBNull.Value;
                }

                command.Parameters.Add(p);
            }
        }

        private static void AssignParameterValues(IDbDataParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                return;
            }

            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                commandParameters[i].Value = parameterValues[i];
            }
        }

        private static void PrepareCommand(IDbCommand command, IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            command.Connection = connection;

            command.CommandText = commandText;

            if (transaction != null)
            {
                command.Transaction = transaction;
            }

            command.CommandType = commandType;

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            return;
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connectionString, commandType, commandText, (IDbDataParameter[])null);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            using (IDbConnection cn = DataFactory.CreateConnection())
            {
                cn.ConnectionString = connectionString;
                cn.Open();
                return ExecuteNonQuery(cn, commandType, commandText, commandParameters);
            }
        }

        public static int ExecuteNonQuery(string commandText, params IDbDataParameter[] commandParameters)
        {
            using (IDbConnection cn = DataFactory.CreateConnection(DBConfig.ConnectionString))
            {
                return ExecuteNonQuery(cn, CommandType.Text, commandText, commandParameters);
            }
        }

        public static int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(connection, commandType, commandText, (IDbDataParameter[])null);
        }

        public static int ExecuteNonQuery(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            string generatedSql = GetGeneratedSQL(commandText, commandParameters);
            try
            {
                logger.Trace(generatedSql);
                IDbCommand cmd = DataFactory.CreateCommand();
                PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);

                int retval = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                DataFactory.DisposeCommand(cmd);
                return retval;
            }
            catch (Exception ex)
            {
                //string failedSQL = GetGeneratedSQL(commandText, commandParameters);
                logger.Error(generatedSql);
                throw (ex);
            }

        }

        public static int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(transaction, commandType, commandText, (IDbDataParameter[])null);
        }

        public static int ExecuteNonQuery(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            string generatedSql = GetGeneratedSQL(commandText, commandParameters);
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();
                logger.Trace(generatedSql);
                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
                int retval = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                DataFactory.DisposeCommand(cmd);
                return retval;
            }
            catch (Exception ex)
            {
                logger.Error(generatedSql);
                throw (ex);
            }


            return 0;
        }

        public static DataSet ExecuteDataset(string commandText)
        {
            return ExecuteDataset(DBConfig.ConnectionString, CommandType.Text, commandText);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connectionString, commandType, commandText, (IDbDataParameter[])null);
        }
        //PRIMEPOS-3187 Added Connection Close
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            DataSet dsData = null;
            using (IDbConnection cn = DataFactory.CreateConnection())
            {
                cn.ConnectionString = connectionString;
                if (cn.State != ConnectionState.Open)
                {
                    cn.Open();
                }
                dsData = ExecuteDataset(cn, commandType, commandText, commandParameters);
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                return dsData;
            }
        }

        public static DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, (IDbDataParameter[])null);
        }

        public static DataSet ExecuteDataset(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            string generatedSql = GetGeneratedSQL(commandText, commandParameters);

            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();
                logger.Trace(generatedSql);
                PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);
                IDbDataAdapter da = DataFactory.CreateDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 1200;

                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                DataFactory.DisposeCommand(cmd);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Error(generatedSql);
                throw (ex);
            }

        }

        public static DataSet ExecuteDatasetForCustomer(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {

            string generatedSql = GetGeneratedSQL(commandText, commandParameters);
            try
            {
                logger.Trace(generatedSql);
                logger.Trace("Customer  ExecuteDatasetForCustomer() Executing query to database for Customer: ");
                IDbCommand cmd = DataFactory.CreateCommand();
                PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);
                IDbDataAdapter da = DataFactory.CreateDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 1200;

                DataSet ds = new DataSet();
                da.Fill(ds);
                //POS.ErrorLogging.Logs.Logger("Customer ", " ExecuteDatasetForCustomer()", " Executing query to database for Customer:");
                cmd.Parameters.Clear();
                DataFactory.DisposeCommand(cmd);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Error(generatedSql);
                throw (ex);
            }

        }

        public static DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataset(transaction, commandType, commandText, (IDbDataParameter[])null);
        }

        public static DataSet ExecuteDataset(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            string generatedSql = GetGeneratedSQL(commandText, commandParameters);
            try
            {
                logger.Trace(generatedSql);
                IDbCommand cmd = DataFactory.CreateCommand();
                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
                IDbDataAdapter da = DataFactory.CreateDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                DataFactory.DisposeCommand(cmd);
                return ds;
            }
            catch (Exception ex)
            {
                logger.Error(generatedSql);
                throw (ex);
            }

        }

        private enum ConnectionOwnership
        {
            Internal,
            External
        }

        private static IDataReader ExecuteReader(IDbConnection connection, IDbTransaction transaction, CommandType commandType, string commandText, IDbDataParameter[] commandParameters, ConnectionOwnership connectionOwnership)
        {
            string generatedSql = GetGeneratedSQL(commandText, commandParameters);
            try
            {
                logger.Trace(generatedSql);
                IDbCommand cmd = DataFactory.CreateCommand();
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);
                IDataReader dr;
                if (connectionOwnership == ConnectionOwnership.External)
                {
                    dr = cmd.ExecuteReader();
                }
                else
                {
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
                cmd.Parameters.Clear();
                return dr;
            }
            catch (Exception ex)
            {
                logger.Error(generatedSql);
                throw (ex);
            }

        }

        public static IDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteReader(connectionString, commandType, commandText, (IDbDataParameter[])null);
        }

        public static IDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            IDbConnection cn = DataFactory.CreateConnection();
            cn.ConnectionString = connectionString;
            cn.Open();

            try
            {
                return ExecuteReader(cn, null, commandType, commandText, commandParameters, ConnectionOwnership.Internal);
            }
            catch
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }
                throw;
            }

        }

        public static IDataReader ExecuteReader(string commandText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(DBConfig.ConnectionString, CommandType.Text, commandText, commandParameters);

        }

        public static IDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, (IDbDataParameter[])null);

        }

        public static IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, (IDbDataParameter[])null);
        }

        public static IDataReader ExecuteReader(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(connection, (IDbTransaction)null, commandType, commandText, commandParameters, ConnectionOwnership.External);
        }


        public static IDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteReader(transaction, commandType, commandText, (IDbDataParameter[])null);
        }

        public static IDataReader ExecuteReader(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, ConnectionOwnership.External);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connectionString, commandType, commandText, (IDbDataParameter[])null);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            using (IDbConnection cn = DataFactory.CreateConnection())
            {
                cn.ConnectionString = connectionString;
                cn.Open();
                return ExecuteScalar(cn, commandType, commandText, commandParameters);
            }
        }

        public static object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteScalar(connection, commandType, commandText, (IDbDataParameter[])null);
        }

        public static object ExecuteScalar(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            string generatedSql = GetGeneratedSQL(commandText, commandParameters);
            try
            {
                logger.Trace(generatedSql);
                IDbCommand cmd = DataFactory.CreateCommand();
                PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);
                object retval = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                DataFactory.DisposeCommand(cmd);
                return retval;
            }
            catch (Exception ex)
            {
                logger.Error(generatedSql);

                throw (ex);
            }

        }

        public static object ExecuteScalar(string commandText, params IDbDataParameter[] commandParameters)
        {
            return ExecuteScalar(commandText, DBConfig.ConnectionString, commandParameters);

        }

        public static object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, (IDbDataParameter[])null);

        }

        public static object ExecuteScalar(string commandText, string ConnString, params IDbDataParameter[] commandParameters)
        {
            string generatedSql = GetGeneratedSQL(commandText, commandParameters);
            try
            {
                object retval = null;
                logger.Trace(generatedSql);
                using (IDbConnection connection = DataFactory.CreateConnection(ConnString))
                {
                    IDbCommand cmd = DataFactory.CreateCommand();
                    PrepareCommand(cmd, connection, (IDbTransaction)null, CommandType.Text, commandText, commandParameters);
                    retval = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    DataFactory.DisposeCommand(cmd);
                }

                return retval;
            }
            catch (Exception ex)
            {
                logger.Error(generatedSql);
                throw (ex);
            }

        }




        public static object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteScalar(transaction, commandType, commandText, (IDbDataParameter[])null);
        }

        public static object ExecuteScalar(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            string generatedSql = GetGeneratedSQL(commandText, commandParameters);
            try
            {
                logger.Trace(generatedSql);

                IDbCommand cmd = DataFactory.CreateCommand();
                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
                object retval = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                DataFactory.DisposeCommand(cmd);
                return retval;
            }
            catch (Exception ex)
            {

                logger.Error(generatedSql);
                throw (ex);
            }

        }
        public static IDbTransaction CreateTransaction()
        {
            IDbConnection oConn = DataFactory.CreateConnection(DBConfig.ConnectionString);
            return oConn.BeginTransaction();
        }

        private static string GetGeneratedSQL(string ssql, params IDbDataParameter[] commandParameters)
        {
            string strSQL = ssql;
            try
            {
                if (commandParameters != null && commandParameters.Length > 0)
                {
                    foreach (IDbDataParameter parm in commandParameters)
                    {
                        strSQL = strSQL.Replace( Convert.ToString(parm.ParameterName), "'" + Convert.ToString(parm.Value) + "'"); //PRIMEPOS-3207
                    }
                }
                else
                {
                    strSQL = ssql;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Unable to Get Query value from params");
                strSQL = ssql;

            }

            return strSQL;
        }

        #region 30-Nov-2017 JY added to return datatable
        public static DataTable ExecuteDataTable(IDbConnection connection, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            try
            {
                IDbCommand cmd = DataFactory.CreateCommand();
                PrepareCommand(cmd, connection, (IDbTransaction)null, commandType, commandText, commandParameters);
                IDbDataAdapter da = DataFactory.CreateDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandTimeout = 1200;

                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                DataFactory.DisposeCommand(cmd);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                string failedSQL = GetGeneratedSQL(commandText, commandParameters);
                logger.Error(failedSQL);
                throw (ex);
            }

        }
        #endregion

        #region PRIMEPOS-2639 27-Mar-2019 JY Added
        public static DataTable ExecuteDataTable(IDbTransaction transaction, CommandType commandType, string commandText)
        {
            return ExecuteDataTable(transaction, commandType, commandText, (IDbDataParameter[])null);
        }
        public static DataTable ExecuteDataTable(IDbTransaction transaction, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            string generatedSql = GetGeneratedSQL(commandText, commandParameters);
            try
            {
                logger.Trace(generatedSql);
                IDbCommand cmd = DataFactory.CreateCommand();
                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
                IDbDataAdapter da = DataFactory.CreateDataAdapter();
                da.SelectCommand = cmd;
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                DataFactory.DisposeCommand(cmd);
                return ds.Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(generatedSql);
                throw (ex);
            }
        }
        #endregion

        #region PRIMEPOS-3060 01-Mar-2022 JY Added
        public static DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(DBConfig.ConnectionString, CommandType.Text, commandText);
        }

        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText)
        {
            return ExecuteDataTable(connectionString, commandType, commandText, (IDbDataParameter[])null);
        }
        
        public static DataTable ExecuteDataTable(string connectionString, CommandType commandType, string commandText, params IDbDataParameter[] commandParameters)
        {
            using (IDbConnection cn = DataFactory.CreateConnection())
            {
                cn.ConnectionString = connectionString;
                cn.Open();
                return ExecuteDataTable(cn, commandType, commandText, commandParameters);
            }
        }
        #endregion
    }
}
