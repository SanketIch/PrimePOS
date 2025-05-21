using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
//using POS.Resources;
using MMSChargeAccount;
using NLog;
//using Resources;
using POS_Core.Resources;
using Resources;
using System.Text.RegularExpressions;
using PharmData;

namespace POS_Core.BusinessRules
{
    
    /// <summary>
    /// This is Business Tier Class for customer.
    /// This class contains all business rules related to customer.
    /// </summary>
    public class Customer : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public delegate void DataRowSavedHandler();
        public event DataRowSavedHandler DataRowSaved;
        Regex CheckvalidEmailid = new Regex(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");  //PRIMEPOS-2903 06-Oct-2020 JY Added

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating, Delete Customer data it will delete rows from database which has been deleted from dataset. 
        /// </summary>
        /// <param name="updates">It is customer type dataset class. It contains all information of customers.</param>
        public void Persist(CustomerData updates)
        {
            Persist(updates, false);
        }

        public void Persist(CustomerData updates,bool ignoreValidation)
        {
            try
            {
                logger.Trace("Persist() - " + clsPOSDBConstants.Log_Entering);
                UserPriviliges.IsUserHasPriviliges(UserPriviliges.Modules.POSTransaction.ID, UserPriviliges.Screens.Customers.ID, -998);
                if (ignoreValidation == false)
                {
                    checkIsValidData(updates);
                }
                using (CustomerSvr dao = new CustomerSvr())
                {
                    dao.DataRowSaved += new CustomerSvr.DataRowSavedHandler(dao_DataRowSaved);
                    dao.Persist(updates);
                }
                logger.Trace("Persist() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Persist()");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Persist()");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist()");
                ErrorHandler.throwException(ex, "", "");
            }
        }

        void dao_DataRowSaved()
        {
            RaiseDataRowSaved();
        }
        #endregion



