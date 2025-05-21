using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using POS.Resources;
using NLog;
using System.Net;   //PRIMEPOS-2558 10-Jul-2018 JY Added
using POS_Core.Resources;
using POS_Core.DataAccess;
using System.Data;
using RestSharp;    //PRIMEPOS-2999 09-Sep-2021 JY Added
using Newtonsoft.Json.Linq; //PRIMEPOS-2999 09-Sep-2021 JY Added
using POS_Core.CommonData;  //PRIMEPOS-2999 09-Sep-2021 JY Added

namespace POS_Core.BusinessRules
{
    public class NplexBL
    {
        /// <summary>
        /// List of Meth Items that should be sent 
        /// </summary>
        public List<ProductType> MethItem = new List<ProductType>();
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Do inquiry to check if this item is eligable for sale
        /// </summary>
        /// <param name="PatInfo"></param>
        public InquiryResponseType DoInquiryRequest(PatientInfo PatInfo, ref string strErrorMsg)
        {
            InquiryResponseType inqResponse = new InquiryResponseType();
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //PRIMEPOS-2558 10-Jul-2018 JY Added  //PRIMEPOS-3179 27-Jan-2023 JY Commented
                RetailServicesWS nplex = new RetailServicesWS();
                PersonIdentifierType person = new PersonIdentifierType();
                ProductType[] products = null;

                StoreType PharmInfo = new StoreType();
                PharmInfo.ItemElementName = ItemChoiceType.storeId;
                PharmInfo.siteId = Configuration.CInfo.StoreSiteId;
                PharmInfo.Item = Configuration.CInfo.nplexStoreId;
                PharmInfo.storeTech = Configuration.UserName;
                //PharmInfo.pharmacistApproval = Configuration.CInfo.pharmacistApproval;

                if (PatInfo.IsIDScan)
                    person.Item = Base64Encode(PatInfo.IDScan);
                else
                    person.Item = PersonInfo(PatInfo);

                products = MethItem.ToArray();
                InquiryRequestType inqRequest = new InquiryRequestType();
                inqRequest.personIdentifier = person;
                inqRequest.products = products;
                inqRequest.store = PharmInfo;

                #region PRIMEPOS-2731 12-Sep-2019 JY Added
                try
                {
                    string strCustomerDetails = "Suffix: " + Configuration.convertNullToString(PatInfo.Suffix) +
                                                ", FirstName: " + Configuration.convertNullToString(PatInfo.FirstName) +
                                                ", MiddleName: " + Configuration.convertNullToString(PatInfo.middleName) +
                                                ", LastName: " + Configuration.convertNullToString(PatInfo.LastName) +
                                                ", Address1: " + Configuration.convertNullToString(PatInfo.Address1) +
                                                ", Address2: " + Configuration.convertNullToString(PatInfo.Address2) +
                                                ", City: " + Configuration.convertNullToString(PatInfo.City) +
                                                ", State: " + Configuration.convertNullToString(PatInfo.State) +
                                                ", PostalCode: " + Configuration.convertNullToString(PatInfo.PostalCode) +
                                                ", DateOfBirth: " + Configuration.convertNullToString(PatInfo.DateOfBirth) +
                                                ", ID: " + Configuration.convertNullToString(PatInfo.ID) +
                                                ", IDType: " + Configuration.convertNullToString(PatInfo.IDType);

                    logger.Trace("DoInquiryRequest() - " + strCustomerDetails);
                }
                catch { }
                #endregion

                inqResponse = nplex.doInquiry(inqRequest);

                #region sample testing
                //InquiryRequestType reqtype1 = sampledata();
                //InquiryResponseType inqResponse1 = new InquiryResponseType();
                //inqResponse1 = nplex.doInquiry(reqtype1);
                #endregion
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message;
                logger.Fatal(ex, "DoInquiryRequest()");
            }
            return inqResponse;
        }

