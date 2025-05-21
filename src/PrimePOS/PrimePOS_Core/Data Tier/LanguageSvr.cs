using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace POS.DataTier
{
    class LanguageSvr : IDisposable
    {
        public DataTable PopulateList()
        {
            DataSet ds = new DataSet();
            try
            {
                IDbConnection conn = DataFactory.CreateConnection();
                conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                string sSQL = "SELECT * FROM Language ORDER BY Name";

                ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                if (ds != null && ds.Tables.Count > 0)
                    return ds.Tables[0];
                else
                    return null;
            }
            catch (Exception Ex)
            {
                return null;
                throw Ex;
            }

        }

        #region 30-Nov-2017 JY added to get all languages records
        //public DataTable PopulateList()
        //{
        //    using (IDbConnection conn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString))
        //    {
        //        DataTable dtLanguage = DataHelper.ExecuteDataTable(conn, CommandType.StoredProcedure, "POS_GetLanguage", (IDbDataParameter[])null);
        //        return dtLanguage;
        //    }
        //}
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
