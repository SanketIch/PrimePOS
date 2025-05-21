using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NLog;
using System.Net;

namespace PharmData
{
    public enum RxFilter
    {
        DATEF,
        ANY,
        EVERYTHING
    }

    public class PharmWebService
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        private string sWebServUrl;
        private string sWebServToken;
        public PharmWebService(string sUrl, string sToken)
        {
            sWebServUrl = sUrl;
            sWebServToken = sToken;
        }

        public DataTable GetPhInfo()
        {
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/PharmacyInfo";

            DataTable dt = new DataTable();
            dt.Columns.Add("NABP", typeof(string));
            dt.Columns.Add("PHNPINO", typeof(string));

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    int nextIdx = 0;
                    string sNABP = string.Empty;
                    string sPHNPINO = string.Empty;

                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "NCPDPId", out sNABP, true);
                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "NPINum", out sPHNPINO, true);
                    dr["NABP"] = sNABP;
                    dr["PHNPINO"] = sPHNPINO;
                }
                else
                    errMsg = "Failed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);
            return dt;
        }

        private DataTable CreatePatMedAdherenceTbl()
        {
            DataTable dt = new DataTable("PATIENT");
            dt.Columns.Add("LNAME", typeof(string));
            dt.Columns.Add("FNAME", typeof(string));
            dt.Columns.Add("PATTYPE", typeof(string));
            dt.Columns.Add("FACILITYCD", typeof(string));
            dt.Columns.Add("patientNo", typeof(int));
            dt.Columns.Add("MedAdherCatID", typeof(int));
            dt.Columns.Add("SpecificProductId", typeof(int));
            dt.Columns.Add("TherapeuticConceptId", typeof(int));
            dt.Columns.Add("MPRValue", typeof(decimal));
            dt.Columns.Add("PDCValue", typeof(decimal));
            dt.Columns.Add("FirstFill", typeof(DateTime));
            dt.Columns.Add("lastfill", typeof(DateTime));
            dt.Columns.Add("totalDaysSupply", typeof(int));
            dt.Columns.Add("totalDaysCovered", typeof(int));
            dt.Columns.Add("PatientName", typeof(string));
            dt.Columns.Add("SpecProdName", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            return dt;
        }

        public DataSet GetPatMedAdherence(int patientNo)
        {
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/PatMedAdherence/" + patientNo.ToString();
            DataSet ds = new DataSet();
            DataTable dt = CreatePatMedAdherenceTbl();
            ds.Tables.Add(dt);
            if (patientNo <= 0)
                return ds;

            int iMedAdIdx = 0;
            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    string strResult = (string)result;
                    int numRec = 0;
                    for (int i = 0; i < strResult.Length; i++)
                    {
                        if (result[i] == '{')
                            numRec++;
                    }


                    for (int iCount = 0; iCount < numRec; iCount++)
                    {
                        int nextIdx = 0;
                        string sWork = string.Empty;
                        string rtnStr = string.Empty;
                        iMedAdIdx = ePrimeRx_GetReturnXMLRecord(strResult, iMedAdIdx, out rtnStr);

                        if (string.IsNullOrWhiteSpace(rtnStr))
                            continue;


                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);

                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "LNAME", out sWork);
                        dr["LNAME"] = sWork; ;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "FNAME", out sWork);
                        dr["FNAME"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATTYPE", out sWork);
                        dr["PATTYPE"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "FACILITYCD", out sWork);
                        dr["FACILITYCD"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "patientId", out sWork, false);
                        dr["patientNo"] = ValStringToI(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "MedAdherCatID", out sWork, false);
                        dr["MedAdherCatID"] = ValStringToI(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SpecificProductId", out sWork, false);
                        dr["SpecificProductId"] = ValStringToI(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "TherapeuticConceptId", out sWork, false);
                        dr["TherapeuticConceptId"] = ValStringToI(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "MPRValue", out sWork, false);
                        dr["MPRValue"] = ValStringToDecimal(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PDCValue", out sWork, false);
                        dr["PDCValue"] = ValStringToDecimal(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "FirstFill", out sWork);
                        dr["FirstFill"] = ValStringToDateTime(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "lastfill", out sWork);
                        dr["lastfill"] = ValStringToDateTime(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "totalDaysSupply", out sWork, false);
                        dr["totalDaysSupply"] = ValStringToI(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "totalDaysCovered", out sWork, false);
                        dr["totalDaysCovered"] = ValStringToI(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PatientName", out sWork);
                        dr["PatientName"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SpecProdName", out sWork);
                        dr["SpecProdName"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Category", out sWork);
                        dr["Category"] = sWork;
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }

            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);

            return ds;
        }

        public DataTable GetAllPatInsurance(string patientNo)
        {
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/PatInsurance/" + patientNo.Trim();
            DataTable dt = new DataTable("PATINS");
            dt.Columns.Add("INS_CD", typeof(string));
            dt.Columns.Add("INS_ID", typeof(string));
            dt.Columns.Add("GROUPNO", typeof(string));
            dt.Columns.Add("RELATION", typeof(string));
            dt.Columns.Add("PERSON_CD", typeof(string));
            int iInsIdx = 0;

            if (ValStringToI(patientNo) <= 0)
                return dt;

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    string strResult = (string)result;
                    int numRec = 0;
                    for (int i = 0; i < strResult.Length; i++)
                    {
                        if (result[i] == '{')
                            numRec++;
                    }

                    for (int iCount = 0; iCount < numRec; iCount++)
                    {
                        int nextIdx = 0;
                        string sWork = string.Empty;
                        string rtnStr = string.Empty;
                        iInsIdx = ePrimeRx_GetReturnXMLRecord(strResult, iInsIdx, out rtnStr);

                        if (string.IsNullOrWhiteSpace(rtnStr))
                            continue;

                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);

                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "INS_CD", out sWork);
                        dr["INS_CD"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "INS_ID", out sWork, false);
                        dr["INS_ID"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "GROUPNO", out sWork);
                        dr["GROUPNO"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "RELATION", out sWork);
                        dr["RELATION"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PERSON_CD", out sWork);
                        dr["PERSON_CD"] = sWork;
                    }
                }
                else
                    errMsg = "Failed to get patient insurance info from ePrimeRx by API : " + strAPI_Url;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get patient insurance info from ePrimeRx by API : " + strAPI_Url;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);
            return dt;
        }

        private DataTable CreateNoteTable(string tableName)
        {
            DataTable dt = new DataTable(tableName);
            dt.Columns.Add("NoteId", typeof(long));
            dt.Columns.Add("CategoryId", typeof(int));
            dt.Columns.Add("EntityId", typeof(string));
            dt.Columns.Add("EntityIdType", typeof(int));
            dt.Columns.Add("Note", typeof(string));
            dt.Columns.Add("POPUPMSG", typeof(bool));

            return dt;
        }

        private void ePrimeRx_GetPopupNotes(ref DataTable dt, string strAPI_Url)
        {
            string errMsg = string.Empty;
            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    int nextIdx = 0;
                    string sWork = string.Empty;

                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "NoteId", out sWork, false);
                    dr["NoteId"] = ValStringToL(sWork);
                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "CategoryId", out sWork, false);
                    dr["CategoryId"] = ValStringToI(sWork);
                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "EntityId", out sWork, false);
                    dr["EntityId"] = sWork;
                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "EntityIdType", out sWork, false);
                    dr["EntityIdType"] = ValStringToI(sWork);
                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "Note", out sWork);
                    dr["Note"] = sWork;
                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "POPUPMSG", out sWork, false);
                    dr["POPUPMSG"] = ValStringToBool(sWork);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }

            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);
        }

        public DataTable GetRxNotes(string sRxNo)
        {
            DataTable dt = CreateNoteTable("RXNOTES");
            string strAPI_Url = "api/PrimePos/RxNotes/" + sRxNo.Trim();
            ePrimeRx_GetPopupNotes(ref dt, strAPI_Url);
            return dt;
        }

        public DataTable GetPatientNotes(string sPatNo)
        {
            DataTable dt = CreateNoteTable("PATIENTNOTES");
            string strAPI_Url = "api/PrimePos/PatientNotes/" + sPatNo.Trim();
            ePrimeRx_GetPopupNotes(ref dt, strAPI_Url);
            return dt;
        }

        public DataTable GetPatientByFamilyID(int iFamilyID)
        {
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/Family/" + iFamilyID.ToString();
            DataTable dt = new DataTable("PATIENT");
            dt.Columns.Add("PATIENTNO", typeof(int));

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    string sWork1 = result.Substring(1).Replace("]", " ");
                    string[] sWork2 = sWork1.Split(',');
                    for(int i=0; i<sWork2.Length; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = ValStringToI(sWork2[i]);
                        dt.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);

            return dt;
        }

        public string GetLastRefill(string sRxNo)
        {
            string sReFillPartial = "0";
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/LastFill/" + sRxNo.Trim();

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    int nextIdx = 0;
                    string sReFillNum = string.Empty;
                    string sPartialFillNo = string.Empty;

                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "ReFillNum", out sReFillNum, false);
                    nextIdx = ePrimeRx_GetValueByFieldName(result, nextIdx, "PartialFillNo", out sPartialFillNo, false);

                    sReFillPartial = sReFillNum;
                    if (!string.IsNullOrWhiteSpace(sPartialFillNo))
                        sReFillPartial += "+" + sPartialFillNo.Trim();
                }
                else
                {
                    sReFillPartial = "-1+-1"; //RxNo does not exist, so RefillNo=-1 , PartialFillNo = -1
                    errMsg = "Failed to get data from ePrimeRx by API : " + strAPI_Url;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);

            return sReFillPartial;
        }

        public int GetLastPartialFillNo(string sRxNo, string sRefNo)
        {
            int iPartialFill = 0;
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/LatestPartialFill/" + sRxNo.Trim()+ "/" + sRefNo.Trim();

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    string sPartialFillNo = (string)result;
                    iPartialFill = ValStringToI(sPartialFillNo);
                }
                //else    no partial fill #3322
                //errMsg = "Failed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);

            return iPartialFill;
        }

        public DataTable GetRxs(string sRxNo, string sRefNo, string sBillStatus) //sRefNo = RefillNo + PartialFillNo
        {
            DataTable dt = new DataTable();
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/RxDetails";
            string[] strWork = sRefNo.Split('+');
            string sRefillNo = "0";
            string sParFillNo = "0";
            if (strWork.Length >= 1 && !string.IsNullOrWhiteSpace(strWork[0]))
                sRefillNo = strWork[0];
            if (strWork.Length >= 2 && !string.IsNullOrWhiteSpace(strWork[1]))
                sParFillNo = strWork[1];

            if (sParFillNo == "0")
                sParFillNo = "1";
            RequestApiParameter oReqParam = new RequestApiParameter();
            oReqParam.RxNo = sRxNo;
            oReqParam.RefNo = sRefillNo;
            oReqParam.FillNo = sParFillNo;

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, oReqParam).Result).Result;
                if ((string)result != "")
                {
                    dt = ePrimeRx_LoadDataClaims((string)result);
                }
                else
                {
                    dt = ePrimeRx_CreateClaimsTable();
                    errMsg = "Failed to get data from ePrimeRx by API : " + strAPI_Url + " " + sRxNo + "-" + sRefillNo + "-" + sParFillNo;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url + " " + sRxNo + "-" + sRefillNo + "-" + sParFillNo;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);
            return dt;
        }

        public DataTable GetRxDetails(string sRxNo, string sRefNo, string sBillStatus, int iPartialFillNo)
        {
            DataTable dt = null;
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/RxDetails";
            string sRefillNo = sRefNo.Trim();
            string sParFillNo = iPartialFillNo.ToString();

            if (sParFillNo == "0")
                sParFillNo = "1";
            RequestApiParameter oReqParam = new RequestApiParameter();
            oReqParam.RxNo = sRxNo;
            oReqParam.RefNo = sRefillNo;
            oReqParam.FillNo = sParFillNo;

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, oReqParam).Result).Result;
                if ((string)result != "")
                {
                    dt = ePrimeRx_LoadDataClaims((string)result);
                }
                else
                {
                    dt = ePrimeRx_CreateClaimsTable();
                    errMsg = "Failed to get data from ePrimeRx calling API : " + strAPI_Url + " " + sRxNo + "-" + sRefillNo + "-" + sParFillNo;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx calling API : " + strAPI_Url + " " + sRxNo + "-" + sRefillNo + "-" + sParFillNo;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);
            return dt;
        }

        private DataTable ePrimeRx_LoadDataClaims(string rtnStr)
        {
            DataTable dt = ePrimeRx_CreateClaimsTable();
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            int nextIdx = 0;
            string sWork = string.Empty;

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATTYPE", out sWork);
            dr["PATTYPE"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "BILLTYPE", out sWork);
            dr["BILLTYPE"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Status", out sWork);
            dr["Status"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "RXNO", out sWork, false);
            dr["RXNO"] = ValStringToL(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PatientName", out sWork);
            dr["PatientName"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PRESNO", out sWork, false);
            dr["PRESNO"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATAMT", out sWork, false);
            dr["PATAMT"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "NDC", out sWork);
            dr["NDC"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DRGNAME", out sWork);
            dr["DRGNAME"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DATEO", out sWork);

            dr["DATEO"] = ValStringToDateTime(sWork);

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DATEF", out sWork);
            dr["DATEF"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "QUANT", out sWork, false);
            dr["QUANT"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "QTY_ORD", out sWork, false);
            dr["QTY_ORD"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DAYS", out sWork, false);
            dr["DAYS"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "TREFILLS", out sWork, false);
            dr["TREFILLS"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "NREFILL", out sWork, false);
            dr["NREFILL"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PartialFillNo", out sWork, false);  // column is added at end
            dr["PartialFillNo"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DELIVERY", out sWork);
            dr["DELIVERY"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "siglines", out sWork);
            dr["siglines"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "strong", out sWork);
            dr["strong"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "form", out sWork);
            dr["form"] = sWork;

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "units", out sWork);
            dr["units"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "drugclass", out sWork, false);
            dr["class"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PICKEDUP", out sWork);
            dr["PICKEDUP"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PICKUPDATE", out sWork);
            dr["PICKUPDATE"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PICKUPTIME", out sWork);
            dr["PICKUPTIME"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PICKUPFROM", out sWork);
            dr["PICKUPFROM"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "AMOUNT", out sWork, false);
            dr["AMOUNT"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "COPAY", out sWork, false);
            dr["COPAY"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "TOTAMT", out sWork, false);
            dr["TOTAMT"] = ValStringToDecimal(sWork);

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PFEE", out sWork, false);
            dr["PFEE"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "STAX", out sWork, false);
            dr["STAX"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DISCOUNT", out sWork, false);
            dr["DISCOUNT"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "OTHFEE", out sWork, false);
            dr["OTHFEE"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "OTHAMT", out sWork, false);
            dr["OTHAMT"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "BILLEDAMT", out sWork, false);
            dr["BILLEDAMT"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "UNC", out sWork, false);
            dr["UNC"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DETDRGNAME", out sWork);
            dr["DETDRGNAME"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Verified", out sWork);
            dr["Verified"] = sWork;

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "VRFStage", out sWork, false);
            dr["VRFStage"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "BinStatus", out sWork);
            dr["BinStatus"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "BIN", out sWork);
            dr["BIN"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "FamilyID", out sWork, false);
            dr["FamilyID"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATIENTNO", out sWork, false);
            dr["PATIENTNO"] = ValStringToI(sWork);

            return dt;
        }

        private DataTable ePrimeRx_LoadDataPatientRXs(string sRXsStr)
        {
            int numRec = 0;
            int iClaimIdx = 0;
            string sWork = string.Empty;

            for (int i = 0; i < sRXsStr.Length; i++)
            {
                if (sRXsStr[i] == '{')
                    numRec++;
            }

            DataTable dt = ePrimeRx_CreateClaimsTable();

            for (int iCount = 0; iCount < numRec; iCount++)
            {
                int nextIdx = 0;
                string rtnStr = string.Empty;
                iClaimIdx = ePrimeRx_GetReturnXMLRecord(sRXsStr, iClaimIdx, out rtnStr);

                if (string.IsNullOrWhiteSpace(rtnStr))
                    continue;

                DataRow dr = dt.NewRow();
                dt.Rows.Add(dr);

                sWork = string.Empty;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATTYPE", out sWork);
                dr["PATTYPE"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "BILLTYPE", out sWork);
                dr["BILLTYPE"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Status", out sWork);
                dr["Status"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "RXNO", out sWork, false);
                dr["RXNO"] = ValStringToL(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PatientName", out sWork);
                dr["PatientName"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PRESNO", out sWork, false);
                dr["PRESNO"] = ValStringToI(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATAMT", out sWork, false);
                dr["PATAMT"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "NDC", out sWork);
                dr["NDC"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DRGNAME", out sWork);
                dr["DRGNAME"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DATEO", out sWork);

                dr["DATEO"] = ValStringToDateTime(sWork);

                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DATEF", out sWork);
                dr["DATEF"] = ValStringToDateTime(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "QUANT", out sWork, false);
                dr["QUANT"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "QTY_ORD", out sWork, false);
                dr["QTY_ORD"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DAYS", out sWork, false);
                dr["DAYS"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "TREFILLS", out sWork, false);
                dr["TREFILLS"] = ValStringToI(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "NREFILL", out sWork, false);
                dr["NREFILL"] = ValStringToI(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PartialFillNo", out sWork, false);  // column is added at end
                dr["PartialFillNo"] = ValStringToI(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DELIVERY", out sWork);
                dr["DELIVERY"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "siglines", out sWork);
                dr["siglines"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "strong", out sWork);
                dr["strong"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "form", out sWork);
                dr["form"] = sWork;

                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "units", out sWork);
                dr["units"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "drugclass", out sWork, false);
                dr["class"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PICKEDUP", out sWork);
                dr["PICKEDUP"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PICKUPDATE", out sWork);
                dr["PICKUPDATE"] = ValStringToDateTime(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PICKUPTIME", out sWork);
                dr["PICKUPTIME"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PICKUPFROM", out sWork);
                dr["PICKUPFROM"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "AMOUNT", out sWork, false);
                dr["AMOUNT"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "COPAY", out sWork, false);
                dr["COPAY"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "TOTAMT", out sWork, false);
                dr["TOTAMT"] = ValStringToDecimal(sWork);

                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PFEE", out sWork, false);
                dr["PFEE"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "STAX", out sWork, false);
                dr["STAX"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DISCOUNT", out sWork, false);
                dr["DISCOUNT"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "OTHFEE", out sWork, false);
                dr["OTHFEE"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "OTHAMT", out sWork, false);
                dr["OTHAMT"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "BILLEDAMT", out sWork, false);
                dr["BILLEDAMT"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "UNC", out sWork, false);
                dr["UNC"] = ValStringToDecimal(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DETDRGNAME", out sWork);
                dr["DETDRGNAME"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Verified", out sWork);
                dr["Verified"] = sWork;

                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "VRFStage", out sWork, false);
                dr["VRFStage"] = ValStringToI(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "BinStatus", out sWork);
                dr["BinStatus"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "BIN", out sWork);
                dr["BIN"] = sWork;
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "FamilyID", out sWork, false);
                dr["FamilyID"] = ValStringToI(sWork);
                nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATIENTNO", out sWork, false);
                dr["PATIENTNO"] = ValStringToI(sWork);
            }

            return dt;
        }

        private DataTable ePrimeRx_CreateClaimsTable()
        {
            DataTable dt = new DataTable("CLAIMS");

            dt.Columns.Add("PATTYPE", typeof(string));
            dt.Columns.Add("BILLTYPE", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("RXNO", typeof(long));
            dt.Columns.Add("PatientName", typeof(string));
            dt.Columns.Add("PRESNO", typeof(int));
            dt.Columns.Add("PATAMT", typeof(decimal));
            dt.Columns.Add("NDC", typeof(string));
            dt.Columns.Add("DRGNAME", typeof(string));
            dt.Columns.Add("DATEO", typeof(DateTime));

            dt.Columns.Add("DATEF", typeof(DateTime));
            dt.Columns.Add("QUANT", typeof(decimal));
            dt.Columns.Add("QTY_ORD", typeof(decimal));
            dt.Columns.Add("DAYS", typeof(string));
            dt.Columns.Add("TREFILLS", typeof(int));
            dt.Columns.Add("NREFILL", typeof(int));
            dt.Columns.Add("DELIVERY", typeof(string));
            dt.Columns.Add("siglines", typeof(string));
            dt.Columns.Add("strong", typeof(string));
            dt.Columns.Add("form", typeof(string));

            dt.Columns.Add("units", typeof(string));
            dt.Columns.Add("class", typeof(string));
            dt.Columns.Add("PATIENTNO", typeof(int));  // moved to the end
            dt.Columns.Add("PICKEDUP", typeof(string));
            dt.Columns.Add("PICKUPDATE", typeof(DateTime));
            dt.Columns.Add("PICKUPTIME", typeof(string));
            dt.Columns.Add("PICKUPFROM", typeof(string));
            dt.Columns.Add("AMOUNT", typeof(decimal));
            dt.Columns.Add("COPAY", typeof(decimal));
            dt.Columns.Add("TOTAMT", typeof(decimal));

            dt.Columns.Add("PFEE", typeof(decimal));
            dt.Columns.Add("STAX", typeof(decimal));
            dt.Columns.Add("DISCOUNT", typeof(decimal));
            dt.Columns.Add("OTHFEE", typeof(decimal));
            dt.Columns.Add("OTHAMT", typeof(decimal));
            dt.Columns.Add("BILLEDAMT", typeof(decimal));
            dt.Columns.Add("UNC", typeof(decimal));
            dt.Columns.Add("DETDRGNAME", typeof(string));
            dt.Columns.Add("pickuppos", typeof(string));  // no such column retrieved
            dt.Columns.Add("Verified", typeof(string));

            dt.Columns.Add("VRFStage", typeof(int));
            dt.Columns.Add("BIN", typeof(string));
            dt.Columns.Add("BinStatus", typeof(string));
            dt.Columns.Add("FamilyID", typeof(int));
            dt.Columns.Add("PartialFillNo", typeof(int));

            return dt;
        }

        private static int ePrimeRx_GetReturnXMLRecord(string strResult, int startIdx, out string rtnXMLRecord)
        {
            int nextPos = startIdx;
            rtnXMLRecord = string.Empty;
            int nextLeftBr = 0;
            int nextRightBr = 0;

            nextLeftBr = strResult.IndexOf("{", nextPos);
            if (nextLeftBr < 0)
                return nextPos;
            nextRightBr = strResult.IndexOf("}", nextLeftBr);
            if (nextLeftBr > 0 && nextRightBr > 0)
            {
                int len = nextRightBr - nextLeftBr - 1;
                rtnXMLRecord = strResult.Substring(nextLeftBr + 1, len);
                nextPos = nextLeftBr + 1 + len;
            }
            return nextPos;
        }

        public DataTable GetPatient(string sPatNo)
        {
            DataTable dt = new DataTable();
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/Patient/" + sPatNo.Trim();

            if (ValStringToI(sPatNo) <= 0)
                return dt;

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    dt = ePrimeRx_LoadDataPatient((string)result);
                }
                else
                {
                    dt = ePrimeRx_CreatePatientTable();
                    errMsg = "Failed to get data from ePrimeRx by API : " + strAPI_Url;
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }

            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);
            return dt;
        }

        private DataTable ePrimeRx_LoadDataPatient(string rtnStr)
        {
            DataTable dt = ePrimeRx_CreatePatientTable();
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            int nextIdx = 0;
            string sWork = string.Empty;

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATIENTNO", out sWork, false);
            dr["PATIENTNO"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATTYPE", out sWork);
            dr["PATTYPE"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "MEDNO", out sWork);
            dr["MEDNO"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "LNAME", out sWork);
            dr["LNAME"] = sWork.ToUpper();
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "FNAME", out sWork);
            dr["FNAME"] = sWork.ToUpper();
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "MI", out sWork);
            dr["MI"] = sWork.ToUpper();
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DOB", out sWork);
            dr["DOB"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SEX", out sWork);
            dr["SEX"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "GROUPNO", out sWork);
            dr["GROUPNO"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "RELATION", out sWork);
            dr["RELATION"] = sWork;//

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PERSON_CD", out sWork);
            dr["PERSON_CD"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CH_LNAME", out sWork);
            dr["CH_LNAME"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CH_FNAME", out sWork);
            dr["CH_FNAME"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CH_MI", out sWork);
            dr["CH_MI"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CRD_EXPDT", out sWork);
            dr["CRD_EXPDT"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "COPAYTYPE", out sWork);
            dr["COPAYTYPE"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "COPAY", out sWork, false);
            dr["COPAY"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "COPAY_G", out sWork, false);
            dr["COPAY_G"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "COPAY_M", out sWork, false);
            dr["COPAY_M"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "MEDICARE", out sWork);
            dr["MEDICARE"] = sWork;//

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "OTHINS", out sWork);
            dr["OTHINS"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ADDRSTR", out sWork);
            dr["ADDRSTR"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ADDRSTRLINE2", out sWork);
            dr["ADDRSTRLINE2"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ADDRCT", out sWork);
            dr["ADDRCT"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ADDRST", out sWork);
            dr["ADDRST"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ADDRZP", out sWork);
            dr["ADDRZP"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PHONE", out sWork);
            dr["PHONE"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ALLERGY", out sWork);
            dr["ALLERGY"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "LANGUAGE", out sWork);
            dr["LANGUAGE"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "P_PROV", out sWork);
            dr["P_PROV"] = sWork;//

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PRCARE_PRV", out sWork, false);
            dr["PRCARE_PRV"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DISC_CD", out sWork);
            dr["DISC_CD"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PRICE_CD", out sWork);
            dr["PRICE_CD"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PRICE_CDG", out sWork);
            dr["PRICE_CDG"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "YTD_CLAIMS", out sWork, false);
            dr["YTD_CLAIMS"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "YTD_AMT", out sWork, false);
            dr["YTD_AMT"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "YTD_DED", out sWork, false);
            dr["YTD_DED"] = ValStringToDecimal(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "LC_DATE", out sWork);
            dr["LC_DATE"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ACCT_NO", out sWork, false);
            dr["ACCT_NO"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CHARGETYPE", out sWork);
            dr["CHARGETYPE"] = sWork;//

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "LDOCNO", out sWork, false); //stop here, Jenny
            dr["LDOCNO"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "FACILITYCD", out sWork);
            dr["FACILITYCD"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PFPPS", out sWork);
            dr["PFPPS"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "REMARK", out sWork);
            dr["REMARK"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "EZCAP", out sWork, false);
            dr["EZCAP"] = "N";
            if (ValStringToBool(sWork))
                dr["EZCAP"] = "Y";
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "REFREMIND", out sWork, false);
            dr["REFREMIND"] = "N";
            if (ValStringToBool(sWork))
                dr["REFREMIND"] = "Y";
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "NOTES", out sWork);
            dr["NOTES"] = sWork;
            //nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PATALTID", out sWork); not existing in API result
            //dr["PATALTID"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "QPATID", out sWork);
            dr["QPATID"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PAT_LOC", out sWork);
            dr["PAT_LOC"] = sWork;//

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SMOKER_CD", out sWork);
            dr["SMOKER_CD"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PREG_CD", out sWork);
            dr["PREG_CD"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "EMPLOYERID", out sWork);
            dr["EMPLOYERID"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "HOME_PLAN", out sWork);
            dr["HOME_PLAN"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ELIG_CLRCD", out sWork, false);
            dr["ELIG_CLRCD"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "FACILITYID", out sWork);
            dr["FACILITYID"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PLAN_ID", out sWork);
            dr["PLAN_ID"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SIGACK", out sWork);
            dr["SIGACK"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SIGACKDATE", out sWork);
            dr["SIGACKDATE"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "EMAIL", out sWork);
            dr["EMAIL"] = sWork;//

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "WORKNO", out sWork);
            dr["WORKNO"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "MOBILENO", out sWork);
            dr["MOBILENO"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DELIVERY", out sWork);
            dr["DELIVERY"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ADDRSTR2", out sWork);
            dr["ADDRSTR2"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ADDRCT2", out sWork);
            dr["ADDRCT2"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ADDRST2", out sWork);
            dr["ADDRST2"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ADDRZP2", out sWork);
            dr["ADDRZP2"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CUSTCATEG", out sWork);
            dr["CUSTCATEG"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ALTLANGPRF", out sWork);
            dr["ALTLANGPRF"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Active", out sWork);
            dr["Active"] = sWork;//

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PrintDrugCouns", out sWork, false);
            dr["PrintDrugCouns"] = "N";
            if (ValStringToBool(sWork))
                dr["PrintDrugCouns"] = "Y";
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CreationDate", out sWork);
            dr["CreationDate"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "LastModified", out sWork);
            dr["LastModified"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Salutation", out sWork);
            dr["Salutation"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PatSuffix", out sWork);
            dr["PatSuffix"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Is340B", out sWork, false);
            dr["Is340B"] = ValStringToBool(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SSN", out sWork);
            dr["SSN"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SalesPersonId", out sWork, false);
            dr["SalesPersonId"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DriversLicense", out sWork);
            dr["DriversLicense"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "UnitDoseDisp", out sWork);
            dr["UnitDoseDisp"] = sWork;//

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Deceased", out sWork, false);
            dr["Deceased"] = "N";
            if (ValStringToBool(sWork))
                dr["Deceased"] = "Y";
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "FamilyID", out sWork, false);
            dr["FamilyID"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "LMOD_BY", out sWork);
            dr["LMOD_BY"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PatientResidence", out sWork);
            dr["PatientResidence"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PatAssignment", out sWork, false);
            dr["PatAssignment"] = "N";
            if (ValStringToBool(sWork))
                dr["PatAssignment"] = "Y";
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PCSynch", out sWork, false);
            dr["PCSynch"] = ValStringToBool(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CentralId", out sWork, false);
            dr["CentralId"] = ValStringToL(sWork);
            //nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "NCPDPStrengthForm", out sWork);
            //dr["NCPDPStrengthForm"] = sWork;
            //nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "NCPDPStrengthUnitOfMeasure", out sWork);
            //dr["NCPDPStrengthUnitOfMeasure"] = sWork;
            //nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "NCPDPQuantityUnitOfMeasure", out sWork);
            //dr["NCPDPQuantityUnitOfMeasure"] = sWork;

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SpeciesCD", out sWork);
            dr["SpeciesCD"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DriversLicenseExpDT", out sWork);
            dr["DriversLicenseExpDT"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "IsWorkFlowPatient", out sWork, false);
            dr["IsWorkFlowPatient"] = ValStringToBool(sWork);
            /*
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "IsMappedWithGSDD", out sWork);
            dr["IsMappedWithGSDD"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "allergyConfirmDate", out sWork);
            dr["allergyConfirmDate"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ConfirmedBy", out sWork);
            dr["ConfirmedBy"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "IsICD9MappedWithICD10", out sWork);
            dr["IsICD9MappedWithICD10"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ICD10MappedDate", out sWork);
            dr["ICD10MappedDate"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ICD10MappedBy", out sWork);
            dr["ICD10MappedBy"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ScriptCenter_EnrollmentStatus", out sWork);
            dr["ScriptCenter_EnrollmentStatus"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ScriptCenter_KioskLocation", out sWork);
            dr["ScriptCenter_KioskLocation"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "ScriptCenter_PaymentCode", out sWork);
            dr["ScriptCenter_PaymentCode"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "EmployeeLocationCode", out sWork);
            dr["EmployeeLocationCode"] = sWork;
            */

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "IsCentralUPDT", out sWork, false);
            dr["IsCentralUPDT"] = ValStringToBool(sWork);
            //nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PrintQrCodeOnLabel", out sWork);
            //dr["PrintQrCodeOnLabel"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CRD_EFFECTIVEDT", out sWork);
            dr["CRD_EFFECTIVEDT"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CH_DOB", out sWork);
            dr["CH_DOB"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "AddrZipExtension", out sWork);
            dr["AddrZipExtension"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "MaritalStatus", out sWork);
            dr["MaritalStatus"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "CardHolderDOB", out sWork);
            dr["CardHolderDOB"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "InsEffectiveDate", out sWork);
            dr["InsEffectiveDate"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "AddrCountryCode", out sWork);
            dr["AddrCountryCode"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PackagingPatient", out sWork, false);
            dr["PackagingPatient"] = "N";
            if (ValStringToBool(sWork))
                dr["PackagingPatient"] = "Y";

            return dt;
        }

        private DataTable ePrimeRx_CreatePatientTable()
        {
            DataTable dt = new DataTable("PATIENT");

            dt.Columns.Add("PATIENTNO", typeof(int));
            dt.Columns.Add("PATTYPE", typeof(string));
            dt.Columns.Add("MEDNO", typeof(string));
            dt.Columns.Add("LNAME", typeof(string));
            dt.Columns.Add("FNAME", typeof(string));
            dt.Columns.Add("MI", typeof(string));
            dt.Columns.Add("DOB", typeof(DateTime));
            dt.Columns.Add("SEX", typeof(string));
            dt.Columns.Add("GROUPNO", typeof(string));
            dt.Columns.Add("RELATION", typeof(string));

            dt.Columns.Add("PERSON_CD", typeof(string));
            dt.Columns.Add("CH_LNAME", typeof(string));
            dt.Columns.Add("CH_FNAME", typeof(string));
            dt.Columns.Add("CH_MI", typeof(string));
            dt.Columns.Add("CRD_EXPDT", typeof(DateTime));
            dt.Columns.Add("COPAYTYPE", typeof(string));
            dt.Columns.Add("COPAY", typeof(decimal));
            dt.Columns.Add("COPAY_G", typeof(decimal));
            dt.Columns.Add("COPAY_M", typeof(decimal));
            dt.Columns.Add("MEDICARE", typeof(string));

            dt.Columns.Add("OTHINS", typeof(string));
            dt.Columns.Add("ADDRSTR", typeof(string));
            dt.Columns.Add("ADDRSTRLINE2", typeof(string));
            dt.Columns.Add("ADDRCT", typeof(string));
            dt.Columns.Add("ADDRST", typeof(string));
            dt.Columns.Add("ADDRZP", typeof(string));
            dt.Columns.Add("PHONE", typeof(string));
            dt.Columns.Add("ALLERGY", typeof(string));
            dt.Columns.Add("LANGUAGE", typeof(string));
            dt.Columns.Add("P_PROV", typeof(string));

            dt.Columns.Add("PRCARE_PRV", typeof(int));
            dt.Columns.Add("DISC_CD", typeof(string));
            dt.Columns.Add("PRICE_CD", typeof(string));
            dt.Columns.Add("PRICE_CDG", typeof(string));
            dt.Columns.Add("YTD_CLAIMS", typeof(int));
            dt.Columns.Add("YTD_AMT", typeof(decimal));
            dt.Columns.Add("YTD_DED", typeof(decimal));
            dt.Columns.Add("LC_DATE", typeof(DateTime));
            dt.Columns.Add("ACCT_NO", typeof(int));
            dt.Columns.Add("CHARGETYPE", typeof(string));

            dt.Columns.Add("LDOCNO", typeof(int));
            dt.Columns.Add("FACILITYCD", typeof(string));
            dt.Columns.Add("PFPPS", typeof(string));
            dt.Columns.Add("REMARK", typeof(string));
            dt.Columns.Add("EZCAP", typeof(string));
            dt.Columns.Add("REFREMIND", typeof(string));
            dt.Columns.Add("NOTES", typeof(string));
            dt.Columns.Add("PATALTID", typeof(string));
            dt.Columns.Add("QPATID", typeof(string));
            dt.Columns.Add("PAT_LOC", typeof(string));

            dt.Columns.Add("SMOKER_CD", typeof(string));
            dt.Columns.Add("PREG_CD", typeof(string));
            dt.Columns.Add("EMPLOYERID", typeof(string));
            dt.Columns.Add("HOME_PLAN", typeof(string));
            dt.Columns.Add("ELIG_CLRCD", typeof(int));
            dt.Columns.Add("FACILITYID", typeof(string));
            dt.Columns.Add("PLAN_ID", typeof(string));
            dt.Columns.Add("SIGACK", typeof(string));
            dt.Columns.Add("SIGACKDATE", typeof(DateTime));
            dt.Columns.Add("EMAIL", typeof(string));

            dt.Columns.Add("WORKNO", typeof(string));
            dt.Columns.Add("MOBILENO", typeof(string));
            dt.Columns.Add("DELIVERY", typeof(string));
            dt.Columns.Add("ADDRSTR2", typeof(string));
            dt.Columns.Add("ADDRCT2", typeof(string));
            dt.Columns.Add("ADDRST2", typeof(string));
            dt.Columns.Add("ADDRZP2", typeof(string));
            dt.Columns.Add("CUSTCATEG", typeof(string));
            dt.Columns.Add("ALTLANGPRF", typeof(string));
            dt.Columns.Add("Active", typeof(string));

            dt.Columns.Add("PrintDrugCouns", typeof(string));
            dt.Columns.Add("CreationDate", typeof(DateTime));
            dt.Columns.Add("LastModified", typeof(DateTime));
            dt.Columns.Add("Salutation", typeof(string));
            dt.Columns.Add("PatSuffix", typeof(string));
            dt.Columns.Add("Is340B", typeof(Boolean));
            dt.Columns.Add("SSN", typeof(string));
            dt.Columns.Add("SalesPersonId", typeof(int));
            dt.Columns.Add("DriversLicense", typeof(string));
            dt.Columns.Add("UnitDoseDisp", typeof(string));

            dt.Columns.Add("Deceased", typeof(string));
            dt.Columns.Add("FamilyID", typeof(int));
            dt.Columns.Add("LMOD_BY", typeof(string));
            dt.Columns.Add("PatientResidence", typeof(string));
            dt.Columns.Add("PatAssignment", typeof(string));
            dt.Columns.Add("PCSynch", typeof(Boolean));
            dt.Columns.Add("CentralId", typeof(Int64));
            dt.Columns.Add("NCPDPStrengthForm", typeof(string));
            dt.Columns.Add("NCPDPStrengthUnitOfMeasure", typeof(string));
            dt.Columns.Add("NCPDPQuantityUnitOfMeasure", typeof(string));

            dt.Columns.Add("SpeciesCD", typeof(string));
            dt.Columns.Add("DriversLicenseExpDT", typeof(DateTime));
            dt.Columns.Add("IsWorkFlowPatient", typeof(Boolean));
            dt.Columns.Add("IsMappedWithGSDD", typeof(int));
            dt.Columns.Add("allergyConfirmDate", typeof(DateTime));
            dt.Columns.Add("ConfirmedBy", typeof(string));
            dt.Columns.Add("IsICD9MappedWithICD10", typeof(string));
            dt.Columns.Add("ICD10MappedDate", typeof(DateTime));
            dt.Columns.Add("ICD10MappedBy", typeof(string));
            dt.Columns.Add("ScriptCenter_EnrollmentStatus", typeof(string));
            dt.Columns.Add("ScriptCenter_KioskLocation", typeof(string));
            dt.Columns.Add("ScriptCenter_PaymentCode", typeof(string));
            dt.Columns.Add("EmployeeLocationCode", typeof(string));

            dt.Columns.Add("IsCentralUPDT", typeof(Boolean));
            //"PrintQrCodeOnLabel":null,
            dt.Columns.Add("CRD_EFFECTIVEDT", typeof(DateTime));
            dt.Columns.Add("CH_DOB", typeof(DateTime));
            dt.Columns.Add("AddrZipExtension", typeof(string));
            dt.Columns.Add("MaritalStatus", typeof(string));
            dt.Columns.Add("CardHolderDOB", typeof(DateTime));
            dt.Columns.Add("InsEffectiveDate", typeof(DateTime));
            dt.Columns.Add("AddrCountryCode", typeof(string));
            dt.Columns.Add("PackagingPatient", typeof(string));

            return dt;
        }


        public DataTable GetPrivackAck(string sPatNo)
        {
            DataTable dt = new DataTable();
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/PrivackAck/" + sPatNo.Trim();

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    dt = ePrimeRx_LoadDataPrivackAck((string)result);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);
            return dt;
        }

        private DataTable ePrimeRx_LoadDataPrivackAck(string rtnStr)
        {
            DataTable dt = ePrimeRx_CreatePrivackAckTable();
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);

            int nextIdx = 0;
            string sWork = string.Empty;

            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Id", out sWork, false);
            dr["PRVACKTRANSNO"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PatientId", out sWork, false);
            dr["PATIENTNO"] = ValStringToI(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DateSigned", out sWork);
            dr["DATESIGNED"] = ValStringToDateTime(sWork);
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PatAccept", out sWork);
            dr["PATACCEPT"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PrivacyText", out sWork);
            dr["PRIVACYTEXT"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "PrivacyStringSig", out sWork);
            dr["PRIVACYSIG"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "SigType", out sWork);
            dr["SIGTYPE"] = sWork;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "BinarySign", out sWork);
            //dr["PRIVACYSIG"] = sWork; actually the data type is string not byte[]
            return dt;
        }

        private DataTable ePrimeRx_CreatePrivackAckTable()
        {
            DataTable dt = new DataTable("PRIVACYACK");

            dt.Columns.Add("PRVACKTRANSNO", typeof(int));
            dt.Columns.Add("PATIENTNO", typeof(int));
            dt.Columns.Add("DATESIGNED", typeof(DateTime));
            dt.Columns.Add("PATACCEPT", typeof(string));
            dt.Columns.Add("PRIVACYTEXT", typeof(string));
            dt.Columns.Add("PRIVACYSIG", typeof(string));
            dt.Columns.Add("SIGTYPE", typeof(string));
            dt.Columns.Add("BinarySign", typeof(byte[]));
            dt.Columns.Add("EventType", typeof(string));

            return dt;
        }

        public long GetBatchIDFromRxno(long Rxno, int nrefill)
        {
            string sSql = string.Empty;
            long batchID = 0;
            /*Jenny not implemented yet
            try
            {
                sSql = string.Format("SELECT BatchId FROM IntakeQueue WHERE RxNo ='{0}' AND NRefill='{1}'", Rxno.ToString(), nrefill.ToString());
                batchID = 0;
            }
            catch (Exception ex)
            {
                batchID = -1;
                throw ex;
            }*/
            return batchID;
        }

        public DataTable GetBatchStatusfromView(string BatchID)  //Needs to implement in future
        {
            DataTable dt = null;
            string sSql = string.Empty;
            try
            {
                //PRIMEPOS-2927
                sSql = string.Format(@"SELECT V.BatchId,V.CreateDate,V.PhUser,V.ReadyByDate,V.IsReadyForPayment,ISNULL(V.ShippingPrice,0.00) as ShippingPrice,
                (SELECT COUNT(Q.BatchId) FROM IntakeQueue Q WHERE Q.BatchId=V.BatchId) AS 'RX_COUNT' FROM v_IntakeBatch V
                WHERE V.BatchId IN ({0}) ", BatchID.ToString());


                sSql = "SELECT FieldValue FROM SettingDetail WHERE FieldName = 'MailOrderType'";

                DataTable dtSettingDetail = new DataTable();

                DataColumn dcolColumn = new DataColumn("MailOrderType", typeof(string));
                dt.Columns.Add(dcolColumn);

                dt.Rows[0]["MailOrderType"] = dtSettingDetail.Rows[0][0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, string sBillStatus, bool IsBatchDelivery = false)
        {
            DataTable dt = new DataTable();
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/PatientRxDetails";

            RequestApiParameter oPatRXsParam = new RequestApiParameter();
            oPatRXsParam.PatientNo = sPatientNo;
            oPatRXsParam.FillDateFrom = dFillDateFrom;
            oPatRXsParam.FillDateTo = dFillDateTo;
            oPatRXsParam.RxStatus = sBillStatus;
            oPatRXsParam.IsBatchDelivery = IsBatchDelivery;

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, oPatRXsParam).Result).Result;
                if ((string)result != "")
                {
                    dt = ePrimeRx_LoadDataPatientRXs((string)result);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);
            return dt;
        }

        public DataTable GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, string sBillStatus, char cType)    //Jenny, not sure it is used
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public async Task<string> PrimeESC_GetRxData(string RxNo, bool singleMode = true, RxFilter filterType = RxFilter.DATEF)
        {
            string strDays = "30";
            string urlBase = sWebServUrl;
            string token = sWebServToken;
            string apiUrl = "api/PrimeESC/Prescription/" + RxNo + "/true/FillDate/" + strDays;
            string rtnStr = string.Empty;

            if (!singleMode)
            {
                switch (filterType)
                {
                    case RxFilter.ANY:
                        apiUrl = "api/PrimeESC/Prescription/" + RxNo + "/false/Any/" + strDays;
                        break;
                    case RxFilter.EVERYTHING:
                        apiUrl = "api/PrimeESC/Prescription/" + RxNo + "/false/Everything/" + strDays;
                        break;
                    case RxFilter.DATEF:
                    default:
                        apiUrl = "api/PrimeESC/Prescription/" + RxNo + "/false/FillDate/" + strDays;
                        break;
                }
            }
            ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;    //PRIMEPOS-3179 27-Jan-2023 JY Commented
            ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(urlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    rtnStr = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    string errMessage = "Failed to get patient and Rx info from Web Sevice.";
                    //Jenny logger.Error(errMessage);
                }
            }
            return rtnStr;
        }


        public async Task<string> PrimePOS_ePrimeRx_Get(string sApiUrl, RequestApiParameter oParam)
        {
            string rtnStr = string.Empty;

            ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;    //PRIMEPOS-3179 27-Jan-2023 JY Commented
            ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(sWebServUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sWebServToken);

                HttpResponseMessage response = null;
                if (oParam == null)
                {
                    response = await client.GetAsync(sApiUrl);
                }
                else
                {
                    var myContent = JsonConvert.SerializeObject(oParam);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var req = new HttpRequestMessage(HttpMethod.Post, sApiUrl) { Content = byteContent };
                    response = await client.SendAsync(req);
                }
                if (response.IsSuccessStatusCode)
                {
                    rtnStr = await response.Content.ReadAsStringAsync();
                    if (rtnStr.Equals("null", StringComparison.OrdinalIgnoreCase) || rtnStr.Equals("[]"))
                        rtnStr = string.Empty;
                }
            }
            return rtnStr;
        }

        private void ePrimeRx_LoadPatientData(string rtnResult, ref DataTable dtPatInfo)
        {
            int nextIdx = 0;
            dtPatInfo.Columns.Add("PATIENTNO", typeof(int)); //from PATIENT table
            dtPatInfo.Columns.Add("LNAME", typeof(string));
            dtPatInfo.Columns.Add("FNAME", typeof(string));
            dtPatInfo.Columns.Add("ADDRSTR", typeof(string));
            dtPatInfo.Columns.Add("ADDRCT", typeof(string));
            dtPatInfo.Columns.Add("ADDRZP", typeof(string));
            dtPatInfo.Columns.Add("ADDRST", typeof(string));
            dtPatInfo.Columns.Add("PATTYPE", typeof(string));
            dtPatInfo.Columns.Add("SIGACK", typeof(string));
            dtPatInfo.Columns.Add("SIGACKDATE", typeof(DateTime));
            dtPatInfo.Columns.Add("PATACCEPT", typeof(string));   //from PRIVACYACK table
            dtPatInfo.Columns.Add("DATESIGNED", typeof(DateTime));

            DataRow dr1 = dtPatInfo.NewRow();
            dtPatInfo.Rows.Add(dr1);

            string strPatientId = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "PatientId", out strPatientId, false);
            Int32 iPatientId = 0;
            try
            {
                iPatientId = Convert.ToInt32(strPatientId);
            }
            catch (Exception)
            { }
            dr1["PATIENTNO"] = iPatientId;
            string patFirstName = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "FirstName", out patFirstName);
            dr1["FNAME"] = patFirstName;
            string patLastName = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "LastName", out patLastName);
            dr1["LNAME"] = patLastName;
            string patAddressLine1 = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "AddressLine1", out patAddressLine1);
            dr1["ADDRSTR"] = patAddressLine1;
            string patCityName = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "CityName", out patCityName);
            dr1["ADDRCT"] = patCityName;
            string patZipCode = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "ZipCode", out patZipCode);
            dr1["ADDRZP"] = patZipCode;
            string patStateName = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "StateName", out patStateName);
            dr1["ADDRST"] = patStateName;
            string patPatType = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "PatType", out patPatType);
            dr1["PATTYPE"] = patPatType;

            string patPatAccept = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "PatAccept", out patPatAccept);
            dr1["PATACCEPT"] = "N";
            if (string.IsNullOrWhiteSpace(patPatAccept) || patPatAccept[0] == 'N')
                dr1["PATACCEPT"] = "N";
            else if (patPatAccept[0] == 'Y' || patPatAccept[0] == 'S')
                dr1["PATACCEPT"] = "Y";

            string patDateSigned = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "DateSigned", out patDateSigned);
            if (string.IsNullOrWhiteSpace(patDateSigned) || patDateSigned.Equals("Not Yet Signed", StringComparison.OrdinalIgnoreCase))
                dr1["DATESIGNED"] = DBNull.Value;
            else
            {
                try
                {
                    dr1["DATESIGNED"] = Convert.ToDateTime(patDateSigned);
                }
                catch (Exception)
                {

                }
            }

            string patSigAckDate = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "SigAckDate", out patSigAckDate);
            if (string.IsNullOrWhiteSpace(patSigAckDate) || patSigAckDate.Equals("Not Yet Signed", StringComparison.OrdinalIgnoreCase))
                dr1["SIGACKDATE"] = DBNull.Value;
            else
            {
                try
                {
                    dr1["SIGACKDATE"] = Convert.ToDateTime(patSigAckDate);
                }
                catch (Exception)
                {

                }
            }

            string patSigAck = string.Empty;
            nextIdx = ePrimeRx_GetValueByFieldName(rtnResult, nextIdx, "SigAck", out patSigAck);
            dr1["SIGACK"] = "N";
            if (string.IsNullOrWhiteSpace(patSigAck) || patSigAck[0] == 'N')
                dr1["SIGACK"] = "N";
            else if (patSigAck[0] == 'Y' || patSigAck[0] == 'S')
                dr1["SIGACK"] = "Y";
            return;
        }

        private int ePrimeRx_GetValueByFieldName(string strResult, int startIdx, string fieldName, out string fieldValue, bool typeStr = true)
        {
            int nextPos = startIdx;
            fieldValue = string.Empty;
            string tokenN = "\"" + fieldName.Trim() + "\":";
            bool isNullValue = false;

            int foundIdx = strResult.IndexOf(tokenN, startIdx, StringComparison.OrdinalIgnoreCase);
            if (foundIdx > startIdx || (foundIdx == 0 && startIdx == 0))
            {
                nextPos = foundIdx + tokenN.Length;

                int nullStart = nextPos - 1;
                if (nullStart > 2)
                    nullStart = nullStart - 1;
                int nullPos = strResult.IndexOf("null", nullStart, StringComparison.OrdinalIgnoreCase);
                if (nextPos == nullPos)
                    isNullValue = true;

                if (typeStr == true)
                {
                    if (!isNullValue)
                    {
                        int nextQuote = strResult.IndexOf("\"", nextPos + 1);
                        int valueLength = nextQuote - nextPos - 1;
                        fieldValue = strResult.Substring(nextPos + 1, valueLength);
                        nextPos += fieldValue.Length;
                    }
                    else
                    {
                        nextPos += 4;
                        fieldValue = string.Empty;
                    }
                }
                else // decimal type, or bool type
                {
                    if (!isNullValue)
                    {
                        int valueLen = 0;
                        int nextComa = strResult.IndexOf(",", nextPos);
                        if (nextComa == -1)// the end of claims record
                        {
                            valueLen = strResult.Length - nextPos;
                            int iPos = strResult.Length - 1;
                            if (strResult[iPos] == '}')
                                valueLen--;
                        }
                        else
                            valueLen = nextComa - nextPos;
                        fieldValue = strResult.Substring(nextPos, valueLen);
                        nextPos += fieldValue.Length;
                    }
                    else
                    {
                        nextPos += 4;
                        fieldValue = "0";
                    }
                }
            }
            return nextPos;
        }

        public DataTable GetRxForDelivery(string sRxNo, out string sStatus, string RefillNo, string PartialFillNo)
        {
            sStatus = "false";
            string sRefillPartial = RefillNo + "+" + PartialFillNo;
            DataTable oRx = this.GetRxs(sRxNo, sRefillPartial, "");

            if (oRx.Rows.Count > 0)
            {
                if ((!oRx.Rows[0]["STATUS"].ToString().TrimEnd().Equals("F")))
                {
                    if ((!(oRx.Rows[0]["PICKEDUP"].ToString().TrimEnd().Equals("Y"))))
                    {
                        if (oRx.Rows[0]["DELIVERY"].ToString().TrimEnd().Equals("C") || oRx.Rows[0]["DELIVERY"].ToString().TrimEnd().Equals("I"))
                            sStatus = oRx.Rows[0]["DELIVERY"].ToString().TrimEnd();
                        else
                        {
                            sStatus = "true";
                        }
                    }
                    else
                        sStatus = "Rx Has Already Been Picked Up";
                }
            }

            return oRx;
        }

        public bool RxExistsIn_ePrimeRxDb(string sRxNo, string sRefNo, string sPartialFillNo)
        {
            bool rtnCode = false;
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/RxExists";
            string sParFillNum = sPartialFillNo;
            if (string.IsNullOrWhiteSpace(sPartialFillNo) || sPartialFillNo.Trim() == "0")
                sParFillNum = "1";
            RequestApiParameter oReqParam = new RequestApiParameter();
            oReqParam.RxNo = sRxNo;
            oReqParam.RefNo = sRefNo;
            oReqParam.FillNo = sParFillNum;

            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, oReqParam).Result).Result;
                if ((string)result != "")
                {
                    if (result.Equals("true", StringComparison.OrdinalIgnoreCase))
                        rtnCode = true;
                }

            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);

            return rtnCode;
        }

        public DataSet FetchRxDetails(string RxNo, string Nrefill, string sPartialFillNo)//Needs to double check this API
        {
            DataSet ds = new DataSet();
            string sRefillPartial = Nrefill + "+" + sPartialFillNo;
            DataTable dt = GetRxs(RxNo, sRefillPartial, "");
            ds.Tables.Add(dt);
            return ds;
        }

        public DataTable PopulateConsentSource()
        {
            string errMsg = string.Empty;
            string strAPI_Url = "api/PrimePos/ConsentSource";
            DataTable dt = new DataTable("Consent_Source");
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("DisplayOrder", typeof(int));
            dt.Columns.Add("Active", typeof(bool));
            int iCSIdx = 0;
            return dt;
            /*
            try
            {
                var result = Task.Run(() => PrimePOS_ePrimeRx_Get(strAPI_Url, null).Result).Result;
                if ((string)result != "")
                {
                    string strResult = (string)result;
                    int numRec = 0;
                    for (int i = 0; i < strResult.Length; i++)
                    {
                        if (result[i] == '{')
                            numRec++;
                    }

                    for (int iCount = 0; iCount < numRec; iCount++)
                    {
                        int nextIdx = 0;
                        string sWork = string.Empty;
                        string rtnStr = string.Empty;
                        iCSIdx = ePrimeRx_GetReturnXMLRecord(strResult, iCSIdx, out rtnStr);

                        if (string.IsNullOrWhiteSpace(rtnStr))
                            continue;

                        DataRow dr = dt.NewRow();
                        //dt.Rows.Add(dr);  consent for Refill not implemented yet, Jenny

                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Id", out sWork, false);
                        dr["ID"] = ValStringToI(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Name", out sWork);
                        dr["Name"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Description", out sWork);
                        dr["Description"] = sWork;
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "DisplayOrder", out sWork, false);
                        dr["DisplayOrder"] = ValStringToI(sWork);
                        nextIdx = ePrimeRx_GetValueByFieldName(rtnStr, nextIdx, "Active", out sWork, false);
                        dr["Active"] = ValStringToBool(sWork);
                        //consent for Refill and  other type are not implemented yet, Jenny
                        if (dr["Name"].ToString().Equals("Healthix", StringComparison.OrdinalIgnoreCase))
                            dt.Rows.Add(dr);
                    }
                }
                else
                    errMsg = "Failed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message + "\nFailed to get data from ePrimeRx by API : " + strAPI_Url;
            }
            if (!string.IsNullOrWhiteSpace(errMsg))
                logger.Error(errMsg);
            return dt;
            */
        }

        public DataTable GetActivePatientConsent(int PatientNo, System.Collections.Generic.Dictionary<int, string> activeConsentList, out bool isConsentExpired, out bool isConsentHave)
        {
            DataTable dt = new DataTable();
            isConsentExpired = false;
            isConsentHave = false;

            return dt;
        }

        public DataTable GetConsentTextById(int ConsentSourceID)
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public DataTable GetConsentRelationshipById(int ConsentSourceID)
        {
            DataTable dt = new DataTable();

            return dt;
        }
        
        public DataTable GetActiveConsentStatusById(int ConsentSourceID)
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public DataTable GetActiveConsentTypeById(int ConsentSourceID)
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public DataTable PopulateConsentNameBasedOnID(string consentId)
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public DataTable GetConsentSourceByName(string consentSourceName)
        {
            DataTable dt = new DataTable();

            return dt;
        }

        public int GetConsentSourceID(string consentSourceName)
        {
            return 1;
        }

        public int GetConsentTextID(int ConsentSourceID, int languageno)
        {
            return 1;
        }

        public int GetConsentTypeID(int ConsentSourceID, string typeCode)
        {
            return 1;
        }

        public int GetConsentStatusID(int ConsentSourceID, string StatusCode)
        {
            return 1;
        }
        //PRIMEPOS-3120
        public int GetConsentValidityPeriod(int ConsentSourceID, int StatusID)
        {
            return 1;
        }
        public int GetConsentRelationShipID(int ConsentSourceID, string RelationShipString)
        {
            return 1;
        }

        private async Task<string> ePrimeRx_SendSaveData(object oParam, string sApiUrl)
        {
            string rtnStr = string.Empty;

            ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;    //PRIMEPOS-3179 27-Jan-2023 JY Commented
            ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(sWebServUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sWebServToken);

                var myContent = JsonConvert.SerializeObject(oParam);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var req = new HttpRequestMessage(HttpMethod.Post, sApiUrl) { Content = byteContent };
                HttpResponseMessage response = await client.SendAsync(req);

                if (response.IsSuccessStatusCode)
                {
                    rtnStr = await response.Content.ReadAsStringAsync();
                }
            }

            return rtnStr;
        }

        private async Task<string> ePrimeRx_SavePatientAck(object oParam, string sApiUrl)
        {
            string rtnStr = string.Empty;

            ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;    //PRIMEPOS-3179 27-Jan-2023 JY Commented
            ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(sWebServUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sWebServToken);

                var myContent = JsonConvert.SerializeObject(oParam);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var req = new HttpRequestMessage(HttpMethod.Put, sApiUrl) { Content = byteContent };
                HttpResponseMessage response = await client.SendAsync(req);

                if (response.IsSuccessStatusCode)
                {
                    rtnStr = await response.Content.ReadAsStringAsync();
                }
            }

            return rtnStr;
        }

        public void MarkDelivery(string sRxNo, string sRefNo, string sPartialFillNo, string sDelivery, string sPickedUp, DateTime PickUpDate, string sPickUpPOS, out string sError, bool isBatchDelivery)
        {
            sError = string.Empty;

            try
            {
                MarkDeliveryInputParams oParam = new MarkDeliveryInputParams();
                oParam.RxNo = ValStringToL(sRxNo);
                oParam.RefNo = ValStringToI(sRefNo);
                if (sPartialFillNo == "0")
                    oParam.FillNo = null;
                else
                    oParam.FillNo = ValStringToI(sPartialFillNo);
                //string sDelivery, 
                oParam.PickedUp = sPickedUp;
                oParam.PickUpDate = PickUpDate;
                //string sPickUpPOS,
                oParam.IsBatchDelivery = isBatchDelivery;
                var result = Task.Run(() => ePrimeRx_SendSaveData(oParam, "api/PrimePos/MarkDelivery").Result).Result;
            }
            catch (Exception ex)
            {
                sError = "Failed to mark delivery at ePrimeRx." + ex.Message;
            }
            return;
        }


        public void MarkCopayPaid(string sRxNo, string sRefNo, string sPartialFillNo, char val)
        {
            try
            {
                PrimePOSInputParams oParam = new PrimePOSInputParams();
                oParam.RxNo = ValStringToL(sRxNo);
                oParam.RefNo = ValStringToI(sRefNo);
                if (sPartialFillNo == "0")
                    oParam.FillNo = null;
                else
                    oParam.FillNo = ValStringToI(sPartialFillNo);
                oParam.CopayPaidValue = val.ToString();
                var result = Task.Run(() => ePrimeRx_SendSaveData(oParam, "api/PrimePos/MarkCopayPaid").Result).Result;
            }
            catch { }
            return;
        }

        public void SavePrivacyAck(long lPatNo, System.DateTime dtSigned, string sPatAccept, string sPrivacyText, string sSignature, string sSigType, byte[] bBinarySign)
        {
            try
            {
                PrivacyAcknowledge oParam = new PrivacyAcknowledge();
                oParam.PatientId = (int)lPatNo;
                oParam.DateSigned = dtSigned;
                if (!string.IsNullOrWhiteSpace(sPatAccept))
                    oParam.PatAccept = sPatAccept[0];
                else
                    oParam.PatAccept = ' ';
                oParam.PrivacyText = sPrivacyText;
                oParam.PrivacyStringSig = sSignature;
                if (!string.IsNullOrWhiteSpace(sSigType))
                    oParam.SigType = sSigType[0];
                else
                    oParam.SigType = null;
                //oParam.BinarySign = bBinarySign;
                //oParam.Signature = sSignature;
                var result = Task.Run(() => ePrimeRx_SendSaveData(oParam, "api/PrimePos/PrivacyAck").Result).Result;
            }
            catch { }
            return;
        }

        public void SavePatientAck(long lPatNo, string sAck, DateTime dtAckDate)
        {
            try
            {
                PatientInfoParam oParam = new PatientInfoParam();
                oParam.PatientId = (int)lPatNo;
                oParam.SigAck = false;
                if (sAck == "Y")
                    oParam.SigAck = true;
                oParam.SigAckDate = dtAckDate;
                var result = Task.Run(() => ePrimeRx_SavePatientAck(oParam, "api/PrimePos/SigAck").Result).Result; //JennyQ this API failed
            }
            catch { }
            return;
        }

        public long SaveInsSigTrans(System.DateTime dtTransDate, long lPatNo, string sInsType, string sTransData, string sSignature, string sCounselingReq, string sSigType, byte[] bBinarySign)
        {
            long rtn = 0;

            try
            {
                InsSignatureTrans oParam = new InsSignatureTrans();
                //System.DateTime dtTransDate, 
                oParam.PatientId = (int)lPatNo;
                oParam.InsType = sInsType;
                oParam.TranSignatureData = sTransData;
                if (!string.IsNullOrWhiteSpace(sCounselingReq))
                    oParam.CounselingReq = sCounselingReq[0];
                else
                    oParam.CounselingReq = null;
                if (!string.IsNullOrWhiteSpace(sSigType))
                    oParam.SigType = sSigType[0];
                else
                    oParam.SigType = null;
                //oParam.BinarySign = bBinarySign;
                oParam.Signature = sSignature;
                var result = Task.Run(() => ePrimeRx_SendSaveData(oParam, "api/PrimePos/Signature").Result).Result;
                rtn = ValStringToL((string)result);
            }
            catch { }

            return rtn;
        }

        public void SaveTransDet(long lTransNo, long lRxNo, int iRefNo, string sPartialFillNo)
        {
            try
            {
                ParamTransDet oParam = new ParamTransDet();
                oParam.InsSigTransId = (int)lTransNo;
                oParam.RxNo = lRxNo;
                oParam.RefNo = iRefNo;
                if (sPartialFillNo == "0")
                    oParam.FillNo = null;
                else
                    oParam.FillNo = ValStringToI(sPartialFillNo);
                var result = Task.Run(() => ePrimeRx_SendSaveData(oParam, "api/PrimePos/TransDet").Result).Result;
            }
            catch { }
        }

        public bool SaveDetails_ePrimeRx(DataTable dtRxTransMissionData, DataTable dtSigTransData, DataTable dtRxData)
        {
            bool rtnCode = false;

            try
            {
                PrimePOSDataModel oParam = DataLoadSaveDeatils_ePrimeRx(dtRxTransMissionData, dtSigTransData, dtRxData);
                var result = Task.Run(() => ePrimeRx_SendSaveData(oParam, "api/PrimePos/SaveDetails").Result).Result;
                if (!string.IsNullOrWhiteSpace(result) && result.Equals("true", StringComparison.OrdinalIgnoreCase))
                    rtnCode = true;
            }
            catch (Exception ex)
            {
                logger.Error("Errors happened at SaveDetails_ePrimeRx() \n" + ex.Message);
            }
            return rtnCode;
        }

        private void LoadRxPickupLogInfo(MarkDeliveryInputParams oMarkDeli, DataTable dtRxData)
        {
            string sRxNo = oMarkDeli.RxNo.ToString();
            string sRefillNo = oMarkDeli.RefNo.ToString();
            string sPartialFillNo = "0";
            if (oMarkDeli.FillNo != null)
                sPartialFillNo = oMarkDeli.FillNo.ToString();

            if (dtRxData != null && dtRxData.Rows.Count > 0)
            {
                foreach (DataRow dr in dtRxData.Rows)
                {
                    if (dr["RxNo"].ToString() == sRxNo && dr["RefillNo"].ToString() == sRefillNo && dr["PartialFillNo"].ToString() == sPartialFillNo)
                    {
                        oMarkDeli.PickByIDQ = dr["IDTYPE"].ToString();
                        oMarkDeli.PickByRelation = dr["RELATION"].ToString();
                        oMarkDeli.PickByID = dr["IDNUM"].ToString();
                        oMarkDeli.PickByIDAuth = dr["STATE"].ToString();
                        oMarkDeli.PickedupByLname = dr["LASTNAME"].ToString();
                        oMarkDeli.PickedupByFname = dr["FIRSTNAME"].ToString();

                        break;
                    }
                }
            }
        }

        private PrimePOSDataModel DataLoadSaveDeatils_ePrimeRx(DataTable dtRxTransMissionData, DataTable dtSigTransData, DataTable dtRxData)
        {
            PrimePOSDataModel oParam = new PrimePOSDataModel();

            List<MarkDeliveryInputParams> listMarkDelivery = new List<MarkDeliveryInputParams>();
            List<PrimePOSInputParams> listMarkCopay = new List<PrimePOSInputParams>();
            List<PrivacyAcknowledge> listPrivacyAck = new List<PrivacyAcknowledge>();// oPrivacyAck = null;
            List<PatientInfoParam> listPatInfo = new List<PatientInfoParam>();       // oPatSig = null;
            List<InsSignatureTrans> listInsSig = new List<InsSignatureTrans>();      // oInsSig = null;
            /*public PatientConsentParam patientConsent { get; set; } */
            for (int i = 0; i < dtRxTransMissionData.Rows.Count; i++)
            {
                if (ValStringToL(dtRxTransMissionData.Rows[i]["RxTransNo"].ToString()) <= 0)
                    continue;

                string sPartialFillNo = dtRxTransMissionData.Rows[i]["PartialFillNo"].ToString();

                MarkDeliveryInputParams oMarkDeli = new MarkDeliveryInputParams();
                oMarkDeli.RxNo = ValStringToL(dtRxTransMissionData.Rows[i]["RxNo"].ToString());
                oMarkDeli.RefNo = ValStringToI(dtRxTransMissionData.Rows[i]["Nrefill"].ToString());
                if (sPartialFillNo == "0")
                    oMarkDeli.FillNo = null;
                else
                    oMarkDeli.FillNo = ValStringToI(sPartialFillNo);
                oMarkDeli.PickedUp = dtRxTransMissionData.Rows[i]["PickedUp"].ToString();
                //oMarkDeli.PickUpDate = ValStringToDateTime(dtRxTransMissionData.Rows[i]["PickUpDate"].ToString()); //PRIMEPOS-3323
                oMarkDeli.PickUpDate = ValDateToUTCDateTime(Convert.ToDateTime(dtRxTransMissionData.Rows[i]["PickUpDate"])); //PRIMEPOS-3323
                oMarkDeli.IsBatchDelivery = ValStringToBool(dtRxTransMissionData.Rows[i]["IsDelivery"].ToString());
                LoadRxPickupLogInfo(oMarkDeli, dtRxData);
                listMarkDelivery.Add(oMarkDeli);

                PrimePOSInputParams oMarkCopay = new PrimePOSInputParams();
                oMarkCopay.RxNo = ValStringToL(dtRxTransMissionData.Rows[i]["RxNo"].ToString());
                oMarkCopay.RefNo = ValStringToI(dtRxTransMissionData.Rows[i]["Nrefill"].ToString());
                if (sPartialFillNo == "0")
                    oMarkCopay.FillNo = null;
                else
                    oMarkCopay.FillNo = ValStringToI(sPartialFillNo);
                oMarkCopay.CopayPaidValue = "Y";
                if (ValStringToBool(dtRxTransMissionData.Rows[i]["CopayPaid"].ToString()) == false)
                    oMarkCopay.CopayPaidValue = "N";
                listMarkCopay.Add(oMarkCopay);
            }

            if (dtSigTransData != null && dtSigTransData.Rows.Count > 0)
            {
                for (int k = 0; k < dtSigTransData.Rows.Count; k++)
                {
                    PrivacyAcknowledge oPrivacyAck = new PrivacyAcknowledge();
                    oPrivacyAck.PatientId = ValStringToI(dtSigTransData.Rows[k]["PatientNo"].ToString());
                    //oPrivacyAck.DateSigned = DateTime.Now; //PRIMEPOS-3323
                    oPrivacyAck.DateSigned = ValDateToUTCDateTime(DateTime.Now); //PRIMEPOS-3323
                    string sPatAccept = dtSigTransData.Rows[k]["PrivacyPatAccept"].ToString();
                    if (!string.IsNullOrWhiteSpace(sPatAccept))
                        oPrivacyAck.PatAccept = sPatAccept[0];
                    else
                        oPrivacyAck.PatAccept = ' ';
                    oPrivacyAck.PrivacyText = dtSigTransData.Rows[k]["PrivacyText"].ToString();
                    oPrivacyAck.PrivacyStringSig = string.Empty;
                    string sSigType = dtSigTransData.Rows[k]["SigType"].ToString();
                    if (!string.IsNullOrWhiteSpace(sSigType))
                        oPrivacyAck.SigType = sSigType[0];
                    else
                        oPrivacyAck.SigType = null;
                    listPrivacyAck.Add(oPrivacyAck);

                    PatientInfoParam oPatSig = new PatientInfoParam();
                    oPatSig.PatientId = ValStringToI(dtSigTransData.Rows[k]["PatientNo"].ToString());
                    oPatSig.SigAck = false;
                    if (string.IsNullOrWhiteSpace(dtSigTransData.Rows[k]["PrivacyPatAccept"].ToString()))
                        oPatSig.SigAck = null;
                    else if (dtSigTransData.Rows[k]["PrivacyPatAccept"].ToString() == "Y")
                        oPatSig.SigAck = true;
                    //oPatSig.SigAckDate = DateTime.Now; //PRIMEPOS-3323
                    oPatSig.SigAckDate = ValDateToUTCDateTime(DateTime.Now);
                    listPatInfo.Add(oPatSig);

                    InsSignatureTrans oInsSig = new InsSignatureTrans();
                    oInsSig.PatientId = ValStringToI(dtSigTransData.Rows[k]["PatientNo"].ToString());
                    oInsSig.InsType = dtSigTransData.Rows[k]["InsType"].ToString();
                    oInsSig.TranSignatureData = dtSigTransData.Rows[k]["TransData"].ToString();
                    string sCounselingReq = dtSigTransData.Rows[k]["CounselingReq"].ToString();
                    if (!string.IsNullOrWhiteSpace(sCounselingReq))
                        oInsSig.CounselingReq = sCounselingReq[0];
                    else
                        oInsSig.CounselingReq = null;
                    string sSigType2 = dtSigTransData.Rows[k]["SigType"].ToString();
                    if (!string.IsNullOrWhiteSpace(sSigType2))
                        oInsSig.SigType = sSigType[0];
                    else
                        oInsSig.SigType = null;
                    oInsSig.Signature = dtSigTransData.Rows[k]["TransSigData"].ToString();
                    listInsSig.Add(oInsSig);
                }
            }
        
            oParam.markDeliveryInputParams = listMarkDelivery;
            oParam.primePOSInputParams = listMarkCopay;
            oParam.privacyAcknowledge =  listPrivacyAck;
            oParam.sigAcknowledgement = listPatInfo;
            oParam.insSignatureTrans = listInsSig;
            if (dtSigTransData != null && dtSigTransData.Rows.Count > 0)
                oParam.BinarySign = (byte[])dtSigTransData.Rows[0]["BinarySign"];
            oParam.POSTransID = ValStringToI(dtRxTransMissionData.Rows[0]["TransID"].ToString());
            oParam.POSUserID = dtRxTransMissionData.Rows[0]["UserID"].ToString();

            return oParam;
        }

        public double ValStringToD(string sDoubleString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sDoubleString))
                    return 0.00d;

                return Convert.ToDouble(sDoubleString);
            }
            catch
            {
                return 0.00d;
            }
        }

        public int ValStringToI(string sIntString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sIntString))
                    return 0;

                return Convert.ToInt32(sIntString);
            }
            catch
            {
                return 0;
            }
        }

        public long ValStringToL(string sIntString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sIntString))
                    return 0;

                return Convert.ToInt64(sIntString);
            }
            catch
            {
                return 0;
            }
        }

        public decimal ValStringToDecimal(string sValString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sValString))
                    return 0;

                return Convert.ToDecimal(sValString);
            }
            catch
            {
                return 0;
            }
        }

        public bool ValStringToBool(string sValString)
        {
            bool rtn = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(sValString))
                    rtn = Convert.ToBoolean(sValString);
            }
            catch
            {
                rtn = false;
            }
            return rtn;
        }

        public DateTime ValStringToDateTime(string sValString)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                if (!string.IsNullOrWhiteSpace(sValString))
                    dt = Convert.ToDateTime(sValString);
            }
            catch { }
            return dt;
        }

        #region PRIMEPOS-3323
        public DateTime ValDateToUTCDateTime(DateTime sValDate)
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                if (sValDate != null)
                {
                    //dt = Convert.ToDateTime(sValDate);
                    dt = TimeZoneInfo.ConvertTimeToUtc(sValDate, TimeZoneInfo.Local);
                }
            }
            catch { }
            return dt;
        }
        #endregion
    }

}

public class RequestApiParameter
{
    public string RxNo   { get; set; }
    public string RefNo  { get; set; }
    public string FillNo { get; set; }

    public string PatientNo { get; set; }
    public DateTime FillDateFrom { get; set; }
    public DateTime FillDateTo { get; set; }
    public string RxStatus { get; set; }
    // bool SendUnbilled { get; set; }
    public bool IsBatchDelivery { get; set; }

}

public class PrimePOSDataModel
{
    public List<MarkDeliveryInputParams> markDeliveryInputParams { get; set; }
    public List<PrimePOSInputParams> primePOSInputParams { get; set; }
    public List<PrivacyAcknowledge> privacyAcknowledge { get; set; }
    public List<PatientInfoParam> sigAcknowledgement { get; set; }
    public List<InsSignatureTrans> insSignatureTrans { get; set; }
    public PatientConsentParam patientConsent { get; set; }
    public byte[] BinarySign { get; set; }
    public int POSTransID { get; set; }
    public string POSUserID { get; set; }
}

public class MarkDeliveryInputParams
{
    public long RxNo { get; set; }
    public int RefNo { get; set; }
    public int? FillNo { get; set; }

    public string PickedUp { get; set; }
    public DateTime PickUpDate { get; set; }
    public bool IsBatchDelivery { get; set; }
    public string PickByIDQ { get; set; }
    public string DropPickQ { get; set; }
    public string PickByRelation { get; set; }
    public string PickByIDAuth { get; set; }
    public string PickByID { get; set; }
    public string PickedupByFname { get; set; }
    public string PickedupByLname { get; set; }
}

public class PrimePOSInputParams
{
    public long RxNo { get; set; }
    public int RefNo { get; set; }
    public int? FillNo { get; set; }

    public string CopayPaidValue { get; set; }
}

public class PrivacyAcknowledge
{
    public int PatientId { get; set; }
    public DateTime DateSigned { get; set; }
    public char PatAccept { get; set; }
    public string PrivacyText { get; set; }
    public string PrivacyStringSig { get; set; }
    public char? SigType { get; set; }
}

public class PatientInfoParam
{
    public int PatientId { get; set; }
    public bool? SigAck { get; set; }
    public DateTime SigAckDate { get; set; }
}

public class InsSignatureTrans
{
    public int PatientId { get; set; }
    public string InsType { get; set; }
    public string TranSignatureData { get; set; }
    public char? CounselingReq { get; set; }
    public char? SigType { get; set; }
    public string Signature { get; set; }
}

public class PatientConsentParam
{
    public int PatientId { get; set; }
    public int ConsentTextID { get; set; }
    public int ConsentTypeID { get; set; }
    public int ConsentStatusID { get; set; }
    public DateTime? ConsentCaptureDate { get; set; }
    public DateTime? ConsentEffectiveDate { get; set; }
    public DateTime? ConsentEndDate { get; set; }
    public string Relation { get; set; }
    public string SigneeName { get; set; }
    public byte[] SignatureData { get; set; }
    public int ConsentSourceID { get; set; }
}

public class ParamTransDet
{
    public int InsSigTransId { get; set; }
    public long RxNo { get; set; }
    public int RefNo { get; set; }
    public int? FillNo { get; set; }
}