        public PurchaseResponseType DoPurchaseRequest(PatientInfo PatInfo, string inquiryId, signatureType oSignatureType, int CustomerId)  //PRIMEPOS-2572 11-Jun-2020 JY Added CustomerId just for recording purpose
        {
            PurchaseResponseType purResponse = new PurchaseResponseType();
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //PRIMEPOS-2558 10-Jul-2018 JY Added  //PRIMEPOS-3179 27-Jan-2023 JY Commented
                RetailServicesWS nplex = new RetailServicesWS();
                PersonIdentifierType person = new PersonIdentifierType();
                ProductType[] products = null;

                StoreType PharmInfo = new StoreType();
                PharmInfo.ItemElementName = ItemChoiceType.storeId;
                PharmInfo.siteId = Configuration.CInfo.StoreSiteId;
                PharmInfo.Item = Configuration.CInfo.nplexStoreId;
                PharmInfo.storeTech = Configuration.UserName;
                //PharmInfo.pharmacistApproval = Configuration.CInfo.pharmacistApproval;

                products = MethItem.ToArray();
                PurchaseRequestType purRequest = new PurchaseRequestType();
                purRequest.person = PersonInfo(PatInfo);
                purRequest.products = products;
                purRequest.store = PharmInfo;

                purRequest.storeTrxTime = DateTime.Now;
                purRequest.postSaleInd = Configuration.CInfo.postSaleInd;
                purRequest.inquiryId = inquiryId;

                purRequest.signature = oSignatureType;  //Sprint-24 - PRIMEPOS-2332 23-Aug-2016 JY Added

                #region PRIMEPOS-2731 12-Sep-2019 JY Added
                try
                {
                    string strCustomerDetails = "Suffix: " + Configuration.convertNullToString(PatInfo.Suffix) +
                                                ", FirstName: " + Configuration.convertNullToString(PatInfo.FirstName) +
                                                ", MiddleName: " + Configuration.convertNullToString(PatInfo.middleName) +
                                                ", LastName: " + Configuration.convertNullToString(PatInfo.LastName) +
                                                ", Address1: " + Configuration.convertNullToString(PatInfo.Address1) +
                                                ", Address2: " + Configuration.convertNullToString(PatInfo.Address2) +
                                                ", City: " + Configuration.convertNullToString(PatInfo.City) +
                                                ", State: " + Configuration.convertNullToString(PatInfo.State) +
                                                ", PostalCode: " + Configuration.convertNullToString(PatInfo.PostalCode) +
                                                ", DateOfBirth: " + Configuration.convertNullToString(PatInfo.DateOfBirth) +
                                                ", ID: " + Configuration.convertNullToString(PatInfo.ID) +
                                                ", IDType: " + Configuration.convertNullToString(PatInfo.IDType);

                    logger.Trace("DoPurchaseRequest() - " + strCustomerDetails);
                }
                catch { }
                #endregion

                purResponse = nplex.doPurchase(purRequest);

                #region PRIMEPOS-2572 11-Jun-2020 JY Added logic to add record to nPlexRecovery
                if (purResponse.trxStatus.resultCode == 0)
                {
                    InsertNplexRecovery(CustomerId, purResponse.pseTrxId);
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DoPurchaseRequest()");
            }
            return purResponse;
        }

