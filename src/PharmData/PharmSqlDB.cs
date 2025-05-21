using System;
using System.Data;
using MMSDur;
using NLog;
using System.Linq; // PRIMERX-7688 - NileshJ -23-Sept-2019
using DbAcc;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace PharmData
{
    /// <summary>
    /// Summary description for PharmSqlDB.
    /// </summary>
    internal class PharmSqlDB : MMSBClass.MMSBDClass
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        private bool isRxTxnSuccess = false;
        private bool isActiveConsent = false; //PRIMEPOS-3276
        public PharmSqlDB()
        {
            //
            // TODO: Add constructor logic here
            string appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            if (appName.Equals("POS", StringComparison.OrdinalIgnoreCase))
                appName = "PrimePOS";
            DBAccess.ApplicationName = appName;
        }

        public override void InitNames()
        {
        }

        public DataTable GetPhUser(string sPhUser)
        {
            this.sTableName = "PHUSER";
            string sSql = "Select * From PHUSER where PH_INIT = '" + sPhUser + "'";
            return base.GetRecs(sSql).Tables[0];
        }

        public DataTable GetUsers(string logintype, string lastname, string firstname)
        {
            string sSql = string.Empty;
            string sWhere = string.Empty;
            switch (logintype)
            {
                case "PA":
                    this.sTableName = "Patient";
                    sSql = "select patientno as pharmid,(lname + ',' + fname) as name,addrstr as address,addrct as city,addrst as state," +
                        "addrzp as zip,phone as phone,username,'" + logintype + "' as logintype from patient left join weblogin " +
                        "on cast(patientno as char) = pharmid ";
                    if ((lastname != "0") && (firstname != "0"))
                        sWhere += " where lname like '" + lastname + "%' and fname like '" + firstname + "%'";
                    else if ((lastname != "0") && (firstname == "0"))
                        sWhere += " where lname like '" + lastname + "%'";
                    else if ((lastname == "0") && (firstname != "0"))
                        sWhere += " where fname like '" + firstname + "%'";
                    if (sWhere.Length > 0)
                        sSql = sSql + sWhere;
                    break;
                case "DO":
                    this.sTableName = "Prescrib";
                    sSql = "select presno as pharmid,(preslnm + ',' + presfnm) as name,addrstr as address,addrct as city," +
                        "addrst as state,addrzp as zip,phone as phone,username,'" + logintype + "' as logintype  from prescrib left join weblogin " +
                        "on cast(presno as char) = pharmid";
                    if ((lastname != "0") && (firstname != "0"))
                        sWhere += " where preslnm like '" + lastname + "%' and presfnm like '" + firstname + "%'";
                    else if ((lastname != "0") && (firstname == "0"))
                        sWhere += " where preslnm like '" + lastname + "%'";
                    else if ((lastname == "0") && (firstname != "0"))
                        sWhere += " where presfnm like '" + firstname + "%'";
                    if (sWhere.Length > 0)
                        sSql = sSql + sWhere;
                    break;
                case "FO":
                    this.sTableName = "Prescrib";
                    sSql = "select presno as pharmid,(preslnm + ',' + presfnm) as name,addrstr as address,addrct as city," +
                        "addrst as state,addrzp as zip,phone as phone,username,'" + logintype + "' as logintype  from prescrib left join weblogin " +
                        "on cast(presno as char) = pharmid ";
                    if ((lastname != "0") && (firstname != "0"))
                        sWhere += " where preslnm like '" + lastname + "%' and presfnm like '" + firstname + "%'";
                    else if ((lastname != "0") && (firstname == "0"))
                        sWhere += " where preslnm like '" + lastname + "%'";
                    else if ((lastname == "0") && (firstname != "0"))
                        sWhere += " where presfnm like '" + firstname + "%'";
                    if (sWhere.Length > 0)
                        sSql = sSql + sWhere;
                    break;
                default:
                    this.sTableName = "Facility";
                    break;
            }
            try
            {
                return base.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                //MMSException mex = new MMSException();
                //mex.publish(ex);
                logger.Error(ex, "PharmaSqlDB==>GetUsers(): An Exception Occured"); //PRIMEPOS-3211
                return null;
            }
        }

        public string insertLoginDetails(DataTable dtLogin)
        {
            string res = "success";
            DataRow orow = null;
            orow = dtLogin.Rows[0];
            if (isUserNameExists(orow["username"].ToString()))
            {
                res = "exists";
            }
            else
            {
                string sSql = "insert into weblogin(username,password,logintype,pharmid,prescribrx,passwordvh,docid) " +
                    "values('" + orow["username"].ToString().Trim().ToUpper() + "','" + orow["password"].ToString().Trim().ToUpper() +
                    "','" + orow["logintype"].ToString().Trim() + "','" + orow["pharmid"].ToString().Trim() + "','Y','" +
                    orow["passwordvh"].ToString().Trim().ToUpper() + "','" + orow["docid"].ToString().Trim() + "')";
                if (!base.ExecuteSql(sSql))
                    res = "fail";
            }
            return res;
        }

        private bool isUserNameExists(string username)
        {
            string sSql = "select loginid from weblogin where username='" + username + "'";
            this.sTableName = "WEBLOGIN";
            return (this.GetRecs(sSql).Tables[0].Rows.Count > 0);
        }

        public DataTable GetPatient(string sPatNo)
        {
            string sSql = "Select * From Patient Where PATIENTNO = " + sPatNo.Trim();
            this.sTableName = "PATIENT";

            logger.Trace("GetPatient(string sPatNo) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return base.GetRecs(sSql).Tables[0];
        }

        #region Sprint-25 - PRIMEPOS-2322 01-Feb-2017 JY Added logic to get all patients w.r.t. patients FamilyId
        public DataTable GetPatientByFamilyID(int iFamilyID)
        {
            string sSql = "SELECT PATIENTNO FROM PATIENT WHERE FamilyID = " + iFamilyID;
            this.sTableName = "PATIENT";

            logger.Trace("GetPatientByFamilyID(int iFamilyID) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return base.GetRecs(sSql).Tables[0];
        }
        #endregion

        public DataTable GetPatientPayPref(string sPatNo, string sPaytype)
        {
            string sSql = "Select * From PatPayPref Where PATIENTNO = " + sPatNo.Trim();
            if (sPaytype.Trim().Length > 0)
                sSql += " AND Paytype = '" + sPaytype + "'";
            this.sTableName = "PATPAYPREF";
            return base.GetRecs(sSql).Tables[0];
        }

        /// <summary>
        /// This Function Get the Patient Notes by Patient NO
        /// </summary>
        /// Author: Manoj Kumar 1/28/2013
        /// <param name="sPatNo"></param>
        /// <returns>DataTable</returns>
        public DataTable GetPatientNotes(string sPatNo)
        {
            string sSql = "SELECT NoteId, CategoryId, EntityId, EntityIdType, Note, POPUPMSG from Notes where POPUPMSG = '1' and EntityIdType = '1' and Note !='' and " +
                "CategoryId = (SELECT CategoryId from NotesCategory where Category = 'POSPOPUP') and EntityId = '" + sPatNo.Trim() + "'";
            this.sTableName = "PATIENTNOTES";
            return base.GetRecs(sSql).Tables[0];
        }

        /// <summary>
        /// This Function Get the Rx Notes
        /// </summary>
        /// Author: Manoj Kumar 1/28/2013
        /// <param name="sRxNo"></param>
        /// <returns>DataTable</returns>
        public DataTable GetRxNotes(string sRxNo)
        {
            string sSql;
            string[] arrRxNo = sRxNo.Split('-'); //PRIMEPOS-3449
            if (arrRxNo!=null && arrRxNo.Length > 0) //PRIMEPOS-3449
            {
                sSql = "SELECT NoteId, CategoryId, EntityId, EntityIdType, Note,POPUPMSG from Notes where POPUPMSG = '1' and EntityIdType = '5' and ISNULL(Note,'') !='' and " +
                "CategoryId = (Select CategoryId from NotesCategory where Category = 'POSPOPUP') and EntityId = '" + arrRxNo[0].Trim() + "'";
            }
            else
            {
                sSql = "SELECT NoteId, CategoryId, EntityId, EntityIdType, Note,POPUPMSG from Notes where POPUPMSG = '1' and EntityIdType = '5' and ISNULL(Note,'') !='' and " +
                "CategoryId = (Select CategoryId from NotesCategory where Category = 'POSPOPUP') and EntityId = '" + sRxNo.Trim() + "'";
            }

            this.sTableName = "RXNOTES";
            return base.GetRecs(sSql).Tables[0];
        }

        /// <summary>
        /// Get the Patient Name and PatPayPref
        /// </summary>
        /// Author: Manoj, Date: 12/4/2012
        /// <param name="sAccNo"></param>
        /// <param name="sPayType"></param>
        /// <returns></returns>
        public DataTable GetPatientPayPrefByAccNo(string sAccNo, string sPayType)
        {
            string sSql = "SELECT PT.LNAME, PT.FNAME,PF.* from PATIENT PT INNER JOIN PatPayPref PF on " +
                "PT.PATIENTNO = PF.PatientNo Where ACCT_NO = '" + sAccNo.Trim() + "'";
            if (sPayType.Trim().Length > 0)
                sSql += " AND PayType = '" + sPayType.Trim() + "'";
            this.sTableName = "PATACCINFO";

            logger.Trace("GetPatientPayPrefByAccNo(string sAccNo, string sPayType) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return base.GetRecs(sSql).Tables[0];
        }
        /// <summary>
        /// PRIMEPOS-3103 Get the Patient Name and PatPayPref
        /// </summary>
        /// <param name="sPatientNo"></param>
        /// <param name="sPayType"></param>
        /// <returns></returns>
        public DataTable GetPatientPayPrefByPatientNo(string sPatientNo, string sPayType)
        {
            string sSql = "SELECT PT.LNAME, PT.FNAME,PF.* from PATIENT PT INNER JOIN PatPayPref PF on " +
                "PT.PATIENTNO = PF.PatientNo Where PT.PATIENTNO = '" + sPatientNo.Trim() + "'";
            if (sPayType.Trim().Length > 0)
                sSql += " AND PayType = '" + sPayType.Trim() + "'";
            this.sTableName = "PATACCINFO";

            logger.Trace("GetPatientPayPrefByPatientNo(string sAccNo, string sPayType) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return base.GetRecs(sSql).Tables[0];
        }
        /// <summary>
        /// Get the Patient info by Account NO
        /// </summary>
        /// Author: Manoj, Date: 12/4/2012
        /// <param name="sAccNo"></param>
        /// <param name="sPayType"></param>
        /// <returns></returns>
        public DataTable GetPatientByAccNo(string sAccNo)
        {
            string sSql = "Select * From Patient Where ACCT_NO = " + sAccNo;
            this.sTableName = "PATIENTBYACCNO";

            logger.Trace("GetPatientByAccNo(string sAccNo) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return base.GetRecs(sSql).Tables[0];
        }

        #region PRIMEPOS-2536 14-May-2019 JY Added
        public DataTable GetPatientByRxNo(string sRxNo)
        {
            string sSql = "SELECT TOP 1 cl.PATIENTNO FROM claims cl WHERE cl.RXNO = " + sRxNo + " ORDER BY cl.RXNO DESC";
            this.sTableName = "PATIENTBYRxNo";

            logger.Trace("GetPatientByRxNo(string sRxNo) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return base.GetRecs(sSql).Tables[0];
        }
        #endregion

        public DataTable GetDrug(string sNdc)
        {
            string sSql = "Select * From Drug";
            if (sNdc != "-1")
                sSql += " Where DRGNDC = '" + sNdc + "'";
            this.sTableName = "DRUG";

            logger.Trace("GetDrug(string sNdc) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetDrugCategory()
        {
            string sSql = "SELECT txrCode As Code, txrDescription as Description FROM DrugCategory WHERE Len(RTrim(txrCode)) <=2";
            this.sTableName = "DRUGCATEGORY";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetDrugOrderedByDrugCatDS(string sFromDate, string sToDate, string sFacility, string sDrugCategory, string sPrescriber, string sPatient)
        {
            // FieldsToSelect:	PRESNO , PATIENTNO , NDC , DATEF , RXNO , QUANT, DAYS
            // Tables List:		PATIENT(PATIENTNO,LNAME+FNAME) , PRESCRIB(PRESNO,PRESLNM+PRESFNM) , 	DRUG(DRGNDC,DRGNAME)

            string sSQL = "SELECT " +
                " CLAIMS.PRESNO " +
                " , CLAIMS.PATIENTNO " +
                " , ISNULL(RTRIM(PRESCRIB.PRESLNM),'') + ', ' + ISNULL(PRESCRIB.PRESFNM,'') As PresName" +
                " , ISNULL(RTRIM(PATIENT.LNAME),'') + ', ' + ISNULL(PATIENT.FNAME,'') As PatName " +
                " , DRUG.DRGNAME As drugname " +
                " , DRUG.DRGNDC " +
                " , CLAIMS.DATEF " +
                " , CLAIMS.RXNO " +
                " , CLAIMS.QUANT " +
                " , CLAIMS.DAYS " +
                " FROM " +
                " CLAIMS " +
                " , DRUG " +
                " , PRESCRIB " +
                " , PATIENT " +
                " WHERE " +
                " CLAIMS.NDC = DRUG.DRGNDC " +
                " AND CLAIMS.PRESNO = PRESCRIB.PRESNO " +
                " AND CLAIMS.PATIENTNO = PATIENT.PATIENTNO " +
                " AND PATIENT.FACILITYCD = '" + sFacility + "'";

            if (sFromDate != "" && sToDate != "")
                sSQL = sSQL + " AND Convert(datetime,CLAIMS.DATEF,109) between convert(datetime, cast('" + sFromDate + " 00:00:00' as datetime) ,113) and convert(datetime, cast('" + sToDate + " 23:59:59' as datetime) ,113)";

            if (sDrugCategory != "")
                sSQL = sSQL + " AND (" + sDrugCategory + ")";

            if (sPatient != "" && sPatient != "NEW" && sPatient != "0")
                sSQL = sSQL + " AND CLAIMS.PATIENTNO = " + sPatient.ToString();

            if (sPrescriber != "" && sPrescriber != "NEW" && sPrescriber != "0")
                sSQL = sSQL + " AND CLAIMS.PRESNO =" + sPrescriber.ToString();

            this.sTableName = "CLAIMS";
            DataTable dt = null;
            dt = base.GetRecs(sSQL).Tables[0];
            return dt;
        }

        public DataTable GetAdvImg(string sAdvImg)
        {
            this.sTableName = "AdvImg";
            string sSql = string.Empty;
            try
            {
                if (sAdvImg == "-1")
                {
                    sSql = "select * from AdvImg";
                }
                else
                    sSql = "select * from AdvImg where imgid = " + sAdvImg;

                return base.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public string CheckDDIDUP(string sDrgNdc, string sTxrCode, long lPatNo, int iDDIDays, out DataTable oDtInteracts, out bool bFoundDupDrugs, out string sDupDrugs)
        //{
        //    Dur oDur=new Dur();
        //    return oDur.CheckDDIDUP(sDrgNdc, sTxrCode, lPatNo, iDDIDays, out oDtInteracts, out bFoundDupDrugs, out sDupDrugs);
        //}

        public bool CheckAllergy(string strTxrCode, string strNDC, string strPatAllergy, out string strRetInfo)
        {
            Dur oDur = new Dur();
            strRetInfo = "";
            //return oDur.CheckAllergy(strTxrCode,strNDC,strPatAllergy,out strRetInfo);
            return false;
        }

        public void SaveDrug(DataTable oTable)
        {
            base.SaveTable(oTable);
        }

        public DataTable GetWebDrug(string sNdc)
        {
            string sSql = "select * from WebDrug where drgNDC = '" + sNdc + "'";
            this.sTableName = "WEBDRUG";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetDoctor(string sPresNo)
        {
            string sSql = "Select * From PRESCRIB where PRESNO = " + sPresNo;
            this.sTableName = "PRESCRIB";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetFacilityDoctors(string sFacID)
        {
            string sSql = "select presno,ltrim(rtrim(presfnm)) + ',' + ltrim(rtrim(preslnm)) as presname from prescrib where presno in" +
                "(select docid from weblogin where pharmid='" + sFacID + "')";
            this.sTableName = "WEBLOGIN";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetDoctor(string fname, string lname)
        {
            string sSql = "Select * From PRESCRIB where PRESLNM like '" + lname + "%' and PRESFNM like '" + fname + "%'";
            this.sTableName = "PRESCRIB";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetConstant()
        {
            string sSql = "Select * From Constant";
            this.sTableName = "CONSTANT";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetLastRefill(string sRxNo, string sRefNo)
        {
            //Sprint-27 - PRIMEPOS-2455 25-Oct-2017 JY fetched drug name from drug table instead of claims
            //PRIMEPOS-2789 04-Feb-2020 JY Added VRFStage field
            string sSql = "SELECT cl.PATTYPE,cl.BILLTYPE,INS.SUB_TYPE, IsNull(cl.Status,'') as Status,cl.RXNO,cl.PRESNO, CASE WHEN (cl.BILLTYPE='C' OR RTRIM(cl.PATTYPE)='C' OR (ins.sub_type='0' )) THEN ISNULL(cl.AMOUNT,0)+ISNULL(cl.PFEE,0)+ISNULL(cl.OTHFEE,0)+ISNULL(cl.OTHAMT,0)+ISNULL(cl.STax,0)- ISNULL(cl.DISCOUNT,0) ELSE cl.COPAY END AS PATAMT,cl.NDC,DG.DRGNAME,cl.DATEO,cl.DATEF,cl.QUANT,cl.QTY_ORD,cl.DAYS,cl.TREFILLS,cl.NREFILL," +
               "(cl.sig +','+ cl.siglines) siglines,dg.strong,dg.form,dg.units,cl.PATIENTNO,cl.PICKEDUP,cl.PICKUPDATE,cl.PICKUPTIME,cl.PICKUPFROM,cl.AMOUNT,cl.COPAY,cl.TOTAMT,cl.PFEE,cl.STAX,cl.DISCOUNT,cl.OTHFEE,cl.OTHAMT,cl.BILLEDAMT,cl.UNC,RTRIM(DG.DRGNAME)+' '+RTRIM(DG.STRONG)+' '+RTRIM(DG.FORM) AS DETDRGNAME ,cl.pickuppos," +
               //", IsNull((Select VerifStatus From PharmVerifLog PVL Where PVL.RXNo=Cl.RXNo And PVL.RefillNo=Cl.NRefill), '') as Verified," +
               " ISNULL(PVL.VerifStatus, '') AS Verified, ISNULL(PVL.VRFStage, 0) AS VRFStage," +
               " DG.CLASS FROM claims cl" +
               " INNER JOIN drug dg ON cl.ndc = dg.drgndc" +
               " INNER JOIN inscar ins ON cl.pattype = ins.ic_code" +
               " LEFT JOIN PharmVerifLog PVL ON PVL.RXNo = cl.RXNo And PVL.RefillNo = cl.NRefill" +
               " WHERE cl.rxno = " + sRxNo;

            if (!string.IsNullOrEmpty(sRefNo) && sRefNo.Length > 0)
                sSql += " AND cl.NREFILL = " + sRefNo;

            sSql += " ORDER BY cl.NREFILL";
            this.sTableName = "CLAIMS";

            logger.Trace("GetLastRefill(string sRxNo, string sRefNo) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added            
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetRxs(string sRxNo, string sRefNo, string sBillStatus)
        {
            //Added cl.DELIVERY Col in select query by shitaljit on 5Sept2013
            //Sprint-25 - PRIMEPOS-2322 31-Jan-2017 JY Added PatientName and FamilyID
            //Sprint-27 - PRIMEPOS-2455 25-Oct-2017 JY fetched drug name from drug table instead of claims
            //PRIMEPOS-2789 04-Feb-2020 JY Added VRFStage field
            string sSql = "SELECT cl.PATTYPE,cl.BILLTYPE,IsNull(cl.Status,'') as Status,cl.RXNO," +
                " LTRIM(RTRIM(ISNULL(p.LNAME, ''))) + ', ' + LTRIM(RTRIM(ISNULL(p.FNAME, ''))) AS PatientName," +
                " cl.PRESNO, CASE WHEN (cl.BILLTYPE='C' OR RTRIM(cl.PATTYPE)='C' OR (ins.sub_type='0' )) THEN ISNULL(cl.AMOUNT,0)+ISNULL(cl.PFEE,0)+ISNULL(cl.OTHFEE,0)+ISNULL(cl.OTHAMT,0)+ISNULL(cl.STax,0)- ISNULL(cl.DISCOUNT,0) ELSE cl.COPAY END AS PATAMT,cl.NDC,DG.DRGNAME,cl.DATEO,cl.DATEF,cl.QUANT,cl.QTY_ORD,cl.DAYS,cl.TREFILLS,cl.NREFILL, cl.DELIVERY," +
                " (cl.sig +','+ cl.siglines) siglines,dg.strong,dg.form,dg.units,dg.class,cl.PATIENTNO,cl.PICKEDUP,cl.PICKUPDATE,cl.PICKUPTIME,cl.PICKUPFROM,cl.AMOUNT,cl.COPAY,cl.TOTAMT,cl.PFEE,cl.STAX,cl.DISCOUNT,cl.OTHFEE,cl.OTHAMT,cl.BILLEDAMT,cl.UNC,RTRIM(DG.DRGNAME)+' '+RTRIM(DG.STRONG)+' '+RTRIM(DG.FORM) AS DETDRGNAME ,cl.pickuppos," +
                //" ,IsNull((Select VerifStatus From PharmVerifLog PVL Where PVL.RXNo=Cl.RXNo And PVL.RefillNo=Cl.NRefill), '') as Verified" +
                " ISNULL(PVL.VerifStatus, '') AS Verified, ISNULL(PVL.VRFStage, 0) AS VRFStage," +
                " b.BinNo AS BIN, CASE WHEN a.BinStatus = 'I' THEN 'IN' WHEN a.BinStatus = 'O' THEN 'OUT' WHEN a.BinStatus = 'R' THEN 'Remove' ELSE a.BinStatus END AS BinStatus," +
                " p.FamilyID, cl.PHARMACIST, cl.SIG, cl.COPAYPAID, cl.RXNOTES" +
                " FROM claims cl " +
                " INNER JOIN drug dg ON cl.ndc = dg.drgndc " +
                " INNER JOIN inscar ins ON cl.pattype = ins.ic_code " +
                " INNER JOIN PATIENT p ON p.PATIENTNO = cl.PATIENTNO " +
                " LEFT JOIN RXBin a ON a.RXNo = cl.RXNO AND a.RefillNo = cl.NREFILL " +
                " LEFT JOIN Bin b ON a.BinID = b.BinID " +
                " LEFT JOIN PharmVerifLog PVL ON PVL.RXNo = cl.RXNo And PVL.RefillNo = cl.NRefill" +
                " WHERE cl.rxno = " + sRxNo;

            if (sBillStatus.Trim().Length > 0)
            {
                sSql += " and IsNull(cl.Status,'')='" + sBillStatus + "' ";
            }

            if (sRefNo.Length > 0 && sRefNo != "-1")
                sSql += " AND cl.NREFILL = " + sRefNo;

            //sSql += " ORDER BY cl.NREFILL";
            sSql += " ORDER BY p.LNAME, p.FNAME, cl.DATEF, cl.RXNO";
            this.sTableName = "CLAIMS";

            logger.Trace("GetRxs(string sRxNo, string sRefNo, string sBillStatus) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, string sBillStatus, bool IsBatchDelivery = false) // PRIMERX-7688 - NileshJ - Added :  bool IsBatchDelivery = false - 25-Sept-2019
        {
            string sSql = string.Empty;
            //Sprint-25 - PRIMEPOS-2322 31-Jan-2017 JY Added PatientName and FamilyID
            //Sprint-27 - PRIMEPOS-2455 25-Oct-2017 JY fetched drug name from drug table instead of claims
            //PRIMEPOS-2789 04-Feb-2020 JY Added VRFStage field
            //PRIMEPOS-3008 30-Sep-2021 JY Added cl.DELIVERY
            if (sPatientNo.Trim() == string.Empty)
            {
                sSql = "select cl.PATTYPE,cl.BILLTYPE,IsNull(cl.Status,'') as Status,cl.RXNO," +
                    "LTRIM(RTRIM(ISNULL(p.LNAME, ''))) + ', ' + LTRIM(RTRIM(ISNULL(p.FNAME, ''))) AS PatientName," +
                    "cl.PRESNO, CASE WHEN (cl.BILLTYPE='C' OR RTRIM(cl.PATTYPE)='C' OR (ins.sub_type='0' )) THEN ISNULL(cl.AMOUNT,0)+ISNULL(cl.PFEE,0)+ISNULL(cl.OTHFEE,0)+ISNULL(cl.OTHAMT,0)+ISNULL(cl.STax,0)- ISNULL(cl.DISCOUNT,0) ELSE cl.COPAY END AS PATAMT,cl.NDC,DG.DRGNAME,cl.DATEO,cl.DATEF,cl.QUANT,cl.QTY_ORD,cl.DAYS,cl.TREFILLS,cl.NREFILL, cl.DELIVERY," +
                    "(cl.sig +','+ cl.siglines) siglines,dg.strong,dg.form,dg.units,dg.class,cl.PATIENTNO,cl.PICKEDUP,cl.PICKUPDATE,cl.PICKUPTIME,cl.PICKUPFROM,cl.AMOUNT,cl.COPAY,cl.TOTAMT,cl.PFEE,cl.STAX,cl.DISCOUNT,cl.OTHFEE,cl.OTHAMT,cl.BILLEDAMT,cl.UNC,RTRIM(DG.DRGNAME)+' '+RTRIM(DG.STRONG)+' '+RTRIM(DG.FORM) AS DETDRGNAME ,cl.pickuppos," +
                    //" ,IsNull((Select Top 1 VerifStatus From PharmVerifLog PVL Where PVL.RXNo=Cl.RXNo And PVL.RefillNo=Cl.NRefill Order by VerifDate), '') as Verified " +
                    " ISNULL(PVL.VerifStatus, '') AS Verified, ISNULL(PVL.VRFStage, 0) AS VRFStage," +
                    " b.BinNo AS BIN, CASE WHEN a.BinStatus = 'I' THEN 'IN' WHEN a.BinStatus = 'O' THEN 'OUT' WHEN a.BinStatus = 'R' THEN 'Remove' ELSE a.BinStatus END AS BinStatus," +    //,ISNULL((SELECT B.BinNo FROM RXBIN RB INNER JOIN Bin B ON RB.BinID=B.BinID WHERE RB.RXNo=Cl.RXNo AND RB.RefillNo =Cl.NRefill),'') AS BIN
                    " p.FamilyID FROM claims cl " +
                    " INNER JOIN drug dg ON cl.ndc = dg.drgndc " +
                    " INNER JOIN inscar ins ON cl.pattype = ins.ic_code " +
                    " INNER JOIN PATIENT p ON p.PATIENTNO = cl.PATIENTNO " +
                    " LEFT JOIN RXBin a ON a.RXNo = cl.RXNO AND a.RefillNo = cl.NREFILL " +
                    " LEFT JOIN Bin b ON a.BinID = b.BinID " +
                    " LEFT JOIN PharmVerifLog PVL ON PVL.RXNo = cl.RXNo And PVL.RefillNo = cl.NRefill" +
                    " WHERE IsNUll(Pickedup,'')<>'Y' and IsNUll(PickupPOS,'')<>'Y' " + //unpicked rx only
                                                                                       //" and IsNull(cl.Status,'')='B' " + //billed RX only
                                                                                       //" and cl.Status<>'F' " + //filed RX only
                                                                                       //" and cl.Status<>'T' " + // RXs should not be transferd
                    " and cl.PatientNo ='" + sPatientNo + "'" +
                    " and cl.DATEF between cast('" + dFillDateFrom.ToString("d") + "' as datetime ) and cast('" + dFillDateTo.ToString("d") + "' as datetime)";
            }
            else
            {
                sSql = "select cl.PATTYPE,cl.BILLTYPE,IsNull(cl.Status,'') as Status,cl.RXNO," +
                    " LTRIM(RTRIM(ISNULL(p.LNAME, ''))) + ', ' + LTRIM(RTRIM(ISNULL(p.FNAME, ''))) AS PatientName," +
                    " cl.PRESNO, CASE WHEN (cl.BILLTYPE='C' OR RTRIM(cl.PATTYPE)='C' OR (ins.sub_type='0' )) THEN ISNULL(cl.AMOUNT,0)+ISNULL(cl.PFEE,0)+ISNULL(cl.OTHFEE,0)+ISNULL(cl.OTHAMT,0)+ISNULL(cl.STax,0)- ISNULL(cl.DISCOUNT,0) ELSE cl.COPAY END AS PATAMT,cl.NDC,DG.DRGNAME,cl.DATEO,cl.DATEF,cl.QUANT,cl.QTY_ORD,cl.DAYS,cl.TREFILLS,cl.NREFILL,cl.DELIVERY," +
                    " (cl.sig +','+ cl.siglines) siglines,dg.strong,dg.form,dg.units,dg.class,cl.PATIENTNO,cl.PICKEDUP,cl.PICKUPDATE,cl.PICKUPTIME,cl.PICKUPFROM,cl.AMOUNT,cl.COPAY,cl.TOTAMT,cl.PFEE,cl.STAX,cl.DISCOUNT,cl.OTHFEE,cl.OTHAMT,cl.BILLEDAMT,cl.UNC,RTRIM(DG.DRGNAME)+' '+RTRIM(DG.STRONG)+' '+RTRIM(DG.FORM) AS DETDRGNAME ,cl.pickuppos," +
                    //" ,IsNull((Select Top 1 VerifStatus From PharmVerifLog PVL Where PVL.RXNo=Cl.RXNo And PVL.RefillNo=Cl.NRefill Order by VerifDate), '') as Verified " +
                    " ISNULL(PVL.VerifStatus,'') AS Verified, ISNULL(PVL.VRFStage, 0) AS VRFStage," +
                    " b.BinNo AS BIN, CASE WHEN a.BinStatus = 'I' THEN 'IN' WHEN a.BinStatus = 'O' THEN 'OUT' WHEN a.BinStatus = 'R' THEN 'Remove' ELSE a.BinStatus END AS BinStatus," +    //,ISNULL((SELECT B.BinNo FROM RXBIN RB INNER JOIN Bin B ON RB.BinID=B.BinID WHERE RB.RXNo=Cl.RXNo AND RB.RefillNo =Cl.NRefill),'') AS BIN
                    " p.FamilyID FROM claims cl " +
                    " INNER JOIN drug dg ON cl.ndc = dg.drgndc " +
                    " INNER JOIN inscar ins ON cl.pattype = ins.ic_code " +
                    " INNER JOIN PATIENT p ON p.PATIENTNO = cl.PATIENTNO " +
                    " LEFT JOIN RXBin a ON a.RXNo = cl.RXNO AND a.RefillNo = cl.NREFILL " +
                    " LEFT JOIN Bin b ON a.BinID = b.BinID " +
                    " LEFT JOIN PharmVerifLog PVL ON PVL.RXNo = cl.RXNo And PVL.RefillNo = cl.NRefill" +
                   " where IsNUll(PickupPOS,'')<>'Y' " + //unpicked rx only
                                                         //" and IsNull(cl.Status,'')='B' " + //billed RX only
                                                         //" and cl.Status<>'F' " + //filed RX only
                                                         //" and cl.Status<>'T' " + // RXs should not be transferd
                    " and cl.PatientNo IN (" + sPatientNo + ")" +
                    " and cl.DATEF between cast('" + dFillDateFrom.ToString("d") + "' as datetime ) and cast('" + dFillDateTo.ToString("d") + "' as datetime)";
            }
            #region NileshJ - PRIMERX-7688  - 25-Sept-2019
            if (!IsBatchDelivery)
            {
                sSql += " and IsNUll(Pickedup, '') <> 'Y'"; // Moved this condition out from above query to separate place for skipping Pickup condition - NileshJ - PRIMERX-7688
            }
            #endregion
            if (sBillStatus.Trim().Length > 0)
            {
                sSql += " and IsNull(cl.Status,'')='" + sBillStatus + "' ";
            }

            //sSql += "  ORDER BY cl.DATEF ";
            sSql += " ORDER BY p.LNAME, p.FNAME, cl.DATEF, cl.RXNO";
            this.sTableName = "CLAIMS";

            logger.Trace("GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, string sBillStatus) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, string sBillStatus, char cType)    //PRIMEPOS-2036 22-Jan-2019 JY Added
        {
            //PRIMEPOS-3008 30-Sep-2021 JY Added cl.DELIVERY
            string sSql = string.Empty;
            if (cType == 'F')    //PRIMEPOS-2036 22-Jan-2019 JY Added
            {
                sSql = "select cl.PATTYPE,cl.BILLTYPE,IsNull(cl.Status,'') as Status,cl.RXNO," +
                        " LTRIM(RTRIM(ISNULL(p.LNAME, ''))) + ', ' + LTRIM(RTRIM(ISNULL(p.FNAME, ''))) AS PatientName," +
                        " cl.PRESNO, CASE WHEN (cl.BILLTYPE='C' OR RTRIM(cl.PATTYPE)='C' OR (ins.sub_type='0' )) THEN ISNULL(cl.AMOUNT,0)+ISNULL(cl.PFEE,0)+ISNULL(cl.OTHFEE,0)+ISNULL(cl.OTHAMT,0)+ISNULL(cl.STax,0)- ISNULL(cl.DISCOUNT,0) ELSE cl.COPAY END AS PATAMT,cl.NDC,DG.DRGNAME,cl.DATEO,cl.DATEF,cl.QUANT,cl.QTY_ORD,cl.DAYS,cl.TREFILLS,cl.NREFILL,cl.DELIVERY," +
                        " (cl.sig +','+ cl.siglines) siglines,dg.strong,dg.form,dg.units,dg.class,cl.PATIENTNO,cl.PICKEDUP,cl.PICKUPDATE,cl.PICKUPTIME,cl.PICKUPFROM,cl.AMOUNT,cl.COPAY,cl.TOTAMT,cl.PFEE,cl.STAX,cl.DISCOUNT,cl.OTHFEE,cl.OTHAMT,cl.BILLEDAMT,cl.UNC,RTRIM(DG.DRGNAME)+' '+RTRIM(DG.STRONG)+' '+RTRIM(DG.FORM) AS DETDRGNAME ,cl.pickuppos," +
                        //" ,IsNull((Select Top 1 VerifStatus From PharmVerifLog PVL Where PVL.RXNo=Cl.RXNo And PVL.RefillNo=Cl.NRefill Order by VerifDate), '') as Verified " +
                        " ISNULL(PVL.VerifStatus, '') AS Verified, ISNULL(PVL.VRFStage, 0) AS VRFStage," +
                        " b.BinNo AS BIN, CASE WHEN a.BinStatus = 'I' THEN 'IN' WHEN a.BinStatus = 'O' THEN 'OUT' WHEN a.BinStatus = 'R' THEN 'Remove' ELSE a.BinStatus END AS BinStatus, " +    //,ISNULL((SELECT B.BinNo FROM RXBIN RB INNER JOIN Bin B ON RB.BinID=B.BinID WHERE RB.RXNo=Cl.RXNo AND RB.RefillNo =Cl.NRefill),'') AS BIN
                        " p.FamilyID FROM claims cl " +
                        " INNER JOIN drug dg ON cl.ndc = dg.drgndc " +
                        " INNER JOIN inscar ins ON cl.pattype = ins.ic_code " +
                        " INNER JOIN PATIENT p ON p.PATIENTNO = cl.PATIENTNO " +
                        " LEFT JOIN RXBin a ON a.RXNo = cl.RXNO AND a.RefillNo = cl.NREFILL " +
                        " LEFT JOIN Bin b ON a.BinID = b.BinID " +
                        " LEFT JOIN PharmVerifLog PVL ON PVL.RXNo = cl.RXNo And PVL.RefillNo = cl.NRefill" +
                        " WHERE IsNUll(Pickedup,'') <> 'Y' and IsNUll(PickupPOS,'') <> 'Y' " +
                        " AND P.FACILITYCD = '" + sPatientNo + "'" +
                        " and cl.DATEF between cast('" + dFillDateFrom.ToString("d") + "' as datetime ) and cast('" + dFillDateTo.ToString("d") + "' as datetime)";
            }

            if (sBillStatus.Trim().Length > 0)
            {
                sSql += " and IsNull(cl.Status,'')='" + sBillStatus + "' ";
            }

            sSql += " ORDER BY p.LNAME, p.FNAME, cl.DATEF, cl.RXNO";
            this.sTableName = "CLAIMS";

            logger.Trace("GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, string sBillStatus, char cType) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return this.GetRecs(sSql).Tables[0];
        }

        public string GetBillStatus(string sRxNo, string sRefNo)
        {
            string status = string.Empty;
            string sSqlString = "SELECT STATUS from claims WHERE rtrim(RXNO) = '" + sRxNo.Trim() + "' and NREFILL ='" + sRefNo.Trim() + "'";

            logger.Trace("GetBatchStatusfromView(string BatchID) - SQL - " + sSqlString);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            object oBillStatus = this.db.DataReaderScalar(sSqlString);

            if (oBillStatus != null)
            {
                status = oBillStatus.ToString();
            }
            return status;
        }

        public DataTable GetPrivackAck(string sPatNo)
        {
            string sSql = "Select top 1 * From PrivacyAck Where PATIENTNO = " + sPatNo.Trim() + " order by prvacktransno desc";
            this.sTableName = "PrivacyAck";
            return base.GetRecs(sSql).Tables[0];
        }

        public DataSet GetPrivackAckAndConsentInfo(string sPatNo, int consentType)
        {
            DataSet ds = new DataSet();

            this.sTableName = "PrivacyAck";
            string sSql = "Select top 1 * From PrivacyAck Where PATIENTNO = " + sPatNo.Trim() + " order by prvacktransno desc";
            sSql += " SELECT top 1 * from Patient_Consent Where ConsentTypeID= " + consentType.ToString() + " and PATIENTNO = " + sPatNo.Trim() + " order by ID desc";

            ds = this.GetRecs(sSql);
            if (ds != null)
            {
                ds.Tables[0].TableName = "PrivacyAck";
                ds.Tables[1].TableName = "Patient_Consent";
            }
            return ds;
        }

        public DataSet GetConsentText(int consentSource)
        {
            DataSet ds = new DataSet();

            this.sTableName = "ConsentTextVersion";
            string sSql = " SELECT top 1 * From ConsentTextVersion Where SourceID= " + consentSource.ToString() + " order by ID desc";
            ds = this.GetRecs(sSql);
            if (ds != null)
            {
                ds.Tables[0].TableName = "ConsentTextVersion";
            }

            return ds;
        }

        public DataSet GetConsentReferenceData()
        {
            DataSet ds = new DataSet();

            this.sTableName = "Consent_Source";
            string sSql = "Select * from Consent_Source ";
            sSql += " SELECT * from  Consent_Status ";
            sSql += " SELECT * from  Consent_RelationShip ";
            sSql += " SELECT * from  Consent_Type ";

            ds = this.GetRecs(sSql);
            if (ds != null)
            {
                ds.Tables[0].TableName = "Consent_Source";
                ds.Tables[1].TableName = "Consent_Status";
                ds.Tables[2].TableName = "Consent_RelationShip";
                ds.Tables[3].TableName = "Consent_Type";
            }
            return ds;
        }

        public DataTable GetRxsByPatient(string sPatNo, string sBillStatus)
        {
            string sSql = "Select cl.*, dg.CLASS FROM claims cl, drug dg Where cl.ndc=dg.drgndc and cl.PatientNo = " + sPatNo;

            if (sBillStatus.Trim().Length > 0)
            {
                sSql += " and IsNull(Status,'')='" + sBillStatus + "' ";
            }

            sSql += " ORDER BY RXNO";

            this.sTableName = "CLAIMS";

            return this.GetRecs(sSql).Tables[0];
        }

        public long GetLastInsSigTransNo()
        {
            object oInsSigTransNo;
            oInsSigTransNo = db.DataReaderScalar("Select Max(TRANSNO) as oInsSigTransNo from INSSIGTRANS");
            db.Close();

            if (oInsSigTransNo.ToString().Length > 0)
                return Convert.ToInt32(oInsSigTransNo.ToString());
            else
                return 0;
        }

        public int GetMaxAccessNo()
        {
            object oSes_No;
            oSes_No = db.DataReaderScalar("select isNull(max(SES_NO),0)+1 from ACCESS");
            db.Close();

            if (oSes_No.ToString().Length > 0)
                return Convert.ToInt32(oSes_No.ToString());
            else
                return 1;
        }

        public int GetMaxNewRxOrd()
        {
            object oNewRxOrd;
            oNewRxOrd = db.DataReaderScalar("select isNull(max(NEWRXORDID),0)+1 from NEWRXORD");
            db.Close();

            if (oNewRxOrd.ToString().Length > 0)
                return Convert.ToInt32(oNewRxOrd.ToString());
            else
                return 1;
        }

        public string GetMaxContID()
        {
            object oContID;
            oContID = db.DataReaderScalar("select isNull(max(CONTACTID),0)+1 from WEBCONT");
            db.Close();

            if (oContID.ToString().Length > 0)
                return oContID.ToString();
            else
                return "1";
        }

        public string GetMaxNewPatID()
        {
            object oNewPatID;
            oNewPatID = db.DataReaderScalar("select isNull(max(NEWPATID),0)+1 from NEWPATIENT");
            db.Close();

            if (oNewPatID.ToString().Length > 0)
                return oNewPatID.ToString();
            else
                return "1";
        }

        public bool DoesRxExist(string sRxNo, string sRefNo)
        {
            return (this.GetRxs(sRxNo, sRefNo, "").Rows.Count > 0);
        }

        public void SavePrivacyAck(long lPatNo, System.DateTime dtSigned, string sPatAccept, string sPrivacyText, string sSignature, string sSigType)
        {
            string sSql = "Insert into PRIVACYACK(PATIENTNO,DATESIGNED,PATACCEPT,PRIVACYTEXT,PRIVACYSIG,SIGTYPE) ";
            sSql += " VALUES(" + lPatNo.ToString() + ", '" + dtSigned.ToShortDateString() + "', '" + sPatAccept + "', '" + (sPrivacyText.Replace("'", "''")) + "', '" + (sSignature.Replace("'", "''")) + "', '" + sSigType + "')";
            logger.Trace("SavePrivacyAck(long lPatNo, System.DateTime dtSigned, string sPatAccept, string sPrivacyText, string sSignature, string sSigType) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
            base.ExecuteSql(sSql);
        }

        public void SavePrivacyAck(long lPatNo, System.DateTime dtSigned, string sPatAccept, string sPrivacyText, string sSignature, string sSigType, byte[] bBinarySign)
        {
            try
            {
                /*string sSql = "Insert into PRIVACYACK(PATIENTNO,DATESIGNED,PATACCEPT,PRIVACYTEXT,PRIVACYSIG,SIGTYPE,BinarySign) ";
                sSql += " VALUES(" + lPatNo.ToString() + ", '" + dtSigned.ToShortDateString() + "', '" + sPatAccept + "', '" + (sPrivacyText.Replace("'", "''")) + "', '" + (sSignature.Replace("'", "''")) + "', '" + sSigType + "',"+bBinarySign+")";
                base.ExecuteSql(sSql);*/

                this.sTableName = "PRIVACYACK";
                DataSet dt = this.GetRecs("Select * From PRIVACYACK Where PRVACKTRANSNO = 0");
                DataRow nRow = dt.Tables["PRIVACYACK"].NewRow();
                dt.Tables["PRIVACYACK"].Rows.Add(nRow);
                dt.Tables["PRIVACYACK"].Rows[0]["PATIENTNO"] = lPatNo;
                dt.Tables["PRIVACYACK"].Rows[0]["DATESIGNED"] = dtSigned;
                dt.Tables["PRIVACYACK"].Rows[0]["PATACCEPT"] = sPatAccept;
                dt.Tables["PRIVACYACK"].Rows[0]["PRIVACYTEXT"] = sPrivacyText;
                dt.Tables["PRIVACYACK"].Rows[0]["PRIVACYSIG"] = sSignature;

                dt.Tables["PRIVACYACK"].Rows[0]["SIGTYPE"] = sSigType;
                dt.Tables["PRIVACYACK"].Rows[0]["BinarySign"] = bBinarySign;
                this.Save(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long GetLastPrvAckTransNo()
        {
            object oPrvAckTransNo;
            oPrvAckTransNo = db.DataReaderScalar("Select Max(PRVACKTRANSNO) as oPrvAckTransNo from PRIVACYACK");
            db.Close();
            if (oPrvAckTransNo.ToString().Length > 0)
                return Convert.ToInt32(oPrvAckTransNo.ToString());
            else
                return 0;
        }

        public void SavePatientAck(long lPatNo, string sAck, DateTime dtAckDate)
        {
            string sSql = "Update PATIENT Set SIGACK = '" + sAck + "', SIGACKDATE = '" + dtAckDate.ToShortDateString() + "',PCSynch=1 WHERE PATIENTNO = " + lPatNo.ToString(); //PRIMEPOS-3246 Added PCSynch

            logger.Trace("SavePatientAck(long lPatNo, string sAck, DateTime dtAckDate) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
            base.ExecuteSql(sSql);
        }

        public void SaveTransDet(long lTransNo, long lRxNo, int iRefNo)
        {
            string sSql = "Insert INTO TRANSDET (TRANSNO,RXNO,REFILLNO) values( " + lTransNo + ", " + lRxNo + ", " + iRefNo + ")";  //PRIMEPOS-2960 07-May-2021 JY added column list

            logger.Trace("SaveTransDet(long lTransNo, long lRxNo, int iRefNo) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
            base.ExecuteSql(sSql);
        }

        public long SaveInsSigTrans(System.DateTime dtTransDate, long lPatNo, string sInsType, string sTransData, string sSignature, string sCounselingReq, string sSigType)
        {
            long lNextSigTransNo = -1;
            this.sTableName = "INSSIGTRANS";
            DataSet dt = this.GetRecs("Select * From INSSIGTRANS Where TRANSNO = 0");
            DataRow nRow = dt.Tables["INSSIGTRANS"].NewRow();
            dt.Tables["INSSIGTRANS"].Rows.Add(nRow);
            dt.Tables["INSSIGTRANS"].Rows[0]["PATIENTNO"] = lPatNo;
            dt.Tables["INSSIGTRANS"].Rows[0]["INSTYPE"] = sInsType;
            dt.Tables["INSSIGTRANS"].Rows[0]["TRANSDATA"] = sTransData;
            dt.Tables["INSSIGTRANS"].Rows[0]["TRANSDATE"] = dtTransDate;
            dt.Tables["INSSIGTRANS"].Rows[0]["TRANSSIGDATA"] = sSignature;
            dt.Tables["INSSIGTRANS"].Rows[0]["COUNSELINGREQ"] = sCounselingReq;
            dt.Tables["INSSIGTRANS"].Rows[0]["SIGTYPE"] = sSigType;
            this.Save(dt);
            lNextSigTransNo = this.GetLastInsSigTransNo();
            return lNextSigTransNo;
        }

        public long SaveInsSigTrans(System.DateTime dtTransDate, long lPatNo, string sInsType, string sTransData, string sSignature, string sCounselingReq, string sSigType, byte[] bBinarySign)
        {
            long lNextSigTransNo = -1;
            try
            {
                //long lNextSigTransNo=this.GetLastInsSigTransNo();
                //dt=oInsSigDB.GetRec(lNextSigTransNo.ToString());

                this.sTableName = "INSSIGTRANS";
                DataSet dt = this.GetRecs("Select * From INSSIGTRANS Where TRANSNO = 0");
                DataRow nRow = dt.Tables["INSSIGTRANS"].NewRow();
                dt.Tables["INSSIGTRANS"].Rows.Add(nRow);
                dt.Tables["INSSIGTRANS"].Rows[0]["PATIENTNO"] = lPatNo;
                dt.Tables["INSSIGTRANS"].Rows[0]["INSTYPE"] = sInsType;
                dt.Tables["INSSIGTRANS"].Rows[0]["TRANSDATA"] = sTransData;
                dt.Tables["INSSIGTRANS"].Rows[0]["TRANSDATE"] = dtTransDate;
                dt.Tables["INSSIGTRANS"].Rows[0]["TRANSSIGDATA"] = sSignature;
                dt.Tables["INSSIGTRANS"].Rows[0]["COUNSELINGREQ"] = sCounselingReq;
                dt.Tables["INSSIGTRANS"].Rows[0]["SIGTYPE"] = sSigType;
                dt.Tables["INSSIGTRANS"].Rows[0]["BINARYSIGN"] = bBinarySign;
                this.Save(dt);
                lNextSigTransNo = this.GetLastInsSigTransNo();
                return lNextSigTransNo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetLastRefill(string sRxNo)
        {
            string sRetVal = "";
            string sSql = "Select MAX(NREFILL) as NREFILL From Claims Where RXNO = " + sRxNo;

            logger.Trace("GetLastRefill(string sRxNo) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            object oNRefill = this.db.DataReaderScalar(sSql);

            if (oNRefill != null)
            {
                sRetVal = oNRefill.ToString();
            }

            return sRetVal;
        }

        public void MarkDelivery(string sRxNo, string sRefNo, string sDelivery, string sPickedUp, DateTime PickUpDate, string sPickupPOS, bool isBatchDelivery = false) // PRIMERX-7688 NileshJ - Added isBatchDelivery
        {
            string sSql = " Update Claims SET ";

            string sExtra = "";

            if (sDelivery != null)
            {
                sSql += " DELIVERY = '" + sDelivery + "' ";
                sExtra = ", ";
            }
            if (!isBatchDelivery)  // PRIMERX-7688 NileshJ - Add condition for BatchDelivery  - 25-Sept-2019
            {
                //Added By Rohit Nair on Dec 19 2016 for PrimePOS-2366
                if (sPickedUp != null && (sPickedUp.ToUpper() == "Y" || sPickedUp.ToUpper() == "N"))
                {
                    sSql += sExtra + " PICKEDUP = '" + sPickedUp + "' ";
                    sExtra = ", ";
                }
            }

            if (sPickupPOS != null && sPickupPOS != "")
            {
                sSql += sExtra + " PICKUPPOS = '" + sPickupPOS + "' ";
                sExtra = ", ";
            }

            try
            {
                if (!(PickUpDate.Equals(DateTime.MinValue) || (PickUpDate == System.Data.SqlTypes.SqlDateTime.MinValue)))    //PRIMEPOS-2914 22-Oct-2020 JY Added additional condition to check sql min date
                {
                    sSql += sExtra + " PICKUPDATE = '" + PickUpDate.ToString() + "'";
                    sExtra = ", ";
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>MarkDelivery(): An Exception Occured"); //PRIMEPOS-3211
            }

            sSql += " WHERE RXNO = " + sRxNo + " AND NREFILL = " + sRefNo;

            int iID = GetRXBInID(sRxNo, sRefNo);
            if (iID > 0)
            {
                MarkRXBinAsOut(iID, DateTime.Now, "POS", "O");
            }
            logger.Trace("MarkDelivery(string sRxNo, string sRefNo, string sDelivery, string sPickedUp, DateTime PickUpDate, string sPickupPOS, bool isBatchDelivery = false) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
            base.Update(sSql);
        }

        private int GetRXBInID(string lRXNo, string iRefillNo)
        {
            string sSQL = "select id from rxbin where binstatus='I' and RXNo= " + lRXNo + " and refillno=" + iRefillNo;
            this.sTableName = "CLAIMS";
            DataSet oDS = this.GetRecs(sSQL);
            if (oDS.Tables[0].Rows.Count > 0)
            {
                return int.Parse(oDS.Tables[0].Rows[0]["ID"].ToString());
            }
            else
            {
                return 0;
            }
        }

        public void MarkRXBinAsOut(int iID, DateTime dtOut, string sUserID, string sStatus)
        {
            string sSQL = "UPDATE RXBin SET BinStatus='" + sStatus + "' , DateOut = '" + dtOut.ToString() + "' , UserOut=  '" + sUserID.Replace("'", "''") + "'  WHERE ID= " + iID;

            logger.Trace("MarkRXBinAsOut(int iID, DateTime dtOut, string sUserID, string sStatus) - SQL - " + sSQL);    //PRIMEPOS-3014 13-Oct-2021 JY Added
            base.Update(sSQL);
        }

        public DataTable GetRxsByDate(DateTime FromDate, DateTime ToDate)
        {
            this.sTableName = "CLAIMS";
            string sSql = "Select * From CLAIMS where DATEF between cast('" + FromDate.ToString("d") + "' as datetime ) and cast('" + ToDate.ToString("d") + "' as datetime) ORDER BY DATEF";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetFaq(string sFaqNo)
        {
            /*
             * this function will return the records from the FAQS table
             * if the faqno is -1 , it will return all the rows
             * else it will return only the row which matches the faqno
            */
            string sSql = "select * from FAQS ";
            this.sTableName = "FAQS";
            if (sFaqNo != "-1")
                sSql += "where FAQNO=" + sFaqNo;
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetPhInfo()
        {
            /*
             * this function will return the name and address of the pharmacy
             */
            string sSql = "select * from DF0001";
            this.sTableName = "DF0001";

            logger.Trace("GetPhInfo() - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetWebLoginByUserName(string sUserName, string sPassword)
        {
            /*
             * this function returns the rows from the weblogin table if the matching username and password exist
             */
            DataTable dt = null;
            string sSql = "select * from WEBLOGIN where username ='" + sUserName + "' and password='" + sPassword + "'";
            this.sTableName = "WEBLOGIN";
            dt = this.GetRecs(sSql).Tables[0];
            return dt;
        }

        public DataTable GetWebLoginByUserName(string sUserName)
        {
            /*
             * this function returns the rows from the weblogin table if the matching username exist
             */
            DataTable dt = null;
            string sSql = "select * from WEBLOGIN where username ='" + sUserName + "'";
            this.sTableName = "WEBLOGIN";
            dt = this.GetRecs(sSql).Tables[0];
            return dt;
        }

        public DataTable GetWebSet(string sType)
        {
            string sSql = "select * from WEBSET where SETTYPE='" + sType + "'";
            this.sTableName = "WEBSET";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetDocPat(string sDocNo, string sFacNo, string sPatFname, string sPatLname)
        {
            string sSql = "";
            sSql = "select * from patient where lname like '" + sPatLname + "%' and fname like '" + sPatFname +
                    "%' and patientno in (select patientno from claims ";
            if (sDocNo != "-1")
                sSql += "where presno = '" + sDocNo + "') order by lname";
            else
                sSql += "where facilitycd = '" + sFacNo + "') order by lname";
            this.sTableName = "PATIENT";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetPatHistory(string sDocNo, System.DateTime odtStartDate, System.DateTime odtEndDate, string rxno1, string rxno2, bool bIncludeDiscontinued)
        {
            string sSql;
            if (rxno2.Length > 0)
            {
                sSql = "select PATIENTNO,PATTYPE,BILLTYPE,RXNO,STATUS,cl.PRESNO,cl.NDC,cl.BRAND,Convert(varchar(10),DATEO,101) DATEO,Convert(varchar(10),DATEF,101) DATEF," +
                    "QUANT,QTY_ORD,DAYS,ORDSTATUS,TREFILLS,NREFILL,AMOUNT,PFEE,DISCOUNT,COPAY,TOTAMT," +
                    "(AMOUNT+PFEE-DISCOUNT) TOTRXAMT,SIG,PHARMACIST,ORDSTATUS,(preslnm + ',' + presfnm) prsinfo," +
                    "(rtrim(drg.drgname)+','+rtrim(drg.strong)+','+rtrim(drg.form)) drginfo,drg.obsdate from claims cl,prescrib pres,drug drg " +
                    "where datef between '" + odtStartDate + "' and '" + odtEndDate + "' and " +
                    "cl.presno = pres.presno and cl.ndc = drg.drgndc and cl.rxno between " + rxno1 + "and " + rxno2;
            }
            else
            {
                sSql = "select PATIENTNO,PATTYPE,BILLTYPE,RXNO,STATUS,cl.PRESNO,cl.NDC,cl.BRAND,Convert(varchar(10),DATEO,101) DATEO,Convert(varchar(10),DATEF,101) DATEF," +
                    "QUANT,QTY_ORD,DAYS,ORDSTATUS,TREFILLS,NREFILL,AMOUNT,PFEE,DISCOUNT,COPAY,TOTAMT," +
                    "(AMOUNT+PFEE-DISCOUNT) TOTRXAMT,SIG,PHARMACIST,ORDSTATUS,(preslnm + ',' + presfnm) prsinfo," +
                    "(rtrim(drg.drgname)+','+rtrim(drg.strong)+','+rtrim(drg.form)) drginfo,drg.obsdate from claims cl,prescrib pres,drug drg " +
                    "where datef between '" + odtStartDate + "' and '" + odtEndDate + "' and " +
                    "cl.presno = pres.presno and cl.ndc = drg.drgndc and cl.rxno = " + rxno1;
            }
            if (sDocNo != "-1")
            {
                if (rxno2.Length > 0)
                {
                    sSql = "select PATIENTNO,PATTYPE,BILLTYPE,RXNO,STATUS,cl.PRESNO,cl.NDC,cl.BRAND,Convert(varchar(10),DATEO,101) DATEO,Convert(varchar(10),DATEF,101) DATEF," +
                        "QUANT,QTY_ORD,DAYS,ORDSTATUS,TREFILLS,NREFILL,AMOUNT,PFEE,DISCOUNT,COPAY,TOTAMT," +
                        "(AMOUNT+PFEE-DISCOUNT) TOTRXAMT,SIG,PHARMACIST,ORDSTATUS,(preslnm + ',' + presfnm) prsinfo," +
                        "(rtrim(drg.drgname)+','+rtrim(drg.strong)+','+rtrim(drg.form)) drginfo,drg.obsdate from claims cl,prescrib pres,drug drg " +
                        "where cl.presno=" + sDocNo + " and datef between '" + odtStartDate +
                        "' and '" + odtEndDate + "' and cl.presno = pres.presno and cl.ndc = drg.drgndc and cl.rxno between " + rxno1 + "and " + rxno2;
                }
                else
                {
                    sSql = "select PATIENTNO,PATTYPE,BILLTYPE,RXNO,STATUS,cl.PRESNO,cl.NDC,cl.BRAND,Convert(varchar(10),DATEO,101) DATEO,Convert(varchar(10),DATEF,101) DATEF," +
                        "QUANT,QTY_ORD,DAYS,ORDSTATUS,TREFILLS,NREFILL,AMOUNT,PFEE,DISCOUNT,COPAY,TOTAMT," +
                        "(AMOUNT+PFEE-DISCOUNT) TOTRXAMT,SIG,PHARMACIST,ORDSTATUS,(preslnm + ',' + presfnm) prsinfo," +
                        "(rtrim(drg.drgname)+','+rtrim(drg.strong)+','+rtrim(drg.form)) drginfo,drg.obsdate from claims cl,prescrib pres,drug drg " +
                        "where cl.presno=" + sDocNo + " and datef between '" + odtStartDate +
                        "' and '" + odtEndDate + "' and cl.presno = pres.presno and cl.ndc = drg.drgndc and cl.rxno = " + rxno1;
                }
            }
            if (!bIncludeDiscontinued)
            {
                sSql += "and ordstatus <> 'D' ";
            }
            sSql += "order by DATEF DESC";
            this.sTableName = "CLAIMS";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetPatHistory(string sDocNo, System.DateTime odtStartDate, System.DateTime odtEndDate, bool bIncludeDiscontinued)
        {
            string sSql = "select PATIENTNO,PATTYPE,BILLTYPE,RXNO,STATUS,cl.PRESNO,cl.NDC,cl.BRAND,Convert(varchar(10),DATEO,101) DATEO,Convert(varchar(10),DATEF,101) DATEF," +
                "QUANT,QTY_ORD,DAYS,ORDSTATUS,TREFILLS,NREFILL,AMOUNT,PFEE,DISCOUNT,COPAY,TOTAMT," +
                "(AMOUNT+PFEE-DISCOUNT) TOTRXAMT,SIG,PHARMACIST,ORDSTATUS,(preslnm + ',' + presfnm) prsinfo," +
                "(rtrim(drg.drgname)+','+rtrim(drg.strong)+','+rtrim(drg.form)) drginfo,drg.obsdate from claims cl,prescrib pres,drug drg " +
                "where datef between '" + odtStartDate + "' and '" + odtEndDate + "' and " +
                "cl.presno = pres.presno  and cl.ndc = drg.drgndc ";
            if (sDocNo != "-1")
            {
                sSql = "select PATIENTNO,PATTYPE,BILLTYPE,RXNO,STATUS,cl.PRESNO,cl.NDC,cl.BRAND,Convert(varchar(10),DATEO,101) DATEO,Convert(varchar(10),DATEF,101) DATEF," +
                    "QUANT,QTY_ORD,DAYS,ORDSTATUS,TREFILLS,NREFILL,AMOUNT,PFEE,DISCOUNT,COPAY,TOTAMT," +
                    "(AMOUNT+PFEE-DISCOUNT) TOTRXAMT,SIG,PHARMACIST,ORDSTATUS,(preslnm + ',' + presfnm) prsinfo," +
                    "(rtrim(drg.drgname)+','+rtrim(drg.strong)+','+rtrim(drg.form)) drginfo,drg.obsdate from claims cl,prescrib pres,drug drg " +
                    "where cl.presno=" + sDocNo + " and datef between '" + odtStartDate +
                    "' and '" + odtEndDate + "' and cl.presno = pres.presno and cl.ndc = drg.drgndc ";
            }
            if (!bIncludeDiscontinued)
            {
                sSql += "and ordstatus <> 'D' ";
            }
            sSql += "order by DATEF DESC";
            this.sTableName = "CLAIMS";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetPatHistory(string sPatNo, string sDocNo, System.DateTime odtStartDate, System.DateTime odtEndDate, bool bIncludeDiscontinued)
        {
            string sSql = "select PATIENTNO,PATTYPE,BILLTYPE,RXNO,STATUS,cl.PRESNO,cl.NDC,cl.BRAND,Convert(varchar(10),DATEO,101) DATEO,Convert(varchar(10),DATEF,101) DATEF," +
                        "QUANT,QTY_ORD,DAYS,ORDSTATUS,TREFILLS,NREFILL,AMOUNT,PFEE,DISCOUNT,COPAY,TOTAMT," +
                        "(AMOUNT+PFEE-DISCOUNT) TOTRXAMT,SIG,PHARMACIST,ORDSTATUS,(preslnm + ',' + presfnm) prsinfo," +
                        "(rtrim(drg.drgname)+','+rtrim(drg.strong)+','+rtrim(drg.form)) drginfo,drg.obsdate from claims cl,prescrib pres,drug drg " +
                        "where patientno=" + sPatNo + " and datef between '" + odtStartDate + "' and '" + odtEndDate + "' and " +
                        "cl.presno = pres.presno and cl.ndc = drg.drgndc ";
            if (sDocNo != "-1")
            {
                sSql = "select PATIENTNO,PATTYPE,BILLTYPE,RXNO,STATUS,cl.PRESNO,cl.NDC,cl.BRAND,Convert(varchar(10),DATEO,101) DATEO,Convert(varchar(10),DATEF,101) DATEF," +
                    "QUANT,QTY_ORD,DAYS,ORDSTATUS,TREFILLS,NREFILL,AMOUNT,PFEE,DISCOUNT,COPAY,TOTAMT," +
                    "(AMOUNT+PFEE-DISCOUNT) TOTRXAMT,SIG,PHARMACIST,ORDSTATUS,(preslnm + ',' + presfnm) prsinfo," +
                    "(rtrim(drg.drgname)+','+rtrim(drg.strong)+','+rtrim(drg.form)) drginfo,drg.obsdate from claims cl,prescrib pres,drug drg " +
                    "where patientno=" + sPatNo + " and cl.presno=" + sDocNo + " and datef between '" + odtStartDate +
                    "' and '" + odtEndDate + "' and cl.presno = pres.presno and cl.ndc = drg.drgndc ";
            }
            if (!bIncludeDiscontinued)
            {
                sSql += "and ordstatus <> 'D' ";
            }
            sSql += "order by DATEF DESC";
            this.sTableName = "CLAIMS";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable getRxLabel(string sRxNo, string sRefNo)
        {
            string sSql = "select pat.LNAME LNAME,pat.FNAME FNAME,pat.DOB DOB,pat.SEX SEX,pat.ADDRSTR ADDRSTR," +
                "pat.ADDRCT ADDRCT,pat.ADDRST ADDRST,pat.ADDRZP ADDRZP,cl.RXNO RXNO,cl.NREFILL REFILL,drg.DRGNAME DRGNAME," +
                "drg.STRONG STRONG,drg.FORM FORM,drg.BRAND BRAND,cl.DATEF DATEF,cl.QUANT QUANT,cl.DAYS DAYS,(SIG+','+SIGLINES) SIGLINE1," +
                "pres.PRESLNM PRESLNM,pres.PRESFNM PRESFNM,pres.PRESLIC PRESLIC,pres.PRESDEA PRESDEA,pres.PHONE PRESPHONE " +
                "from claims cl,patient pat,prescrib pres,drug drg where cl.patientno=pat.patientno and cl.presno=pres.presno " +
                "and cl.ndc=drg.drgndc and cl.rxno=" + sRxNo;
            if (sRefNo.Length > 0 && sRefNo != "-1")
                sSql += " AND NREFILL = " + sRefNo;
            this.sTableName = "CLAIMS";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetRxQueue(string sRxNo)
        {
            string sSql = "select * from RXREFQUE where RXNO = " + sRxNo;
            this.sTableName = "RXREFQUE";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetInsInfo(string sIns)
        {
            string sSql = "select MDREFILL from INSCAR where IC_CODE = '" + sIns + "'";
            this.sTableName = "INSCAR";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetCategory(string sCategory)
        {
            string sSql = string.Empty;
            if (sCategory == "-1")
                sSql = "select * from DRUGCATEGORY";
            else
                sSql = "select * from DRUGCATEGORY where CATEGORY='" + sCategory + "'";
            this.sTableName = "DRUGCATEGORY";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetCounselling(string sNdc, string sType)
        {
            DataTable dtDrug = null;
            string sSql = "";
            dtDrug = this.GetDrug(sNdc);
            this.sTableName = "PCSHORT";
            if ((dtDrug == null) || (dtDrug.Rows.Count == 0) || (dtDrug.Rows[0]["TXRCODE"].ToString().TrimEnd() == ""))
                return null;
            if (sType == "L")
                this.sTableName = "PCLONG";
            sSql = "select * from " + this.sTableName + " where TXRCODE = '" + dtDrug.Rows[0]["TXRCODE"].ToString() + "'";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetFacility(string sFacCode)
        {
            string sSql = "select * from FACILITY";
            if (sFacCode != "-1")
                sSql += " where FACILITYCD='" + sFacCode + "'";
            this.sTableName = "FACILITY";

            logger.Trace("GetFacility(string sFacCode) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetFacility()
        {
            string sSql = "select * from FACILITY where facilitycd not in(select pharmid from weblogin where logintype='FA')";
            this.sTableName = "FACILITY";
            return this.GetRecs(sSql).Tables[0];
        }

        public void SaveFacility(DataTable oTable)
        {
            base.SaveTable(oTable);
        }

        public DataTable GetContsByDate(DateTime fromDate, DateTime toDate, string sPharmId, string sProcess)
        {
            string sSql = string.Empty;
            if (sPharmId == "-1")
            {
                sSql = "select * from WEBCONT where CONTDATE between '" + fromDate + "' and '" + toDate + "' and " +
                    "PHARMID='' and PROCESS='" + sProcess + "'";
            }
            else
            {
                sSql = "select * from WEBCONT where CONTDATE between '" + fromDate + "' and '" + toDate + "' and " +
                    "PHARMID='" + sPharmId + "' and PROCESS='" + sProcess + "'";
            }
            this.sTableName = "WEBCONT";
            return this.GetRecs(sSql).Tables[0];
        }

        public string InsertAccess(DataTable dtInsert)
        {
            string maxSesNo = string.Empty;
            string sSql = string.Empty;
            string password = dtInsert.Rows[0]["PASSWORD"].ToString();
            if (dtInsert.Rows[0]["SES_NO"].ToString() == String.Empty)
            {
                maxSesNo = this.GetMaxAccessNo().ToString();
                sSql = "insert into access values(" + maxSesNo + ",'" + dtInsert.Rows[0]["LOGIN_DATE"].ToString() +
                    "','" + dtInsert.Rows[0]["LOGIN_TIME"].ToString() + "'," + System.Data.SqlTypes.SqlDateTime.Null +
                    ",'','" + dtInsert.Rows[0]["USERNAME"].ToString() + "')";
            }
            else
            {
                sSql = "update ACCESS set LOGOUT_DATE = '" + dtInsert.Rows[0]["LOGOUT_DT"].ToString() + "'," +
                    "LOGOUT_TIME='" + dtInsert.Rows[0]["LOGOUT_TM"].ToString() +
                    "' where SES_NO = " + dtInsert.Rows[0]["SES_NO"].ToString();
            }
            try
            {
                logger.Trace("InsertAccess(DataTable dtInsert) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                base.ExecuteSql(sSql);

                //if(password.StartsWith("**"))
                //	InsertTempAccess(dtInsert);
            }
            catch (Exception ex)
            {
                //MMSException mex = new MMSException();
                //mex.publish(ex);
                throw ex;
            }
            return maxSesNo;
        }

        private void InsertTempAccess(DataTable dtInsert)
        {
            string sSql = string.Empty;
            sSql = "insert into Tempaccess(username,password,logintime,logindate) values('" +
                dtInsert.Rows[0]["USERNAME"].ToString() + "','" + dtInsert.Rows[0]["PASSWORD"].ToString() +
                "','" + dtInsert.Rows[0]["LOGIN_TIME"].ToString() + "'," + dtInsert.Rows[0]["LOGIN_DATE"].ToString() + ")";

            try
            {
                logger.Trace("InsertTempAccess(DataTable dtInsert) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                base.ExecuteSql(sSql);
            }
            catch (Exception ex)
            {
                //MMSException mex = new MMSException();
                //mex.publish(ex);
                throw ex;
            }
        }

        public bool checkRandomPassword(string username, string password)
        {
            string slno = "0";
            string sSql = "select count(*) as totcnt from TempAccess where username='" + username + "' and password = '" + password + "'";
            this.sTableName = "TempAccess";
            slno = this.GetRecs(sSql).Tables[0].Rows[0]["totcnt"].ToString();
            if (slno == "0")
                return true; // there is no entry in the table
            else
                return false;
        }

        public void InsertRxQue(DataTable dtRefQue)
        {
            string sSql = string.Empty;
            if ((dtRefQue != null) && (dtRefQue.Rows.Count > 0))
            {
                try
                {
                    for (int i = 0; i < dtRefQue.Rows.Count; i++)
                    {
                        DataRow orow = dtRefQue.Rows[i];
                        sSql = "insert into RXREFQUE (RXNO,DATE_QUED,TIME_QUED,SENTBYPROG) values(" +
                            orow["RXNO"].ToString() + ",'" + orow["DATE_QUED"].ToString() + "','" +
                            orow["TIME_QUED"].ToString() + "','" + orow["SENTBYPROG"].ToString() + "')";
                        logger.Trace("InsertRxQue(DataTable dtRefQue) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                        base.ExecuteSql(sSql);
                    }
                }
                catch (Exception ex)
                {
                    //MMSException mex = new MMSException();
                    //mex.publish(ex);
                    throw ex;
                }
            }
        }

        //Update by Manoj on 12/10/2014 with sRefNo.
        public void MarkCopayPaid(string sRxNo, string sRefNo, char val)
        {
            string sSql = string.Empty;
            try
            {
                if (sRxNo != string.Empty)
                {
                    sSql = "Update CLAIMS set COPAYPAID ='" + val + "' where RXNO='" + sRxNo + "' AND NREFILL='" + sRefNo + "'";
                    logger.Trace("MarkCopayPaid(string sRxNo, string sRefNo, char val) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                    base.ExecuteSql(sSql);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DiscontinueRx(DataTable dtDCRxs)
        {
            DataRow oCurrRow;
            string sSql = string.Empty;
            string res = "Success";
            try
            {
                for (int i = 0; i < dtDCRxs.Rows.Count; i++)
                {
                    oCurrRow = dtDCRxs.Rows[i];
                    sSql = "INSERT INTO PRESMSG(RECVDTTM, MSGTYPE,RECVSTR,COMMPROTOCOL,DRGNAME,PATLNAME,PATFNAME," +
                        "PRSLNAME,PRSFNAME,INITBY,RXNO) VALUES('" + DateTime.Now.ToString() + "','DCRX','" +
                        "DCRX-" + oCurrRow["RXNO"].ToString() + "','MMS','" + oCurrRow["DRGNAME"].ToString() +
                        "','" + oCurrRow["LNAME"].ToString() + "','" + oCurrRow["FNAME"].ToString() + "','" +
                        oCurrRow["PRESLNM"].ToString() + "','" + oCurrRow["PRESFNM"].ToString() + "','" +
                        oCurrRow["INITBY"].ToString() + "'," + oCurrRow["RXNO"].ToString() + ")";

                    logger.Trace("DiscontinueRx(DataTable dtDCRxs) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                    base.ExecuteSql(sSql);
                }
            }
            catch (Exception ex)
            {
                //MMSException mex = new MMSException();
                //mex.publish(ex);
                logger.Error(ex, "PharmaSqlDB==>DiscontinueRx(): An Exception Occured"); //PRIMEPOS-3211
                return ex.Message;
            }
            return res;
        }

        public string InsertNewRxOrd(DataTable dtNewRxs)
        {
            DataRow oCurrRow = null;

            string sEdiFact = "";

            string sSql = "";

            Script41StrGen oScr41StrGen = null;

            DataTable oTable = dtNewRxs.Clone();

            DateTime dtCreationDate = DateTime.Now;

            for (int i = 0; i < dtNewRxs.Rows.Count; i++)
            {
                oCurrRow = dtNewRxs.Rows[i];

                oScr41StrGen = new Script41StrGen();

                oScr41StrGen.ERxTrans = ERxTransType.NEWRX;
                oScr41StrGen.oCreated = dtCreationDate;

                //oScr41StrGen.dsScrDataStruct = new DataSet();

                oScr41StrGen.ERxRequestData = new DataSet();

                oScr41StrGen.ERxRequestData.Tables.Add(oTable);

                oTable.Clear();

                oTable.ImportRow(dtNewRxs.Rows[i]);

                sEdiFact = oScr41StrGen.CreateString();

                sSql = "INSERT INTO PRESMSG(RECVDTTM, MSGTYPE,RECVSTR,COMMPROTOCOL, DRGNAME,PATLNAME,PATFNAME,PRSLNAME,PRSFNAME,INITBY,NOTES)";

                sSql += " VALUES('" + dtCreationDate.ToString() + "', 'NEWRX', '" + sEdiFact + "', 'SCRIPT41', '" + oCurrRow["DRGNAME"].ToString() + "', '" + oCurrRow["LNAME"].ToString() + "', '" + oCurrRow["FNAME"].ToString() + "', '" + oCurrRow["PRESLNM"].ToString() + "', '" + oCurrRow["PRESFNM"].ToString() + "', '" + oCurrRow["INITBY"].ToString() + "', '" + oCurrRow["COMMENT"].ToString() + "')";

                logger.Trace("InsertNewRxOrd(DataTable dtNewRxs) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                base.ExecuteSql(sSql);

                oScr41StrGen.ERxRequestData.Tables.RemoveAt(0);
            }

            return "";
        }

        /*

        public string InsertNewRxOrd(DataTable dtNewRx)
        {
            string maxOrdId = String.Empty;
            maxOrdId = this.GetMaxNewRxOrd().ToString();
            foreach(DataRow orow in dtNewRx.Rows)
            {
                string sSql = "insert into NEWRXORD(NEWRXORDID,DATE_ORD,TIME_ORD,PATIENTNO,LNAME,FNAME,DOB,AGE,SEX," +
                    "ADDRSTR,ADDRCT,ADDRST,ADDRZP,EMAIL,PHONE,PAYTYPE,PRESNO,PRESLNM,PRESFNM,PRESPHONE," +
                    "PRESLIC,PRESDEA,PRESSTATE,DRGNDC,DRGNAME,STRONG,FORM,QUANT,BRAND,DAYS,REFILLS,REFILLSNUM," +
                    "SIGLINE1,FACILITYCD,INITBY,SENTBY,COMMENT,MEDICAID,MEDNO,SEQNUM,MSGTYPE) values(" + maxOrdId +
                    ",'" + orow["DATE_ORD"].ToString() + "','" + orow["TIME_ORD"].ToString() +
                    "'," + orow["PATIENTNO"].ToString() + ",'" + orow["LNAME"].ToString() +
                    "','" + orow["FNAME"].ToString() + "','" + orow["DOB"].ToString() +
                    "','" + orow["AGE"].ToString() + "','" + orow["SEX"].ToString() +
                    "','" + orow["ADDRSTR"].ToString() + "','" + orow["ADDRCT"].ToString() +
                    "','" + orow["ADDRST"].ToString() + "','" + orow["ADDRZP"].ToString() +
                    "','" + orow["EMAIL"].ToString() + "','" + orow["PHONE"].ToString() +
                    "','" + orow["PAYTYPE"].ToString() + "'," + orow["PRESNO"].ToString() +
                    ",'" + orow["PRESLNM"].ToString() + "','" + orow["PRESFNM"].ToString() +
                    "','" + orow["PRESPHONE"].ToString() + "','" + orow["PRESLIC"].ToString() +
                    "','" + orow["PRESDEA"].ToString() + "','" + orow["PRESSTATE"].ToString() +
                    "','" + orow["DRGNDC"].ToString() + "','" + orow["DRGNAME"].ToString() +
                    "','" + orow["STRONG"].ToString() + "','" + orow["FORM"].ToString() +
                    "'," + orow["QUANT"].ToString() + ",'" + orow["BRAND"].ToString() +
                    "','" + orow["DAYS"].ToString() + "','" + orow["REFILLS"].ToString() +
                    "'," + orow["REFILLSNUM"].ToString() + ",'" + orow["SIGLINE1"].ToString() +
                    "','" + orow["FACILITYCD"].ToString() + "','" + orow["INITBY"].ToString() +
                    "','" + orow["SENTBY"].ToString() + "','" + orow["COMMENT"].ToString() +
                    "','" + orow["MEDICAID"].ToString() + "','" + orow["MEDNO"].ToString() +
                    "','" + orow["SEQNUM"].ToString() + "','" + orow["MSGTYPE"].ToString() + "')";
                base.ExecuteSql(sSql);
                maxOrdId = Convert.ToString(Convert.ToInt32(maxOrdId)+1);
            }
            return maxOrdId;
        }

        */

        public string UpdateLogin(string username, string password)
        {
            /*
             * this function updates the login table with the new random generated password
             */
            string sSql = "update weblogin set password = '" + password + "',passwordvh='" + password + "'" +
                " where username='" + username + "'";
            string res = "success";
            try
            {
                logger.Trace("UpdateLogin(string username, string password) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                base.ExecuteSql(sSql);
            }
            catch (Exception ex)
            {
                //MMSException mex = new MMSException();
                //mex.publish(ex);
                logger.Error(ex, "PharmaSqlDB==>UpdateLogin(string username, string password): An Exception Occured"); //PRIMEPOS-3211
                res = "error";
            }
            return res;
        }

        public string UpdateLogin(DataTable dtUpdate)
        {
            string sSql = "update WEBLOGIN set PASSWORD='" + dtUpdate.Rows[0]["PASSWORD"].ToString() + "',PASSWORDVH= " +
                        "'" + dtUpdate.Rows[0]["PASSWORDVH"].ToString() + "' where LOGINID = " + dtUpdate.Rows[0]["LOGINID"].ToString();
            string res = "Success";
            try
            {
                logger.Trace("UpdateLogin(DataTable dtUpdate) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                base.ExecuteSql(sSql);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>UpdateLogin(DataTable dtUpdate): An Exception Occured"); //PRIMEPOS-3211
                res = "Error";
            }
            return res;
        }

        public void InsertExtPat(DataTable dtInsert)
        {
            string maxNewPatID = string.Empty;
            maxNewPatID = this.GetMaxNewPatID();
            string sSql = "insert into NEWPATIENT (NEWPATID,LNAME,FNAME,SEX,DOB,ADDRSTR,ADDRCT,ADDRST,ADDRZP,PHONE," +
                        "EMAIL,PAYTYPE,PATTYPE,SEQNO,MEDNO,GROUPNO,RELATION,SENTBY,USERNAME,PHARMID,PROCESSED," +
                        "REQDATE,COMMENTS) values (" + maxNewPatID + ",'" + dtInsert.Rows[0]["LNAME"].ToString() +
                        "','" + dtInsert.Rows[0]["FNAME"].ToString() + "','" + dtInsert.Rows[0]["SEX"].ToString() +
                        "','" + dtInsert.Rows[0]["DOB"].ToString() + "','" + dtInsert.Rows[0]["ADDRSTR"].ToString() +
                        "','" + dtInsert.Rows[0]["ADDRCT"].ToString() + "','" + dtInsert.Rows[0]["ADDRST"].ToString() +
                        "','" + dtInsert.Rows[0]["ADDRZP"].ToString() + "','" + dtInsert.Rows[0]["PHONE"].ToString() +
                        "','" + dtInsert.Rows[0]["EMAIL"].ToString() + "','" + dtInsert.Rows[0]["PAYTYPE"].ToString() +
                        "','" + dtInsert.Rows[0]["PATTYPE"].ToString() + "','" + dtInsert.Rows[0]["SEQNO"].ToString() +
                        "','" + dtInsert.Rows[0]["MEDNO"].ToString() + "','" + dtInsert.Rows[0]["GROUPNO"].ToString() +
                        "','" + dtInsert.Rows[0]["RELATION"].ToString() + "','" + dtInsert.Rows[0]["SENTBY"].ToString() +
                        "','" + dtInsert.Rows[0]["USERNAME"].ToString() + "','" + dtInsert.Rows[0]["PHARMID"].ToString() +
                        "','" + dtInsert.Rows[0]["PROCESSED"].ToString() + "','" + dtInsert.Rows[0]["REQDATE"].ToString() +
                        "','" + dtInsert.Rows[0]["COMMENTS"].ToString() + "')";

            logger.Trace("InsertExtPat(DataTable dtInsert) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
            base.ExecuteSql(sSql);
        }

        public void InsertContact(DataTable dtContact)
        {
            string maxContID = string.Empty;
            DataRow orow = null;
            string sSql = string.Empty;
            for (int i = 0; i < dtContact.Rows.Count; i++)
            {
                orow = dtContact.Rows[i];
                if (orow["CONTACTID"].ToString().Trim().Length == 0)
                {
                    maxContID = this.GetMaxContID();
                    sSql = "insert into WEBCONT(CONTACTID,WEBFNAME,WEBLNAME,ADDRSTR,ADDRCT,ADDRST,ADDRZP,PHONE," +
                        "EMAIL,COMMENTS,CONTDATE,PHARMID,PROCESS) values " +
                        "(" + maxContID + ",'" + orow["WEBFNAME"].ToString() +
                        "','" + orow["WEBLNAME"].ToString() + "','" + orow["ADDRSTR"].ToString() +
                        "','" + orow["ADDRCT"].ToString() + "','" + orow["ADDRST"].ToString() +
                        "','" + orow["ADDRZP"].ToString() + "','" + orow["PHONE"].ToString() +
                        "','" + orow["EMAIL"].ToString() + "','" + orow["COMMENTS"].ToString() +
                        "','" + orow["CONTDATE"].ToString() + "','" + orow["PHARMID"].ToString() +
                        "','" + orow["PROCESS"].ToString() + "')";
                }
                else
                {
                    sSql = "update WEBCONT set PROCESS='Y' where CONTACTID=" + orow["CONTACTID"].ToString();
                }

                logger.Trace("InsertContact(DataTable dtContact) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                base.ExecuteSql(sSql);
            }
        }

        public void InsertRxPickUpLog(DataTable dtRxinfo)
        {
            DataTable RxPickUp = new DataTable();
            string sSql = string.Empty;
            string sSql1 = string.Empty;
            foreach (DataRow oRow in dtRxinfo.Rows)
            {
                sSql1 = "SELECT RXNO, REFILLNO from RXPICKUPLOG where RXNO='" + oRow["RXNO"].ToString() + "' and REFILLNO='" + oRow["REFILLNO"].ToString() + "'";
                this.sTableName = "RXPICKLOG";
                RxPickUp = this.GetRecs(sSql1).Tables[0];

                if (RxPickUp.Rows.Count < 1)
                {
                    sSql = "INSERT into RXPICKUPLOG(RXNO,REFILLNO,PICKUPDATE,PICKEDUPBYLNAME,PICKEDUPBYFNAME,PICKBYIDQ," +
                           "PICKBYID,PICKBYIDAUTH,PICKBYRELATION, DropPickQ) values ('" + oRow["RXNO"].ToString() + "','" + oRow["REFILLNO"].ToString() +
                           "','" + oRow["PICKUPDATE"].ToString() + "','" + Convert.ToString(oRow["LASTNAME"]).Replace("'", "''") + "','" + Convert.ToString(oRow["FIRSTNAME"]).Replace("'", "''") +
                           "','" + oRow["IDTYPE"].ToString() + "','" + oRow["IDNUM"].ToString() + "','" + oRow["STATE"].ToString() +
                           "','" + oRow["RELATION"].ToString() + "','02')"; //PRIMEPOS-3017 21-Jan-2021 JY modified //PRIMEPOS-3293
                }
                else
                {
                    sSql = "UPDATE RXPICKUPLOG SET RXNO='" + oRow["RXNO"].ToString() + "',REFILLNO='" + oRow["REFILLNO"].ToString() +
                        "',PICKUPDATE='" + oRow["PICKUPDATE"].ToString() + "',PICKEDUPBYLNAME='" + Convert.ToString(oRow["LASTNAME"]).Replace("'", "''") +
                        "',PICKEDUPBYFNAME='" + Convert.ToString(oRow["FIRSTNAME"]).Replace("'", "''") + "',PICKBYIDQ='" + oRow["IDTYPE"].ToString() +
                        "',PICKBYID='" + oRow["IDNUM"].ToString() + "',PICKBYIDAUTH='" + oRow["STATE"].ToString() + "',PICKBYRELATION='" + oRow["RELATION"].ToString() +
                        "',DropPickQ='02'" + " where RXNO='" + oRow["RXNO"].ToString() + "' and REFILLNO='" + oRow["REFILLNO"].ToString() + "'";    //PRIMEPOS-3017 21-Jan-2021 JY modified //PRIMEPOS-3293
                }

                logger.Trace("InsertRxPickUpLog(DataTable dtRxinfo) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
                base.ExecuteSql(sSql);
            }

            #region PRIMEPOS-3065 10-Mar-2022 JY Added
            try
            {
                if (dtRxinfo != null && dtRxinfo.Rows.Count > 0)
                {
                    foreach (DataRow oRow in dtRxinfo.Rows)
                    {
                        if (oRow["RELATION"].ToString().Trim() == "01") //means if patient
                        {
                            //fetch DriversLicense
                            string DriversLicense = string.Empty;
                            DateTime DriversLicenseExpDT = DateTime.MinValue;
                            sSql = "SELECT DRIVERSLICENSE, DriversLicenseExpDT FROM PATIENT WHERE PATIENTNO = (SELECT TOP 1 PATIENTNO FROM CLAIMS WHERE RXNO = '" + oRow["RXNO"].ToString() + "')";
                            this.sTableName = "Patient";
                            DataTable dtPatient = this.GetRecs(sSql).Tables[0];
                            if (dtPatient != null && dtPatient.Rows.Count > 0)
                            {
                                if (dtPatient.Rows[0]["DRIVERSLICENSE"] != null)
                                    DriversLicense = dtPatient.Rows[0]["DRIVERSLICENSE"].ToString().Trim();
                                if (dtPatient.Rows[0]["DriversLicenseExpDT"] != null && dtPatient.Rows[0]["DriversLicenseExpDT"].ToString() != "")
                                    DriversLicenseExpDT = Convert.ToDateTime(dtPatient.Rows[0]["DriversLicenseExpDT"].ToString());

                                string strUpd = string.Empty;
                                if (oRow["IDNUM"].ToString().Trim() != "")
                                {
                                    if (DriversLicense.ToUpper() != oRow["IDNUM"].ToString().Trim().ToUpper())
                                    {
                                        strUpd = "UPDATE PATIENT SET DriversLicense = '" + oRow["IDNUM"].ToString().Trim() + "',PCSynch=1 WHERE PATIENTNO = (SELECT TOP 1 PATIENTNO FROM CLAIMS WHERE RXNO = '" + oRow["RXNO"].ToString() + "')";
                                        if (oRow["DriversLicenseExpDate"] != null && oRow["DriversLicenseExpDate"].ToString() != "" && Convert.ToDateTime(oRow["DriversLicenseExpDate"]).Date != DateTime.MinValue.Date && DriversLicenseExpDT.Date != Convert.ToDateTime(oRow["DriversLicenseExpDate"]).Date)
                                            strUpd = "UPDATE PATIENT SET DriversLicense = '" + oRow["IDNUM"].ToString().Trim() + "', DriversLicenseExpDT = '" + oRow["DriversLicenseExpDate"].ToString() + "',PCSynch=1 WHERE PATIENTNO = (SELECT TOP 1 PATIENTNO FROM CLAIMS WHERE RXNO = '" + oRow["RXNO"].ToString() + "')";
                                    }
                                    else if (oRow["DriversLicenseExpDate"] != null && oRow["DriversLicenseExpDate"].ToString() != "" && Convert.ToDateTime(oRow["DriversLicenseExpDate"]).Date != DateTime.MinValue.Date && DriversLicenseExpDT.Date != Convert.ToDateTime(oRow["DriversLicenseExpDate"]).Date)
                                        strUpd = "UPDATE PATIENT SET DriversLicenseExpDT = '" + oRow["DriversLicenseExpDate"].ToString() + "',PCSynch=1 WHERE PATIENTNO = (SELECT TOP 1 PATIENTNO FROM CLAIMS WHERE RXNO = '" + oRow["RXNO"].ToString() + "')";

                                    logger.Trace("InsertRxPickUpLog(DataTable dtRxinfo) - Update Patient SQL - " + strUpd);
                                    base.ExecuteSql(strUpd);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "InsertRxPickUpLog(DataTable dtRxinfo) - An Error Occured while executing sql " + sSql);
            }
            #endregion
        }

        public DataTable GetLoginDetails(string dtFrom, string dtTo)
        {
            string sSql = "select ac.login_date,ac.username,log.logintype from access ac,weblogin log " +
                "where ac.username = log.username and ac.login_date between '" + dtFrom + "' and '" + dtTo + "'";
            this.sTableName = "ACCESS";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetPharmacyMessage()
        {
            string sSql = "SELECT PharmMessage FROM DF0001";
            this.sTableName = "DF0001";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetPatInsurance(string sInsID)
        {
            string sSql = "SELECT InsMessageEnable, InsMessage FROM INSCAR WHERE IC_CODE='" + sInsID + "'";
            this.sTableName = "INSCAR";
            return this.GetRecs(sSql).Tables[0];
        }

        public void SaveMessages(string sPharMsg, string sInsMsg, string sPhmSigData, string sInsSig, string sPatientNo, long sTransNo)
        {
            string sSql = "UPDATE INSSIGTRANS SET PHARMMSGACCEPTANCE='" + sPharMsg + "', INSMSGACCEPTANCE='" + sInsMsg + "', PHARMSIGDATA='" + sPhmSigData + "', INSSIGDATA='" + sInsSig + "' WHERE PATIENTNO='" + sPatientNo + "' AND TRANSNO='" + sTransNo + "'";
            this.sTableName = "INSSIGTRANS";

            logger.Trace("SaveMessages(string sPharMsg, string sInsMsg, string sPhmSigData, string sInsSig, string sPatientNo, long sTransNo) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
            base.ExecuteSql(sSql);
        }

        //Added By Rohit Nair For PRIMEPOS-2372
        public DataSet GetPatientsDeliveryAddr(string commaSeparatedPatients)
        {

            string sSearchString = string.Concat(new string[]
            {
                "Select * from (  Select rtrim(lname) + ' ' + ltrim(fname) as Name, RTrim(addrstr) + ', ' +RTrim(addrct) +' '+ RTrim(addrzp) as Address From Patient  where patientno in (",
                commaSeparatedPatients,
                ")  Union  select rtrim(p.lname) + ' ' + ltrim(p.fname) as Name, e.street1 + ' ' + e.street2  + ', ' + e.city + ', ' + e.state + ' ' + e.zip as Address  from entityaddress e, patient p where e.entity='P' and e.entityid=p.patientno and p.patientno in (",
                commaSeparatedPatients,
                ")) as delvList  Where delvList.name is not null and delvList.address is not null order by Name"
            });
            this.sTableName = "delvList";
            return base.GetRecs(sSearchString);

        }


        #region PRIMEPOS-CONSENT SAJID DHUKKA PRIMEPOS-2871
        public DataTable PopulateConsentSource()
        {
            string sSql = string.Empty;
            try
            {
                sSql = "SELECT [ID],[Name],[Description],[DisplayOrder],[Active] FROM Consent_Source WHERE Active = 1 ORDER BY DisplayOrder";
                this.sTableName = "Consent_Source";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        public DataTable PopulateConsentNameBasedOnID(string consentId)
        {
            DataSet dsCustData = new DataSet();
            try
            {
                string sSql = string.Empty;
                sSql = string.Format("SELECT [ID],[Name],[Description],[DisplayOrder],[Active] FROM Consent_Source WHERE ID = {0} ORDER BY DisplayOrder", consentId);
                this.sTableName = "Consent_Source";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
        }

        public DataTable GetActivePatientConsent(int PatientNo, System.Collections.Generic.Dictionary<int, string> activeConsentList, out bool isConsentExpired, out bool isConsentHave)
        {
            System.Text.StringBuilder sSql = new System.Text.StringBuilder();
            try
            {
                // This is because we have multiple expired record for every consent
                // Result Based on consent return rows
                if (activeConsentList != null)//PRIMEPOS-3192
                {
                    foreach (var item in activeConsentList)
                    {
                        if (sSql.Length > 0)
                        {
                            sSql.Append("UNION ALL SELECT * FROM (SELECT TOP 1 Patient_Consent.* , DATEADD(day, cast(Consent_Status.ValidityPeriod as int), Patient_Consent.ConsentCaptureDate) AS DateValidity FROM Patient_Consent join Consent_Status on ConsentStatusID=Consent_Status.ID WHERE PatientNo = " + PatientNo + " AND Patient_Consent.ConsentSourceId = " + item.Key + " ORDER BY ConsentCaptureDate DESC) AS TBL ");
                        }
                        else
                        {
                            sSql.Append("SELECT * FROM (SELECT TOP 1 Patient_Consent.*, DATEADD(day, cast(Consent_Status.ValidityPeriod as int), Patient_Consent.ConsentCaptureDate) AS DateValidity FROM Patient_Consent join Consent_Status on ConsentStatusID=Consent_Status.ID WHERE PatientNo = " + PatientNo + " AND Patient_Consent.ConsentSourceId = " + item.Key + " ORDER BY ConsentCaptureDate DESC) AS TBL ");
                        }
                        //if (sSql.Length > 0)
                        //{
                        //    sSql.Append("UNION ALL SELECT * FROM (SELECT TOP 1 * FROM Patient_Consent WHERE PatientNo = " + PatientNo + " AND ConsentSourceId = " + item.Key + " ORDER BY ConsentCaptureDate DESC) AS TBL ");
                        //}
                        //else
                        //{
                        //    sSql.Append("SELECT * FROM (SELECT TOP 1 * FROM Patient_Consent WHERE PatientNo = " + PatientNo + " AND ConsentSourceId = " + item.Key + " ORDER BY ConsentCaptureDate DESC) AS TBL ");
                        //}
                        // SELECT TOP 1 * FROM Patient_Consent WHERE ConsentEndDate < GETDATE() AND PatientNo = '988654771' AND ConsentSourceId = 2 ORDER BY ConsentCaptureDate DESC ");
                    }
                    this.sTableName = "Patient_Consent";
                    DataTable dt = new DataTable();
                    dt = this.GetRecs(sSql.ToString()).Tables[0];
                    //isConsentExpired = dt?.Rows?.Count > 0 ? true : false;
                    //isConsentHave = dt?.Rows?.Count > 0 ? true : false;
                    DataView dv = new DataView(dt);
                    dv.RowFilter = "DateValidity <= #" + DateTime.Now + "#";
                    isConsentExpired = dt?.Rows?.Count > 0 ? true : false;
                    isConsentHave = dt?.Rows?.Count > 0 ? true : false;
                    return dv.ToTable();
                }
                else
                {
                    isConsentExpired = false;
                    isConsentHave = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }
        public DataTable GetActiveConsentTypeById(int ConsentSourceID)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT [ID],[ConsentSourceID],[Name],[Code],[Description],[DisplayOrder],[Active] FROM [Consent_Type] WHERE ConsentSourceID='{0}' AND Active=1 ORDER BY DisplayOrder", ConsentSourceID.ToString());
                this.sTableName = "Consent_Type";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }
        public DataTable GetActiveConsentStatusById(int ConsentSourceID)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT [ID],[ConsentSourceID],[Name],[Code],[Description],[DisplayOrder],[Active],[ValidityPeriod] FROM [Consent_Status] WHERE ConsentSourceID='{0}' AND Active=1 ORDER BY DisplayOrder", ConsentSourceID.ToString());
                this.sTableName = "Consent_Status";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }
        public DataTable GetConsentTextById(int ConsentSourceID)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT [ID] ,[VersionNo] ,[SourceID] ,[LanguageNo] ,[ConsentText] ,[ConsentValidForDays] ,[ConsentFileName], [ConsentTextTitle] FROM [ConsentTextVersion] WHERE SourceID ='{0}' ", ConsentSourceID.ToString());
                this.sTableName = "ConsentTextVersion";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }
        public DataTable GetConsentRelationshipById(int ConsentSourceID)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("Select ID,ConsentSourceID,Relation,Description,DisplayOrder,Active from Consent_Relationship where Active=1 and ConsentSourceID = '{0}' ORDER BY DisplayOrder", ConsentSourceID.ToString()); //PRIMEPOS-3192
                this.sTableName = "Consent_Relationship";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }
        #endregion
        /// <summary>
        /// SAJID PRIMEPOS-2794
        /// </summary>
        /// <param name="patientNo"></param>
        /// <returns></returns>
        public DataTable GetAllPatInsurance(string patientNo)
        {
            string sSql = "SELECT INS_CD,INS_ID,GROUPNO,RELATION,PERSON_CD FROM PATINS WHERE PatientNo='" + patientNo + "'";
            this.sTableName = "PATINS";
            return this.GetRecs(sSql).Tables[0];
        }
        //Customer Engagement Details PRIMEPOS-2794 SAJID DHUKKA
        public DataSet GetPatMedAdherence(int patientNo)
        {
            string therapeuticclass = "1,2,3,4";
            string fromDate = DateTime.Now.AddYears(-1).ToShortDateString();
            string todate = DateTime.Now.ToShortDateString();
            string sSQL = String.Format(@"select PATIENT.LNAME,PATIENT.FNAME,PATIENT.PATTYPE,PATIENT.FACILITYCD,pdc.*,PATIENT.LNAME +' , ' +PATIENT.FNAME as PatientName, 
                                         ( SELECT isnull(
                                                    (SELECT top 1 CASE
                                                        WHEN DRGBRNAME IS NULL THEN DRGNAME
                                                        WHEN RTRIM(LTRIM(DRGBRNAME))='' THEN DRGNAME
                                                        ELSE RTRIM(LTRIM(DRGBRNAME))
                                                     END
                                                     FROM drug
                                                     WHERE SpecificProductID=Pdc.SpecificProductID),'') 
                                        ) SpecProdName 
                                    ,(Select CategoryName FROM MedAdherCategory WHERE CategoryId = pdc.MedAdherCatID) as Category from
                                    (select * from fn_PatMedAdherence('" + fromDate + "','" + todate + "','" + patientNo + "','" + therapeuticclass + "')) " +
                                    " as pdc inner join PATIENT on (PATIENT.PATIENTNO = pdc.patientno) WHERE firstfill is not null order by PATIENT.LNAME,PATIENT.FNAME,[Category]");
            this.sTableName = "PATIENT";
            return base.GetRecs(sSQL);
        }

        #region PRIMEPOS-2442 ADDED BY ROHIT NAIR
        public DataTable GetConsentSourceByName(string consentSourceName)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT * FROM Consent_Source WHERE UPPER(Name)='{0}'", consentSourceName.ToUpper());
                this.sTableName = "Consent_Source";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }

        public int GetConsentSourceID(string consentSourceName)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT ID FROM Consent_Source WHERE UPPER(Name)='{0}'", consentSourceName.ToUpper());
                object oSourceID;
                oSourceID = db.DataReaderScalar(sSql);

                db.Close();

                if (!string.IsNullOrWhiteSpace(oSourceID.ToString()))
                {
                    return Convert.ToInt32(oSourceID.ToString());
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }

        public int GetConsentTextID(int ConsentSourceID, int languageno)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT MAX(ID) FROM ConsentTextVersion WHERE SourceID = '{0}' AND LanguageNo='{1}'", ConsentSourceID.ToString(), languageno.ToString());
                object otxtID;
                otxtID = db.DataReaderScalar(sSql);

                db.Close();

                if (!string.IsNullOrWhiteSpace(otxtID.ToString()))
                {
                    return Convert.ToInt32(otxtID.ToString());
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }
        public int GetConsentTypeID(int ConsentSourceID, string typeCode)
        {
            string sSql = string.Empty;
            try
            {
                //PRIMEPOS-3120
                if (!string.IsNullOrWhiteSpace(typeCode))
                {
                    sSql = string.Format("SELECT ID FROM Consent_Type WHERE ConsentSourceID='{0}' AND UPPER(Code)='{1}'", ConsentSourceID.ToString(), typeCode?.ToUpper());
                }
                else
                {
                    sSql = string.Format("SELECT ID FROM Consent_Type WHERE ConsentSourceID='{0}'", ConsentSourceID.ToString());
                }
                object otypeID;
                otypeID = db.DataReaderScalar(sSql);

                db.Close();

                if (!string.IsNullOrWhiteSpace(otypeID.ToString()))
                {
                    return Convert.ToInt32(otypeID.ToString());
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }
        public int GetConsentStatusID(int ConsentSourceID, string StatusCode)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT ID FROM Consent_Status WHERE ConsentSourceID= '{0}' AND UPPER(Code)='{1}'", ConsentSourceID.ToString(), StatusCode.ToUpper());
                object oStatusID;
                oStatusID = db.DataReaderScalar(sSql);

                db.Close();

                if (!string.IsNullOrWhiteSpace(oStatusID.ToString()))
                {
                    return Convert.ToInt32(oStatusID.ToString());
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }


        public int GetConsentRelationShipID(int ConsentSourceID, string RelationShipString)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT ID FROM Consent_RelationShip WHERE ConsentSourceID= '{0}' AND UPPER(Relation)='{1}'", ConsentSourceID.ToString(), RelationShipString.ToUpper());
                object oStatusID;
                oStatusID = db.DataReaderScalar(sSql);

                db.Close();

                if (!string.IsNullOrWhiteSpace(oStatusID.ToString()))
                {
                    return Convert.ToInt32(oStatusID.ToString());
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        #region PRIMEPOS-3276
        public bool isConsentActiveForPatient(int PatientNo, int ConsentSourceID)
        {
            try
            {
                string sSql = "SELECT 1 FROM Patient_Consent WHERE PatientNo =" + PatientNo + " AND ConsentSourceID = " + ConsentSourceID + " AND ConsentEndDate >= GETDATE()";
                this.sTableName = "Patient_Consent";

                logger.Trace("isConsentActiveForPatient(int PatientNo, DateTime ConsentEndDate, int ConsentSourceID) - SQL - " + sSql);
                DataTable dt = base.GetRecs(sSql).Tables[0];
                if (dt.Rows.Count > 0)
                    isActiveConsent = false;
                else
                    isActiveConsent = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while Fetching Patient Consent");
                throw ex;
            }
            return isActiveConsent;
        }
        #endregion

        // PRIMEPOS-2761 chnage void to bool
        public bool SavePatientConsent(int PatientNo, int ConsentTextID, int ConsentTypeID, int ConsentStatusID, DateTime ConsentCaptureDate, DateTime ConsentEffectiveDate, DateTime ConsentEndDate, int RelationID, string SigneeName, byte[] SignatureData, int ConsentSourceID)
        {
            try
            {
                this.sTableName = "Patient_Consent";
                DataSet ds = this.GetRecs("SELECT * FROM Patient_Consent WHERE 0=1");

                DataRow dr = ds.Tables[sTableName].NewRow();

                ds.Tables[sTableName].Rows.Add(dr);

                ds.Tables[sTableName].Rows[0]["PatientNo"] = PatientNo;
                ds.Tables[sTableName].Rows[0]["ConsentTextID"] = ConsentTextID;
                ds.Tables[sTableName].Rows[0]["ConsentTypeID"] = ConsentTypeID;
                ds.Tables[sTableName].Rows[0]["ConsentStatusID"] = ConsentStatusID;
                ds.Tables[sTableName].Rows[0]["ConsentCaptureDate"] = ConsentCaptureDate;
                ds.Tables[sTableName].Rows[0]["ConsentEffectiveDate"] = ConsentEffectiveDate;
                //Added Back by Rohit Nair on 08/29/2017 for PRIMEPOS-2442
                if (ConsentEndDate != DateTime.MinValue)//PRIMEPOS-2871 Consent
                {
                    ds.Tables[sTableName].Rows[0]["ConsentEndDate"] = ConsentEndDate;
                }//Sprint-26 - PRIMEPOS-2442 24-Aug-2017 JY Consent end date should be null, don't know the impact so keeping parameter as it is
                ds.Tables[sTableName].Rows[0]["RelationID"] = RelationID;
                ds.Tables[sTableName].Rows[0]["SigneeName"] = SigneeName;
                ds.Tables[sTableName].Rows[0]["SignatureData"] = SignatureData;
                ds.Tables[sTableName].Rows[0]["ConsentSourceID"] = ConsentSourceID; // PRIMEPOS-CONSENT SAJID DHUKKA New added for Patient Consent PRIMEPOS-2871

                this.Save(ds);

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while saving Patient Consent");
                throw ex;
            }
            return isRxTxnSuccess;
        }

        //PRIMEPOS-3192
        public long SaveAndGetIDPatientConsent(int PatientNo, int ConsentTextID, int ConsentTypeID, int ConsentStatusID, DateTime ConsentCaptureDate, DateTime ConsentEffectiveDate, DateTime ConsentEndDate, int RelationID, string SigneeName, byte[] SignatureData, int ConsentSourceID)
        {
            long patientConsentID = 0;
            try
            {
                this.sTableName = "Patient_Consent";
                DataSet ds = this.GetRecs("SELECT * FROM Patient_Consent WHERE 0=1");

                DataRow dr = ds.Tables[sTableName].NewRow();

                ds.Tables[sTableName].Rows.Add(dr);

                ds.Tables[sTableName].Rows[0]["PatientNo"] = PatientNo;
                ds.Tables[sTableName].Rows[0]["ConsentTextID"] = ConsentTextID;
                ds.Tables[sTableName].Rows[0]["ConsentTypeID"] = ConsentTypeID;
                ds.Tables[sTableName].Rows[0]["ConsentStatusID"] = ConsentStatusID;
                ds.Tables[sTableName].Rows[0]["ConsentCaptureDate"] = ConsentCaptureDate;
                ds.Tables[sTableName].Rows[0]["ConsentEffectiveDate"] = ConsentEffectiveDate;
                //Added Back by Rohit Nair on 08/29/2017 for PRIMEPOS-2442
                if (ConsentEndDate != DateTime.MinValue)//PRIMEPOS-2871 Consent
                {
                    ds.Tables[sTableName].Rows[0]["ConsentEndDate"] = ConsentEndDate;
                }//Sprint-26 - PRIMEPOS-2442 24-Aug-2017 JY Consent end date should be null, don't know the impact so keeping parameter as it is
                ds.Tables[sTableName].Rows[0]["RelationID"] = RelationID;
                ds.Tables[sTableName].Rows[0]["SigneeName"] = SigneeName;
                ds.Tables[sTableName].Rows[0]["SignatureData"] = SignatureData;
                ds.Tables[sTableName].Rows[0]["ConsentSourceID"] = ConsentSourceID; // PRIMEPOS-CONSENT SAJID DHUKKA New added for Patient Consent PRIMEPOS-2871
                ds.Tables[sTableName].Rows[0]["SignaturePending"] = false;
                ds.Tables[sTableName].Rows[0]["DateAdded"] = DateTime.Now;
                ds.Tables[sTableName].Rows[0]["AddedBy"] = "POS";
                this.Save(ds);

                ds = this.GetRecs($"SELECT ID FROM Patient_Consent WHERE PatientNo={PatientNo} and ConsentCaptureDate='{ConsentCaptureDate}' and ConsentTextID='{ConsentTextID}' and ConsentTypeID='{ConsentTypeID}' and ConsentStatusID='{ConsentStatusID}'");

                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    patientConsentID = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while saving Patient Consent");
                throw ex;
            }
            return patientConsentID;
        }


        public DataTable GetLastPatientConsent(int PatientNo)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT TOP 1 * FROM INSSIGTRANS WHERE PATIENTNO = '{0}' ORDER BY TRANSDATE DESC", PatientNo.ToString());//PRIMEPOS-2794 Arvind
                this.sTableName = "Patient_Consent";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }

        public DataTable GetLastPatientConsent(int PatientNo, string ConsentSourceName)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format(@"SELECT TOP 1 P.* FROM Patient_Consent P 
                                        INNER JOIN ConsentTextVersion T ON P.ConsentTextID = T.ID 
                                        INNER JOIN Consent_Source S ON S.ID = T.SourceID
                                        WHERE P.PatientNo = '{0}' AND UPPER(S.NAME) = '{1}'
                                        ORDER BY P.ConsentCaptureDate DESC", PatientNo.ToString(), ConsentSourceName.ToUpper());
                this.sTableName = "Patient_Consent";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }
        #endregion

        #region PrimePOS-2448 Added BY Rohit Nair

        public DataTable GetIntakeBatchByBatchID(long BatchID)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format(@"SELECT * FROM IntakeBatch WHERE BATCHID='{0}'", BatchID.ToString());
                this.sTableName = "IntakeBatch";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        public long GetBatchIDFromRxno(long Rxno, int nrefill)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT BatchId FROM IntakeQueue WHERE RxNo ='{0}' AND NRefill='{1}'", Rxno.ToString(), nrefill.ToString());
                object oBatchID;
                oBatchID = db.DataReaderScalar(sSql);

                db.Close();

                if (oBatchID != null && !string.IsNullOrWhiteSpace(oBatchID.ToString()))    //PRIMEPOS-2848 21-May-2020 JY handeled null
                {
                    return Convert.ToInt64(oBatchID.ToString());
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        public long GetBatchIDFromPatno(int PatientNo)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT top 1 BatchId FROM IntakeQueue WHERE PatientNo = '{0}' order by CreationTime Desc", PatientNo.ToString());

                logger.Trace("GetBatchIDFromPatno(int PatientNo) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
                object oBatchID;
                oBatchID = db.DataReaderScalar(sSql);

                db.Close();

                if (!string.IsNullOrWhiteSpace(oBatchID.ToString()))
                {
                    return Convert.ToInt64(oBatchID.ToString());
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        public string GetAllBatchesForPatient(int PatientNo)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format(@"SELECT DISTINCT  Q.PatientNo
                , STUFF((SELECT DISTINCT ', ' + CAST(IQ.BatchId AS VARCHAR(20))  FROM IntakeQueue IQ WHERE Q.PatientNo = IQ.PatientNo AND IQ.BatchId>0 FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 1, '') as 'Batch_List'
                FROM IntakeQueue Q WHERE Q.PatientNo ='{0}' ", PatientNo.ToString());
                this.sTableName = "IntakeQueue";

                logger.Trace("GetAllBatchesForPatient(int PatientNo) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
                object oBatchID;
                oBatchID = db.DataReaderScalar(sSql);

                if (!string.IsNullOrWhiteSpace(oBatchID.ToString()))
                {
                    return oBatchID.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        public DataTable GetIntakeQueueByBatchID(long BatchID)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format(@"SELECT * FROM IntakeQueue WHERE BatchId='{0}'", BatchID.ToString());
                this.sTableName = "IntakeQueue";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        public DataTable GetRXOFSInfo(long Rxno, int nrefill)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format(@"SELECT * FROM RXOFSInfo WHERE RxNo = '{0}' AND Nrefill ='{1}'", Rxno.ToString(), nrefill.ToString());
                this.sTableName = "RXOFSInfo";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        public DataTable GetAllRXOFSInfofromBatchID(long BatchID)
        {
            string sSql = string.Empty;
            this.sTableName = "RXOFSInfo";
            try
            {
                sSql = string.Format(@"SELECT R.*,C.STATUS FROM RXOFSInfo R 
                                    INNER JOIN IntakeQueue Q ON R.RxNo=Q.RxNo AND R.Nrefill = Q.NRefill
                                    INNER JOIN CLAIMS C ON R.RxNo =C.RXNO AND R.Nrefill=C.NREFILL
                                    WHERE Q.BatchId='{0}'", BatchID.ToString());
                this.sTableName = "RXOFSInfo";

                logger.Trace("GetAllRXOFSInfofromBatchID(long BatchID) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        public DataTable GetRxsinBatch(long BatchID)
        {
            string sSql = string.Empty;
            this.sTableName = "RXOFSInfo";
            try
            {
                //PRIMEPOS-2897 14-Sep-2020 JY modified to refer DRGName from drug table
                sSql = string.Format(@"SELECT C.PATTYPE,C.BILLTYPE,IsNull(C.Status,'') as Status,C.RXNO,
                        LTRIM(RTRIM(ISNULL(p.LNAME, ''))) + ', ' + LTRIM(RTRIM(ISNULL(p.FNAME, ''))) AS PatientName,C.PRESNO,
                        CASE WHEN (C.BILLTYPE='C' OR RTRIM(C.PATTYPE)='C' OR (I.sub_type='0' )) THEN ISNULL(C.AMOUNT,0)+ISNULL(C.PFEE,0)+ISNULL(C.OTHFEE,0)+ISNULL(C.OTHAMT,0)+ISNULL(C.STax,0)- ISNULL(C.DISCOUNT,0) ELSE C.COPAY END AS PATAMT,
                        C.NDC,D.DRGNAME,C.DATEO,C.DATEF,C.QUANT,C.QTY_ORD,C.DAYS,C.TREFILLS,C.NREFILL, C.DELIVERY,
                        (C.sig +','+ C.siglines) siglines,D.strong,D.form,D.units,D.class,C.PATIENTNO,C.PICKEDUP,C.PICKUPDATE,C.PICKUPTIME,C.PICKUPFROM,C.AMOUNT,C.COPAY,
                        C.TOTAMT,C.PFEE,C.STAX,C.DISCOUNT,C.OTHFEE,C.OTHAMT,C.BILLEDAMT,C.UNC,RTRIM(D.DRGNAME)+' '+RTRIM(D.STRONG)+' '+RTRIM(D.FORM) AS DETDRGNAME ,C.pickuppos,
                        ISNULL(PVL.VerifStatus,'') AS Verified, ISNULL(PVL.VRFStage, 0) AS VRFStage,
                        p.FamilyID FROM CLAIMS C 
                        INNER JOIN DRUG D ON C.NDC = D.DRGNDC 
                        INNER JOIN INSCAR I ON C.PATTYPE = I.IC_CODE
                        INNER JOIN PATIENT P ON C.PATIENTNO = P.PATIENTNO 
                        INNER JOIN IntakeQueue Q ON C.RXNO = Q.RxNo AND C.NREFILL = Q.NRefill
                        LEFT JOIN PharmVerifLog PVL ON PVL.RXNo = C.RXNo And PVL.RefillNo = C.NRefill
                        WHERE Q.BatchId = '{0}'", BatchID.ToString());
                this.sTableName = "CLAIMS";

                logger.Trace("GetRxsinBatch(long BatchID) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        public DataTable GetBatchStatusfromView(string BatchID)
        {
            string sSql = string.Empty;
            try
            {
                //PRIMEPOS-2927
                sSql = string.Format(@"SELECT V.BatchId,V.CreateDate,V.PhUser,V.ReadyByDate,V.IsReadyForPayment,ISNULL(V.ShippingPrice,0.00) as ShippingPrice,
                (SELECT COUNT(Q.BatchId) FROM IntakeQueue Q WHERE Q.BatchId=V.BatchId) AS 'RX_COUNT' FROM v_IntakeBatch V
                WHERE V.BatchId IN ({0}) ", BatchID.ToString());
                this.sTableName = "v_OFS_BatchStatus";

                logger.Trace("GetBatchStatusfromView(string BatchID) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
                DataTable dt = this.GetRecs(sSql).Tables[0];

                sSql = "SELECT FieldValue FROM SettingDetail WHERE FieldName = 'MailOrderType'";

                DataTable dtSettingDetail = this.GetRecs(sSql).Tables[0];

                DataColumn dcolColumn = new DataColumn("MailOrderType", typeof(string));
                dt.Columns.Add(dcolColumn);

                if (dt.Rows.Count > 0) //PRIMEPOS-3251
                    dt.Rows[0]["MailOrderType"] = dtSettingDetail.Rows[0][0];

                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }


        #endregion

        public DataTable GetCustomerAuthIDTypes()
        {
            string sQueryStr = string.Empty;
            this.sTableName = "ncpdpv51";

            try
            {
                sQueryStr = @"SELECT LTRIM(RTRIM(FIELD_VAL)) as FIELD_VAL, LTRIM(RTRIM(VAL_DESCR)) as VAL_DESCR, LTRIM(RTRIM(FIELD_VAL + VAL_DESCR)) AS FULL_VALUES
                            FROM  ncpdpv51  WHERE FIELD_ID = 'AI' ORDER BY FIELD_VAL";
                return base.GetRecs(sQueryStr).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sQueryStr);
                throw ex;
            }
        }

        private void DeleteExistingRecBeforeInsert(Int64 rxno, int refillno)
        {
            string sqlCmd = "delete RxPickupLog where RxNo=" + rxno.ToString() + " and RefillNo=" + refillno.ToString();

            logger.Trace("DeleteExistingRecBeforeInsert(Int64 rxno, int refillno) - SQL - " + sqlCmd);    //PRIMEPOS-3014 13-Oct-2021 JY Added
            this.Update(sqlCmd);
        }

        public void SaveRxPickupLog(DataTable rxListing, DataTable custInfo)
        {
            try
            {
                this.sTableName = "RxPickupLog";
                DataSet ds = this.GetRecs("SELECT * FROM RxPickupLog WHERE 0=1");

                int iCount = 0;
                foreach (DataRow dRow in rxListing.Rows)
                {
                    DataRow dr = ds.Tables[sTableName].NewRow();
                    ds.Tables[sTableName].Rows.Add(dr);

                    ds.Tables[sTableName].Rows[iCount]["RxNo"] = Convert.ToInt64(dRow["Rxno"]);
                    ds.Tables[sTableName].Rows[iCount]["RefillNo"] = Convert.ToUInt16(dRow["RefillNo"]);
                    ds.Tables[sTableName].Rows[iCount]["PickupDate"] = DateTime.Now;
                    ds.Tables[sTableName].Rows[iCount]["PickedupByLname"] = custInfo.Rows[0]["PickedupByLname"];
                    ds.Tables[sTableName].Rows[iCount]["PickedupByFname"] = custInfo.Rows[0]["PickedupByFname"];
                    ds.Tables[sTableName].Rows[iCount]["PickByIDQ"] = custInfo.Rows[0]["PickByIDQ"];
                    ds.Tables[sTableName].Rows[iCount]["PickByID"] = custInfo.Rows[0]["PickByID"];
                    ds.Tables[sTableName].Rows[iCount]["PickByIDAuth"] = custInfo.Rows[0]["PickByIDAuth"];
                    ds.Tables[sTableName].Rows[iCount]["PickByRelation"] = custInfo.Rows[0]["PickByRelation"];
                    ds.Tables[sTableName].Rows[iCount]["DropPickQ"] = custInfo.Rows[0]["DropPickQ"];
                    ds.Tables[sTableName].Rows[iCount]["Notes"] = custInfo.Rows[0]["Notes"];

                    iCount++;

                    DeleteExistingRecBeforeInsert(Convert.ToInt64(dRow["Rxno"]), Convert.ToUInt16(dRow["RefillNo"]));
                }

                this.Save(ds);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while saving RxPickupLog.");
                throw ex;
            }
        }

        public DataSet LoadPatientRxsByFilledDate(DateTime FromDate, DateTime ToDate)
        {
            string strFrom = FromDate.ToString("d") + " 00:00";
            string strTo = ToDate.ToString("d") + " 23:59";
            DataSet ds = new DataSet();

            this.sTableName = "CLAIMS";
            string sSql = "SELECT RTRIM(rtrim(P.LNAME) + ', ' + LTRIM(P.FNAME))+'  #'+CAST(P.PATIENTNO as varchar) as 'Patient Name', P.* from (SELECT distinct PATIENTNO FROM CLAIMS where DATEF between '" + strFrom + "' and '" + strTo + "') UniquePat INNER JOIN Patient P on P.PATIENTNO = UniquePat.PATIENTNO ORDER BY 'Patient Name' ";
            sSql += " SELECT * FROM CLAIMS where DATEF between '" + strFrom + "' and '" + strTo + "' order by PATIENTNO";
            sSql += " SELECT distinct C.PATIENTNO, QBC.CustomerName, QBC.CreatedOn, QBC.CreatedBy from Claims C, QB_PatientCustomer QBC where C.DATEF between '" + strFrom + "' and '" + strTo + "' and C.PATIENTNO=QBC.PATIENTNO";
            sSql += " SELECT distinct QBI.ItemID, QBI.ItemName, QBI.ItemPrice, QBI.ItemDesc, C.RXNO, C.NREFILL, QBI.CreatedOn, QBI.CreatedBy from Claims C, QB_RxItem QBI where C.DATEF between '" + strFrom + "' and '" + strTo + "' and C.RXNO=QBI.RXNO and C.NREFILL=QBI.NREFILL";

            ds = this.GetRecs(sSql);
            if (ds != null)
            {
                ds.Tables[0].TableName = "Patients";
                ds.Tables[1].TableName = "PatientRXs";
                ds.Tables[2].TableName = "QBCustomers";
                ds.Tables[3].TableName = "QBItems";
            }
            return ds;
        }

        public string GetCompoundDrugNames(long rxNo, int refillNo)
        {
            string compDesc = string.Empty;

            this.sTableName = "RXCOMP";
            DataSet ds = new DataSet();
            string sSql = "SELECT DRGNAME, QTY, LNO from RXCOMP where RXNO=" + rxNo.ToString() + " and REFILL_NO=" + refillNo.ToString() + " order by LNO ";
            ds = this.GetRecs(sSql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                int iCount = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    iCount++;
                    compDesc += dr["DRGNAME"].ToString().Trim() + "  Qty:" + Convert.ToInt32(dr["QTY"]);
                    if (iCount < ds.Tables[0].Rows.Count)
                        compDesc += Environment.NewLine;
                }
            }

            return compDesc;
        }

        public DataSet LoadPatientInvoiceByTxnDate(int patientNo, DateTime FromDate, DateTime ToDate)
        {
            string strFrom = FromDate.ToString("d") + " 00:00:01";
            string strTo = ToDate.ToString("d") + " 23:59:59";
            DataSet ds = new DataSet();

            this.sTableName = "QB_PatientInvoice";
            string sSql = "SELECT * from  QB_PatientInvoice where PATIENTNO=" + patientNo.ToString() + " and TxnDate between '" + strFrom + "' and '" + strTo + "' ORDER BY InvoiceID ";
            sSql += " SELECT QV.InvoiceID, QI.* from QB_RxItem QI, QB_InvoiceItem QV where QI.ItemID=QV.ItemID and QV.InvoiceID in (select InvoiceID from QB_PatientInvoice where PATIENTNO=" + patientNo.ToString() + " and TxnDate between '" + strFrom + "' and '" + strTo + "') ORDER BY InvoiceID ";

            ds = this.GetRecs(sSql);
            if (ds != null)
            {
                ds.Tables[0].TableName = "Invoices";
                ds.Tables[1].TableName = "InvItems";
            }
            return ds;
        }

        public void SaveQB_PatientCustomer(DataTable patientTbl)
        {
            try
            {
                this.sTableName = "QB_PatientCustomer";
                DataSet ds = this.GetRecs("SELECT * FROM QB_PatientCustomer WHERE 0=1");

                int iCount = 0;
                foreach (DataRow dRow in patientTbl.Rows)
                {
                    DataRow dr = ds.Tables[sTableName].NewRow();
                    ds.Tables[sTableName].Rows.Add(dr);

                    ds.Tables[sTableName].Rows[iCount]["PATIENTNO"] = dRow["PATIENTNO"];
                    ds.Tables[sTableName].Rows[iCount]["CustomerName"] = dRow["QBCustomerName"];
                    ds.Tables[sTableName].Rows[iCount]["CreatedOn"] = DateTime.Now;
                    ds.Tables[sTableName].Rows[iCount]["CreatedBy"] = "QBApp";

                    iCount++;
                }

                this.Save(ds);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while saving QB_PatientCustomer.");
                throw ex;
            }
        }

        public void SaveQB_RxItem(ref DataTable itemTbl)
        {
            try
            {
                this.sTableName = "QB_RxItem";
                DataSet ds = this.GetRecs("SELECT * FROM QB_RxItem WHERE 0=1");

                int iCount = 0;
                foreach (DataRow dRow in itemTbl.Rows)
                {
                    DataRow dr = ds.Tables[sTableName].NewRow();
                    ds.Tables[sTableName].Rows.Add(dr);

                    ds.Tables[sTableName].Rows[iCount]["RXNO"] = dRow["RXNO"];
                    ds.Tables[sTableName].Rows[iCount]["NREFILL"] = dRow["NREFILL"];
                    ds.Tables[sTableName].Rows[iCount]["PATIENTNO"] = dRow["PATIENTNO"];
                    ds.Tables[sTableName].Rows[iCount]["ItemName"] = dRow["Item Name"];
                    ds.Tables[sTableName].Rows[iCount]["ItemPrice"] = dRow["Price"];
                    ds.Tables[sTableName].Rows[iCount]["ItemDesc"] = dRow["Description"];
                    ds.Tables[sTableName].Rows[iCount]["CreatedOn"] = DateTime.Now;
                    ds.Tables[sTableName].Rows[iCount]["CreatedBy"] = "QBApp";

                    iCount++;
                }

                this.Save(ds);

                object itemID = this.db.DataReaderScalar("Select Max(ItemID) from QB_RxItem");
                int maxID = Convert.ToInt32(itemID);

                for (int i = itemTbl.Rows.Count - 1; i >= 0; i--)
                {
                    itemTbl.Rows[i]["Item ID"] = maxID;
                    maxID--;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while saving QB_RxItem.");
                throw ex;
            }
        }

        public void SaveQB_PatientInvoice(int QBRefNumber, decimal invoiceBalanceDue, DateTime invoiceDueDate, DataTable itemTbl)
        {
            try
            {
                int iCount = 0;

                this.sTableName = "QB_PatientInvoice";
                DataSet ds1 = this.GetRecs("SELECT * FROM QB_PatientInvoice WHERE 0=1");

                DataRow dr1 = ds1.Tables[sTableName].NewRow();
                ds1.Tables[sTableName].Rows.Add(dr1);

                ds1.Tables[sTableName].Rows[iCount]["PATIENTNO"] = itemTbl.Rows[0]["PATIENTNO"];
                ds1.Tables[sTableName].Rows[iCount]["QBInvoiceNum"] = QBRefNumber;
                ds1.Tables[sTableName].Rows[iCount]["Amount"] = invoiceBalanceDue;
                ds1.Tables[sTableName].Rows[iCount]["InvoiceDueDate"] = invoiceDueDate;
                ds1.Tables[sTableName].Rows[iCount]["TxnDate"] = DateTime.Now;
                ds1.Tables[sTableName].Rows[iCount]["CreatedBy"] = "QBApp";

                this.Save(ds1);

                object InvoiceID = this.db.DataReaderScalar("Select Max(InvoiceID) from QB_PatientInvoice");
                int maxID = Convert.ToInt32(InvoiceID);


                this.sTableName = "QB_InvoiceItem";
                DataSet ds2 = this.GetRecs("SELECT * FROM QB_InvoiceItem WHERE 0=1");

                iCount = 0;
                foreach (DataRow dRow in itemTbl.Rows)
                {
                    DataRow dr = ds2.Tables[sTableName].NewRow();
                    ds2.Tables[sTableName].Rows.Add(dr);

                    ds2.Tables[sTableName].Rows[iCount]["InvoiceID"] = maxID;
                    ds2.Tables[sTableName].Rows[iCount]["ItemID"] = dRow["Item ID"];

                    iCount++;
                }

                this.Save(ds2);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while saving QB_PatientInvoice or QB_InvoiceItem.");
                throw ex;
            }
        }

        public DataSet LoadFacilityRxsByFilledDate(DateTime FromDate, DateTime ToDate, string facilityFilter)
        {
            string strFrom = FromDate.ToString("d") + " 00:00";
            string strTo = ToDate.ToString("d") + " 23:59";
            DataSet ds = new DataSet();

            this.sTableName = "CLAIMS";
            string sSql = string.Empty;

            if (string.IsNullOrWhiteSpace(facilityFilter))
            {
                sSql = "SELECT RTRIM(rtrim(F.FACILITYNM) + ' - ' + LTRIM(F.FACILITYCD)) as 'Facility Name', F.* from (SELECT distinct P.FACILITYCD FROM  PATIENT P where  P.FACILITYCD<>'' and P.PATIENTNO in (Select distinct PATIENTNO from CLAIMS where DATEF between '" + strFrom + "' and '" + strTo + "')) UniqueFacility INNER JOIN FACILITY F on F.FACILITYCD = UniqueFacility.FACILITYCD ORDER BY 'Facility Name' ";
                sSql += " SELECT C.*, RTRIM(rtrim(P.FNAME) + ' ' + LTRIM(P.LNAME)) as 'Patient Name', (case WHEN P.FACILITYCD='' THEN 'ZZZZZZZ' ELSE isnull(P.FACILITYCD, 'ZZZZZZZ') END) as FACILITYCD from CLAIMS C LEFT OUTER JOIN PATIENT P on C.PATIENTNO=P.PATIENTNO where C.DATEF between '" + strFrom + "' and '" + strTo + "' order by FACILITYCD, 'Patient Name' ";
            }
            else
            {
                sSql = "SELECT RTRIM(rtrim(F.FACILITYNM) + ' - ' + LTRIM(F.FACILITYCD)) as 'Facility Name', F.* from (SELECT distinct P.FACILITYCD FROM  PATIENT P where  P.FACILITYCD in " + facilityFilter + " and P.PATIENTNO in (Select distinct PATIENTNO from CLAIMS where DATEF between '" + strFrom + "' and '" + strTo + "')) UniqueFacility INNER JOIN FACILITY F on F.FACILITYCD = UniqueFacility.FACILITYCD ORDER BY 'Facility Name' ";
                sSql += " SELECT C.*, RTRIM(rtrim(P.FNAME) + ' ' + LTRIM(P.LNAME)) as 'Patient Name', (case WHEN P.FACILITYCD='' THEN 'ZZZZZZZ' ELSE isnull(P.FACILITYCD, 'ZZZZZZZ') END) as FACILITYCD from CLAIMS C LEFT OUTER JOIN PATIENT P on C.PATIENTNO=P.PATIENTNO where C.DATEF between '" + strFrom + "' and '" + strTo + "' and FACILITYCD in " + facilityFilter + " order by FACILITYCD, 'Patient Name' ";
            }
            sSql += " SELECT * from QB_FacilityAccount ";
            sSql += " SELECT distinct QBI.ItemID, QBI.ItemName, QBI.ItemPrice, QBI.ItemDesc, C.RXNO, C.NREFILL, QBI.CreatedOn, QBI.CreatedBy from Claims C, QB_RxItem QBI where C.DATEF between '" + strFrom + "' and '" + strTo + "' and C.RXNO=QBI.RXNO and C.NREFILL=QBI.NREFILL";
            sSql += " SELECT FACILITYCD, FACILITYNM from FACILITY where FACILITYNM<>'' order by FACILITYNM ";

            ds = this.GetRecs(sSql);
            if (ds != null)
            {
                ds.Tables[0].TableName = "Facilities";
                if (ds.Tables[1].Rows.Count > 0 && string.IsNullOrWhiteSpace(facilityFilter)) //there is RXs loaded, All facilities including None
                {
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["Facility Name"] = "NONE";
                    dr["FACILITYCD"] = "ZZZZZZZ";
                    ds.Tables[0].Rows.Add(dr);
                }

                ds.Tables[1].TableName = "FacilityRXs";
                ds.Tables[2].TableName = "QBAccounts";
                ds.Tables[3].TableName = "QBItems";
                ds.Tables[4].TableName = "FacilityList";
            }
            return ds;
        }

        //This is for utility
        public DataSet LoadAttachedDocumentByScanDate(DateTime FromDate, DateTime ToDate)
        {
            string strFrom = FromDate.ToString("d") + " 00:00";
            string strTo = ToDate.ToString("d") + " 23:59";
            DataSet ds = new DataSet();

            this.sTableName = "DM_Document";
            //string sSql = " SELECT * FROM CLAIMS where DATEF between '" + strFrom + "' and '" + strTo + "' order by PATIENTNO";

            string sSql = "SELECT Doc.DocumentID, Sub.Description as SubCategory, DMRx.ReferenceNo, Doc.ScanDate from DM_Document Doc ";
            sSql += " LEFT OUTER JOIN DM_DocumentSubCat Sub on Sub.SubCategoryId=Doc.SubCategoryId LEFT OUTER JOIN DM_DocumentRx DMRx on DMRx.DocumentId = Doc.DocumentId ";
            sSql += "  where Doc.CategoryId=1 and  Doc.ScanDate between '" + strFrom + "' and '" + strTo + "' order by SubCategory desc, DocumentId desc ";
            sSql += " select IMG_STORE_PATH from CONSTANT ";

            ds = this.GetRecs(sSql);

            return ds;
        }

        public void SaveQB_FacilityAccount(DataTable facilityTbl)
        {
            try
            {
                this.sTableName = "QB_FacilityAccount";
                DataSet ds = this.GetRecs("SELECT * FROM QB_FacilityAccount WHERE 0=1");

                int iCount = 0;
                foreach (DataRow dRow in facilityTbl.Rows)
                {
                    DataRow dr = ds.Tables[sTableName].NewRow();
                    ds.Tables[sTableName].Rows.Add(dr);

                    ds.Tables[sTableName].Rows[iCount]["FACILITYCD"] = dRow["FACILITYCD"];
                    ds.Tables[sTableName].Rows[iCount]["AccountName"] = dRow["QBAccountName"];
                    ds.Tables[sTableName].Rows[iCount]["Description"] = dRow["QBFacilityAccountDesc"];
                    ds.Tables[sTableName].Rows[iCount]["CreatedOn"] = DateTime.Now;
                    ds.Tables[sTableName].Rows[iCount]["CreatedBy"] = "QBApp";

                    iCount++;
                }

                this.Save(ds);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while saving QB_FacilityAccount.");
                throw ex;
            }
        }

        public void SaveQB_FacilityInvoice(int QBRefNumber, decimal invoiceBalanceDue, DateTime invoiceDueDate, DataTable itemTbl, string accountName, string facilityCode)
        {
            try
            {
                int iCount = 0;

                this.sTableName = "QB_PatientInvoice";
                DataSet ds1 = this.GetRecs("SELECT * FROM QB_PatientInvoice WHERE 0=1");

                DataRow dr1 = ds1.Tables[sTableName].NewRow();
                ds1.Tables[sTableName].Rows.Add(dr1);

                ds1.Tables[sTableName].Rows[iCount]["PATIENTNO"] = -1;
                ds1.Tables[sTableName].Rows[iCount]["QBInvoiceNum"] = QBRefNumber;
                ds1.Tables[sTableName].Rows[iCount]["Amount"] = invoiceBalanceDue;
                ds1.Tables[sTableName].Rows[iCount]["InvoiceDueDate"] = invoiceDueDate;
                ds1.Tables[sTableName].Rows[iCount]["TxnDate"] = DateTime.Now;
                ds1.Tables[sTableName].Rows[iCount]["CreatedBy"] = "QBApp";
                ds1.Tables[sTableName].Rows[iCount]["AccountName"] = accountName;
                ds1.Tables[sTableName].Rows[iCount]["FACILITYCD"] = facilityCode;

                this.Save(ds1);

                object InvoiceID = this.db.DataReaderScalar("Select Max(InvoiceID) from QB_PatientInvoice");
                int maxID = Convert.ToInt32(InvoiceID);


                this.sTableName = "QB_InvoiceItem";
                DataSet ds2 = this.GetRecs("SELECT * FROM QB_InvoiceItem WHERE 0=1");

                iCount = 0;
                foreach (DataRow dRow in itemTbl.Rows)
                {
                    DataRow dr = ds2.Tables[sTableName].NewRow();
                    ds2.Tables[sTableName].Rows.Add(dr);

                    ds2.Tables[sTableName].Rows[iCount]["InvoiceID"] = maxID;
                    ds2.Tables[sTableName].Rows[iCount]["ItemID"] = dRow["Item ID"];

                    iCount++;
                }

                this.Save(ds2);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occured while saving QB_PatientInvoice or QB_InvoiceItem.");
                throw ex;
            }
        }

        public DataSet LoadFacilityInvoiceByTxnDate(string FacilityCode, DateTime FromDate, DateTime ToDate)
        {
            string strFrom = FromDate.ToString("d") + " 00:00:01";
            string strTo = ToDate.ToString("d") + " 23:59:59";
            DataSet ds = new DataSet();

            this.sTableName = "QB_PatientInvoice";
            string sSql = "SELECT * from QB_PatientInvoice where FACILITYCD='" + FacilityCode + "' and TxnDate between '" + strFrom + "' and '" + strTo + "' ORDER BY InvoiceID ";
            sSql += " SELECT QV.InvoiceID, QI.* from QB_RxItem QI, QB_InvoiceItem QV where QI.ItemID=QV.ItemID and QV.InvoiceID in (select InvoiceID from QB_PatientInvoice where FACILITYCD='" + FacilityCode + "' and TxnDate between '" + strFrom + "' and '" + strTo + "') ORDER BY InvoiceID ";

            ds = this.GetRecs(sSql);
            if (ds != null)
            {
                ds.Tables[0].TableName = "Invoices";
                ds.Tables[1].TableName = "InvItems";
            }
            return ds;
        }

        /// <summary>
        /// Get HC Account details by POS TransID
        /// </summary>
        /// Author: PRIMEPOS-2587 28-Nov-2018 JY Added
        /// <param name="POSTRANSID"></param>
        /// <returns></returns>
        public DataTable GetHCAccountDetailsByPosTransId(long POSTRANSID)
        {
            string sSql = "SELECT a.ACCT_NAME, a.ACCT_NO , b.REFERENCE FROM ACCOUNT a INNER JOIN ACCTTRAN b ON a.ACCT_NO = b.ACCT_NO " +
                            " WHERE b.POSTRANSID = " + POSTRANSID; //PRIMEPOS-3471

            logger.Trace("GetHCAccountDetailsByPosTransId(long POSTRANSID) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            this.sTableName = "ACCOUNT";
            return base.GetRecs(sSql).Tables[0];
        }


        public Int32 GetNumOfQBItemsByDates(DateTime FromDate, DateTime ToDate)
        {
            string strFrom = FromDate.ToString("d") + " 00:00";
            string strTo = ToDate.ToString("d") + " 23:59";
            int num = 0;

            DataSet ds = new DataSet();
            this.sTableName = "QB_RxItem";
            string sSql = "SELECT count(*) as totalItems from QB_RxItem where CreatedOn between '" + strFrom + "' and '" + strTo + "'";

            ds = this.GetRecs(sSql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                num = Convert.ToInt32(ds.Tables[0].Rows[0]["totalItems"]);

            return num;
        }

        public Int32 GetNumOfQBInvoicesByDates(DateTime FromDate, DateTime ToDate)
        {
            string strFrom = FromDate.ToString("d") + " 00:00";
            string strTo = ToDate.ToString("d") + " 23:59";
            int num = 0;

            DataSet ds = new DataSet();
            this.sTableName = "QB_PatientInvoice";
            string sSql = "SELECT count(*) as totalInvoices from QB_PatientInvoice where TxnDate between '" + strFrom + "' and '" + strTo + "'";

            ds = this.GetRecs(sSql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                num = Convert.ToInt32(ds.Tables[0].Rows[0]["totalInvoices"]);

            return num;
        }

        public DataTable LoadQBItemsByCreationDate(DateTime FromDate, DateTime ToDate)
        {
            string strFrom = FromDate.ToString("d") + " 00:00:01";
            string strTo = ToDate.ToString("d") + " 23:59:59";
            DataTable dt = null;
            DataSet ds = new DataSet();

            this.sTableName = "QB_RxItem";
            string sSql = "SELECT ItemName from QB_RxItem where CreatedOn between '" + strFrom + "' and '" + strTo + "' ";
            ds = this.GetRecs(sSql);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public bool DeleteFrom_QB_RxItem(string whereClause)
        {
            string sqlCmd = "DELETE from QB_RxItem where ItemName in " + whereClause;
            bool rtn = this.ExecuteSql(sqlCmd);

            return rtn;
        }

        public DataTable LoadQBInvoicesByCreationDate(DateTime FromDate, DateTime ToDate)
        {
            string strFrom = FromDate.ToString("d") + " 00:00:01";
            string strTo = ToDate.ToString("d") + " 23:59:59";
            DataTable dt = null;
            DataSet ds = new DataSet();

            this.sTableName = "QB_PatientInvoice";
            string sSql = "SELECT QBInvoiceNum, ISNULL(AccountName, 'Accounts Receivable') as AccountName, InvoiceID from QB_PatientInvoice where TxnDate between '" + strFrom + "' and '" + strTo + "' ";
            ds = this.GetRecs(sSql);

            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        public bool DeleteFrom_QB_PatientInvoice(string whereClause)
        {
            string sqlCmd = "DELETE from QB_PatientInvoice where InvoiceID in " + whereClause;
            bool rtn = this.ExecuteSql(sqlCmd);

            string sqlCmd2 = "DELETE from QB_InvoiceItem where InvoiceID in " + whereClause;
            rtn = this.ExecuteSql(sqlCmd2);
            return rtn;
        }

        #region Batch Delivery - NileshJ - PRIMERX-7688  - 25-Sept-2019
        public DataTable GetRxForDelivery(string sRxNo, out string sStatus, string RefillNo)
        {
            sStatus = "false";

            DataTable oRx = GetRxs(sRxNo, RefillNo, "");

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

                for (int i = oRx.Rows.Count - 1; i >= 1; i--) //same as importing the first row
                    oRx.Rows[i].Delete();
                oRx.AcceptChanges();
            }
            return oRx;
        }

        // get data with username from Delivery_Batch table with Delivery_User table (Batch Details)
        public DataTable GetBatchDeliveryDetails(string batchNo)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("select db.DelBatchRecId, db.BatchNo, du.UserName as DelUserId, db.CreationDate, db.BatchStatus, db.BatchType, db.PatientCount, db.RxCount, " +
                " db.TotalCopay, db.TotalCopayCollected, db.CompletionDate, db.Notes, db.FacilityCD, db.DeviceDelivery, db.BatchCategory, db.ShipService_CD, db.ShipCustomCD, db.IntakeBatchID, db.POSStamped " +
                " from Delivery_Batch db left join DELIVERY_User du on du.DelUserRecId = db.DelUserId where BatchNo = '{0}'", batchNo);
                this.sTableName = "Delivery_Batch";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }

        // get data from Delivery_Order table (Patient Details)
        public DataTable GetBatchDeliveryPatient(string batchRecID)
        {
            string sSql = string.Empty;
            try
            {
                //select DelPatRecId,DelPatientNo,(select (FNAME + ' '+ LNAME) from PATIENT where PATIENTNO=DelPatientNo) as DelPatientName,DelStatus,DelBatchId,DelAddress,PatientAddress,FacilityAddress,DelInstructions,ReqDelDate,DateDelivered, DelAcceptedBy, TotalCopay, TotalCopayCollected, NonDelReason, Notes, Priority, Driver, DeliveryDestination, DriverInitials, TRANSNO,DeliveryNote, NonDeliveryNote, DeliveryMethod, ShipService_CD, Ship_TrackingNo, IsExcluded, ShipCustomCD,Reconciled,CopayCollectedPOS from Delivery_Order where DelBatchId = '485' and DelStatus in(select case when  BatchStatus='O' then 'T'  else DelStatus end as delOrderStatus from Delivery_Batch where DelBatchRecId='485')
                //sSql = string.Format("select DelPatRecId,DelPatientNo,(select (FNAME + ' '+ LNAME) from PATIENT where PATIENTNO=DelPatientNo) as DelPatientName,DelStatus,DelBatchId,DelAddress,PatientAddress,FacilityAddress,DelInstructions,ReqDelDate,DateDelivered, DelAcceptedBy, TotalCopay, TotalCopayCollected, NonDelReason, Notes, Priority, Driver, DeliveryDestination, DriverInitials, TRANSNO,DeliveryNote, NonDeliveryNote, DeliveryMethod, ShipService_CD, Ship_TrackingNo, IsExcluded, ShipCustomCD,Reconciled,CopayCollectedPOS from Delivery_Order where DelBatchId = '{0}' and DelStatus in(select case when  BatchStatus='O' then 'T'  else DelStatus end as delOrderStatus from Delivery_Batch where DelBatchRecId='{0}')", batchRecID);
                sSql = string.Format("select do.DelPatRecId,do.DelPatientNo, (pt.FNAME + ' '+ pt.LNAME) as DelPatientName,do.DelStatus,do.DelBatchId,do.DelAddress,do.PatientAddress, " +
                    " do.FacilityAddress,do.DelInstructions,do.ReqDelDate,do.DateDelivered, do.DelAcceptedBy, do.TotalCopay, do.TotalCopayCollected, do.NonDelReason, do.Notes, do.[Priority], do.Driver, " +
                    " do.DeliveryDestination, do.DriverInitials, do.TRANSNO,do.DeliveryNote, do.NonDeliveryNote, do.DeliveryMethod, do.ShipService_CD, do.Ship_TrackingNo, do.IsExcluded, " +
                    " do.ShipCustomCD,do.Reconciled,do.CopayCollectedPOS " +
                    " from Delivery_Order do   left join Delivery_Batch db on do.DelBatchId = db.DelBatchRecId " +
                    " left join PATIENT pt on PATIENTNO = DelPatientNo where DelStatus in( case when BatchStatus = 'O' then 'T'  else DelStatus end) and DelBatchId='{0}'", batchRecID);
                this.sTableName = "Delivery_Order";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }

        // get data from Delivery_Detail (Rx Details)
        public DataTable GetBatchDeliveryRx(string batchRecID)
        {
            string sSql = string.Empty;
            try
            {
                //sSql = string.Format("select DelDetRecId,DelPatRecId,RxNo,RefillNo,DrugName,DelStatus,DateDelivered,DelAcceptedBy,Copay,CopayCollected,NonDelReason,ItemType,Qty,TransID,UnitPrice, RxNoInt, RXNoRefillNo, IsExcluded, NonDeliveryDetailNote,(select DelPatientNo from Delivery_Order where Delivery_Order.DelPatRecId=Delivery_Detail.DelPatRecId) as PatientNo from Delivery_Detail where DelPatRecId in (select DelPatRecId from Delivery_Order where DelBatchId = '{0}') and DelStatus in(select case when  BatchStatus='O' then 'T'  else DelStatus end as delOrderStatus from Delivery_Batch where DelBatchRecId='{0}')", batchRecID);
                sSql = string.Format("select DD.DelDetRecId,DD.DelPatRecId,DD.RxNo,DD.RefillNo,DD.DrugName,DD.DelStatus,DD.DateDelivered,DD.DelAcceptedBy,DD.Copay,DD.CopayCollected,DD.NonDelReason,DD.ItemType,DD.Qty,DD.TransID, " +
               " DD.UnitPrice, DD.RxNoInt, DD.RXNoRefillNo, DD.IsExcluded, DD.NonDeliveryDetailNote, DO.DelPatientNo as PatientNo , DD.CopayCollectedPOS " +
               " from Delivery_Detail DD left join Delivery_Order DO on DO.DelPatRecId = DD.DelPatRecId   " +
               " left join Delivery_Batch DB on DB.DelBatchRecId = DO.DelBatchId " +
               " where DD.DelStatus in ( case when  BatchStatus = 'O' then 'T'  else DD.DelStatus end  ) and DB.DelBatchRecId = '{0}'", batchRecID);
                this.sTableName = "Delivery_Detail";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }

        // Update Delivery_Batch table (set POSStamped)
        public void UpdateBatchDeliveryPaymentStatus(DataTable dtBatchDeliveryData)
        {
            string sSql = string.Empty;
            try
            {
                foreach (DataRow oRow in dtBatchDeliveryData.Rows)
                {
                    sSql = "UPDATE Delivery_Batch SET " +
                            //"DelBatchRecId='" + oRow["DelBatchRecId"].ToString() + "' " +
                            //"BatchNo='" + oRow["BatchNo"].ToString() + "' " +
                            ////",DelUserId='" + oRow["DelUserId"].ToString() + "' " +
                            //",CreationDate='" + oRow["CreationDate"].ToString() + "' " +
                            //",BatchStatus='" + oRow["BatchStatus"].ToString() + "' " +
                            //",BatchType='" + oRow["BatchType"].ToString() + "' " +
                            //",PatientCount='" + oRow["PatientCount"].ToString() + "' " +
                            //",RxCount='" + oRow["RxCount"].ToString() + "' " +
                            //",TotalCopay='" + oRow["TotalCopay"].ToString() + "' " +
                            //",TotalCopayCollected='" + oRow["TotalCopayCollected"].ToString() + "' " +
                            //",CompletionDate='" + oRow["CompletionDate"].ToString() + "' " +
                            //",Notes='" + oRow["Notes"].ToString() + "' " +
                            //",FacilityCD='" + oRow["FacilityCD"].ToString() + "' " +
                            //",DeviceDelivery='" + oRow["DeviceDelivery"].ToString() + "' " +
                            //",BatchCategory='" + oRow["BatchCategory"].ToString() + "' " +
                            //",ShipService_CD='" + oRow["ShipService_CD"].ToString() + "' " +
                            //",ShipCustomCD='" + oRow["ShipCustomCD"].ToString() + "' " +
                            //",IntakeBatchID='" + oRow["IntakeBatchID"].ToString() + "' ," +
                            "POSStamped='" + oRow["POSStamped"].ToString() + "' "
                        + " where BatchNo='" + oRow["BatchNo"].ToString() + "'";

                    base.ExecuteSql(sSql);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }
        //PRIMEPOS-3120
        public int GetConsentValidityPeriod(int ConsentSourceID, int StatusID)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT ValidityPeriod FROM Consent_Status WHERE ConsentSourceID= '{0}' AND ID='{1}'", ConsentSourceID.ToString(), StatusID);
                object oValidityPeriod;
                oValidityPeriod = db.DataReaderScalar(sSql);

                db.Close();

                if (!string.IsNullOrWhiteSpace(oValidityPeriod.ToString()))
                {
                    return Convert.ToInt32(oValidityPeriod.ToString());
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }
        // Update Delivery_Order table 
        public void UpdateBatchDeliveryOrderPaymentStatus(DataTable dtBatchDeliveryData)
        {
            string sSql = string.Empty;
            try
            {
                foreach (DataRow oRow in dtBatchDeliveryData.Rows)
                {
                    sSql = "UPDATE Delivery_Order SET " +

                    //"DelPatRecId='" + oRow["DelPatRecId"].ToString() + "' " +
                    "DelPatientNo='" + oRow["DelPatientNo"].ToString() + "' " +
                    ",DelStatus='" + oRow["DelStatus"].ToString() + "' " +
                    ",DelBatchId='" + oRow["DelBatchId"].ToString() + "' " +
                    ",DelAddress='" + oRow["DelAddress"].ToString() + "' " +
                    ",PatientAddress='" + oRow["PatientAddress"].ToString() + "' " +
                    ",FacilityAddress='" + oRow["FacilityAddress"].ToString() + "' " +
                    ",DelInstructions='" + oRow["DelInstructions"].ToString() + "' " +
                    ",ReqDelDate='" + oRow["ReqDelDate"].ToString() + "' " +
                    ",DateDelivered='" + oRow["DateDelivered"].ToString() + "' " +
                    ",DelAcceptedBy='" + oRow["DelAcceptedBy"].ToString() + "' " +
                    ",TotalCopay='" + oRow["TotalCopay"].ToString() + "' " +
                    ",TotalCopayCollected='" + oRow["TotalCopayCollected"].ToString() + "' " +
                    ",NonDelReason='" + oRow["NonDelReason"].ToString() + "' " +
                    ",Notes='" + oRow["Notes"].ToString() + "' " +
                    ",Priority='" + oRow["Priority"].ToString() + "' " +
                    ",Driver='" + oRow["Driver"].ToString() + "' " +
                    ",DeliveryDestination='" + oRow["DeliveryDestination"].ToString() + "' " +
                    ",Reconciled='" + oRow["Reconciled"].ToString() + "' "
                        + "where DelStatus not in ('U','E') and Reconciled = 0 and DelBatchId='" + oRow["DelBatchId"].ToString() + "'and DelPatientNo='" + oRow["DelPatientNo"].ToString() + "'";

                    logger.Trace("UpdateBatchDeliveryOrderPaymentStatus(DataTable dtBatchDeliveryData) - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                    base.ExecuteSql(sSql);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }


        public void UpdateBatchDeliveryDetailPaymentStatus(DataTable dtBatchDeliveryData)
        {
            DataTable dtDeliveryDetail = new DataTable();
            DataTable dtDeliveryOrder = new DataTable();
            int batchId = 0;
            string sSql = string.Empty;
            decimal rxCopay = 0;
            decimal rxCopayCollected = 0;
            int delPatRecId = (dtBatchDeliveryData.AsEnumerable().Select(p => p.Field<int>("DelPatRecId"))).FirstOrDefault();// get patient record id
            string delStatus = (dtBatchDeliveryData.AsEnumerable().Select(p => p.Field<string>("DelStatus"))).FirstOrDefault(); // get delivery status
            int reconciled = 0;
            decimal remainingCopayAmount = 0;

            rxCopayCollected = dtBatchDeliveryData.AsEnumerable().Sum(x => x.Field<decimal>("CopayCollectedPOS"));// sum of copay collected amount from Delivery_Detail
            rxCopay = dtBatchDeliveryData.AsEnumerable().Sum(x => x.Field<decimal>("Copay")); // sum of copay amount from Delivery_Detail

            try
            {
                foreach (DataRow oRow in dtBatchDeliveryData.Rows)
                {
                    // Update Delivery_Detail (Set Copay , CopayCollected) 
                    sSql = "UPDATE Delivery_Detail SET " +
                    #region Commented
                    //"DelDetRecId='" + oRow["DelDetRecId"].ToString() + "' " +
                    //"DelPatRecId='" + oRow["DelPatRecId"].ToString() + "' " +
                    //",RxNo='" + oRow["RxNo"].ToString() + "' " +
                    //",RefillNo='" + oRow["RefillNo"].ToString() + "' " +
                    //",DrugName='" + oRow["DrugName"].ToString() + "' " +
                    //",DelStatus='" + oRow["DelStatus"].ToString() + "' " +
                    //",DateDelivered='" + oRow["DateDelivered"].ToString() + "' " +
                    //",DelAcceptedBy='" + oRow["DelAcceptedBy"].ToString() + "' ," +
                    //+
                    //",NonDelReason='" + oRow["NonDelReason"].ToString() + "' " +
                    //",ItemType='" + oRow["ItemType"].ToString() + "' " +
                    //",Qty='" + oRow["Qty"].ToString() + "' " +
                    //",TransID='" + oRow["TransID"].ToString() + "' " +
                    //",UnitPrice='" + Convert.ToDecimal(oRow["UnitPrice"]) + "' " +
                    //",RxNoInt='" + oRow["RxNoInt"].ToString() + "' " +
                    //",RXNoRefillNo='" + oRow["RXNoRefillNo"].ToString() + "' " +
                    //",IsExcluded='" + oRow["IsExcluded"].ToString() + "' " +
                    //",NonDeliveryDetailNote='" + oRow["NonDeliveryDetailNote"].ToString() + "' "
                    #endregion
                    "Copay='" + oRow["Copay"].ToString() + "' " +
                    ",CopayCollectedPOS='" + oRow["CopayCollectedPOS"].ToString() + "' "
                    + " where  RxNo='" + oRow["RxNo"].ToString() + "' and DelPatRecId='" + oRow["DelPatRecId"].ToString() + "'";

                    logger.Trace("UpdateBatchDeliveryDetailPaymentStatus(DataTable dtBatchDeliveryData) 1 - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                    base.ExecuteSql(sSql);
                }

                // get Delivery_Detail data with exclude U and E status
                //sSql = string.Format("select DelDetRecId,DelPatRecId,RxNo,RefillNo,DrugName,DelStatus,DateDelivered,DelAcceptedBy,Copay,CopayCollected,NonDelReason,ItemType,Qty,TransID,UnitPrice, RxNoInt, RXNoRefillNo, IsExcluded, NonDeliveryDetailNote,(select DelPatientNo from Delivery_Order where Delivery_Order.DelPatRecId=Delivery_Detail.DelPatRecId) as PatientNo from Delivery_Detail where DelStatus not in ('U','E') and DelPatRecId in (" + delPatRecId + ")");
                sSql = string.Format("select DD.DelDetRecId,DD.DelPatRecId,DD.RxNo,DD.RefillNo,DD.DrugName,DD.DelStatus,DD.DateDelivered,DD.DelAcceptedBy,DD.Copay,DD.CopayCollected,DD.NonDelReason,DD.ItemType,DD.Qty,DD.TransID,DD.UnitPrice, " +
                " DD.RxNoInt, DD.RXNoRefillNo, DD.IsExcluded, DD.NonDeliveryDetailNote, DO.DelPatientNo as PatientNo , DD.CopayCollectedPOS  " +
                " from Delivery_Detail DD left join Delivery_Order DO on DO.DelPatRecId = DD.DelPatRecId " +
                " where DD.DelStatus not in ('U', 'E') and DO.DelPatRecId in (" + delPatRecId + ")");
                this.sTableName = "Delivery_Detail";

                logger.Trace("UpdateBatchDeliveryDetailPaymentStatus(DataTable dtBatchDeliveryData) 2 - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                dtDeliveryDetail = this.GetRecs(sSql).Tables[0];

                rxCopayCollected = dtDeliveryDetail.AsEnumerable().Sum(x => x.Field<decimal>("CopayCollectedPOS")); // After update delivery_detail again sum of Copay Collected amount for finding remaining amount and update CopayCollectedPOS
                rxCopay = dtDeliveryDetail.AsEnumerable().Sum(x => x.Field<decimal>("Copay")); // After update delivery_detail again sum of Copay amount for finding remaining amount and update CopayCollectedPOS
                remainingCopayAmount = rxCopay - rxCopayCollected; // remaining amount

                string extQuery = "";

                if (delStatus != "T") // Check CLOSE status
                {
                    if (rxCopay == rxCopayCollected)
                    {
                        reconciled = 1; // Set Recoinciled true in Delivery_Order
                    }
                }
                else
                {
                    extQuery = " , TotalCopay= " + remainingCopayAmount; // If status OPEN then set Remaining amount in TotalCopay
                }
                extQuery += " , CopayCollectedPOS= IIF(CopayCollectedPOS IS NULL, 0, CopayCollectedPOS)+ " + rxCopayCollected;

                //sSql = "UPDATE Delivery_Order SET Reconciled=" + Reconciled + extQuery + " , CopayCollectedPOS= IIF(CopayCollectedPOS IS NULL, 0, CopayCollectedPOS)+ " + CopayCollectedPOS+" where DelPatRecId in(" + DelPatRecId + ")";
                sSql = "UPDATE Delivery_Order SET Reconciled=" + reconciled + extQuery + " where DelPatRecId in(" + delPatRecId + ")";

                logger.Trace("UpdateBatchDeliveryDetailPaymentStatus(DataTable dtBatchDeliveryData) 3 - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                base.ExecuteSql(sSql);

                //sSql = string.Format("select DelPatRecId,DelPatientNo,(select (FNAME + ' '+ LNAME) from PATIENT where PATIENTNO=DelPatientNo) as DelPatientName,DelStatus,DelBatchId,DelAddress,PatientAddress,FacilityAddress,DelInstructions,ReqDelDate,DateDelivered, DelAcceptedBy, TotalCopay, TotalCopayCollected, NonDelReason, Notes, Priority, Driver, DeliveryDestination, DriverInitials, TRANSNO,DeliveryNote, NonDeliveryNote, DeliveryMethod, ShipService_CD, Ship_TrackingNo, IsExcluded, ShipCustomCD,Reconciled,CopayCollectedPOS from Delivery_Order where DelPatRecId = '{0}'", delPatRecId);
                sSql = string.Format("select Do.DelPatRecId,Do.DelPatientNo,(PT.FNAME + ' '+ PT.LNAME) as DelPatientName,Do.DelStatus, Do.DelBatchId,Do.DelAddress,Do.PatientAddress,Do.FacilityAddress,Do.DelInstructions, " +
                " Do.ReqDelDate, Do.DateDelivered, Do.DelAcceptedBy, Do.TotalCopay, Do.TotalCopayCollected, Do.NonDelReason, Do.Notes, Do.Priority, Do.Driver, Do.DeliveryDestination, " +
                " Do.DriverInitials, Do.TRANSNO, Do.DeliveryNote, Do.NonDeliveryNote, Do.DeliveryMethod, Do.ShipService_CD, Do.Ship_TrackingNo, IsExcluded, ShipCustomCD, Reconciled, CopayCollectedPOS " +
                " from Delivery_Order DO left join PATIENT PT on PT.PATIENTNO = Do.DelPatientNo where DelPatRecId = '{0}'", delPatRecId);
                this.sTableName = "Delivery_Order";

                logger.Trace("UpdateBatchDeliveryDetailPaymentStatus(DataTable dtBatchDeliveryData) 4 - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                dtDeliveryOrder = this.GetRecs(sSql).Tables[0];

                batchId = (dtDeliveryOrder.AsEnumerable().Select(p => p.Field<int>("DelBatchId"))).FirstOrDefault(); // Get batch Id for update posstamped

                sSql = "UPDATE Delivery_Batch SET POSStamped = 1 where DelBatchRecId in(" + batchId + ")";

                logger.Trace("UpdateBatchDeliveryDetailPaymentStatus(DataTable dtBatchDeliveryData) 5 - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                base.ExecuteSql(sSql);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }


        // get all batch detail for search with date filter
        public DataTable GetBatchDeliveryData(DateTime dFillDateFrom, DateTime dFillDateTo)
        {
            string sSql = string.Empty;
            try
            {
                sSql = "select DB.DelBatchRecId, DB.BatchNo, DU.UserName  as DelUserId, DB.CreationDate, DB.BatchStatus, DB.BatchType, DB.PatientCount, DB.RxCount, DB.TotalCopay , " +
                " DB.TotalCopayCollected, DB.CompletionDate, DB.Notes,DB.FacilityCD, DB.DeviceDelivery, DB.BatchCategory, DB.ShipService_CD, DB.ShipCustomCD, IntakeBatchID " +
                " from Delivery_Batch DB left join DELIVERY_User DU on DU.DelUserRecId = DB.DelUserId " +
                " where EXISTS(Select 1 from Delivery_Order DO where DB.DelBatchRecId = DO.DelBatchId and DO.Reconciled = 0) and ";

                if (dFillDateFrom != null && dFillDateTo != null)
                    // sSql = sSql + " CreationDate between cast('" + dFillDateFrom.ToString("d") + "' as datetime ) and cast('" + dFillDateTo.ToString("d") + "' as datetime)";
                    sSql = sSql + " Convert(datetime, CreationDate, 109) between convert(datetime, cast('" + dFillDateFrom + "' as datetime) ,113) and convert(datetime, cast('" + dFillDateTo + "' as datetime) ,113)";

                this.sTableName = "Delivery_Batch";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }

        }
        #endregion

        #region PRIMEPOS-2760 18-Nov-2019 JY Added
        public bool RxExistsInPrimeRxDb(string sRxNo, string sRefNo)
        {
            string sSql = "SELECT * FROM CLAIMS WHERE RXNO = " + sRxNo + " AND NREFILL = " + sRefNo;
            this.sTableName = "CLAIMS";

            logger.Trace("RxNotExistsInPrimeRxDb(string sRxNo, string sRefNo) - SQL - " + sSql);
            DataTable dt = base.GetRecs(sSql).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region PRIMEPOS-2761
        public DataSet FetchRxDetails(string RxNo, string Nrefill)
        {
            DataSet ds = new DataSet();
            try
            {
                //string sSql = "SELECT PATIENTNO,PATTYPE,RXNO,STATUS,NDC,DRGNAME,DATEF,NREFILL,AMOUNT,COPAY,TOTAMT,PHARMACIST,STATION_ID,COPAYPAID,PICKEDUP,PICKUPDATE,PICKUPTIME,DELIVERY,PICKUPPOS FROM CLAIMS where RXNO='" + RxNo + "' and NREFILL = '" + Nrefill + "' ";  //PRIMEPOS-2897 14-Sep-2020 JY Commented
                string sSql = "SELECT c.PATIENTNO, c.PATTYPE, c.RXNO, c.STATUS, c.NDC," +
                    " d.DRGNAME," +
                    " c.DATEF, c.NREFILL, c.AMOUNT, c.COPAY, c.TOTAMT, c.PHARMACIST, c.STATION_ID, c.COPAYPAID, c.PICKEDUP, c.PICKUPDATE, c.PICKUPTIME, c.DELIVERY, c.PICKUPPOS " +
                    " FROM CLAIMS c INNER JOIN DRUG d ON c.NDC = d.DRGNDC " +
                    " WHERE c.RXNO = '" + RxNo + "' AND c.NREFILL = '" + Nrefill + "' ";

                sSql += " SELECT do.DelPatRecId,do.DelPatientNo,do.DelStatus,do.DelBatchId,do.TotalCopay,do.TotalCopayCollected,do.CopayCollectedPOS,do.Reconciled from DELIVERY_ORDER do left join DELIVERY_DETAIL dd on do.DelPatRecId=dd.DelPatRecId where dd.RXNO = '" + RxNo + "' and dd.RefillNo ='" + Nrefill + "' ";
                sSql += " SELECT dd.DelDetRecId,dd.DelPatRecId,dd.RxNo,dd.RefillNo,dd.DelStatus,dd.Copay,dd.CopayCollected,dd.TransID,dd.CopayCollectedPOS from DELIVERY_DETAIL dd where dd.RXNO='" + RxNo + "' and dd.RefillNo ='" + Nrefill + "' ";

                this.sTableName = "Claims";
                ds = this.GetRecs(sSql);
                if (ds != null)
                {
                    ds.Tables[0].TableName = "Claims";
                    ds.Tables[1].TableName = "DELIVERY_ORDER";
                    ds.Tables[2].TableName = "DELIVERY_DETAIL";
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, " - FetchRxDetails(string RxNo, string Nrefill)"); //PRIMEPOS-2848 21-May-2020 JY Added
                throw ex;
            }
            return ds;
        }
        public bool RollbackRxUpdate(DataSet dsRxData)
        {
            bool isRollBackSuccess = false;
            string sSql = string.Empty;
            try
            {
                if (dsRxData.Tables.Count > 0)
                {
                    if (dsRxData.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsRxData.Tables["Claims"].Rows.Count; i++)
                        {
                            sSql = "Update Claims Set PICKEDUP = '" + dsRxData.Tables["Claims"].Rows[i]["PICKEDUP"].ToString() + "', PickUpDate = '" + dsRxData.Tables["Claims"].Rows[i]["PickUpDate"].ToString() + "' , pickuppos = '" + dsRxData.Tables["Claims"].Rows[i]["pickuppos"].ToString() + "' , COPAYPAID = '" + dsRxData.Tables["Claims"].Rows[i]["COPAYPAID"].ToString() + "' WHERE RxNO = '" + dsRxData.Tables["Claims"].Rows[i]["RxNO"].ToString() + "'  and  NREFILL = '" + dsRxData.Tables["Claims"].Rows[i]["NREFILL"].ToString() + "'";

                            logger.Trace("RollbackRxUpdate(DataSet dsRxData) 1 - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                            isRollBackSuccess = base.ExecuteSql(sSql);
                        }

                        for (int i = 0; i < dsRxData.Tables["DELIVERY_ORDER"].Rows.Count; i++)
                        {
                            //sSql = "update DELIVERY_ORDER set CopayCollectedPOS='" + dsRxData.Tables["DELIVERY_ORDER"].Rows[i]["CopayCollectedPOS"].ToString() + "' , TotalCopay='" + dsRxData.Tables["DELIVERY_ORDER"].Rows[i]["TotalCopay"].ToString() + "', Reconciled='" + dsRxData.Tables["DELIVERY_ORDER"].Rows[i]["Reconciled"].ToString() + "' from  DELIVERY_ORDER do left join  DELIVERY_DETAIL dd on do.DelPatRecId = dd.DelPatRecId where RxNo='" + dsRxData.Tables["DELIVERY_ORDER"].Rows[i]["RxNo"].ToString() + "' and RefillNo='" + dsRxData.Tables["DELIVERY_ORDER"].Rows[i]["RefillNo"].ToString() + "'";
                            sSql = "UPDATE DO SET DO.CopayCollectedPOS = '" + dsRxData.Tables["DELIVERY_ORDER"].Rows[i]["CopayCollectedPOS"].ToString() +
                                "',DO.TotalCopay = '" + dsRxData.Tables["DELIVERY_ORDER"].Rows[i]["TotalCopay"].ToString() + "',DO.Reconciled = '" + dsRxData.Tables["DELIVERY_ORDER"].Rows[i]["Reconciled"].ToString()
                                + "' FROM DELIVERY_ORDER DO LEFT JOIN DELIVERY_DETAIL DD ON DO.DelPatRecId = DD.DelPatRecId WHERE DD.RxNo = '" + dsRxData.Tables["DELIVERY_DETAIL"].Rows[i]["RxNo"].ToString() + "' AND DD.RefillNo = '" + dsRxData.Tables["DELIVERY_DETAIL"].Rows[i]["RefillNo"].ToString() + "'";

                            logger.Trace("RollbackRxUpdate(DataSet dsRxData) 2 - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                            isRollBackSuccess = base.ExecuteSql(sSql);
                        }

                        for (int i = 0; i < dsRxData.Tables["DELIVERY_DETAIL"].Rows.Count; i++)
                        {
                            sSql = "update DELIVERY_DETAIL set Copay='" + dsRxData.Tables["DELIVERY_DETAIL"].Rows[i]["Copay"].ToString() + "' , CopayCollectedPOS='" + dsRxData.Tables["DELIVERY_DETAIL"].Rows[i]["CopayCollectedPOS"].ToString() + "' where RxNo='" + dsRxData.Tables["DELIVERY_DETAIL"].Rows[i]["RxNo"].ToString() + "' and RefillNo='" + dsRxData.Tables["DELIVERY_DETAIL"].Rows[i]["RefillNo"].ToString() + "'";

                            logger.Trace("RollbackRxUpdate(DataSet dsRxData) 3 - SQL - " + sSql);    //PRIMEPOS-3014 13-Oct-2021 JY Added
                            isRollBackSuccess = base.ExecuteSql(sSql);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                isRollBackSuccess = false;
                throw Ex;
            }
            return isRollBackSuccess;
        }
        #endregion

        #region PRIMEESC-36 24-Jul-2020 JY Added
        public DataTable GetAutoUpdateServiceURL()
        {
            string sSql = "SELECT D.FieldValue FROM SETTING S INNER JOIN SETTINGDETAIL D ON S.ID = D.SettingID WHERE(S.ID = 3 AND (FIELDNAME = 'AutoUpdateUrl'))";
            this.sTableName = "SETTING";

            logger.Trace("GetAutoUpdateServiceURL() - SQL - " + sSql);
            return this.GetRecs(sSql).Tables[0];
        }
        #endregion

        
        #region PRIMEPOS-3207
        public DataTable GetHyphenSettings()
        {
            string sSql = "SELECT D.FieldName,D.FieldValue FROM SETTING S INNER JOIN SETTINGDETAIL D ON S.ID = D.SettingID WHERE(S.ID = 62 And S.Active =1)";
            this.sTableName = "HyphenSetting";
            return this.GetRecs(sSql).Tables[0];
        }

        public DataTable GetPatAllIns(string PatientNo)
        {
            string sSql = "SELECT INS.PATIENTNO, INS.IC_CODE, INS.INS_ID, INS.GROUPNO, INS.BIN_NO, INS.PC, INS.CH_FNAME, INS.CH_LNAME, INS.CH_DOB,  INS.RELATION, INS.PERSON_CD, INS.CRD_EFFECTIVEDT, INS.CRD_EXPDT, INS.IDType, INS.IsPrimary, B.BILL_ORDER AS BillingSequence, INS.IC_NAME,INS.Active  FROM (SELECT P.PATIENTNO, I.IC_CODE AS IC_CODE, P.MEDNO AS INS_ID, P.GROUPNO AS GROUPNO, I.BIN_NO AS BIN_NO, I.MAG_CODE AS PC, P.CH_FNAME , P.CH_LNAME, P.CH_DOB, P.RELATION, P.PERSON_CD, P.CRD_EFFECTIVEDT,P.CRD_EXPDT, P.MEDNO AS IDType, 'Y' AS IsPrimary,I.IC_NAME, ISNULL(I.Active, 'N') Active   FROM Patient P INNER JOIN INSCAR I on P.PATTYPE = I.IC_CODE  WHERE P.PATIENTNO = " + PatientNo + " union SELECT P.PATIENTNO, I.IC_CODE AS IC_CODE, P.INS_ID AS INS_ID, P.GROUPNO AS GROUPNO, I.BIN_NO AS BIN_NO, I.MAG_CODE AS PC, P.CH_FNAME , P.CH_LNAME, P.CH_DOB, P.RELATION, P.PERSON_CD, P.CRD_EFFECTIVEDT, P.CRD_EXPDT, '' AS IDType,  'N' AS IsPrimary,I.IC_NAME,ISNULL(P.Active, 'N') Active  FROM INSCAR I INNER JOIN PATINS P on I.IC_CODE = P.INS_CD  WHERE P.PATIENTNO = " + PatientNo + " ) INS LEFT JOIN PATMINSB B ON INS.IC_CODE = B.INS_CD AND INS.PATIENTNO = B.PATIENTNO";
            this.sTableName = "PatientAllIns";
            return this.GetRecs(sSql).Tables[0];
        }

        #endregion
        #region PRIMEPOS-1637 26-May-2021 JY Added
        public DataTable GetDrugClass(string sRxNo, string sRefill)
        {
            string sSql = string.Format(@"SELECT ISNULL(b.CLASS,0) AS Class FROM CLAIMS a INNER JOIN DRUG b ON a.NDC = b.DRGNDC WHERE a.RXNO = '{0}' AND NREFILL = '{1}'", sRxNo, sRefill);
            this.sTableName = "DRUG";

            logger.Trace("GetDrugClass(string sRxNo, string sRefill) - SQL - " + sSql);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
            return this.GetRecs(sSql).Tables[0];
        }
        #endregion

        #region PRIMEPOS-3060 01-Mar-2022 JY Added
        public DataTable GetInvalidSign(string sFilter)
        {
            //sFilter = "FFFFFFFF%FFFFFFFF";
            this.sTableName = "INSSIGTRANS";
            string sSql = "SELECT COUNT(TRANSNO) FROM INSSIGTRANS WITH(NOLOCK) WHERE CONVERT(VARCHAR(max), BinarySign, 2) LIKE '" + sFilter + "'";
            return base.GetRecs(sSql).Tables[0];
        }

        public DataTable GetInvalidSign(string sFilter, int nTop)
        {
            //sFilter = "FFFFFFFF%FFFFFFFF";
            this.sTableName = "INSSIGTRANS";
            string sSql = "select TOP " + nTop + " TRANSNO, BinarySign FROM INSSIGTRANS WHERE CONVERT(VARCHAR(max), BinarySign, 2) LIKE '" + sFilter + "' ORDER BY TRANSNO DESC";
            return base.GetRecs(sSql).Tables[0];
        }

        public void IndexReOrganizeAndRebuild()
        {
            string strSQL = string.Empty;
            try
            {
                strSQL = "ALTER INDEX ALL ON INSSIGTRANS REORGANIZE";
                base.ExecuteSql(strSQL);
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "PharmaSqlDB==>IndexReOrganizeAndRebuild(): An Error Occured while executing REORGANIZE"); //PRIMEPOS-3211
            }
            try
            {
                strSQL = "ALTER INDEX ALL ON INSSIGTRANS REBUILD";
                base.ExecuteSql(strSQL);
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "PharmaSqlDB==>IndexReOrganizeAndRebuild(): An Error Occured while executing REBUILD"); //PRIMEPOS-3211
            }
        }

        public void UpdateInsSigTrans(long TRANSNO, byte[] bBinarySign)
        {
            try
            {
                this.sTableName = "INSSIGTRANS";
                DataSet dt = this.GetRecs("Select * From INSSIGTRANS Where TRANSNO = " + TRANSNO);
                dt.Tables["INSSIGTRANS"].Rows[0]["BINARYSIGN"] = bBinarySign;
                this.Save(dt);
            }
            catch (Exception ex)
            {
                //throw ex;
                logger.Error(ex, "PharmaSqlDB==>UpdateInsSigTrans(): An Exception Occured"); //PRIMEPOS-3211
            }
        }
        #endregion

        #region PRIMEPOS-2651 07-Mar-2022 JY Added
        public Boolean IsDrugRefrigerated(string sNDC11)
        {
            Boolean bstatus = false;
            try
            {
                object obj;
                string sSql = "SELECT COUNT(D.NDC11) FROM " + db.GSDDCatalog + ".dbo.UNIQUENDCLIST D INNER JOIN " + db.GSDDCatalog + ".dbo.PRODUCT_STORAGE_LOCATION PSL ON D.PRODUCTID = PSL.PRODUCTID INNER JOIN " + db.GSDDCatalog + ".dbo.STORAGE_LOCATION SL ON PSL.STORAGELOCATIONID = SL.STORAGELOCATIONID " +
                            " WHERE SL.StorageLocationID IN (1,3) AND D.NDC11 = '" + sNDC11 + "'";
                obj = db.DataReaderScalar(sSql);
                db.Close();

                if (obj != null)
                    bstatus = Convert.ToInt32(obj.ToString()) > 0 ? true : false;
                else
                    return false;
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "PharmaSqlDB==>IsDrugRefrigerated(): An Exception Occured"); //PRIMEPOS-3211
                bstatus = false;
            }
            return bstatus;
        }
        #endregion

        #region PRIMEPOS-3085 05-Apr-2022 JY Added
        public Boolean UpdateInvalidSignForMultiplePatientsTrans(string TransData, string PatientNo, byte[] bBinarySign)
        {
            bool bstatus = false;
            try
            {
                DataSet ds = new DataSet();
                DataTable dtInsSigTrans = new DataTable();
                string sSql = string.Empty;
                this.sTableName = "INSSIGTRANS";
                sSql = "SELECT TRANSNO FROM INSSIGTRANS WITH(NOLOCK) WHERE LTRIM(RTRIM(CAST(TRANSDATA AS VARCHAR(MAX)))) = '" + TransData.Trim() + "' AND PATIENTNO = '" + PatientNo + "'";
                dtInsSigTrans = base.GetRecs(sSql).Tables[0];
                if (dtInsSigTrans != null && dtInsSigTrans.Rows.Count > 0)
                {
                    this.sTableName = "INSSIGTRANS";
                    ds = this.GetRecs("SELECT * FROM INSSIGTRANS WITH(NOLOCK) WHERE TRANSNO = " + dtInsSigTrans.Rows[0][0].ToString());
                    ds.Tables["INSSIGTRANS"].Rows[0]["BINARYSIGN"] = bBinarySign;
                    this.Save(ds);
                }
                bstatus = true;
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "PharmaSqlDB==>UpdateInvalidSignForMultiplePatientsTrans(): An Exception Occured"); //PRIMEPOS-3211
                bstatus = false;
            }
            return bstatus;
        }
        #endregion

        #region PRIMEPOS-3088 28-Apr-2022 JY Added
        public DataTable UpdateInsSigTrans(DataTable dt)
        {
            try
            {
                string sSql = string.Empty;
                object obj = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sSql = "SELECT TRANSNO FROM INSSIGTRANS WITH (NOLOCK) WHERE CAST(TRANSDATA AS VARCHAR(MAX)) = '" + dt.Rows[i]["TransData"] + "'";
                    obj = db.DataReaderScalar(sSql);
                    db.Close();
                    if (obj != null && obj.ToString().Length > 0)
                    {
                        dt.Rows[i]["IsVerified"] = 1;
                    }
                    else
                    {
                        try
                        {
                            this.sTableName = "INSSIGTRANS";
                            DataSet dtInsSigTrans = this.GetRecs("SELECT * FROM INSSIGTRANS WITH (NOLOCK) WHERE TRANSNO = 0");
                            DataRow nRow = dtInsSigTrans.Tables["INSSIGTRANS"].NewRow();
                            dtInsSigTrans.Tables["INSSIGTRANS"].Rows.Add(nRow);
                            dtInsSigTrans.Tables["INSSIGTRANS"].Rows[0]["PATIENTNO"] = dt.Rows[i]["PatientNo"];
                            dtInsSigTrans.Tables["INSSIGTRANS"].Rows[0]["INSTYPE"] = dt.Rows[i]["InsType"];
                            dtInsSigTrans.Tables["INSSIGTRANS"].Rows[0]["TRANSDATA"] = dt.Rows[i]["TransData"];
                            dtInsSigTrans.Tables["INSSIGTRANS"].Rows[0]["TRANSDATE"] = dt.Rows[i]["TransDate"];
                            dtInsSigTrans.Tables["INSSIGTRANS"].Rows[0]["TRANSSIGDATA"] = dt.Rows[i]["TransSigData"];
                            dtInsSigTrans.Tables["INSSIGTRANS"].Rows[0]["COUNSELINGREQ"] = dt.Rows[i]["CounselingReq"];
                            dtInsSigTrans.Tables["INSSIGTRANS"].Rows[0]["SIGTYPE"] = dt.Rows[i]["SigType"];
                            dtInsSigTrans.Tables["INSSIGTRANS"].Rows[0]["BINARYSIGN"] = dt.Rows[i]["BinarySign"];
                            this.Save(dtInsSigTrans);
                            sSql = "SELECT IDENT_CURRENT('INSSIGTRANS')";
                            DataTable dtTransNo = base.GetRecs(sSql).Tables[0];
                            if (dtTransNo != null && dtTransNo.Rows.Count > 0)
                            {
                                string[] arrRxNos = dt.Rows[i]["TransData"].ToString().Split(',');
                                sSql = "";
                                for (int j = 0; j < arrRxNos.Length; j++)
                                {
                                    if (arrRxNos[j].Length > 0)
                                    {
                                        string[] arrRxNoRefillNo = arrRxNos[j].Split('-');
                                        if (arrRxNoRefillNo.Length > 1)
                                        {
                                            if (sSql == "")
                                                sSql = "(" + dtTransNo.Rows[0][0].ToString() + "," + arrRxNoRefillNo[0] + "," + arrRxNoRefillNo[1] + ")";
                                            else
                                                sSql += ", (" + dtTransNo.Rows[0][0].ToString() + "," + arrRxNoRefillNo[0] + "," + arrRxNoRefillNo[1] + ")";
                                        }
                                    }
                                }
                                if (sSql != "")
                                {
                                    sSql = "INSERT INTO TRANSDET (TRANSNO, RXNO, REFILLNO) VALUES" + sSql + ";";
                                    base.ExecuteSql(sSql);
                                }
                            }
                            dt.Rows[i]["IsVerified"] = 1;
                        }
                        catch(Exception Ex)
                        {
                            logger.Error(Ex, "PharmaSqlDB==>UpdateInsSigTrans(): An Exception Occured");
                            //dt.Rows[i]["IsVerified"] = 0;
                        }
                    }
                }
                dt.AcceptChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "PharmaSqlDB==>UpdateInsSigTrans(): An Exception Occured");
            }
            return dt;
        }
        #endregion

        #region PRIMEPOS-3091 06-May-2022 JY Added
        public DataTable GetPOSTransactionRxDetail(DataTable dt)
        {
            try
            {
                string sSql = string.Empty;
                DataTable dtPrimeRx = null;
                this.sTableName = "POSTransactionRxDetail";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sSql = "SELECT a.PATIENTNO, a.BillType AS InsType, a.NDC, a.DATEF AS FilledDate, a.PatType, b.EZCAP FROM CLAIMS a WITH(NOLOCK)" +
                        " INNER JOIN PATIENT b WITH(NOLOCK) ON a.PATIENTNO = b.PATIENTNO" +
                        " WHERE a.RXNO = " + dt.Rows[i]["RxNo"] + " AND a.NREFILL = " + dt.Rows[i]["nRefill"]; //PRIMEPOS-3243 Added WITH(NOLOCK)
                    dtPrimeRx = base.GetRecs(sSql).Tables[0];
                    if (dtPrimeRx != null && dtPrimeRx.Rows.Count > 0)
                    {
                        dt.Rows[i]["PATIENTNO"] = dtPrimeRx.Rows[0]["PATIENTNO"];
                        dt.Rows[i]["InsType"] = dtPrimeRx.Rows[0]["InsType"].ToString();
                        dt.Rows[i]["NDC"] = dtPrimeRx.Rows[0]["NDC"].ToString();
                        dt.Rows[i]["FilledDate"] = Convert.ToDateTime(dtPrimeRx.Rows[0]["FilledDate"]);
                        dt.Rows[i]["PatType"] = dtPrimeRx.Rows[0]["PatType"].ToString();
                        dt.Rows[i]["EZCAP"] = dtPrimeRx.Rows[0]["EZCAP"].ToString();
                        dt.Rows[i]["Modified"] = 1;
                    }
                }
                dt.AcceptChanges();
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "PharmaSqlDB==>GetPOSTransactionRxDetail(): An Exception Occured"); //PRIMEPOS-3211
            }
            return dt;
        }
        #endregion

        #region PRIMEPOS-3094 19-May-2022 JY Added
        public string UpdateMissingTransDet(string TransNo)
        {
            string retTransNo = TransNo;
            try
            {
                string sSql = string.Empty;
                this.sTableName = "INSSIGTRANS";
                DataSet dtInsSigTrans = this.GetRecs("SELECT MIN(TRANSNO) AS MinNo, MAX(TRANSNO) AS MaxNo FROM INSSIGTRANS WITH(NOLOCK)"); //PRIMEPOS-3243 Added WITH(NOLOCK)
                if (dtInsSigTrans != null && dtInsSigTrans.Tables.Count > 0 && dtInsSigTrans.Tables[0].Rows.Count > 0)
                {
                    int StartNo = Convert.ToInt32(dtInsSigTrans.Tables[0].Rows[0][0]);
                    int EndNo = Convert.ToInt32(dtInsSigTrans.Tables[0].Rows[0][1]);
                    if (Convert.ToInt32(TransNo) >= StartNo)
                        StartNo = Convert.ToInt32(TransNo);

                    while (StartNo <= EndNo)
                    {
                        sSql = "INSERT INTO TRANSDET (TRANSNO,RXNO,REFILLNO)" +
                                " SELECT DISTINCT a.TRANSNO, a.RXNO, a.REFILLNO FROM" +
                                        " (SELECT a.TRANSNO, CASE WHEN CHARINDEX('-',value)-1 > 0 THEN SUBSTRING(value,1, CHARINDEX('-', value) - 1) ELSE ISNULL(value,'0') END AS RXNO," +
                                            " CASE WHEN CHARINDEX('-', REPLACE(value, ',', '')) - 1 > 0 THEN SUBSTRING(REPLACE(value,',',''),CHARINDEX('-', REPLACE(value, ',', '')) + 1, LEN(REPLACE(value, ',', ''))) ELSE '0' END AS REFILLNO" +
                                                            " FROM INSSIGTRANS a" +
                                                " CROSS APPLY fn_SplitBy(CAST(a.TRANSDATA AS varchar(max)), ',')) a" +
                                    " LEFT JOIN TRANSDET b ON a.TRANSNO = b.TRANSNO AND a.RXNO = b.RXNO AND a.REFILLNO = b.REFILLNO" +
                                    " WHERE b.TRANSNO IS NULL AND a.TRANSNO >= " + StartNo + " AND a.TRANSNO <= " + (StartNo + 1000) +
                                    " ORDER BY a.TRANSNO ";
                        base.ExecuteSql(sSql);
                        StartNo += 1000;
                        if (StartNo <= EndNo)
                            retTransNo = StartNo.ToString();
                        else
                            retTransNo = EndNo.ToString();
                    }
                }
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "PharmaSqlDB==>UpdateMissingTransDet(): An Exception Occured"); //PRIMEPOS-3211
            }
            return retTransNo;
        }

        public void UpdateBlankSignWithSamePatientsSign()
        {
            try
            {
                string sSql = string.Empty;
                this.sTableName = "INSSIGTRANS";
                sSql = "SELECT DISTINCT PATIENTNO INTO #TEMPPATIENTNO FROM INSSIGTRANS WITH (NOLOCK) WHERE ISNULL(CAST(TRANSSIGDATA AS VARCHAR(MAX)), '') = '' AND ISNULL(CAST(BINARYSIGN AS VARCHAR(MAX)), '') = '' and isnull (PATIENTNO,0) <> 0;" +
                    "UPDATE a SET a.BinarySign = b.BinarySign FROM inssigtrans a WITH(NOLOCK)" +
                    " INNER JOIN(SELECT ROW_NUMBER() OVER (PARTITION BY bb.PATIENTNO ORDER BY bb.TRANSNO DESC) rNum, bb.TRANSNO, bb.PATIENTNO, bb.BinarySign FROM inssigtrans bb WITH(NOLOCK)" +
                                                            " WHERE ISNULL(CAST(bb.BINARYSIGN AS VARCHAR(MAX)), '') <> '' and exists(Select 1 from #TEMPPATIENTNO where bb.PATIENTNO=#tempPATIENTNO.PATIENTNO)) b ON a.PATIENTNO = b.PATIENTNO and b.rNum = 1" +
                    " WHERE ISNULL(CAST(a.TRANSSIGDATA AS VARCHAR(MAX)),'') = '' AND ISNULL(CAST(a.BinarySign AS VARCHAR(MAX)),'') = ''"; //PRIMEPOS-3143 Added WITH(NOLOCK)
                base.ExecuteSql(sSql);
            }
            catch (Exception Ex)
            {
                logger.Error(Ex, "PharmaSqlDB==>UpdateBlankSignWithSamePatientsSign(): An Exception Occured"); //PRIMEPOS-3211
            }
        }
        #endregion

        #region PRIMEPOS-3157 28-Nov-2022 JY Added
        public DataTable GetDocSubCat(int CategoryId)
        {
            this.sTableName = "DM_DocumentSubCat";
            string sSql = "SELECT SubCategoryId, Description FROM DM_DocumentSubCat WHERE CategoryId = " + CategoryId + " ORDER BY Description";
            return base.GetRecs(sSql).Tables[0];
        }

        public DataTable GetDocuments(int PATIENTNO, int CategoryId, string SubCategoryIds)
        {
            this.sTableName = "DM_Document";
            string sSql = "SELECT a.DocumentId, a.ScanDate, ISNULL(a.DocTypeExt,'') AS DocTypeExt FROM DM_Document a INNER JOIN DM_DocumentRx b ON a.DocumentId = b.DocumentId" +
                " WHERE a.CategoryId = " + CategoryId + " AND a.SubCategoryId IN (" + SubCategoryIds + ") AND b.ReferenceNo = '" + PATIENTNO + "' ORDER BY a.SCANDATE DESC";
            return base.GetRecs(sSql).Tables[0];
        }

        public int GetDocumentLocation()
        {
            int nDocumentLocation = 0;
            try
            {
                this.sTableName = "SettingDetail";
                string sSql = "SELECT FieldValue FROM SettingDetail WHERE SettingID = 44 AND FieldName = 'DocumentLocation'";
                DataSet ds = this.GetRecs(sSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    nDocumentLocation = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetDocumentLocation()");
            }
            return nDocumentLocation;
        }

        public string GetDocumentPhysicalPath()
        {
            string strPath = string.Empty;
            try
            {
                this.sTableName = "CONSTANT";
                string sSql = "SELECT ISNULL(IMG_STORE_PATH,'') AS IMG_STORE_PATH FROM CONSTANT";
                DataSet ds = this.GetRecs(sSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0][0] != null)
                        strPath = ds.Tables[0].Rows[0][0].ToString().Trim();
                }
            }
            catch (Exception Ex)
            {
                logger.Fatal(Ex, "GetDocumentPhysicalPath()");
            }
            return strPath;
        }

        public byte[] GetDocumentFromdb(string document)
        {
            byte[] doc = null;
            try
            {
                this.sTableName = "DM_DocumentImageFiles";
                string sSql = $@"SELECT Documents FROM DM_DocumentImageFiles WHERE DocumentId = '" + document + "'";
                DataSet ds = this.GetRecs(sSql);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    doc = (Byte[])ds.Tables[0].Rows[0][0];
                }
            }
            catch (Exception Ex)
            {
                //throw;
                logger.Fatal(Ex, "GetDocument(string document)");
            }
            return doc;
        }
        #endregion

        #region PRIMEPOS-3192
        public DataTable GetPendingSignatureForAutoRefillConsent(string sPatientNo, string rxs)
        {
            string sSql = string.Empty;
            try
            {
                sSql = string.Format("SELECT PC.PatientConsentID, PC.PatientNo, PC.LNAME +', '+PC.FNAME as [PatientName], PC.RxNo, PC.DrugNDC ,D.SPECIFICPRODUCTID , D.DrgName, pc.ConsentCaptureDate, pc.ConsentEndDate,'Yes' as [ConsentStatus],'Yes' as [SignPending], PC.IsNewRx FROM (SELECT * FROM ( SELECT RANK() OVER ( PARTITION BY prc.RxNo ORDER BY pc.ConsentCaptureDate DESC) AS RowNo, prc.PatientConsentID, P.LNAME, P.FNAME,pc.PatientNo,pc.ConsentCaptureDate,pc.ConsentEndDate,(CASE when isnull(prc.ReplacedByRxNo,'')='' then prc.RxNo else prc.ReplacedByRxNo END) AS RxNo, prc.DrugNDC,dbo.fn_GetAutoRefillConsentRequired(pc.PATIENTNO, (CASE when isnull(prc.ReplacedByRxNo,'')='' then prc.RxNo else prc.ReplacedByRxNo END)) AS AutoRefillConsentRequired, Cast(0 AS BIT) IsNewRx FROM Patient_consent pc WITH(NOLOCK) INNER JOIN Patient_PrescriptionConsent prc WITH(NOLOCK) ON prc.PatientConsentID = pc.ID INNER JOIN PATIENT P  WITH (NOLOCK) ON P.PATIENTNO = pc.PatientNo WHERE isnull(pc.SignaturePending, 0) = 1 AND pc.PatientNo IN ( SELECT Value FROM fnSplit('" + sPatientNo + "', ','))) PC WHERE PC.RowNo = 1) PC INNER JOIN DRUG D WITH(NOLOCK) ON PC.DrugNDC = d.DRGNDC UNION SELECT NULL AS PatientConsentID, P.PATIENTNO, P.LNAME +', '+P.FNAME as [PatientName], CON.RXNO, CON.DRGNDC ,D.SPECIFICPRODUCTID , D.DrgName, NULL as ConsentCaptureDate, NULL as ConsentEndDate,'' as [ConsentStatus],'' as [SignPending], Cast(1 AS BIT) IsNewRx FROM(SELECT RANK() OVER ( PARTITION BY c.RxNo ORDER BY c.NREFILL DESC) AS RowNo, C.PATIENTNO,C.RXNO,C.NDC AS DRGNDC,dbo.fn_GetAutoRefillConsentRequired(c.PATIENTNO, c.RXNO) AS AutoRefillConsentRequired FROM CLAIMS C  WITH(NOLOCK) WHERE C.PatientNo IN ( SELECT Value FROM fnSplit('" + sPatientNo + "', ','))AND C.RxNo IN (SELECT Value FROM fnSplit('" + rxs + "', ','))) AS CON INNER JOIN DRUG D  WITH(NOLOCK) ON CON.DRGNDC = d.DRGNDC INNER JOIN PATIENT P WITH (NOLOCK) ON CON.PATIENTNO = P.PATIENTNO WHERE con.AutoRefillConsentRequired= 'Y' AND con.RowNo = 1");

                this.sTableName = "Patient_Consent";
                return this.GetRecs(sSql).Tables[0];
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>GetPendingSignatureForAutoRefillConsent(): An Error Occured while executing sql ");
                throw;
            }
        }
        public void SavePatientPrescriptionConsent(long PatientConsentID, long RxNo, string DrugNDC, int ConsentStatusID, int specificProductId)
        {
            try
            {
                string sSql = "Insert into Patient_PrescriptionConsent(PatientConsentID,RxNo,DrugNDC,SpecificProductId,ConsentStatusID) ";
                sSql += " VALUES( " + PatientConsentID + ", " + RxNo + ", '" + DrugNDC + "', " + specificProductId + ", " + ConsentStatusID + " )";
                base.ExecuteSql(sSql);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>SavePatientPrescriptionConsent(): An Error Occured while executing sql ");
                throw;
            }
        }
        public void UpdatePatientConsentSignature(long PatientConsentID, long PatientNo, byte[] SignatureData)
        {
            try
            {
                this.sTableName = "Patient_Consent";
                DataSet dt = this.GetRecs("Select * From Patient_Consent Where ID = " + PatientConsentID + " AND PatientNo = " + PatientNo);
                dt.Tables["Patient_Consent"].Rows[0]["SignatureData"] = SignatureData;
                dt.Tables["Patient_Consent"].Rows[0]["SignaturePending"] = 0;
                dt.Tables["Patient_Consent"].Rows[0]["DateModified"] = DateTime.Now;
                dt.Tables["Patient_Consent"].Rows[0]["ModifiedBy"] = "POS";

                this.Save(dt);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>UpdatePatientConsentSignature(): An Exception Occured while Update PatientConsent Signature");
                throw;
            }

        }
        public string checkPatientPrescriptionRecord(string RxNo)
        {
            string sSql = string.Empty;
            try
            {
                sSql = "Select 1 AS IsExist from Patient_PrescriptionConsent as prc where RxNo=" + RxNo + " and Exists(select 1 from Patient_Consent as pc where prc.PatientConsentID = pc.ID and pc.ConsentEndDate > getDate())";
                this.sTableName = "PatPrescriptionRecord";
                logger.Trace("checkPatientPrescriptionRecord(string RxNo) - SQL - " + sSql);
                DataTable dt;
                dt = this.GetRecs(sSql).Tables[0];
                if (dt?.Rows.Count > 0)
                {
                    return dt.Rows[0]["IsExist"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing sql " + sSql);
                throw ex;
            }
        }
        #endregion

        public bool isPrescriptionConsentActive(int consentId, string strCode) //PRIMEPOS-3192N
        {
            bool bstatus = false;
            try
            {
                object obj;
                string sSql = "Select Active from Consent_Type where Code='" + strCode + "' and ConsentSourceID=" + Convert.ToString(consentId);
                obj = db.DataReaderScalar(sSql);
                db.Close();

                if (obj != null)
                    bstatus = Convert.ToBoolean(obj);
                else
                    return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>SavePatientPrescriptionConsent(): An Error Occured while executing sql ");
                throw;
            }
            return bstatus;
        }

        public DataSet GetPhUsers(string strQuery)
        {
            this.sTableName = "PHUSER";
            DataSet ds = null;
            ds = base.GetRecs(strQuery);
            if (ds != null && ds.Tables.Count > 0)
            {
                ds.Tables[0].TableName = "PHUSER";
            }
            return ds;
        }

        public DataTable LoadPatientCounselHistory(int PatientNo)  // TOP (200)
        {
            this.sTableName = "PatientCounselingHistory";
            DataSet oDS = new DataSet();
            DataTable dt = null;

            SqlParameter[] oParam = new SqlParameter[1];
            oParam[0] = new SqlParameter("@PatientNo", SqlDbType.Int);
            oParam[0].Value = PatientNo;

            string sSql = "Select P.PatientNo,P.CounsID,L.RxNo,L.RefillNo,P.isCounsCompleted,P.CounselingResult,P.CounsCompletedDate,P.AddedToQueueDate,P.CounseledByUser,ConductMethod,P.Remark,RecordAddedByUser from PatientCounselingHistory P WITH (NOLOCK), PatientCounselingHistRxList L WITH (NOLOCK) where P.PatientNo=@PatientNo AND P.CounsID = L.CounsID order by CounsID desc ";
            oDS = this.GetRecs(sSql, oParam);
            if (oDS != null && oDS.Tables.Count > 0)
            {
                dt = oDS.Tables[0];
                dt.TableName = "PatientCounselingHistory";
            }
            return dt;
        }

        public bool SavePatCounselHistoryToDB(DataTable dtHist, DataTable dtRXs)
        {
            bool rtn = false;
            bool bAddedToQueueDate = true;
            bool bCounsCompletedDate = true;

            if (dtHist == null || dtHist.Rows.Count == 0)
                return rtn;

            int iCounsID = Convert.ToInt32(dtHist.Rows[0]["CounsID"].ToString());
            if (iCounsID <= 0)
            {
                if (dtRXs == null || dtRXs.Rows.Count == 0)
                    return rtn;
            }

            string sSQL = string.Empty;

            SqlParameter[] oParam = new SqlParameter[12];
            oParam[0] = new SqlParameter("@CounsID", SqlDbType.Int);
            oParam[0].Value = iCounsID;
            oParam[1] = new SqlParameter("@PatientNo", SqlDbType.Int);
            oParam[1].Value = Convert.ToInt32( dtHist.Rows[0]["PatientNo"].ToString() );
            oParam[2] = new SqlParameter("@isCounsCompleted", SqlDbType.Bit);
            oParam[2].Value = Convert.ToBoolean( dtHist.Rows[0]["isCounsCompleted"] );
            oParam[3] = new SqlParameter("@CounselingResult", SqlDbType.VarChar);
            oParam[3].Value = dtHist.Rows[0]["CounselingResult"].ToString();
            oParam[4] = new SqlParameter("@CounsCompletedDate", SqlDbType.DateTime);
            if (dtHist.Rows[0]["CounsCompletedDate"] == DBNull.Value)
            {
                oParam[4].Value = DBNull.Value;
                bCounsCompletedDate = false;
            }
            else
                oParam[4].Value = Convert.ToDateTime( dtHist.Rows[0]["CounsCompletedDate"] );
            oParam[5] = new SqlParameter("@AddedToQueueDate", SqlDbType.DateTime);
            if (dtHist.Rows[0]["AddedToQueueDate"] == DBNull.Value)
            {
                oParam[5].Value = DBNull.Value;
                bAddedToQueueDate = false;
            }
            else
                oParam[5].Value = Convert.ToDateTime(dtHist.Rows[0]["AddedToQueueDate"] );
            oParam[6] = new SqlParameter("@Remark", SqlDbType.VarChar);
            oParam[6].Value = dtHist.Rows[0]["Remark"].ToString();
            oParam[7] = new SqlParameter("@CounseledByUser", SqlDbType.VarChar);
            oParam[7].Value = dtHist.Rows[0]["CounseledByUser"].ToString();
            oParam[8] = new SqlParameter("@RecordAddedByUser", SqlDbType.VarChar);
            oParam[8].Value = dtHist.Rows[0]["RecordAddedByUser"].ToString();
            oParam[9] = new SqlParameter("@ConductMethod", SqlDbType.VarChar);
            oParam[9].Value = dtHist.Rows[0]["ConductMethod"].ToString();
            oParam[10] = new SqlParameter("@RxNo", SqlDbType.BigInt);
            oParam[10].Value = 0;
            oParam[11] = new SqlParameter("@RefillNo", SqlDbType.TinyInt);
            oParam[11].Value = 0;

            if (iCounsID <= 0)
            {
                sSQL = @" insert into PatientCounselingHistory ( PatientNo,isCounsCompleted,CounselingResult,CounsCompletedDate,AddedToQueueDate,Remark,CounseledByUser,RecordAddedByUser,ConductMethod ) 
                          values ( @PatientNo, @isCounsCompleted,@CounselingResult,@CounsCompletedDate,@AddedToQueueDate,@Remark,@CounseledByUser,@RecordAddedByUser,@ConductMethod ) ";
                if (!bCounsCompletedDate)
                {
                    sSQL = @" insert into PatientCounselingHistory ( PatientNo,isCounsCompleted,CounselingResult,AddedToQueueDate,Remark,CounseledByUser,RecordAddedByUser,ConductMethod ) 
                          values ( @PatientNo, @isCounsCompleted,@CounselingResult,@AddedToQueueDate,@Remark,@CounseledByUser,@RecordAddedByUser,@ConductMethod ) ";
                }
                if (!bAddedToQueueDate)
                {
                    sSQL = @" insert into PatientCounselingHistory ( PatientNo,isCounsCompleted,CounselingResult,CounsCompletedDate,Remark,CounseledByUser,RecordAddedByUser,ConductMethod ) 
                          values ( @PatientNo, @isCounsCompleted,@CounselingResult,@CounsCompletedDate,@Remark,@CounseledByUser,@RecordAddedByUser,@ConductMethod ) ";
                }

                rtn = this.ExecuteSql(sSQL, oParam);
            }

            if (rtn)
            {
                string sSqlSel = "Select  MAX(CounsID) as MaxCounsID From PatientCounselingHistory ";
                DataSet oDs = this.GetRecs(sSqlSel);
                if (oDs != null && oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count > 0)
                {
                    iCounsID = Convert.ToInt32( oDs.Tables[0].Rows[0]["MaxCounsID"].ToString());
                    oParam[0].Value = iCounsID;

                    for (int i = 0; i < dtRXs.Rows.Count; i++)
                    {
                        oParam[10].Value = Convert.ToInt64(dtRXs.Rows[i]["RxNo"].ToString());
                        oParam[11].Value = Convert.ToInt32( dtRXs.Rows[i]["RefillNo"].ToString());

                        string sCmdInsert = @" insert into PatientCounselingHistRxList (CounsID, RxNo, RefillNo) values (@CounsID, @RxNo, @RefillNo) ";

                        rtn = this.ExecuteSql(sCmdInsert, oParam);
                    }

                    dtHist.Rows[0]["CounsID"] = iCounsID;
                }
            }

            return rtn;
        }

        public string GetPrimeRxSettingDetailValue(int SettingID, string FieldName)
        {
            string sValSetting = string.Empty;
            SqlParameter[] oParam = new SqlParameter[2];
            oParam[0] = new SqlParameter("@SettingID", SqlDbType.TinyInt);
            oParam[0].Value = SettingID;
            oParam[1] = new SqlParameter("@FieldName", SqlDbType.VarChar);
            oParam[1].Value = FieldName;

            string sSQL = " SELECT * from SettingDetail WITH (NOLOCK) where SettingID=@SettingID AND FieldName=@FieldName ";
            this.sTableName = "SettingDetail";
            DataSet ds = null;
            ds = base.GetRecs(sSQL, oParam);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
            {
                sValSetting = ds.Tables[0].Rows[0]["FieldValue"].ToString();
            }
            
            return sValSetting;
        }

        public DataTable GetPatCounselingRulesByState(string sPharmST)
        {
            this.sTableName = "PatientCounselingRules";
            DataSet oDS = new DataSet();
            DataTable dt = null;
            List<SqlParameter> paramList = new List<SqlParameter>();
            paramList.Add(new SqlParameter("@State", sPharmST) );

            string sSql = "Select * from PatientCounselingRules WITH (NOLOCK) where (State=@State or State='') AND IsActive='1' order by State desc ";
            oDS = this.GetRecs(sSql, paramList.ToArray());

            if (oDS != null && oDS.Tables.Count > 0)
            {
                dt = oDS.Tables[0];
                dt.TableName = "PatientCounselingRules";
            }
            return dt;
        }

        public DataTable GetLastCounselledPatientDrugInfo(int PatientNo, string sNDC)
        {
            this.sTableName = "PatientCounselingHistory";
            DataSet oDS = new DataSet();
            DataTable dt = null;

            SqlParameter[] oParam = new SqlParameter[2];
            oParam[0] = new SqlParameter("@PatientNo", SqlDbType.Int);
            oParam[0].Value = PatientNo;
            oParam[1] = new SqlParameter("@NDC", SqlDbType.VarChar);
            oParam[1].Value = sNDC;

            string sSql = " Select top(1) P.PatientNo,P.CounsID,L.RxNo,L.RefillNo,P.isCounsCompleted,P.CounsCompletedDate, R.NDC  from PatientCounselingHistory P WITH (NOLOCK), PatientCounselingHistRxList L WITH (NOLOCK) LEFT OUTER JOIN RxDetailsShort R WITH (NOLOCK) ON L.RxNo=R.RXNO AND L.RefillNo=R.NREFILL  ";
            sSql += " where P.isCounsCompleted='1' AND P.PatientNo=@PatientNo AND P.CounsID = L.CounsID AND R.NDC=@NDC  order by P.CounsCompletedDate desc ";

            oDS = this.GetRecs(sSql, oParam);
            if (oDS != null && oDS.Tables.Count > 0)
            {
                dt = oDS.Tables[0];
                dt.TableName = "PatientCounselingHistory";
            }
            return dt;
        }

        public bool HasRXRefillDoseChanged(long RxNo, int RefillNo)
        {
            bool rtn = false;
            if (RefillNo == 0)
                return rtn;

            int prevRefillNo = RefillNo - 1;
            DataSet oDs = null;
            string sql = string.Empty;
            List<SqlParameter> sParam = new List<SqlParameter>();
            sParam.Add(new SqlParameter { ParameterName = "@RxNo", Value = RxNo });
            sParam.Add(new SqlParameter { ParameterName = "@NREFILL", Value = RefillNo });
            sParam.Add(new SqlParameter { ParameterName = "@PrevRefillNo", Value = prevRefillNo });

            sql = @" SELECT C.RxNo, C.NREFILL, C.QUANT, C.DAYS, C.NDC FROM CLAIMS C WITH (NOLOCK) WHERE C.RxNo = @RxNo AND (C.NREFILL=@PrevRefillNo OR C.NREFILL= @NREFILL) ";
            oDs = this.GetRecs(sql, sParam.ToArray());

            if (oDs.Tables.Count > 0 && oDs.Tables[0].Rows.Count == 2)
            {
                if (oDs.Tables[0].Rows[0]["NDC"].ToString().Trim() != oDs.Tables[0].Rows[1]["NDC"].ToString().Trim())
                    rtn = true;  //NDC changed meaning Dose changed 
                else
                {
                    int daysPrev = convertNullToInt(oDs.Tables[0].Rows[0]["DAYS"]);
                    int daysRefill = convertNullToInt(oDs.Tables[0].Rows[1]["DAYS"]);
                    if (daysPrev != 0 && daysRefill != 0)
                    {
                        if (convertNullToDecimal(oDs.Tables[0].Rows[0]["QUANT"]) / daysPrev != convertNullToDecimal(oDs.Tables[0].Rows[1]["QUANT"]) / daysRefill)
                            rtn = true;  // Dose changed
                    }
                }
            }

            return rtn;
        }

        private int convertNullToInt(object strValue)
        {
            if (strValue == null)
            {
                return 0;
            }
            else if (strValue.ToString().Trim() == "")
            {
                return 0;
            }
            else
                try
                {
                    return Convert.ToInt32(strValue.ToString().Trim());
                }
                catch (Exception) { return 0; }
        }

        private Decimal convertNullToDecimal(object strValue)
        {
            if (strValue == null)
            {
                return 0;
            }
            else if (strValue.ToString().Trim() == "")
            {
                return 0;
            }
            else
                try
                {
                    return Convert.ToDecimal(strValue.ToString().Trim());
                }
                catch (Exception) { return 0; }
        }
        #region PRIMEPOS-3403
        public DataTable GetRxPayRec(long lRXNO, int iRefNo)
        {
            try
            {
                DataSet oDs = null;
                DataTable dt = null;
                string sql = string.Empty;
                List<SqlParameter> sParam = new List<SqlParameter>();
                sParam.Add(new SqlParameter { ParameterName = "@RxNo", Value = lRXNO });
                sParam.Add(new SqlParameter { ParameterName = "@REFILL", Value = iRefNo });

                this.sTableName = "RXPAY";

                sql = @"select INS_CODE,RECTYPE,REJCODES,RXNO from RXPAY WITH (NOLOCK) WHERE RXNO = @RxNo AND REFILL_NO = @REFILL";

                oDs = this.GetRecs(sql, sParam.ToArray());
                if (oDs != null && oDs.Tables.Count > 0)
                {
                    dt = oDs.Tables[0];
                }

                logger.Trace("GetRxPayRec(long lRXNO, int iRefNo) - SQL - " + sql);
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>GetRxPayRec(long lRXNO, int iRefNo): An Error Occured while executing sql ");
                return null;
            }
        }
        public DataTable GetClaimPaymentView(long lRXNO, int iRefNo)
        {
            try
            {
                DataSet oDs = null;
                DataTable dt = null;
                string sql = string.Empty;
                List<SqlParameter> sParam = new List<SqlParameter>();
                sParam.Add(new SqlParameter { ParameterName = "@RxNo", Value = lRXNO });
                sParam.Add(new SqlParameter { ParameterName = "@REFILL", Value = iRefNo });

                this.sTableName = "ClaimPaymentView";

                sql = @"SELECT DISTINCT CPV.TOTALPATCOPAY, CPV.PriInsPaid, CPV.SecInsPaid, CPV.TerInsPaid, TAG.TagName,
	                           (CASE PVL.VerifStatus WHEN 'V' THEN 'Y' ELSE 'N' END) PharmVerificationDone,
	                           (CASE WHEN P.PrintedBy IS NULL THEN 'N' ELSE 'Y' END) LabelPrinted, WF.QueueName
                                FROM CLAIMPAYMENTVIEW CPV WITH (NOLOCK) 
                                INNER JOIN CLAIMS C WITH (NOLOCK) ON CPV.RXNO=C.RXNO AND CPV.NREFILL=C.NREFILL
	                            LEFT OUTER JOIN RxFollowupTag TAG WITH (NOLOCK) ON C.TagId=TAG.TagId
	                            LEFT OUTER JOIN PHARMVERIFLOG PVL WITH (NOLOCK) ON C.RXNO=PVL.RxNo AND C.NREFILL=PVL.RefillNo
	                            LEFT OUTER JOIN RxLabelPrintingHistory P WITH (NOLOCK) ON C.RXNO = P.RxNo AND C.NREFILL = P.RefillNo
	                            LEFT OUTER JOIN ( SELECT StateName AS QueueName, RxNo, Nrefill FROM WF_State WITH (NOLOCK) 
							                       INNER JOIN WF_Instance WITH (NOLOCK) ON WF_State.StateId = WF_Instance.CurrentState
							                        INNER JOIN WF_InstanceRxContext WITH (NOLOCK) ON WF_Instance.InstanceId = WF_InstanceRxContext.InstanceId 
					                             ) WF ON c.RxNo = WF.RxNo AND C.NREFILL = WF.Nrefill 
                                WHERE C.RXNO = @RxNo AND C.NREFILL = @REFILL";

                oDs = this.GetRecs(sql, sParam.ToArray());
                if (oDs != null && oDs.Tables.Count > 0)
                {
                    dt = oDs.Tables[0];
                }

                logger.Trace("GetClaimPaymentView(long lRXNO, int iRefNo) - SQL - " + sql);
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>GetClaimPaymentView(long lRXNO, int iRefNo): An Error Occured while executing sql ");
                return null;
            }
        }
        public DataTable GetRxDiag(long lRXNO)
        {
            try
            {
                DataSet oDs = null;
                DataTable dt = null;
                string sql = string.Empty;
                List<SqlParameter> sParam = new List<SqlParameter>();
                sParam.Add(new SqlParameter { ParameterName = "@RxNo", Value = lRXNO });

                this.sTableName = "RXDIAG";

                sql = @"Select [RxDiagID],[RxNo],[DiagCode],RxDiag.[DiagCodeQ],[Abbreviation], icd10.Name AS DiagName
                From RxDiag WITH (NOLOCK) inner join [Diagnosis_Code_Qualifier] DCQ WITH (NOLOCK) ON  DCQ.DiagCodeQ = RxDiag.DiagCodeQ Left outer join [" + base.db.GSDDCatalog + "].dbo.ICD10_Codes icd10 WITH (NOLOCK) ON replace(rxdiag.diagcode,'.','')=icd10.code and rxdiag.diagcodeq='02'  where RxNo = @RxNo";

                oDs = this.GetRecs(sql, sParam.ToArray());
                if (oDs != null && oDs.Tables.Count > 0)
                {
                    dt = oDs.Tables[0];
                }
                logger.Trace("GetRxDiag(long lRXNO) - SQL - " + sql);
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>GetRxDiag(long lRXNO): An Error Occured while executing sql ");
                return null;
            }
        }
        public DataTable GetRxExtra(long lRXNO, int iRefNo)
        {
            try
            {
                DataSet oDs = null;
                DataTable dt = null;
                string sql = string.Empty;

                List<SqlParameter> sParam = new List<SqlParameter>();
                sParam.Add(new SqlParameter { ParameterName = "@RxNo", Value = lRXNO });
                sParam.Add(new SqlParameter { ParameterName = "@REFILL", Value = iRefNo });

                this.sTableName = "RXEXTRA";

                sql = @"Select RX_ORIGCD From RXEXTRA WITH (NOLOCK) WHERE RXNO = @RxNo AND REFILL_NO = @REFILL";

                oDs = this.GetRecs(sql, sParam.ToArray());
                if (oDs != null && oDs.Tables.Count > 0)
                {
                    dt = oDs.Tables[0];
                }
                logger.Trace("GetRxExtra(long lRXNO, int iRefNo) - SQL - " + sql);
                return dt;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "PharmaSqlDB==>GetRxExtra(long lRXNO, int iRefNo): An Error Occured while executing sql ");
                return null;
            }
        }
        #endregion
    }
}