using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using Resources;
using System.Data;
using POS_Core.ErrorLogging;

namespace POS_Core.DataAccess
{
    public class NplexSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public void Insert(int CustomerID, string pseTrxId)
        {
            IDbTransaction tx = null;
            try
            {
                IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = oConn.BeginTransaction();
                string strSQL = "INSERT INTO NplexRecovery(CustomerID, pseTrxId) VALUES(" + CustomerID + ",'" + pseTrxId + "')";
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL, null);
                tx.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Insert(int CustomerID, string pseTrxId)");
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Insert(int CustomerID, string pseTrxId)");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Insert(int CustomerID, string pseTrxId)");
                tx.Rollback();
            }            
        }

        public void Update(string pseTrxId)
        {
            IDbTransaction tx = null;
            try
            {
                IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = oConn.BeginTransaction();
                string strSQL = "UPDATE NplexRecovery SET MaxAttempts = MaxAttempts + 1 WHERE pseTrxId = '" + pseTrxId + "'";
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL, null);
                tx.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Delete(string pseTrxId)");
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Delete(string pseTrxId)");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Delete(string pseTrxId)");
                tx.Rollback();
            }
        }

        public void Delete(string pseTrxId)
        {
            IDbTransaction tx = null;
            try
            {
                IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = oConn.BeginTransaction();
                string strSQL = "DELETE FROM NplexRecovery WHERE pseTrxId = '" + pseTrxId + "'";
                DataHelper.ExecuteNonQuery(tx, CommandType.Text, strSQL, null);
                tx.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Delete(string pseTrxId)");
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Delete(string pseTrxId)");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Delete(string pseTrxId)");
                tx.Rollback();
            }
        }

        public DataTable GetNplexRecovery()
        {
            DataTable dt = null;
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = DBConfig.ConnectionString;
                    string strSQL = "SELECT pseTrxId FROM NplexRecovery WHERE AddedOn <= GETDATE() AND MaxAttempts <= 3";
                    dt = DataHelper.ExecuteDataTable(conn, CommandType.Text, strSQL, null);                    
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetNplexRecovery()");
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetNplexRecovery()");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetNplexRecovery()");
            }
            return dt;
        }

        public void Dispose() { }
    }
}
