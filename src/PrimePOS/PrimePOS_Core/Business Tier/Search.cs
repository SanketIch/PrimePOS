namespace POS_Core.BusinessRules
{
    using System;
    using System.Data;
    using POS_Core.DataAccess;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using POS_Core.CommonData;
    using POS_Core.ErrorLogging;
    using System.Windows.Forms;
    using NLog;
    using Resources.DelegateHandler;
    using Resources;

    // Generic Search Class
    public class Search : IDisposable
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        private bool isFor7Days = true;
        public bool IsFor7Days
        {
            set { isFor7Days = value; }
            get { return isFor7Days; }
        }

        public Search()
        {
        }

        public DataSet SearchData(string SearchTable, string SearchCode, string SearchName, System.Int32 ActiveOnly, int AdditionalParameter, int EODID = 0, System.Boolean IsInActive = false)    //PRIMEPOS-2700 02-Jul-2019 JY Added EODID parameter//Added by Arvind
        {
            try
            {
                SearchSvr oSearchSvr = new SearchSvr();
                if (SearchTable == clsPOSDBConstants.PrimeRX_HouseChargeInterface)
                {
                    return clsCoreHouseCharge.getHouseChargeInfo(SearchCode, SearchName);
                }
                else if (SearchTable == clsPOSDBConstants.PrimeRX_PatientInterface)
                {
                    return clsPatient.SearchPatientInfo(SearchCode, SearchName);
                }
                else
                {
                    oSearchSvr.IsFor7Days = this.IsFor7Days;
                    return oSearchSvr.Search(SearchTable, SearchCode, SearchName, ActiveOnly, AdditionalParameter, EODID, IsInActive);  //PRIMEPOS-2700 02-Jul-2019 JY Added EODID parameter//Added by Arvind 2664
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchData()");
                throw (exp);
            }
        }

        public DataSet SearchData(String strQuery)
        {
            try
            {
                SearchSvr oSearchSvr = new SearchSvr();
                return oSearchSvr.Search(strQuery);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchData()");
                throw (exp);
            }
        }

        public String SearchScalar(String strQuery)
        {
            try
            {
                SearchSvr oSearchSvr = new SearchSvr();
                return oSearchSvr.SearchScalar(strQuery);
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "SearchScalar()");
                throw (exp);
            }
        }

        public bool DeleteUserRow(string userID, bool bUser = true) //PRIMEPOS-2780 27-Sep-2021 JY added bUser parameter
        {
            try
            {
                SearchSvr oSearchSvr = new SearchSvr();
                bool retValue = false;
                retValue = oSearchSvr.DeleteUserRow(userID, bUser); //PRIMEPOS-2780 27-Sep-2021 JY modified
                if (retValue == true)
                    clsCoreDBuser.DeleteDBUser(userID);
                return retValue;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "DeleteUserRow()");
                //throw (exp);
                return false;
            }
        }

        /// <summary>
        /// To get TextBox Autocomplete Data.
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="dbFieldName"></param>
        /// <returns></returns>
        public AutoCompleteStringCollection GetAutoCompleteCollectionData(string tblName, string dbFieldName)
        {
            try
            {
                using (SearchSvr oSearchSvr = new SearchSvr())
                {
                    return oSearchSvr.GetAutoCompleteCollectionData(tblName, dbFieldName);
                }
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetAutoCompleteCollectionData()");
                throw (exp);
            }
        }

        /// <summary>
        /// Author: Shitaljit
        /// Get the search engine query.
        /// </summary>
        /// <param name="sTableName"></param>
        /// <returns></returns>
        public string GetSearchEngineQuery(string sTableName)
        {
            string sSQL = string.Empty;
            try
            {
                switch (sTableName)
                {
                    case clsPOSDBConstants.Customer_tbl:
                        sSQL = clsPOSDBConstants.Customer_Fld_AcctNumber + " as Account#," +
                        clsPOSDBConstants.Customer_Fld_CustomerName + "," +
                        clsPOSDBConstants.Customer_Fld_FirstName + ", " +
                        clsPOSDBConstants.Customer_Fld_Address1 + " as Address1," +
                        clsPOSDBConstants.Customer_Fld_Address2 + " as Address2," +
                        clsPOSDBConstants.Customer_Fld_City + " as City," +
                        clsPOSDBConstants.Customer_Fld_CellNo + " [Cell No.]," +
                        clsPOSDBConstants.Customer_Fld_PhoneOffice + " [Phone Office]," +
                        clsPOSDBConstants.Customer_Fld_PhoneHome + " [Phone Home]," +
                        clsPOSDBConstants.Customer_Fld_Email + " as Email ," +
                        clsPOSDBConstants.Customer_Fld_IsActive + " as IsActive ," +
                        clsPOSDBConstants.Customer_Fld_Zip + " as Zip ," +
                        clsPOSDBConstants.Customer_Fld_FaxNo + " as Fax# , " +
                        clsPOSDBConstants.Customer_Fld_DriveLicNo + " as DL# , " +
                        clsPOSDBConstants.Customer_Fld_DriveLicState + " as [DL State], " +
                        "CONVERT(VARCHAR(10)," + clsPOSDBConstants.Customer_Fld_DateOfBirth + ", 126) " + " as DOB , " +
                        clsPOSDBConstants.Customer_Fld_PatientNo + " as Patient# ," +
                        clsPOSDBConstants.Customer_Fld_Email + " as Email, " +
                        clsPOSDBConstants.Customer_Fld_Comments;
                        break;
                }
                return sSQL;
            }
            catch (Exception exp)
            {
                logger.Fatal(exp, "GetSearchEngineQuery()");
                return string.Empty;
                throw exp;
            }
        }

        public DataSet GetHouseChargeSearchData(string strCode, string strName, string strMasterSearchVal)
        {
            using (SearchSvr dao = new SearchSvr())
            {
                return dao.GetHouseChargeSearchData(strCode, strName, strMasterSearchVal);
            }
        }

        public DataSet GetCustomerSearchResult(DateTime DOB1, DateTime DOB2, DateTime ExpDate1, DateTime ExpDate2, string strCode, string strName, string strMasterSearchVal, Boolean bIncludeRXCust, out CustomerData oCustomerData, string strContactNumber = "", Boolean includeCPLCardInfo = false, int ActiveOnly = 0, Boolean bOnlyCLPCardCustomers = false, Boolean bSearchExactCustomer = false, Boolean bSelection = false, string ExpDateOption = "", string DOBOption = "", bool IsNoStoreCard = false)//PRIMEPOS-2896 Added IsNoStoreCard parameter
        {
            oCustomerData = new CustomerData();
            try
            {
                using (SearchSvr dao = new SearchSvr())
                {
                    DataSet dsRxPatient = new DataSet();
                    DataSet ds = dao.GetCustomerSearchResult(DOB1, DOB2, ExpDate1, ExpDate2, strCode, strName, strMasterSearchVal, bIncludeRXCust, out dsRxPatient, strContactNumber, includeCPLCardInfo, ActiveOnly, bOnlyCLPCardCustomers, bSearchExactCustomer, bSelection, ExpDateOption, DOBOption, IsNoStoreCard);  //PRIMEPOS-2645 05-Mar-2019 JY Added DOB//PRIMEPOS-2896 Added ISNOSTORECARD

                    if (dsRxPatient != null && dsRxPatient.Tables.Count > 0 && dsRxPatient.Tables[0].Rows.Count > 0) //PRIMEPOS-2893 30-Sep-2020 JY modified   
                    {
                        Customer oCustomer = new Customer();
                        DataSet dsRxCustomers = new DataSet();
                        DataTable dt = new DataTable();
                        dt = dsRxPatient.Tables[0].Clone();
                        foreach (DataRow dr in dsRxPatient.Tables[0].Rows)
                        {
                            dt.ImportRow(dr);
                        }
                        dsRxCustomers.Tables.Add(dt);
                        oCustomerData = oCustomer.CreateCustomerDSFromPatientDS(dsRxCustomers, true);
                        oCustomerData.Tables[0].Columns.Add("Name", Type.GetType("System.String"));
                        oCustomerData.Tables[0].Columns.Add("Cell No.", Type.GetType("System.String"));
                        oCustomerData.Tables[0].Columns.Add("Phone Office", Type.GetType("System.String"));
                        oCustomerData.Tables[0].Columns.Add("Phone Home", Type.GetType("System.String"));
                        oCustomerData.Tables[0].Columns.Add("Cust. Source", Type.GetType("System.String"));
                        ds.Tables[0].TableName = "POSCustomers";

                        int RowIndex = 0;
                        foreach (CustomerRow oRow in oCustomerData.Tables[0].Rows)
                        {
                            oCustomerData.Tables[0].Rows[RowIndex]["Name"] = oRow.CustomerFullName;
                            oCustomerData.Tables[0].Rows[RowIndex]["Cell No."] = oRow.CellNo;
                            oCustomerData.Tables[0].Rows[RowIndex]["Phone Office"] = oRow.PhoneOffice;
                            oCustomerData.Tables[0].Rows[RowIndex]["Phone Home"] = oRow.PhoneHome;
                            oCustomerData.Tables[0].Rows[RowIndex]["Cust. Source"] = "PrimeRx";
                            ds.Tables[0].ImportRow(oRow);
                            RowIndex++;
                        }
                    }
                    return ds;
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetCustomerSearchResult()");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetCustomerSearchResult()");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetCustomerSearchResult()");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetFrmSearchData(string SearchTable, string strCode, string strName, string strMasterSearchVal, string PrgFlag, string ParamValue, bool IsFromPurchaseOrder, ref string DefaultCode, int ActiveOnly, int AdditionalParameter)
        {
            try
            {
                using (SearchSvr dao = new SearchSvr())
                {
                    dao.IsFor7Days = this.IsFor7Days;
                    DataSet ds = dao.GetFrmSearchData(SearchTable, strCode, strName, strMasterSearchVal, PrgFlag, ParamValue, IsFromPurchaseOrder, ref DefaultCode, ActiveOnly, AdditionalParameter);
                    return ds;
                }
            }
            catch (POSExceptions ex)
            {
                logger.Fatal(ex, "GetFrmSearchData()");
                throw (ex);
            }
            catch (OtherExceptions ex)
            {
                logger.Fatal(ex, "GetFrmSearchData()");
                throw (ex);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "GetFrmSearchData()");
                ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }

        public DataSet GetActiveUsers(DataSet oDataSet)
        {
            string whereClause = " IsActive = 1 ";
            DataRow[] drRow = oDataSet.Tables[0].Select(whereClause);
            DataTable dt = new DataTable();
            dt = oDataSet.Tables[0].Clone();
            foreach (DataRow dr in drRow)
            {
                dt.ImportRow(dr);
            }
            oDataSet = new DataSet();
            oDataSet.Tables.Add(dt);
            return oDataSet;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
