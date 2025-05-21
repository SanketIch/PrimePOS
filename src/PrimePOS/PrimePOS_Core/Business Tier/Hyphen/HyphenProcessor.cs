using Newtonsoft.Json;
using NLog;
using POS_Core.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_Core.Business_Tier.Hyphen
{
    public class HyphenProcessor
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        PharmData.PharmBL oPharmBL = new PharmData.PharmBL();
        POS_Core.BusinessRules.User oUsers = new BusinessRules.User();
        POS_Core.CommonData.UserData oUserData = new CommonData.UserData();

        private string tempUserFullName;

        private string insuranceName;
        private string insuranceBin;
        private string insurancePcn;
        private string insuranceGroupId;
        public static string insuranceID;

        private string patientNo;

        public HyphenProcessor(string PatientNo)
        {
            patientNo = PatientNo;
            tempUserFullName = GetUsername(Configuration.UserName);
        }

        public bool ValidatePatientForHFInsurance(string PatientNo)
        {
            bool result = false;
            string binNo = string.Empty;
            string pcn = string.Empty;
            string activeInsurance = string.Empty;
            string insID = string.Empty;
            string groupid = string.Empty; //PRIMEPOS-3207P

            DataTable dsInsurance = oPharmBL.GetPatAllIns(PatientNo);

            if (dsInsurance != null && dsInsurance.Rows.Count > 0)
            {
                foreach (DataRow dr in dsInsurance.Rows)
                {
                    binNo = Convert.ToString(dr["BIN_NO"]);
                    pcn = Convert.ToString(dr["PC"]).Trim();
                    activeInsurance = Convert.ToString(dr["Active"]);
                    insID = Convert.ToString(dr["INS_ID"]).Trim();
                    groupid = Convert.ToString(dr["GROUPNO"]).Trim();//PRIMEPOS-3207P
                    if (!string.IsNullOrWhiteSpace(binNo) && !string.IsNullOrWhiteSpace(pcn) && !string.IsNullOrWhiteSpace(groupid)) //PRIMEPOS-3207P
                    {
                        if (Configuration.hyphenSetting.HyphenInsuranceCollection.Where(x => Configuration.convertNullToString(x.Bin).ToUpper().Equals(binNo.ToUpper())
                        && Configuration.convertNullToString(x.PCN).ToUpper().Equals(pcn.ToUpper())
                        && Configuration.convertNullToString(x.GroupId).ToUpper().Equals(groupid.ToUpper())).Count() > 0)
                        {
                            insuranceBin = binNo;
                            insuranceName = dr["IC_NAME"].ToString().Trim();
                            insurancePcn = pcn;
                            insuranceID = insID;
                            insuranceGroupId = groupid;
                            result = true;
                            break;
                        }
                    }
                }
            }
            dsInsurance = null;
            return result;
        }

        public async Task<System.Net.Http.HttpResponseMessage> ManageHyphenAlert()
        {
            if (oPharmBL.ConnectedTo_ePrimeRx())
                return null;

            Newtonsoft.Json.Linq.JObject rss = null;
            try
            {
                HyphenSetting hyphenSetting = Configuration.hyphenSetting;

                DataTable oDataTablePat = null;
                if (string.IsNullOrWhiteSpace(patientNo))
                    patientNo = "0";
                oDataTablePat = oPharmBL.GetPatient(patientNo); //PRIMEPOS-3207 check not null

                if (Configuration.hyphenSetting.EnableHyphenIntegration.Equals("Y") && oDataTablePat!=null && oDataTablePat.Rows.Count > 0)
                {
                    bool IsPatientWithHealthfirst = ValidatePatientForHFInsurance(patientNo);
                    if (IsPatientWithHealthfirst)
                    {
                        logger.Trace("HyphenProcessor==>ManageHyphenAlert()->Calling Hyphen Alert");

                        Hyphen.HyphenMMSDataRequestBO hyphenMMSDataRequestBO = new Hyphen.HyphenMMSDataRequestBO()
                        {
                            connInfo = new Hyphen.HyphenMMSConnInfo()
                            {
                                //ClientID = hyphenSetting.ClientID,
                                //ClientSecret = hyphenSetting.ClientSecret,
                                //AuthEndpoint = hyphenSetting.AuthEndpoint,
                                //AlertEndpoint = hyphenSetting.AlertEndpoint,
                                //UIEndpoint = hyphenSetting.UIEndpoint,
                                AzureFunctionUrl = hyphenSetting.AzureFunctionUrl,
                                //VendorBaseUrl = hyphenSetting.VendorBaseUrl,
                                AzureFunctionKeyCode = hyphenSetting.AzureFunctionKeyCode,
                                appId = Hyphen.enumHyphenAppId.PrimePOS.ToString(),
                                source = Hyphen.enumHyphenSource.PrimePOS.ToString(),
                                AppVersion = Convert.ToString(Application.ProductVersion).TrimEnd() //PRIMEPOS-3491
                            },
                            hyphenData = new Hyphen.HyphenDataRequestBO
                            {
                                memberInfo = new Hyphen.HyphenMemberInfo()
                                {
                                    memberId = insuranceID,
                                    firstName = Convert.ToString(oDataTablePat.Rows[0]["FNAME"]).Trim(),
                                    lastName = Convert.ToString(oDataTablePat.Rows[0]["LNAME"]).Trim(),
                                    dob = Convert.ToDateTime(oDataTablePat.Rows[0]["DOB"]).ToShortDateString(),
                                    gender = Convert.ToString(oDataTablePat.Rows[0]["SEX"]).Trim(),
                                    email = Convert.ToString(oDataTablePat.Rows[0]["EMAIL"]).Trim().Length > 0 ? Convert.ToString(oDataTablePat.Rows[0]["EMAIL"]).Trim() : null,
                                    phoneNumber = Convert.ToString(oDataTablePat.Rows[0]["MOBILENO"]).Trim().Length > 0 ? Convert.ToString(oDataTablePat.Rows[0]["MOBILENO"]).Trim() : (Convert.ToString(oDataTablePat.Rows[0]["PHONE"]).Trim().Length > 0 ? Convert.ToString(oDataTablePat.Rows[0]["PHONE"]).Trim() : null),
                                    address = new Hyphen.HyphenMemberAddress()
                                    {
                                        addressLine1 = Convert.ToString(oDataTablePat.Rows[0]["ADDRSTR"]).Trim(),
                                        addressLine2 = (Convert.ToString(oDataTablePat.Rows[0]["ADDRSTRLINE2"]).Trim().Length > 0 ? Convert.ToString(oDataTablePat.Rows[0]["ADDRSTRLINE2"]).Trim() : null),
                                        city = Convert.ToString(oDataTablePat.Rows[0]["ADDRCT"]).Trim(),
                                        state = Convert.ToString(oDataTablePat.Rows[0]["ADDRST"]).Trim(),
                                        zip = Convert.ToString(oDataTablePat.Rows[0]["ADDRZP"]).Trim()
                                    },
                                    insurance = new Hyphen.HyphenMemberInsurance()
                                    {
                                        name = insuranceName,
                                        bin = insuranceBin,
                                        pcn = insurancePcn,
                                        groupid = insuranceGroupId
                                    }
                                },
                                pharmacyInfo = new Hyphen.HyphenPharmacyInfo()
                                {
                                    name = Configuration.CInfo.StoreName,
                                    npi = Configuration.CInfo.PHNPINO
                                },
                                userInfo = new Hyphen.HyphenUserInfo()
                                {
                                    name = tempUserFullName?.Length > 0 ? tempUserFullName : Configuration.UserName, //PRIMEPOS-3207 - need to change
                                    npi = null,
                                    userId = Configuration.UserName,
                                    credentials = null,
                                    userType = "P" //PRIMEPOS-3207 - it is hardcoded bcz in primepos we do not have multiple usertype we only have type as G.
                                }
                            }
                        };

                        string jsonString = JsonConvert.SerializeObject(hyphenMMSDataRequestBO, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include });

                        logger.Trace("HyphenProcessor==>ManageHyphenAlert()==> Payload: " + jsonString);

                        string url = hyphenSetting.AzureFunctionUrl;
                        url += "?code=" + hyphenSetting.AzureFunctionKeyCode + "&ver=" + hyphenSetting.ApiVersion;
                        string contentType = "application/json";

                        System.Net.Http.HttpClient newClient = new System.Net.Http.HttpClient();
                        System.Net.Http.HttpRequestMessage newRequest = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Post, url)
                        {
                            Content = new System.Net.Http.StringContent((string)jsonString, Encoding.UTF8, contentType)
                        };

                        logger.Trace("HyphenProcessor==>ManageHyphenAlert()==> Request: " + newRequest);
                        System.Net.Http.HttpResponseMessage response = await newClient.SendAsync(newRequest);
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "HyphenProcessor==>ManageHyphenAlert(): An Exception Occured");
            }
            return null;
        }

        public string GetUsername(string strUserID)
        {
            string strUsername = "";
            try
            {
                string whereCondition = " WHERE UserID = " + "'" + Configuration.UserName + "'";
                string fName;
                string lName;

                oUserData = oUsers.PopulateList(whereCondition);

                if (oUserData != null && oUserData.Tables.Count > 0 && oUserData.User.Rows.Count > 0)
                {
                    lName = Convert.ToString(oUserData.User.Rows[0]["LNAME"]).Trim();
                    fName = Convert.ToString(oUserData.User.Rows[0]["FNAME"]).Trim();
                    if (lName?.Length > 0 && fName?.Length > 0)
                    {
                        strUsername = lName + " " + fName;
                    }
                }
            }
            catch (Exception exp)
            {
                logger.Error(exp, "HyphenProcessor==>GetUsername(): An Exception Occured");
            }
            return strUsername;
        }
    }
}
