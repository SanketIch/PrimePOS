using POS_Core.Data_Tier;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.BusinessRules
{
    public class AuditTrail : IDisposable
    {
        AuditTrailSvr trDetSvr = null;
        public DataSet oAuditDataSet = new DataSet();

        public DataTable CreateAuditLogDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EntityName", typeof(string));
            dt.Columns.Add("EntityKey", typeof(string));
            dt.Columns.Add("FieldChanged", typeof(string));
            dt.Columns.Add("OldValue", typeof(string));
            dt.Columns.Add("NewValue", typeof(string));
            dt.Columns.Add("DateChanged", typeof(DateTime));
            dt.Columns.Add("ActionBy", typeof(string));
            dt.Columns.Add("Operation", typeof(string));
            dt.Columns.Add("ApplicationName", typeof(string));
            return dt;
        }

        #region GetMethod
        public DataSet getAuditTrailLog(DateTime dtFrom, DateTime dtTo, string entityName = "", string applicationName = "", string user = "")
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                trDetSvr = new AuditTrailSvr();
                dsCcdata = trDetSvr.GetAuditTrailLog(dtFrom, dtTo, entityName, applicationName, user);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }
        public DataSet InsertAuditTrail() //Added by Durgesh
        {
            DataSet dsdata = new DataSet();
            try
            {
                trDetSvr = new AuditTrailSvr();
                dsdata = trDetSvr.InsertAuditTrail();
                
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
            return dsdata;
        }

        public DataSet getEntityName()
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                trDetSvr = new AuditTrailSvr();
                dsCcdata = trDetSvr.GetEntityName();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }

        public DataSet getApplicationName()
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                trDetSvr = new AuditTrailSvr();
                dsCcdata = trDetSvr.GetApplicationName();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }

        public DataSet getUser()
        {
            DataSet dsCcdata = new DataSet();
            try
            {
                trDetSvr = new AuditTrailSvr();
                dsCcdata = trDetSvr.GetUser();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsCcdata;
        }
        #endregion


        public void InsertPriceTaxOverride(DataSet dsAuditData)
        {
            try
            {
                trDetSvr = new AuditTrailSvr();
                for (int i = 0; i < dsAuditData.Tables[0].Rows.Count; i++)
                {
                    trDetSvr.InsertPriceTaxOverride(dsAuditData.Tables[0].Rows[i]["EntityName"].ToString(), dsAuditData.Tables[0].Rows[i]["EntityKey"].ToString(),
                        dsAuditData.Tables[0].Rows[i]["FieldChanged"].ToString(), dsAuditData.Tables[0].Rows[i]["OldValue"].ToString(),
                        dsAuditData.Tables[0].Rows[i]["NewValue"].ToString(), Convert.ToDateTime(dsAuditData.Tables[0].Rows[i]["DateChanged"].ToString()),
                        dsAuditData.Tables[0].Rows[i]["ActionBy"].ToString(), dsAuditData.Tables[0].Rows[i]["Operation"].ToString(),
                        dsAuditData.Tables[0].Rows[i]["ApplicationName"].ToString(), "");
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
