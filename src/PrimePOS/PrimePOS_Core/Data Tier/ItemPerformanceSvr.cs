using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Resources;
using POS_Core.Resources;

namespace POS_Core.DataAccess
{
    public class ItemPerformanceSvr
    {
        public DataSet getItemPerformData(string ItemID, DateTime FromDate, DateTime ToDate)
        {
            DataSet dsCustData = new DataSet();
            try
            {
                using (SqlConnection conn = new SqlConnection(Configuration.ConnectionString))
                {
                    IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);
                    sqlParams[0] = DataFactory.CreateParameter();
                    sqlParams[0].DbType = System.Data.DbType.String;
                    sqlParams[0].Direction = ParameterDirection.Input;
                    sqlParams[0].ParameterName = "@ItemCode";
                    sqlParams[0].Value = ItemID;

                    sqlParams[1] = DataFactory.CreateParameter();
                    sqlParams[1].DbType = System.Data.DbType.DateTime;
                    sqlParams[1].Direction = ParameterDirection.Input;
                    sqlParams[1].ParameterName = "@fromDate";
                    sqlParams[1].Value = FromDate;

                    sqlParams[2] = DataFactory.CreateParameter();
                    sqlParams[2].DbType = System.Data.DbType.DateTime;
                    sqlParams[2].Direction = ParameterDirection.Input;
                    sqlParams[2].ParameterName = "@toDate";
                    sqlParams[2].Value = ToDate;


                    dsCustData = DataHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "getItemData", sqlParams);
                    return dsCustData;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
            //return dsCustData;
        }
    }
}