        public bool DeleteRow(string CurrentID)
        {
            //System.Data.IDbConnection oConn = null;
            try
            {
                logger.Trace("DeleteRow() - " + clsPOSDBConstants.Log_Entering);
                bool retValue;
                //oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                using (CustomerSvr dao = new CustomerSvr())
                {
                    retValue = dao.DeleteRow(CurrentID);
                }
                RaiseDataRowSaved();
                logger.Trace("DeleteRow() - " + clsPOSDBConstants.Log_Exiting);
                return retValue;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DeleteRow()");
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

        #region Get Methods
        /// <summary>
        /// Fills a Custemer type DataSet with all Customers based on a condition.
        /// </summary>
        /// <param name="whereClause">Condition for filtering data.</param>
        /// <returns>Returns customer type Dataset</returns>
        public CustomerData PopulateList(string whereClause)
        {
            //29-Nov-2017 JY reffered in frmPOSPayTypesList.cs, frmPOSTransaction.cs, so need to change it while working on transaction screen
            try
            {                
                using (CustomerSvr dao = new CustomerSvr())
                {
                    return dao.PopulateList(whereClause);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public CustomerData PopulateListByCotactNumber(string sContactNumber)
        {
            //29-Nov-2017 JY reffered in frmPOSTransaction.cs, so need to change it while working on transaction screen
            try
            {
                using (CustomerSvr dao = new CustomerSvr())
                {
                    return dao.PopulateListByCotactNumber(sContactNumber);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateListByCotactNumber(string sContactNumber)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateListByCotactNumber(string sContactNumber)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateListByCotactNumber(string sContactNumber)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }       

        //Added logic to return house charge name, returned from PrimeRd dll
        public string GetHouseChargeName(string sAccountID)
        {
            //29-Nov-2017 JY this query is controlled by MMSChargeAccount.dll which is in PrimeRx
            string strReturnValue = string.Empty;
            try
            {
                ContAccount oAcct = new ContAccount();
                DataSet oDS = new DataSet();
                if (sAccountID.Trim() != string.Empty)  //PRIMEPOS-2205 02-Aug-2016 JY Added if condition as if AcctNo is NULL then it will return wrong data
                {
                    //oAcct.GetAccountByCode(sAccountID, out oDS);
                    oAcct.GetAccountByCode(sAccountID, out oDS, true);  //PRIMEPOS-2888 28-Aug-2020 JY Added third parameter as "true" to get the exact HouseCharge record
                }
                if (oDS != null && oDS.Tables.Count > 0 && oDS.Tables[0].Rows.Count > 0)
                {
                    strReturnValue = oDS.Tables[0].Rows[0]["acct_Name"].ToString();
                }

                oDS.Dispose();
                oAcct = null;
                return strReturnValue;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetHouseChargeName(string sAccountID)");
                ErrorHandler.throwException(ex, "", "");
                return string.Empty;
            }
        }

        #region 30-Nov-2017 JY added to get customer loyalty info w.r.t. customer
        public DataTable PopulateCustomerLoyaltyGrid(int customerID)
        {            
            try
            {                
                CLCards oCLP = new CLCards();
                DataTable dt = oCLP.GetCustomerLoyaltyGrid(customerID);
                return dt;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateCustomerLoyaltyGrid(int customerID)");
                ErrorHandler.throwException(ex, "", "");
                return new DataTable();
            }
        }
        #endregion

        #region 30-Nov-2017 JY added to get token by customer
        public CCCustomerTokInfoData GetTokenByCustomerID(int customerID)//Changed from internal to public for POSLITE
        {
            try
            {
                CCCustomerTokInfo oCCCustomerTokInfo = new CCCustomerTokInfo();
                CCCustomerTokInfoData oCCCustomerTokInfoData = oCCCustomerTokInfo.GetTokenByCustomerID(customerID);
                return oCCCustomerTokInfoData;
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetTokenByCustomerID(int customerID)");
                ErrorHandler.throwException(ex, "", "");
                return new CCCustomerTokInfoData();
            }
        }
        #endregion        

        public virtual CustomerData Populate(System.String Customercode)
        {            
            return Populate(Customercode, false);
        }               

        public virtual CustomerData GetCustomerByID(System.Int32 CustomerID)
        {
            using (CustomerSvr dao = new CustomerSvr())
            {
                return dao.GetCustomerByID(CustomerID);
            }
        }

        public virtual CustomerData GetCustomerByPatientNo(System.Int32 PatientNo)
        {
            using (CustomerSvr dao = new CustomerSvr())
            {
                return dao.GetCustomerByPatientNo(PatientNo);
            }
        }

        #region 30-Nov-2017 JY added to get specific customer
        //public virtual CustomerData GetCustomerByCustomerID(System.Int32 CustomerID)
        //{
        //    using (CustomerSvr dao = new CustomerSvr())
        //    {
        //        return dao.GetCustomer(CustomerID,null,null);
        //    }
        //}

        //public virtual CustomerData GetCustomerByAcctNumber(System.String AcctNumber)
        //{
        //    using (CustomerSvr dao = new CustomerSvr())
        //    {
        //        return dao.GetCustomer(null,Configuration.convertNullToInt(AcctNumber),null);
        //    }
        //}
        
        //public virtual CustomerData GetCustomerByPatientno(System.Int32 PatientNo)
        //{
        //    using (CustomerSvr dao = new CustomerSvr())
        //    {
        //        return dao.GetCustomer(null,null,PatientNo);
        //    }
        //}
        #endregion

        /// <summary>
        /// Get customer data with respect to customer code.
        /// </summary>
        /// <param name="Customercode">This is database field of cutomer.</param>
        /// <returns>Collection of Customer type records.</returns>
        ///<exception cref="">ss</exception>
        public virtual CustomerData Populate(System.String Customercode, bool isActive)
        {
            try
            {
                using (CustomerSvr dao = new CustomerSvr())
                {
                    return dao.Populate(Customercode, false, isActive);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.String Customercode, bool isActive)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "Populate(System.String Customercode, bool isActive)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(System.String Customercode, bool isActive)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public virtual CustomerData PopulateByName(System.String CustomerName, bool isActive)
        {
            try
            {
                using (CustomerSvr dao = new CustomerSvr())
                {
                    return dao.Populate(CustomerName, true, isActive);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "PopulateByName(System.String CustomerName, bool isActive)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "PopulateByName(System.String CustomerName, bool isActive)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateByName(System.String CustomerName, bool isActive)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion

        #region Validation Methods
        /// <summary>
        /// Validate a Customer.This would be the place to put field validations.
        /// </summary>
        /// <param name="updates">Contains collection of customer type data.</param>

        public void checkIsValidData(CustomerData updates)
        {
            CustomerTable table;
            CustomerRow oRow;

            oRow = (CustomerRow)updates.Tables[0].Rows[0];

            table = (CustomerTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (CustomerTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((CustomerTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((CustomerTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (CustomerTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((CustomerTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((CustomerTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

            //foreach (CustomerRow row in table.Rows)
            //{
            //    //if (row.CustomerCode == "")
            //    //    ErrorHandler.throwCustomError(POSErrorENUM.Customer_CodeCanNotBeNULL);
            //    if (row.CustomerName.Trim() == "")
            //        ErrorHandler.throwCustomError(POSErrorENUM.Customer_NameCanNotBeNULL);
            //    if (row.FirstName.Trim() == "")  //Added By Shitaljit to validate first name 
            //        ErrorHandler.throwCustomError(POSErrorENUM.Customer_FirstNameCanNotBeNULL);
            //    if (row.Address1.Trim() == "")
            //        ErrorHandler.throwCustomError(POSErrorENUM.Customer_Address1CanNotBeNULL);
            //    if (row.City.Trim() == "")
            //        ErrorHandler.throwCustomError(POSErrorENUM.Customer_CityCanNotBeNull);
            //    if (row.State.Trim() == "")
            //        ErrorHandler.throwCustomError(POSErrorENUM.Customer_StateCannotBeNull);
            //}
        }

        public bool IsCustomerChangedInPharmacy(CustomerRow oCustomerRow, DataRow row)
        {
            logger.Trace("IsCustomerChangedInPharmacy() - " + clsPOSDBConstants.Log_Entering);
            bool retValue = false;
            //string.IsNullOrEmpty added by Ravindra if any Customer data is change to Null or blank then it will not Updated it is 

            if (oCustomerRow.CustomerName != Configuration.convertNullToString(row["lname"]).Trim()&& !string.IsNullOrEmpty(Configuration.convertNullToString(row["lname"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.FirstName != Configuration.convertNullToString(row["fname"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["fname"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.Address1 != Configuration.convertNullToString(row["addrstr"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["addrstr"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.Address2 != Configuration.convertNullToString(row["ADDRSTRLINE2"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["ADDRSTRLINE2"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.Email != Configuration.convertNullToString(row["email"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["email"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.DriveLicNo != Configuration.convertNullToString(row["driverslicense"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["driverslicense"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.CellNo != Configuration.convertNullToString(row["mobileno"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["mobileno"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.City != Configuration.convertNullToString(row["addrct"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["addrct"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.State != Configuration.convertNullToString(row["addrst"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["addrst"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.Zip != Configuration.convertNullToString(row["addrzp"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["addrzp"]).Trim()))
            {
                retValue = true;
            }
            else if (oCustomerRow.PhoneHome != Configuration.convertNullToString(row["phone"]).Trim() && !string.IsNullOrEmpty(Configuration.convertNullToString(row["phone"]).Trim()))
            {
                retValue = true;
            }
            logger.Trace("IsCustomerChangedInPharmacy() - " + clsPOSDBConstants.Log_Exiting);
            return retValue;
        }

        public CustomerData CreateCustomerDSFromPatientDS(DataSet oDS,bool newPatientsOnly)
        {
            logger.Trace("CreateCustomerDSFromPatientDS() - " + clsPOSDBConstants.Log_Entering);
            CustomerData oCustomerData = new CustomerData();
            if (oDS == null || oDS.Tables[0].Rows.Count == 0)
            {
                throw (new Exception("No patient data selected."));
            }
            else
            {
                CustomerRow oCustomerRow;
                CustomerSvr oCustomerSvr = new CustomerSvr();
                int newCustomerID = int.MaxValue;

                foreach (DataRow row in oDS.Tables[0].Rows)
                {
                    int iPatientNo = Convert.ToInt32(row["PatientNo"].ToString().Trim());

                    CustomerData oCData = oCustomerSvr.GetCustomerByPatientNo(iPatientNo);
                    #region PRIMEPOS-2886 31-Aug-2020 JY Added - if customer not found by PatientNo, then use 
                    if (oCData == null || oCData.Tables.Count == 0 || oCData.Tables[0].Rows.Count == 0)
                    {
                        try //PRIMEPOS-3106 22-Jun-2022 JY Added try catch block
                        {
                            DataSet dsPatient = null;
                            PharmBL oPBL = new PharmBL();
                            if (oPBL.ConnectedTo_ePrimeRx() && oDS.Tables[0].Rows.Count==1)
                                dsPatient = oDS;
                            else
                                oPBL.GetPatient(iPatientNo.ToString(), out dsPatient);
                            oCData = oCustomerSvr.GetExactCustomerByMultiplePatientsPatameters(dsPatient);
                        }
                        catch { }
                    }
                    #endregion

                    if (oCData != null && oCData.Tables.Count > 0 && oCData.Customer.Count > 0) //PRIMEPOS-3106 22-Jun-2022 JY modified
                    {
                        if (newPatientsOnly == true)
                        {
                            continue;
                        }
                        else
                        {
                            oCustomerData.Customer.ImportRow(oCData.Customer[0]);
                            oCustomerRow = oCustomerData.Customer[oCustomerData.Customer.Count - 1];
                        }
                    }
                    else
                    {
                        oCustomerRow = oCustomerData.Customer.AddRow(newCustomerID, "", "", "", "", "", "", "", "", "", "", "", true, 0, 0, Configuration.MinimumDate, "", 0, true, 0, 0, false);  //Sprint-23 - PRIMEPOS-2314 10-Jun-2016 JY Added SaveCardProfile parameter
                        if (Configuration.CInfo.SaveCCToken == true && Configuration.CInfo.DefaultCustomerTokenValue == true)   //Sprint-25 - PRIMEPOS-2373 16-Feb-2017 JY Added
                            oCustomerRow.SaveCardProfile = true;

                        newCustomerID--;
                    }

                    oCustomerRow.CustomerCode = row["patientno"].ToString().Trim();

                    oCustomerRow.PatientNo = Convert.ToInt32(row["PatientNo"].ToString().Trim());
                    oCustomerRow.CustomerName = Configuration.convertNullToString(row["lname"]).Trim();
                    oCustomerRow.FirstName = Configuration.convertNullToString(row["fname"]).Trim();

                    oCustomerRow.Address1 = Configuration.convertNullToString(row["addrstr"]).Trim();
                    string test = Configuration.convertNullToString(row["ADDRSTRLINE2"]).Trim();
                    oCustomerRow.Address2 = Configuration.convertNullToString(row["ADDRSTRLINE2"]).Trim();

                    //oCustomerRow.Email = Configuration.convertNullToString(row["email"]); //PRIMEPOS-2903 06-Oct-2020 JY Commented
                    #region PRIMEPOS-2903 06-Oct-2020 JY Added
                    string strEmailId = Configuration.convertNullToString(row["email"]).ToLower().Trim();
                    if (!string.IsNullOrWhiteSpace(strEmailId) && CheckvalidEmailid.IsMatch(strEmailId))
                        oCustomerRow.Email = strEmailId;
                    #endregion

                    oCustomerRow.DriveLicNo = Configuration.convertNullToString(row["driverslicense"]).Trim();
                    oCustomerRow.CellNo = Configuration.convertNullToString(row["mobileno"]).Trim();
                    oCustomerRow.City = Configuration.convertNullToString(row["addrct"]).Trim();
                    oCustomerRow.State = Configuration.convertNullToString(row["addrst"]).Trim();
                    oCustomerRow.Zip = Configuration.convertNullToString(row["addrzp"]).Trim();

                    oCustomerRow.PhoneHome = Configuration.convertNullToString(row["phone"]).Trim();
                    oCustomerRow.Comments = Configuration.convertNullToString(row["Remark"]).Trim();
                    oCustomerRow.PhoneOffice = Configuration.convertNullToString(row["WORKNO"]).Trim();

                    DateTime oDOB;
                    if (DateTime.TryParse(Configuration.convertNullToString(row["DOB"]).Trim(), out oDOB) == true)
                    {
                        oCustomerRow.DateOfBirth = oDOB;
                    }
                    if (Configuration.convertNullToString(row["SEX"]).Trim().ToUpper() == "F")
                    {
                        oCustomerRow.Gender = 1;
                    }
                    else
                    {
                        oCustomerRow.Gender = 0;
                    }
                }
            }
            logger.Trace("CreateCustomerDSFromPatientDS() - " + clsPOSDBConstants.Log_Exiting);
            return oCustomerData;
        }
        
        /// <summary>
        /// Author Shitaljit
        /// //Added By shitaljit to add customer to DB if it is a customer from PrimeRx that is not exist in POS currently.
        /// </summary>
        /// <param name="sCustomerID"></param>
        /// <param name="oCustRow"></param>
        /// <returns></returns>
        public string ImportNewCust(string sCustomerID, CustomerRow oCustRow)
        {
            Customer oCustomer = new Customer();
            CustomerData oCustdata = new CustomerData();

            string strCode = sCustomerID;
            try
            {
                logger.Trace("ImportNewCust() - " + clsPOSDBConstants.Log_Entering);
                //oCustdata = oCustomer.GetCustomerByCustomerID(Configuration.convertNullToInt(strCode));
                oCustdata = oCustomer.GetCustomerByID(Configuration.convertNullToInt(strCode));
                if (oCustdata.Tables[0].Rows.Count == 0)
                {
                    if (oCustRow != null)
                    {
                        oCustdata.Tables[0].ImportRow(oCustRow);
                        oCustomer.Persist(oCustdata, true);
                        //oCustdata = oCustomer.GetCustomerByPatientno(oCustRow.PatientNo);
                        oCustdata = oCustomer.GetCustomerByPatientNo(oCustRow.PatientNo);
                        if (oCustdata.Tables[0].Rows.Count > 0)
                        {
                            oCustRow = (CustomerRow)oCustdata.Customer.Rows[0];
                            strCode = oCustRow.CustomerId.ToString();
                        }
                    }
                }
                logger.Trace("ImportNewCust() - " + clsPOSDBConstants.Log_Exiting);
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "ImportNewCust()");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "ImportNewCust()");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "ImportNewCust()");
                ErrorHandler.throwException(ex, "", "");
            }
            return strCode;
        }
        #endregion

        #region Sprint-25 - PRIMEPOS-2371 22-Mar-2017 JY Added functionality to turn on the "Save Card Profile" flag
        public Int32 UpdateSaveCardProfileFlag(System.Int32 CustomerID)
        {
            int retVal = 0;
            using (CustomerSvr oCustomerSvr = new CustomerSvr())
            {
                retVal = oCustomerSvr.UpdateSaveCardProfileFlag(CustomerID);
            }
            return retVal;
        }
        #endregion

        /// <summary>
        /// Dispose customer contents.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

        public bool UpdateCustomer(int CustomerID, string DateOfBirth, string Address1, string PostalCode, string State)
        {
            bool returnVal = false;
            //IDbConnection conn = POS.Resources.DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
            //IDbTransaction oTrans = conn.BeginTransaction();
            try
            {
                DateTime oDOB;
                bool b = DateTime.TryParse(Configuration.convertNullToString(DateOfBirth).Trim(), out oDOB);
                
                //string sSQL = "update Customer set DRIVELICNO = '" + ID + "', IDType = '" + IDType + "', CustomerName = '" + LastName + "', FIRSTNAME = '" + FirstName + "', DATEOFBIRTH = '" + oDOB + "', Zip = '" + PostalCode + "', State = '" + State + "' where CustomerID=" + CustomerID;
                string sSQL = "update Customer set DATEOFBIRTH = '" + oDOB + "', Zip = '" + PostalCode + "', State = '" + State + "' where CustomerID=" + CustomerID;

                DataHelper.ExecuteNonQuery(Configuration.ConnectionString, CommandType.Text, sSQL);
                returnVal = true;
                //oTrans.Commit();
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "UpdateCustomer()");
                //oTrans.Rollback();
                //throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "UpdateCustomer()");
                //oTrans.Rollback();
                //throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "UpdateCustomer()");
                //oTrans.Rollback();
                // ErrorHandler.throwException(ex, "", "");
            }
            return returnVal;
        }

        #region PRIMEPOS-2598 12-Oct-2018 JY Added
        public bool IsCustomerAssociatedWithTransaction(string CustomerID)
        {
            bool bReturn = false;
            try
            {
                using (CustomerSvr dao = new CustomerSvr())
                {
                    bReturn = dao.IsCustomerAssociatedWithTransaction(CustomerID);
                }
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "IsCustomerAssociatedWithTransaction(int CustomerID)");
                return false;
            }
            return bReturn;
        }
        #endregion

        #region PRIMEPOS-2613 11-Dec-2018 JY Added
        public string EmailReport(int CustomerId)
        {
            string strMsg = string.Empty;
            try
            {
                //get the card details of selected customer, display it to user before sending email, ask confirmation and send
                CCCustomerTokInfoData oCCCustomerTokInfoData = GetTokenByCustomerID(CustomerId);
                if (oCCCustomerTokInfoData != null && oCCCustomerTokInfoData.Tables.Count > 0)
                {
                    foreach (CCCustomerTokInfoRow row in oCCCustomerTokInfoData.Tables[0].Rows)
                    {
                        if (strMsg == string.Empty)
                        {
                            strMsg = "XXXX XXXX XXXX " + row.Last4 + " | " + row.ExpDate;
                        }
                        else
                        {
                            strMsg += Environment.NewLine + "XXXX XXXX XXXX " + row.Last4 + " | " + row.ExpDate;
                        }
                    }
                }
                return strMsg;
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "IsCustomerAssociatedWithTransaction(int CustomerID)");
                return "";
            }
        }
        #endregion

        #region PRIMEPOS-2886 24-Aug-2020 JY Added        
        public CustomerData GetExactCustomerByMultiplePatientsPatameters(DataSet dsPatient)
        {
            try
            {
                using (CustomerSvr dao = new CustomerSvr())
                {
                    return dao.GetExactCustomerByMultiplePatientsPatameters(dsPatient);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetExactCustomerByMultiplePatientsPatameters(DataSet dsPatient)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetExactCustomerByMultiplePatientsPatameters(DataSet dsPatient)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetExactCustomerByMultiplePatientsPatameters(DataSet dsPatient)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public CustomerData GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)
        {
            try
            {
                using (CustomerSvr dao = new CustomerSvr())
                {
                    return dao.GetCustomerByMultiplePatientsPatameters(dsPatient, ref LevelId);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetCustomerByMultiplePatientsPatameters(DataSet dsPatient, ref int LevelId)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataTable GetCustomersForMapping(string strLastName, string strFirstName, object dtpDOB, string strPhoneNo, Boolean bIncludeLinkedCustomers)
        {
            try
            {
                using (CustomerSvr dao = new CustomerSvr())
                {
                    return dao.GetCustomersForMapping(strLastName, strFirstName, dtpDOB, strPhoneNo, bIncludeLinkedCustomers);
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetCustomersForMapping(string strLastName, string strFirstName, DateTime dtpDOB, string strPhoneNo)");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetCustomersForMapping(string strLastName, string strFirstName, DateTime dtpDOB, string strPhoneNo)");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetCustomersForMapping(string strLastName, string strFirstName, DateTime dtpDOB, string strPhoneNo)");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public Int32 UpdatePatientNo(int CustomerID, int PatientNo)
        {
            int retVal = 0;
            using (CustomerSvr oCustomerSvr = new CustomerSvr())
            {
                retVal = oCustomerSvr.UpdatePatientNo(CustomerID, PatientNo);
            }
            return retVal;
        }
        #endregion

        #region code seems to be not in use
        //public bool IsValidLoyaltyCustomer(CustomerRow oCustomerRow, out long iCLCardID)
        //{
        //    //29-Nov-2017 JY not in use
        //    bool returnValue = false;
        //    iCLCardID = 0;
        //    if (oCustomerRow.UseForCustomerLoyalty == true)
        //    {
        //        CLCards oCLCards = new CLCards();
        //        CLCardsData oCLCardsData = oCLCards.GetByCustomerID(oCustomerRow.CustomerId);
        //        if (oCLCardsData.CLCards.Rows.Count > 0)
        //        {
        //            foreach (CLCardsRow oCLCardRow in oCLCardsData.CLCards.Rows)
        //            {
        //                if (oCLCardRow.RegisterDate.AddDays(oCLCardRow.ExpiryDays) >= DateTime.Now.Date || oCLCardRow.IsPrepetual == true)
        //                {
        //                    iCLCardID = oCLCardsData.CLCards[0].CLCardID;
        //                    returnValue = true;
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    return returnValue;
        //}

        //public void checkIsValidPrimaryKey(CustomerData updates)
        //{
        //    CustomerTable table = (CustomerTable)updates.Tables[clsPOSDBConstants.Customer_tbl];
        //    foreach (CustomerRow row in table.Rows)
        //    {
        //        if (this.Populate(row.CustomerId.ToString()).Tables[clsPOSDBConstants.Customer_tbl].Rows.Count != 0)
        //        {
        //            throw new Exception("Primary key violation for Customer ");
        //        }
        //    }
        //}

        /// <summary>
        /// Check whether an attempted delete is valid for Customer. This function has no implementation. 
        /// </summary>
        /// <param name="updates"></param>
        //public void checkIsValidDelete(CustomerData updates)
        //{
        //}
        #endregion
    }
}
