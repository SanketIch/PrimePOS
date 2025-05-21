// ----------------------------------------------------------------
// Library: Business Tier
//  Author: Adeel Shehzad.
// Company: D-P-S. (www.d-p-s.com)
//
// ----------------------------------------------------------------
using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using Resources;
using POS_Core.Resources;
//using POS.Resources;
using NLog;

namespace POS_Core.BusinessRules
{
    // clsVendors Business Rules Class  
    public class Vendor : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods

        // A method for inserting and updating Vendor data.

        public void Persist(VendorData updates)
        {

            try
            {

                UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.AdminFunc.ID, UserPriviliges.Screens.VendorFile.ID, -998);

                checkIsValidData(updates);
                using (VendorSvr dao = new VendorSvr())
                {
                    dao.Persist(updates);
                }
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
                logger.Fatal(ex, "Persist(VendorData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }

        public bool DeleteRow(string CurrentID)
        {
            System.Data.IDbConnection oConn = null;
            try
            {
                oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                VendorSvr oSvr = new VendorSvr();
                return oSvr.DeleteRow(CurrentID);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #endregion

        #region Get Methods

        // Fills a DataSet with all clsVendors based on a condition.
        public VendorData PopulateList(string whereClause)
        {
            try
            {
                using (VendorSvr dao = new VendorSvr())
                {
                    return dao.PopulateList(whereClause);
                }
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
                logger.Fatal(ex, "PopulateList(string whereClause)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");  //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        #endregion



        #region Validation Methods
        // Validate a Vendor.  This would be the place to put field validations.
        public void checkIsValidData(VendorData updates)
        {
            VendorTable table;

            VendorRow oRow;

            oRow = (VendorRow)updates.Tables[0].Rows[0];

            table = (VendorTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (VendorTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((VendorTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((VendorTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (VendorTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((VendorTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((VendorTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            foreach (VendorRow row in table.Rows)
            {
                if (row.Vendorcode.Trim() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.Vendor_PrimaryKeyVoilation);
                if (row.Vendorname.Trim() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.Vendor_NameCanNotBeNULL);
                if (row.Address1.Trim() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.Vendor_Address1CanNotBeNULL);
                if (row.City.Trim() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.Vendor_CityCanNotBeNull);
                if (row.State.Trim() == "")
                    ErrorHandler.throwCustomError(POSErrorENUM.Vendor_StateCannotBeNull);

            }
        }

        public virtual VendorData Populate(System.String vendorcode)
        {
            try
            {
                using (VendorSvr dao = new VendorSvr())
                {
                    return dao.Populate(vendorcode);
                }
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
                logger.Fatal(ex, "Populate(System.String vendorcode)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        public virtual VendorData Populate(System.Int32 vendorId)
        {
            try
            {
                using (VendorSvr dao = new VendorSvr())
                {
                    return dao.Populate(vendorId);
                }
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
                logger.Fatal(ex, "Populate(System.Int32 vendorId)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        //Added By SRT(Ritesh Parekh) Date : 27-Jul-2009
        public string GetVendorCode(string VendorId)
        {
            string vendorCode = string.Empty;
            System.Int32 VendId = 0;
            if (Int32.TryParse(VendorId, out VendId))
            {
                VendorData oData = new VendorData();
                oData = Populate(VendId);
                if (oData != null && oData.Tables.Count > 0 && oData.Vendor.Rows.Count > 0)
                {
                    vendorCode = oData.Vendor.Rows[0]["VendorCode"].ToString();
                }
            }
            return (vendorCode);
        }

        public int GetVendorId(string VendorCode)
        {
            Int32 vendId = 0;


            VendorData oData = new VendorData();
            oData = Populate(VendorCode);
            if (oData != null && oData.Tables.Count > 0 && oData.Vendor.Rows.Count > 0)
            {
                vendId = Configuration.convertNullToInt(oData.Vendor.Rows[0]["VendorId"].ToString());
            }

            return (vendId);
        }
        //End Of Added By SRT(Ritesh Parekh)

        public void checkIsValidPrimaryKey(VendorData updates)
        {
            VendorTable table = (VendorTable)updates.Tables[VendorData.K_VENDOR_TABLE];
            foreach (VendorRow row in table.Rows)
            {
                if (this.Populate(row.Vendorcode).Tables[VendorData.K_VENDOR_TABLE].Rows.Count != 0)
                {
                    throw new Exception("Primary key violation for Vendor ");
                }
            }
        }

        // Check whether an attempted delete is valid for Vendor
        public void checkIsValidDelete(VendorData updates)
        {
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
