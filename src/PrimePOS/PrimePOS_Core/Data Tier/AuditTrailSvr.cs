using NLog;
using POS_Core.ErrorLogging;
using POS_Core.Resources;
using Resources;
using System;
using System.Data;

namespace POS_Core.Data_Tier
{
    public class AuditTrailSvr : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public DataSet GetAuditTrailLog(DateTime dtFrom, DateTime dtTo, string entityName, string applicationName, string user)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = Configuration.ConnectionString;
                    string sSQL = "";
                    //sSQL = "select AuditID, EntityName, FieldChanged, OldValue, NewValue, DateChanged, ActionBy, Operation, ApplicationName , SQLUser  from AuditTrail "
                    //+ " where  AuditID is not null  "; 
                    sSQL = "select AuditID, EntityName, FieldChanged, OldValue, NewValue, DateChanged, ActionBy, Operation, ApplicationName from AuditTrail "
                   + " where  AuditID is not null  ";
                    if (dtFrom != null && dtTo != null)
                    {
                        sSQL = sSQL + " and Convert(datetime, DateChanged, 109) between cast('" + dtFrom.ToShortDateString() + " 00:00:00 ' as datetime) and cast('" + dtTo.ToShortDateString() + " 23:59:59 ' as datetime)";
                    }
                    if (entityName != "All")
                    {
                        sSQL += " and EntityName = '" + entityName + "'";
                    }
                    if (applicationName != "All")
                    {
                        sSQL += " and ApplicationName = '" + applicationName + "'";
                    }
                    if (user != "All")
                    {
                        sSQL += " and ActionBy = '" + user + "'";
                    }
                    sSQL += " order by AuditID desc";
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetAuditTrailLog(DateTime dtFrom , DateTime dtTo , string entityName , string applicationName , string user )");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        public DataSet InsertAuditTrail()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = Configuration.ConnectionString;
                    string sSQL = "";
                    sSQL = "Insert into AuditTrail Values('Evertec','Settlement','','','',CURRENT_TIMESTAMP,'POS','I','PrimePOS',(select SQLUser from dbo.fnAppNameUserName(app_name())))";
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception Ex)
            {
                ErrorHandler.throwException(Ex,"","");
                return null;
            }

        }

        public DataSet GetEntityName()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "";
                    sSQL = " select ' All' as EntityName union select distinct EntityName from AuditTrail ";     
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetEntityName()");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetUser()
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "";
                    sSQL = " select ' All' as ActionBy union select distinct ActionBy  from AuditTrail WHERE ACTIONBY IS NOT NULL";//Arvind Audit Log
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetUser()");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetApplicationName()
        {
            
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "";
                    sSQL = " select ' All' as ApplicationName union  select distinct ApplicationName  from AuditTrail ";
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetApplicationName()");
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public void InsertPriceTaxOverride(string EntityName, string EntityKey, string FieldChanged, string OldValue, string NewValue,DateTime DateChanged, string ActionBy, string Operation, string ApplicationName, string SqlUser)
        {
            try
            {
                using (IDbConnection conn = DataFactory.CreateConnection())
                {
                    conn.ConnectionString = POS_Core.Resources.Configuration.ConnectionString;
                    string sSQL = "";
                    sSQL = "INSERT INTO AuditTrail (EntityName,EntityKey,FieldChanged,OldValue,NewValue,DateChanged,ActionBy,Operation,ApplicationName,SqlUser)" + 
                            " VALUES('"+ EntityName + "','" + EntityKey + "','" + FieldChanged + "','" + OldValue + "','" + NewValue + "','" + DateChanged + "','" + ActionBy + "','" + Operation + "','" + ApplicationName + "','" + conn.ConnectionString.Split(';')[1].Split('=')[1] + "') ";
                    DataSet ds = new DataSet();
                    ds = DataHelper.ExecuteDataset(conn, CommandType.Text, sSQL);
                   
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "InsertPriceTaxOverride()");
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        public void Dispose() 
        { 
        }
    }
}
