using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
namespace POS_Core.BusinessRules
{
    
    
    public class Util_UserOptionDetailRights : IDisposable 
    {
        public void Persist( Util_UserOptionDetailRightsData updates)
        {
            try
            {
                using (Util_UserOptionDetailRightsSvr dao = new Util_UserOptionDetailRightsSvr())
                {
                    checkIsValidData(updates);
                    dao.Persist(updates);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #region Get Methods

        public Util_UserOptionDetailRightsData Populate(System.String DeptCode)
        {
            try
            {
                using (Util_UserOptionDetailRightsSvr dao = new Util_UserOptionDetailRightsSvr())
                {
                    return dao.Populate(DeptCode);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //public bool DeleteRow(string CurrentID)
        //{
        //    string sSQL;
        //    try
        //    {
        //        sSQL = " delete from Customer where CustomerID= '" + CurrentID + "'";
        //        DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorHandler.throwException(ex, "", "");
        //        return false;
        //    }
        //}

        public delegate void DataRowSavedHandler();
        public event DataRowSavedHandler DataRowSaved;

        public bool DeleteRow(int  ID)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                Util_UserOptionDetailRightsSvr oSvr = new Util_UserOptionDetailRightsSvr();
                bool retValue = oSvr.DeleteRow(ID);
                RaiseDataRowSaved();
                return retValue;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        private void RaiseDataRowSaved()
        {
            if (DataRowSaved != null)
            {
                DataRowSaved();
            }
        }
        public Util_UserOptionDetailRightsData PopulateList(string whereClause)
        {
            try
            {
                using (Util_UserOptionDetailRightsSvr dao = new Util_UserOptionDetailRightsSvr())
                {
                    return dao.Populate(whereClause);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion
        public void checkIsValidData(Util_UserOptionDetailRightsData updates)
        {
          Util_UserOptionDetailRightsTable table;

          DataRow  oRow;

          oRow = updates.Tables[0].Rows[0];

          table = (Util_UserOptionDetailRightsTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (Util_UserOptionDetailRightsTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((Util_UserOptionDetailRightsTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((Util_UserOptionDetailRightsTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (Util_UserOptionDetailRightsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((Util_UserOptionDetailRightsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((Util_UserOptionDetailRightsTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            
        }
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
   
}