        public ReturnResponseType DoReturn(PatientInfo PatInfo, ref string strErrorMsg)
        {
            ReturnResponseType retResponse = new ReturnResponseType();
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //PRIMEPOS-2558 10-Jul-2018 JY Added  //PRIMEPOS-3179 27-Jan-2023 JY Commented
                RetailServicesWS nplex = new RetailServicesWS();
                ProductType[] products = null;

                StoreType PharmInfo = new StoreType();
                PharmInfo.ItemElementName = ItemChoiceType.storeId;
                PharmInfo.siteId = Configuration.CInfo.StoreSiteId;
                PharmInfo.Item = Configuration.CInfo.nplexStoreId;
                PharmInfo.storeTech = Configuration.UserName;
                //PharmInfo.pharmacistApproval = Configuration.CInfo.pharmacistApproval;

                ReturnIdentifierType retIdnType = new ReturnIdentifierType();
                PersonIdentifierType perIdnType = new PersonIdentifierType();
                perIdnType.Item = PersonInfo(PatInfo);
                retIdnType.Item = perIdnType;

                products = MethItem.ToArray();
                ReturnRequestType retRequest = new ReturnRequestType();
                retRequest.personIdentifier = retIdnType;
                retRequest.products = products;
                retRequest.store = PharmInfo;
                retRequest.storeRtrnTime = DateTime.Now;

                #region PRIMEPOS-2731 12-Sep-2019 JY Added
                try
                {
                    string strCustomerDetails = "Suffix: " + Configuration.convertNullToString(PatInfo.Suffix) +
                                                ", FirstName: " + Configuration.convertNullToString(PatInfo.FirstName) +
                                                ", MiddleName: " + Configuration.convertNullToString(PatInfo.middleName) +
                                                ", LastName: " + Configuration.convertNullToString(PatInfo.LastName) +
                                                ", Address1: " + Configuration.convertNullToString(PatInfo.Address1) +
                                                ", Address2: " + Configuration.convertNullToString(PatInfo.Address2) +
                                                ", City: " + Configuration.convertNullToString(PatInfo.City) +
                                                ", State: " + Configuration.convertNullToString(PatInfo.State) +
                                                ", PostalCode: " + Configuration.convertNullToString(PatInfo.PostalCode) +
                                                ", DateOfBirth: " + Configuration.convertNullToString(PatInfo.DateOfBirth) +
                                                ", ID: " + Configuration.convertNullToString(PatInfo.ID) +
                                                ", IDType: " + Configuration.convertNullToString(PatInfo.IDType);

                    logger.Trace("DoReturn() - " + strCustomerDetails);
                }
                catch { }
                #endregion

                retResponse = nplex.doReturn(retRequest);
            }
            catch (Exception ex)
            {
                strErrorMsg = ex.Message;
                logger.Fatal(ex, "DoReturn()");
            }
            return retResponse;
        }

        public VoidResponseType DoVoid(string pseTrxId)
        {
            VoidResponseType voidResponse = new VoidResponseType();
            try
            {
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;  //PRIMEPOS-2558 10-Jul-2018 JY Added  //PRIMEPOS-3179 27-Jan-2023 JY Commented
                RetailServicesWS nplex = new RetailServicesWS();
                StoreType PharmInfo = new StoreType();
                PharmInfo.ItemElementName = ItemChoiceType.storeId;
                PharmInfo.siteId = Configuration.CInfo.StoreSiteId;
                PharmInfo.Item = Configuration.CInfo.nplexStoreId;
                PharmInfo.storeTech = Configuration.UserName;
                //PharmInfo.pharmacistApproval = Configuration.CInfo.pharmacistApproval;

                VoidRequestType voidRequest = new VoidRequestType();
                voidRequest.store = PharmInfo;
                voidRequest.Item = pseTrxId;
                voidRequest.storeVoidTime = DateTime.Now;
                voidResponse = nplex.doVoid(voidRequest);

                #region PRIMEPOS-2572 10-Jun-2020 JY Added
                if (voidResponse.trxStatus.resultCode == 0)
                {
                    DeleteNplexRecovery(pseTrxId);
                }
                else
                {
                    UpdateNplexRecovery(pseTrxId);
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "DoVoid()");
            }
            return voidResponse;
        }

        private InquiryRequestType sampledata()
        {
            InquiryRequestType reqtype = new InquiryRequestType();
            List<ProductType> lstProducts = new List<ProductType>();
            ProductType products = new ProductType();
            products.upc = "123456789012";
            products.name = "TEST PSE PRODUCT";
            products.ndc = "1234-1234-12";
            products.grams = 1.44F;
            products.pills = 20;
            products.dosages = 1;
            products.type = "CAP";
            products.pediatricInd = false;
            products.boxCount = 2;
            lstProducts.Add(products);

            StoreType store = new StoreType();
            store.ItemElementName = ItemChoiceType.storeId;
            store.siteId = Configuration.CInfo.StoreSiteId;
            store.Item = Configuration.CInfo.nplexStoreId;
            store.storeTech = Configuration.UserName;
            //store.pharmacistApproval = Configuration.CInfo.pharmacistApproval;

            PatientInfo PatInfo = new PatientInfo();
            PatInfo.ID = "123456789";
            PatInfo.IDType = PatientInfo.IDTypes.DL_ID;
            PatInfo.IDIssuingAgency = "KY";
            PatInfo.idExpiration = "08/01/2014";
            PatInfo.LastName = "TESTL";
            PatInfo.middleName = "E";
            PatInfo.FirstName = "TESTF";
            PatInfo.Suffix = "JR";
            PatInfo.DateOfBirth = "05/05/1945";
            PatInfo.Address1 = "TEST";
            PatInfo.Address2 = "APT 2B";
            PatInfo.City = "YOURCITY";
            PatInfo.State = "KY";
            PatInfo.PostalCode = "40223";
            PersonIdentifierType person = new PersonIdentifierType();
            person.Item = PersonInfo(PatInfo);

            reqtype.products = lstProducts.ToArray();
            reqtype.store = store;
            reqtype.personIdentifier = person;

            return reqtype;
        }

        /// <summary>
        /// Raw text to Base64Encode. This is use for ID Scan
        /// </summary>
        /// <param name="RawText"></param>
        /// <returns></returns>
        private string Base64Encode(string RawText)
        {
            var RawData = Encoding.UTF8.GetBytes(RawText);
            return Convert.ToBase64String(RawData);
        }

        private StoreType PharmacyInfo(StoreInfo sInfo)
        {
            var store = new StoreType()
            {
                ItemElementName = ItemChoiceType.storeId,
                Item = sInfo.StoreID,
                siteId = sInfo.SiteID,
                storeTech = sInfo.StoreTech,
                pharmacistApproval = sInfo.PharmacistApproval
            };
            return store;
        }

        /// <summary>
        /// Personal information for the patient that needs be sent
        /// </summary>
        /// <param name="PatInfo"></param>
        /// <returns></returns>
        private PersonInfoType PersonInfo(PatientInfo PatInfo)
        {
            var Person = new PersonInfoType()
            {
                id = PatInfo.ID,
                idType = PatInfo.IDType.ToString(),
                suffix = PatInfo.Suffix,
                firstName = PatInfo.FirstName,
                lastName = PatInfo.LastName,
                birthDate = PatInfo.DateOfBirth,
                addressLine1 = PatInfo.Address1,
                addressLine2 = PatInfo.Address2,
                city = PatInfo.City,
                state = PatInfo.State,
                postalCode = PatInfo.PostalCode,
                idIssuingAgency = PatInfo.IDIssuingAgency,
                idExpiration = PatInfo.idExpiration,    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
                middleName = PatInfo.middleName   //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            };
            return Person;
        }

        #region PRIMEPOS-2572 11-Jun-2020 JY Added
        public void InsertNplexRecovery(int CustomerID, string pseTrxId)
        {
            try
            {
                using (NplexSvr oNplexSvr = new NplexSvr())
                {
                    oNplexSvr.Insert(CustomerID, pseTrxId);
                }
            }
            catch { }
        }

        public void UpdateNplexRecovery(string pseTrxId)
        {
            try
            {
                using (NplexSvr oNplexSvr = new NplexSvr())
                {
                    oNplexSvr.Update(pseTrxId);
                }
            }
            catch { }
        }

        public void DeleteNplexRecovery(string pseTrxId)
        {
            try
            {
                using (NplexSvr oNplexSvr = new NplexSvr())
                {
                    oNplexSvr.Delete(pseTrxId);
                }
            }
            catch { }
        }

        public void VoidNplexRecovery()
        {
            try
            {
                using (NplexSvr oNplexSvr = new NplexSvr())
                {
                    DataTable dt = oNplexSvr.GetNplexRecovery();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DoVoid(Configuration.convertNullToString(dt.Rows[i]["pseTrxId"]));
                        }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region PRIMEPOS-2999 09-Sep-2021 JY Added
        public string GetToken()
        {
            try
            {
                logger.Trace("GetToken() " + clsPOSDBConstants.Log_Entering);
                if (Configuration.convertNullToString(ActiveToken.access_token) == "")
                {
                    CreateToken();
                }
                else
                {
                    if (Configuration.convertNullToInt(ActiveToken.expires_in) != 0)  //Check exipred status
                    {
                        if (ActiveToken.Created_DateTime.AddSeconds(ActiveToken.expires_in-600) < DateTime.Now)
                            CreateToken();
                    }
                    else
                        CreateToken();
                }
                logger.Trace("GetToken() " + clsPOSDBConstants.Log_Exiting);
            }
            catch(Exception Ex)
            {
                logger.Fatal(Ex, "GetToken()");
            }
            return ActiveToken.access_token;
        }

        private void CreateToken()
        {
            try
            {
                logger.Trace("CreateToken() " + clsPOSDBConstants.Log_Entering);
                string URL = Configuration.CSetting.NPlexTokenURL + "/oauth2/token";
                var client = new RestClient(URL);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);

                string credentials = "Basic " + Base64Encode(Configuration.CSetting.NPlexClientID + ":" + Configuration.CSetting.NPlexClientSecret);

                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddHeader("Authorization", credentials);
                request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&scope=methcheck.hc.appriss.com/retailwebservice", ParameterType.RequestBody);

                IRestResponse response = client.Execute(request);
                dynamic resp = JObject.Parse(response.Content);
                if (Configuration.convertNullToString(resp.access_token) != "")
                {
                    ActiveToken.access_token = resp.access_token;
                    ActiveToken.expires_in = resp.expires_in;
                    ActiveToken.Created_DateTime = DateTime.Now;
                }
                else
                    throw new Exception(response.StatusCode + " - " + response.StatusDescription);
                logger.Trace("CreateToken() " + clsPOSDBConstants.Log_Exiting);
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "CreateToken()");
                //clsUIHelper.ShowErrorMsg(Ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// Patient Information
        /// </summary>
        public class PatientInfo
        {
            /// <summary>
            /// ID Scan support AAMVA(Driver Lic) and Magnetic Stripe 
            /// </summary>
            public string IDScan { get; set; }
            /// <summary>
            /// Get or Set to True if ID Scan.  No other data is needed
            /// </summary>
            public bool IsIDScan { get; set; }
            /// <summary>
            /// Required Data, must include the ID number submitted during purchase
            /// </summary>
            public string ID { get; set; }
            /// <summary>
            /// Required Data, The ID type submitted 
            /// </summary>
            public enum IDTypes
            {
                /// <summary>
                /// Driver License
                /// </summary>
                DL_ID,
                /// <summary>
                /// Other State issued ID
                /// </summary>
                STATE_ID,
                /// <summary>
                /// Us Military ID
                /// </summary>
                MILITARY_ID,
                /// <summary>
                /// Passport
                /// </summary>
                PASSPORT,
                /// <summary>
                /// Educational / Institutional issued ID
                /// </summary>
                INSTITUTION,
                /// <summary>
                /// Alien Tegistration Card
                /// </summary>
                ALIEN,
                /// <summary>
                /// Other Type of ID
                /// </summary>
                OTHER
            }
            /// <summary>
            /// Required Data, The ID type submitted 
            /// </summary>
            public IDTypes IDType { get; set; }
            /// <summary>
            /// Issuing Agency must contain the appropriate 2 letter identifier for the State
            /// </summary>
            public string IDIssuingAgency { get; set; }
            public string Suffix { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string DateOfBirth { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string PostalCode { get; set; }
            public string idExpiration { get; set; }    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
            public string middleName { get; set; }    //Sprint-23 - PRIMEPOS-2029 30-Mar-2016 JY Added
        }
        public class StoreInfo
        {
            public string StoreID { get; set; }
            public string SiteID { get; set; }
            public string StoreTech { get; set; }
            public string PharmacistApproval { get; set; }
        }
    }

    #region PRIMEPOS-2999 09-Sep-2021 JY Added
    public static class ActiveToken
    {
        public static string access_token { get; set; }
        public static int expires_in { get; set; }
        public static DateTime Created_DateTime { get; set; }
    }
    #endregion
}
