// ----------------------------------------------------------------
// Library: Data Access
// Sprint-21 - 1272 26-Aug-2015 JY Added
// ----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Resources;
using POS_Core.ErrorLogging;
using NLog;

namespace POS_Core.DataAccess
{
   

    public class OtherLanguageDescSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Get Methods
        public DataSet PopulateDescInEnglish()
        {
            try
            {
                IDbConnection conn = DataFactory.CreateConnection();
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

                string strSQL = "select Id, ColumnName, Description from DescInEnglish ORDER BY Id";

                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
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
                logger.Fatal(ex, "PopulateDescInEnglish()");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet PopulateOtherLanguageDesc(long lLanguageId)
        {
            try
            {
                IDbConnection conn = DataFactory.CreateConnection();
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

                string strSQL = "SELECT * FROM OtherLanguageDesc WHERE LanguageId = " + lLanguageId;

                DataSet ds = DataHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
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
                logger.Fatal(ex, "PopulateOtherLanguageDesc(long lLanguageId)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        public void Persist(string strSQL)
        {
            IDbTransaction tx;
            IDbConnection conn = DataFactory.CreateConnection();
            conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;

            conn.Open();
            tx = conn.BeginTransaction();
            try
            {
                DataHelper.ExecuteNonQuery(POS_Core.Resources.Configuration.ConnectionString, CommandType.Text, strSQL);
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
                logger.Fatal(ex, "Persist(string strSQL)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }

        }

        

        public void Dispose() { }   
    }
}
