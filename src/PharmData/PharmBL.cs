using System;
using System.Data;
using NLog;

namespace PharmData
{
    /// <summary>
    /// This class is responsible for holding all the business logic for the pharmacy
    /// data.
    /// </summary>
    public class PharmBL
    {
        ILogger logger = LogManager.GetCurrentClassLogger();
        private bool isRxTxnSuccess = false;//PRIMEPOS-2761
        private bool isConsentActive = false;//PRIMEPOS-3276
        public enum RxType
        {
            NotFound,
            Found,
            RxTooSoon,
            Over1YearOld,
            CtrlDrgOver6Mnth,
            NarcoticNoRefAllowed,
            NoRefLeftOnRx,
            Over1YearFromDate,
            RefAlreadyInQueue,
            RxOk
        }

        private DBController oDBCont;

        public PharmBL()
        {
            //
            // TODO: Add constructor logic here
            //

            try
            {
                oDBCont = new DBController();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDoctor(string sDocNo)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetDoctor(sDocNo);
                return oTable;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public DataTable GetFacilityDoctors(string sFacID)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetFacilityDoctors(sFacID);
                return oTable;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public DataTable GetDoctor(string fname, string lname)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetDoctor(fname, lname);
                return oTable;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public string GetMaxAccessionNo()
        {
            string accno = string.Empty;
            try
            {
                accno = this.oDBCont.GetMaxAccessionNo();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return accno;
        }

        public DataTable GetUsers(string logintype, string lastname, string firstname)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetUsers(logintype, lastname, firstname);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string InsertLoginDetails(DataTable oTable)
        {
            /*
             * this function inserts entry into the login table
             */
            try
            {
                return this.oDBCont.InsertLoginDetails(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDrugCategory()
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetDrugCategory();
                return oTable;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public DataTable GetDrugOrderedByDrugCatDS(string sFromDate, string sToDate, string sFacility, string sDrugCategory, string sPrescriber, string sPatient)
        {
            try
            {
                DataTable dt = null;
                dt = this.oDBCont.GetDrugOrderedByDrugCatDS(sFromDate, sToDate, sFacility, sDrugCategory, sPrescriber, sPatient);
                return dt;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public string GetAckText()
        {
            DataTable oDtConstant=this.GetConstant();
            string sAckText="";
            for(int i = 1; i < 6; i++)
            {
                sAckText += oDtConstant.Rows[0]["ACKTEXT" + i.ToString()];
            }
            return sAckText;
        }

        public DataTable GetPhDrug(string sNdc)
        {
            DataTable oTable=null;
            try
            {
                oTable = this.oDBCont.GetPhDrug(sNdc);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool IsPickedUp(string sRxNo, string sRefNo)
        {
            bool bRetVal=false;
            DataTable oRx=this.GetRxs(sRxNo, sRefNo);
            if(oRx.Rows.Count > 0 && oRx.Rows[0]["PICKEDUP"].Equals("Y"))
                bRetVal = true;
            return bRetVal;
        }

        public DataTable GetPhDrugName(string sName)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetPhDrugName(sName);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAdvImg(string sImgId)
        {
            try
            {
                return this.oDBCont.GetAdvImg(sImgId);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebDrug(string sNdc)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetWebDrug(sNdc);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebSet(string sSetType)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetWebSet(sSetType);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebDrugName(string sName)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetWebDrugName(sName);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFacility(string sFacCode)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetFacility(sFacCode);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFacility()
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetFacility();
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCategory(string sCateg)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetCategory(sCateg);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPhInfo()
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetPhInfo();
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPhUser(string sPhInit)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetPhUser(sPhInit);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string IsValidPharmacist(string sPhInit, string sPassword)
        {
            string sRetVal="false";
            DataTable oDt=this.GetPhUser(sPhInit);
            if(oDt.Rows.Count > 0 && ((System.DBNull.Equals(oDt.Rows[0]["PASSWORD"], System.DBNull.Value) && sPassword.Equals(""))) || oDt.Rows[0]["PASSWORD"].ToString().TrimEnd().Equals(sPassword))
                sRetVal = "true";
            return sRetVal;
        }

        public bool HasOpenSessions(string sUserName)
        {
            return (this.oDBCont.GetAccess(sUserName, "").Rows.Count > 0);
        }

        public DataTable GetWebLoginByUserName(string sUserName, string sPassword)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetWebLoginByUserName(sUserName, sPassword);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebLoginByUserName(string sUserName)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetWebLoginByUserName(sUserName);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebLoginByID(string sLoginId)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetWebLoginByID(sLoginId);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFaq(string sFaqNo)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetFaq(sFaqNo);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDocPat(string sFacNo, string sDocNo, string sPatFname, string sPatLname)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetDocPat(sFacNo, sDocNo, sPatFname, sPatLname);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /*
                public DataTable GetPatHistory(string sPatNo,string sDocNo,System.DateTime odtStartDate,System.DateTime odtEndDate,bool bOnlyRef)
                {
                    try
                    {
                        DataTable oPat=this.oDBCont.GetPatient(sPatNo);
                        if(oPat.Rows.Count > 0)
                        {
                            if(!(oPat.Rows[0]["VIEWHIST"].ToString()=="Y"))
                                return null;
                        }

                        DataTable oTable=null;
                        DataRow [] oSelRows;

                        oTable=this.oDBCont.GetPatHistory(sPatNo,sDocNo,odtStartDate,odtEndDate);

                        RxType oRxType;
                        string sLastRx="";

                        if(bOnlyRef && oTable.Rows.Count > 0)
                        {
                            oTable.Columns.Add("REFTEXT");
                            oTable.Columns.Add("REFSTAT");

                            oTable.DefaultView.Sort = "RXNO";

                            for(int i = 0; i < oTable.DefaultView.Count; i++)
                            {
                                if(sLastRx==oTable.DefaultView[i].Row["RXNO"].ToString())
                                    continue;

                                oSelRows=oTable.Select("RXNO = '"+oTable.DefaultView[i].Row["RXNO"].ToString()+"'");
                                oRxType=this.GetRxStat(oSelRows);
                                oTable.DefaultView[i].Row["REFTEXT"]=GetRxStatString(oRxType);

                                if(oRxType==PharmBL.RxType.RxOk)
                                {
                                    oTable.DefaultView[i].Row["REFSTAT"]="Y";
                                }
                                else
                                    oTable.DefaultView[i].Row["REFSTAT"]="N";

                                sLastRx=oTable.DefaultView[i].Row["RXNO"].ToString();
                            }

                            for(int i = 0; i < oTable.Rows.Count;i++)
                            {
                                if(oTable.Rows[i]["REFSTAT"].ToString()!="Y")
                                {
                                    oTable.Rows[i].Delete();
                                    //i--;
                                    //oTable.AcceptChanges();
                                }
                            }

                            oTable.AcceptChanges();
                        }

                        return oTable;
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
        */

        public DataTable GetPatHistory(string sPatNo, string sDocNo, System.DateTime odtStartDate, System.DateTime odtEndDate, bool bOnlyRef, bool bIncludeDiscontinued)
        {
            try
            {
                DataTable oPat=this.oDBCont.GetPatient(sPatNo);
                /* i am commenting this part...otherwise we have to add the viewhist column and append it to y in all the patients
                if(oPat.Rows.Count > 0)
                {
                    if(!(oPat.Rows[0]["VIEWHIST"].ToString()=="Y"))
                        return null;
                }
                */
                DataTable oTable=null;
                DataRow [] oSelRows;

                oTable = this.oDBCont.GetPatHistory(sPatNo, sDocNo, odtStartDate, odtEndDate, bIncludeDiscontinued);

                RxType oRxType;
                string sLastRx="";

                if(oTable.Rows.Count > 0)
                {
                    oTable.Columns.Add("REFTEXT");
                    oTable.Columns.Add("REFSTAT");

                    oTable.DefaultView.Sort = "RXNO";

                    for(int i = 0; i < oTable.DefaultView.Count; i++)
                    {
                        if(sLastRx == oTable.DefaultView[i].Row["RXNO"].ToString())
                            continue;

                        oSelRows = oTable.Select("RXNO = '" + oTable.DefaultView[i].Row["RXNO"].ToString() + "'");
                        oRxType = this.GetRxStat(oSelRows);
                        oTable.DefaultView[i].Row["REFTEXT"] = GetRxStatString(oRxType);

                        if(oRxType == PharmBL.RxType.RxOk)
                        {
                            oTable.DefaultView[i].Row["REFSTAT"] = "Y";
                        }
                        else
                            oTable.DefaultView[i].Row["REFSTAT"] = "N";

                        sLastRx = oTable.DefaultView[i].Row["RXNO"].ToString();
                    }

                    if(bOnlyRef)
                    {
                        for(int i = 0; i < oTable.Rows.Count; i++)
                        {
                            if(oTable.Rows[i]["REFSTAT"].ToString() != "Y")
                            {
                                oTable.Rows[i].Delete();
                                //i--;
                                //oTable.AcceptChanges();
                            }
                        }

                        oTable.AcceptChanges();
                    }
                }

                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatHistory(string sDocNo, System.DateTime odtStartDate, System.DateTime odtEndDate, bool bOnlyRef, bool bIncludeDiscontinued)
        {
            //for the rx profile search for printing rx labels based - datewise
            try
            {
                DataTable oTable=null;
                DataRow [] oSelRows;

                oTable = this.oDBCont.GetPatHistory(sDocNo, odtStartDate, odtEndDate, bIncludeDiscontinued);

                RxType oRxType;
                string sLastRx="";

                if(oTable.Rows.Count > 0)
                {
                    oTable.Columns.Add("REFTEXT");
                    oTable.Columns.Add("REFSTAT");

                    oTable.DefaultView.Sort = "RXNO";

                    for(int i = 0; i < oTable.DefaultView.Count; i++)
                    {
                        if(sLastRx == oTable.DefaultView[i].Row["RXNO"].ToString())
                            continue;

                        oSelRows = oTable.Select("RXNO = '" + oTable.DefaultView[i].Row["RXNO"].ToString() + "'");
                        oRxType = this.GetRxStat(oSelRows);
                        oTable.DefaultView[i].Row["REFTEXT"] = GetRxStatString(oRxType);

                        if(oRxType == PharmBL.RxType.RxOk)
                        {
                            oTable.DefaultView[i].Row["REFSTAT"] = "Y";
                        }
                        else
                            oTable.DefaultView[i].Row["REFSTAT"] = "N";

                        sLastRx = oTable.DefaultView[i].Row["RXNO"].ToString();
                    }

                    if(bOnlyRef)
                    {
                        for(int i = 0; i < oTable.Rows.Count; i++)
                        {
                            if(oTable.Rows[i]["REFSTAT"].ToString() != "Y")
                            {
                                oTable.Rows[i].Delete();
                                //i--;
                                //oTable.AcceptChanges();
                            }
                        }

                        oTable.AcceptChanges();
                    }
                }

                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatHistory(string sDocNo, System.DateTime odtStartDate, System.DateTime odtEndDate, string rxno1, string rxno2, bool bOnlyRef, bool bIncludeDiscontinued)
        {
            //for the rx profile search for printing rx labels based - rxwise
            try
            {
                DataTable oTable=null;
                DataRow [] oSelRows;

                oTable = this.oDBCont.GetPatHistory(sDocNo, odtStartDate, odtEndDate, rxno1, rxno2, bIncludeDiscontinued);

                RxType oRxType;
                string sLastRx="";

                if(oTable.Rows.Count > 0)
                {
                    oTable.Columns.Add("REFTEXT");
                    oTable.Columns.Add("REFSTAT");

                    oTable.DefaultView.Sort = "RXNO";

                    for(int i = 0; i < oTable.DefaultView.Count; i++)
                    {
                        if(sLastRx == oTable.DefaultView[i].Row["RXNO"].ToString())
                            continue;

                        oSelRows = oTable.Select("RXNO = '" + oTable.DefaultView[i].Row["RXNO"].ToString() + "'");
                        oRxType = this.GetRxStat(oSelRows);
                        oTable.DefaultView[i].Row["REFTEXT"] = GetRxStatString(oRxType);

                        if(oRxType == PharmBL.RxType.RxOk)
                        {
                            oTable.DefaultView[i].Row["REFSTAT"] = "Y";
                        }
                        else
                            oTable.DefaultView[i].Row["REFSTAT"] = "N";

                        sLastRx = oTable.DefaultView[i].Row["RXNO"].ToString();
                    }

                    if(bOnlyRef)
                    {
                        for(int i = 0; i < oTable.Rows.Count; i++)
                        {
                            if(oTable.Rows[i]["REFSTAT"].ToString() != "Y")
                            {
                                oTable.Rows[i].Delete();
                                //i--;
                                //oTable.AcceptChanges();
                            }
                        }

                        oTable.AcceptChanges();
                    }
                }

                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetUser(string username)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetUser(username);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatient(string sPatNo)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetPatient(sPatNo);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #region Sprint-25 - PRIMEPOS-2322 01-Feb-2017 JY Added logic to get all patients w.r.t. patients FamilyId
        /// <summary>
        /// This Function get all patients w.r.t. FamilyId
        /// </summary>
        /// Author: JY 02/01/2017
        /// <param name="iFamilyID"></param>
        /// <returns>DataTable</returns>
        public DataTable GetPatientByFamilyID(int iFamilyID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatientByFamilyID(iFamilyID);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// This Function get the Patient Notes
        /// </summary>
        /// Author: Manoj Kumar 1/28/2013
        /// <param name="sPatNo"></param>
        /// <returns>DataTable</returns>
        public DataTable GetPatientNotes(string sPatNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatientNotes(sPatNo);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxNotes(string sRxNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetRxNotes(sRxNo);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatientPayPref(string sPatNo, string sPayType)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatientPayPref(sPatNo, sPayType);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This is to get the Patient information and PayType info
        /// </summary>
        /// Author: Manoj, Date: 12/4/2012
        /// <param name="sAccNo"></param>
        /// <param name="sPayType"></param>
        /// <returns>Table</returns>
        public DataTable GetPatientPayPrefByAccNo(string sAccNo, string sPayType)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatientPayPrefByAccNo(sAccNo, sPayType);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// PRIMEPOS-3103 This is to get the Patient information and PayType info by PatientNo
        /// </summary>
        /// <param name="sPatientNo"></param>
        /// <param name="sPayType"></param>
        /// <returns></returns>
        public DataTable GetPatientPayPrefByPatientNo(string sPatientNo, string sPayType)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatientPayPrefByPatientNo(sPatientNo, sPayType);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the Patient info by Account No
        /// </summary>
        /// Author: Manoj, Date: 12/4/2012
        /// <param name="sAccNo"></param>
        /// <param name="sPayType"></param>
        /// <returns></returns>
        public DataTable GetPatientByAccNo(string sAccNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatientByAccNo(sAccNo);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the Patient info by RxNo
        /// </summary>
        /// Author: PRIMEPOS-2536 14-May-2019 JY Added
        /// <param name="sRxNo"></param>
        /// <returns></returns>
        public DataTable GetPatientByRxNo(string sRxNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatientByRxNo(sRxNo);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPrivackAck(string sPatNo)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetPrivackAck(sPatNo);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPrivackAckAndConsentInfo(string sPatNo, int consentType)
        {
            try
            {
                DataSet oDS = null;
                oDS = this.oDBCont.GetPrivackAckAndConsentInfo(sPatNo, consentType);
                return oDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetConsentText(int consentSource)
        {
            try
            {
                DataSet oDS = null;
                oDS = this.oDBCont.GetConsentText(consentSource);
                return oDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetConsentReferenceData()
        {
            try
            {
                DataSet oDS = null;
                oDS = this.oDBCont.GetConsentReferenceData();
                return oDS;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatientByName(string lname, string fname, bool bRemoveWebLogin)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetPatientByName(lname, fname, bRemoveWebLogin);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebLoginByType(string sLoginType)
        {
            return this.oDBCont.GetWebLoginByType(sLoginType);
        }

        public DataTable GetDoctorByName(string lname, string fname, bool bRemoveWebLogin)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetDoctorsByName(lname, fname, bRemoveWebLogin);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFacilityByName(string sName, bool bRemoveWebLogin)
        {
            try
            {
                DataTable oTable=null;
                oTable = this.oDBCont.GetFacilityByName(sName, bRemoveWebLogin);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void InsertAccess(DataTable oTable)
        {
            try
            {
                this.oDBCont.InsertAccess(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string InsertAccess1(DataTable oTable)
        {
            /*
             * i am duplicating this function to make it return the ses_no for the time being
             */
            try
            {
                return this.oDBCont.InsertAccess1(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public bool checkRandomPassword(string username, string password)
        {
            return this.oDBCont.checkRandomPassword(username, password);
        }

        public string InsertLogin(DataTable oTable)
        {
            /*
             * this function inserts entry into the login table
             */
            try
            {
                return this.oDBCont.InsertLogin(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAccess(DataTable oTable)
        {
            /*
             * this function updates the access table to fill in the logout date and time
             */
            try
            {
                this.oDBCont.UpdateAccess(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string UpdateLogin(string username, string password)
        {
            /*
             * this function updates the login table with the new random generated password
             */
            return (this.oDBCont.UpdateLogin(username, password));
        }

        public void InsertAdvImg(DataTable oTable)
        {
            try
            {
                this.oDBCont.InsertAdvImg(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void InsertFAQ(DataTable oTable)
        {
            try
            {
                this.oDBCont.InsertFAQ(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetExtPatByDate(DateTime FromDate, DateTime ToDate, string sPharmId, string sProcess)
        {
            return this.oDBCont.GetExtPatByDate(FromDate, ToDate, sPharmId, sProcess);
        }

        public void InsertExtPat(DataTable oTable)
        {
            this.oDBCont.InsertExtPat(oTable);
        }

        public DataTable GetContsByDate(DateTime FromDate, DateTime ToDate, string sPharmId, string sProcess)
        {
            try
            {
                return this.oDBCont.GetContsByDate(FromDate, ToDate, sPharmId, sProcess);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void Hit()
        {
            try
            {
                this.oDBCont.Hit();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string InsertNewRxOrd(DataTable oTable)
        {
            try
            {
                return this.oDBCont.InsertNewRxOrd(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void InsertRxPickUpLog(DataTable oTable)
        {
            try
            {
                this.oDBCont.InsertRxPickUpLog(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void InsertContact(DataTable oTable)
        {
            try
            {
                this.oDBCont.InsertContact(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void InsertRxQue(DataTable oTable)
        {
            try
            {
                this.oDBCont.InsertRxQue(oTable);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public string DiscontinueRx(DataTable dtDCRxs)
        {
            return this.oDBCont.DiscontinueRx(dtDCRxs);
        }

        public void GetRxDetails(DataTable oRxs)
        {
            DataTable oTemp=null;

            if(!(oRxs.Columns.Contains("DRGNAME")))
            {
                oRxs.Columns.Add("DRGNAME", typeof(string));
            }

            for(int i = 0; i < oRxs.Rows.Count; i++)
            {
                oTemp = this.GetPhDrug(oRxs.Rows[i]["NDC"].ToString());
                if(oTemp.Rows.Count > 0)
                    oRxs.Rows[i]["DRGNAME"] = oTemp.Rows[0]["DRGNAME"].ToString();
            }
        }

        public bool DoesRxExist(string sRxNo, string sRefNo)
        {
            try
            {
                return this.oDBCont.DoesRxExist(sRxNo, sRefNo);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return false;
        }

        //Added By Manoj 9/18/2012 - This is to mark the CopayPaid in Pharmsql.
        public void MarkCopayPaid(string sRxNo, string sRefNo, char val, string sPartialFillNo = "0")
        {
            try
            {
                this.oDBCont.MarkCopayPaid(sRxNo, sRefNo, val, sPartialFillNo);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void MarkDelivery(string sRxNo, DateTime PickUpDate)
        {
            string sError="";

            string sRef=this.oDBCont.GetLastRefill(sRxNo);

            this.MarkDelivery(sRxNo, sRef, null, "Y", PickUpDate, out sError);
        }

        public void MarkDelivery(string sRxNo, DateTime PickUpDate, string sPickupPOS)
        {
            string sError="";

            string sRef=this.oDBCont.GetLastRefill(sRxNo);

            this.MarkDelivery(sRxNo, sRef, null, "Y", PickUpDate, sPickupPOS, out sError,false); // PRIMERX-7688 - NileshJ - Added false - 23-Sept-2019
        }

        public void MarkDelivery(string sRxNo, string sPickedUp, DateTime PickUpDate, string sPickupPOS)
        {
            string sError="";

            string sRef=this.oDBCont.GetLastRefill(sRxNo);

            this.MarkDelivery(sRxNo, sRef, null, sPickedUp, PickUpDate, sPickupPOS, out sError,false);// PRIMERX-7688 - NileshJ - Added false- 23-Sept-2019
        }

        public void MarkDelivery(string sRxNo, string sRefNo, string sDelivery, string sPickedUp, System.DateTime PickUpDate, out string sError)
        {
            this.oDBCont.MarkDelivery(sRxNo, sRefNo, sDelivery, sPickedUp, PickUpDate, "", out sError,false);// PRIMERX-7688 - NileshJ - Added false- 23-Sept-2019
        }

        public void MarkDelivery(string sRxNo, string sRefNo, string sDelivery, string sPickedUp, System.DateTime PickUpDate, string sPickUpPOS, out string sError, bool isBatchDelivery = false, string sPartialFillNo = "0") // PRIMERX-7688 - NileshJ - Added isBatchDelivery- 23-Sept-2019
        {
            this.oDBCont.MarkDelivery(sRxNo, sRefNo, sDelivery, sPickedUp, PickUpDate, sPickUpPOS, out sError, isBatchDelivery, sPartialFillNo); // PRIMERX-7688 - NileshJ - Added isBatchDelivery- 23-Sept-2019
        }

        public long SaveInsSigTrans(System.DateTime dtTransDate, long lPatNo, string sInsType, string sTransData, string sSignature, string sCounselingReq, string sSigType)
        {
            return this.oDBCont.SaveInsSigTrans(dtTransDate, lPatNo, sInsType, sTransData, sSignature, sCounselingReq, sSigType);
        }

        public long SaveInsSigTrans(System.DateTime dtTransDate, long lPatNo, string sInsType, string sTransData, string sSignature, string sCounselingReq, string sSigType, byte[] bBinarySign)
        {
            return this.oDBCont.SaveInsSigTrans(dtTransDate, lPatNo, sInsType, sTransData, sSignature, sCounselingReq, sSigType, bBinarySign);
        }

        public void SaveTransDet(long lTransNo, long lRxNo, int iRefNo, string sPartialFillNo = "0")
        {
            this.oDBCont.SaveTransDet(lTransNo, lRxNo, iRefNo, sPartialFillNo);
        }

        //public string CheckDDIDUP(string sDrgNdc, string sTxrCode, long lPatNo, int iDDIDays, out DataTable oDtInteracts, out bool bFoundDupDrugs, out string sDupDrugs)
        //{
        //    try
        //    {
        //        return this.oDBCont.CheckDDIDUP(sDrgNdc, sTxrCode, lPatNo, iDDIDays, out oDtInteracts, out bFoundDupDrugs, out sDupDrugs);
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public bool CheckAllergy(string strTxrCode, string strNDC, string strPatAllergy, out string strRetInfo)
        {
            try
            {
                return this.oDBCont.CheckAllergy(strTxrCode, strNDC, strPatAllergy, out strRetInfo);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void SavePatientAck(long lPatNo, string sAck, DateTime dtAckDate)
        {
            this.oDBCont.SavePatientAck(lPatNo, sAck, dtAckDate);
        }

        public void SavePrivacyAck(long lPatNo, System.DateTime dtSigned, string sPatAccept, string sPrivacyText, string sSignature, string sSigType)
        {
            this.oDBCont.SavePrivacyAck(lPatNo, dtSigned, sPatAccept, sPrivacyText, sSignature, sSigType);
        }

        public void SavePrivacyAck(long lPatNo, System.DateTime dtSigned, string sPatAccept, string sPrivacyText, string sSignature, string sSigType, byte[] bBinarySign)
        {
            this.oDBCont.SavePrivacyAck(lPatNo, dtSigned, sPatAccept, sPrivacyText, sSignature, sSigType, bBinarySign);
        }

        public DataTable GetRxForDelivery(string sRxNo, out string sStatus, string RefillNo = "-1", string PartialFillNo = "0")
        {
            DataTable dt = null;
            sStatus = "false";
            dt = this.oDBCont.GetRxForDelivery(sRxNo, out sStatus, RefillNo, PartialFillNo);

            return dt;
        }

        public DataTable GetAllUndelivered(DateTime FromDate, DateTime ToDate, string sPatNo)
        {
            /*
             * Returns all prescriptions undelivered for the day offset,
             * and for patientno, if patientno = -1 then
             * all patients undelivered.
             * */

            //	DateTime oDtOffSet=System.DateTime.Today.AddDays((-iDaysOffSet));

            DataTable oRxs=this.oDBCont.GetRxsByDate(FromDate, ToDate);
            DataRow oRow=null;
            bool bPatValid=false;

            for(int i = 0; i < oRxs.Rows.Count; i++)
            {
                oRow = oRxs.Rows[i];
                bPatValid = (sPatNo == "-1" || oRow["PATIENTNO"].ToString().Trim() == sPatNo);
                if(oRow["STATUS"].ToString().TrimEnd().Equals("F") || ((!((bPatValid) && (oRow["PICKEDUP"].ToString().TrimEnd() != "Y") && oRow["DELIVERY"].ToString().TrimEnd() == "D"))))
                    oRow.Delete();
            }

            oRxs.AcceptChanges();

            this.GetRxDetails(oRxs);

            return oRxs;
        }

        public DataTable GetLastRefill(string sRxNo)
        {
            /*This function is responsible for returning the last
             * refill for a particular rxno
             * */
            DataTable oRetTable=new DataTable();
            DataTable oRxs=this.oDBCont.GetRxsAllFields(sRxNo, "-1");
            oRetTable = oRxs.Clone();

            if(oRxs.Rows.Count > 0)
                oRetTable.ImportRow(oRxs.Rows[oRxs.Rows.Count - 1]);

            return oRetTable;
        }

        public DataTable GetLastRefill(string sRxNo, string sRefNo)
        {
            try
            {
                return this.oDBCont.GetLastRefill(sRxNo, sRefNo);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxsByDate(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return this.oDBCont.GetRxsByDate(FromDate, ToDate);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #region Get Rx
        public DataTable GetRxs(string sRxNo)
        {
            return this.GetRxs(sRxNo, false);
        }

        public DataTable GetRxs(string sRxNo, bool sendUnbilled)
        {
            string sRef=this.oDBCont.GetLastRefill(sRxNo);
            return this.GetRxs(sRxNo, sRef, sendUnbilled);
        }

        public DataTable GetRxs(string sRxNo, string sRefNo)
        {
            try
            {
                return this.oDBCont.GetRxs(sRxNo, sRefNo, "B");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxs(string sRxNo, string sRefNo, bool sendUnbilled)
        {
            try
            {
                if(sendUnbilled == true)
                {
                    return this.oDBCont.GetRxs(sRxNo, sRefNo, "U");
                }
                else
                {
                    return this.oDBCont.GetRxs(sRxNo, sRefNo, "B");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo)
        {
            return GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, false);
        }

        public DataTable GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, bool sendUnbilled, bool IsBatchDelivery = false)//NileshJ - PRIMERX - 7688 - Added IsBatchDelivery - 23-Sept-2019
        {
            try
            {
                if (sendUnbilled == true)
                {
                    return this.oDBCont.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, "U");
                }
                else
                {
                    return this.oDBCont.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, "B", IsBatchDelivery); //NileshJ - PRIMERX - 7688 - Added IsBatchDelivery - 23-Sept-2019
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, string sBillStatus, bool IsBatchDelivery = false) //NileshJ - PRIMERX - 7688 - Added IsBatchDelivery - 23-Sept-2019
        {
            try
            {
                return this.oDBCont.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, sBillStatus, IsBatchDelivery);//NileshJ - PRIMERX - 7688 - Added IsBatchDelivery - 23-Sept-2019
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, bool sendUnbilled, char cType)    //PRIMEPOS-2036 22-Jan-2019 JY Added
        {
            try
            {
                if (sendUnbilled == true)
                {
                    return this.oDBCont.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, "U", cType);
                }
                else
                {
                    return this.oDBCont.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, "B", cType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxs(string sPatientNo, DateTime dFillDateFrom, DateTime dFillDateTo, string sBillStatus, char cType)    //PRIMEPOS-2036 22-Jan-2019 JY Added
        {
            try
            {
                return this.oDBCont.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, sBillStatus, cType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataTable GetRxsWithStatus(string sRxNo, String billStatus)
        {
            string sRef = this.oDBCont.GetLastRefill(sRxNo);

            return this.oDBCont.GetRxs(sRxNo, sRef, billStatus);
        }

        public DataTable GetRxsWithStatus(string sRxNo, string sRefNo, String billStatus)
        {
            DataTable dt = null;
            try
            {
                if (this.oDBCont.GetDBType() == DBController.DbType.SqlDB)
                {
                    dt = this.oDBCont.GetRxs(sRxNo, sRefNo, billStatus);
                }
                else if (this.oDBCont.GetDBType() == DBController.DbType.WebServiceDB)
                {
                    int iPartialFillNo = 0;
                    if (!sRefNo.Contains("+"))
                        iPartialFillNo = this.oDBCont.GetLastPartialFillNo(sRxNo, sRefNo);
                    dt = this.oDBCont.GetRxDetails(sRxNo, sRefNo, billStatus, iPartialFillNo);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public string GetBillStatus(string sRxNo, string sRefNo)
        {
            try
            {
                return this.oDBCont.GetBillStatus(sRxNo, sRefNo);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetSimilar(string sRxNo, string sSpecialFilter)
        {
            return GetSimilar(sRxNo, sSpecialFilter, false);
        }

        public DataTable GetAnyRx(string sRxNo, string sSpecialFilter)
        {
            DataTable oRx = this.GetLastRefill(sRxNo);
            DataTable oPatRx = new DataTable();
            DataTable tmpRx = new DataTable();
            DataRow oDRow = null;

            if (oRx.Rows.Count > 0)
            {
                oPatRx = this.oDBCont.GetRxsByPatient(oRx.Rows[0]["PATIENTNO"].ToString(), "");
                tmpRx = oPatRx.Clone();

                for (int i = 0; i < oPatRx.Rows.Count; i++)
                {
                    if (oPatRx.Rows[i]["STATUS"].ToString().Trim() != "F" && oPatRx.Rows[i]["PICKEDUP"].ToString().Trim() != "Y")
                    {
                        oDRow = oPatRx.Rows[i];
                        tmpRx.ImportRow(oDRow);
                    }
                }
            }

            this.GetRxDetails(tmpRx);
            return tmpRx;
        }

        public DataTable GetAnyRx(string sRxNo, string sSpecialFilter, bool sendUnbilled)
        {
            DataTable oRx = this.GetLastRefill(sRxNo);
            DataTable oPatRx = new DataTable();
            DataTable tmpRx = new DataTable();
            DataRow oDRow = null;

            if (oRx.Rows.Count > 0)
            {
                if (sendUnbilled == true)
                {
                    oPatRx = this.oDBCont.GetRxsByPatient(oRx.Rows[0]["PATIENTNO"].ToString(), "");
                }
                else
                {
                    oPatRx = this.oDBCont.GetRxsByPatient(oRx.Rows[0]["PATIENTNO"].ToString(), "B");
                }
                tmpRx = oPatRx.Clone();

                for (int i = 0; i < oPatRx.Rows.Count; i++)
                {
                    if (oPatRx.Rows[i]["STATUS"].ToString().Trim() != "F"&& oPatRx.Rows[i]["STATUS"].ToString().Trim() != "T" && oPatRx.Rows[i]["PICKEDUP"].ToString().Trim() != "Y")
                    {
                        oDRow = oPatRx.Rows[i];
                        tmpRx.ImportRow(oDRow);
                    }
                }
            }

            this.GetRxDetails(tmpRx);
            return tmpRx;
        }

        public DataTable GetSimilar(string sRxNo, string sSpecialFilter, bool sendUnbilled)
        {
            DataTable oRx=this.GetLastRefill(sRxNo);
            DataTable oPatRx=new DataTable();
            DataRow oDRow=null;

            if(oRx.Rows.Count > 0)
            {
                if(sendUnbilled == true)
                {
                    oPatRx = this.oDBCont.GetRxsByPatient(oRx.Rows[0]["PATIENTNO"].ToString(), "");
                }
                else
                {
                    oPatRx = this.oDBCont.GetRxsByPatient(oRx.Rows[0]["PATIENTNO"].ToString(), "B");
                }

                DateTime oLastFillDate=DateTime.MinValue;

                if(sSpecialFilter.Equals("DATEF") && MMSUtil.UtilFunc.IsDate(oRx.Rows[0]["DATEF"].ToString()))
                {
                    oLastFillDate = Convert.ToDateTime(oRx.Rows[0]["DATEF"]);
                }
                else
                {
                    sSpecialFilter = "";
                }

                oPatRx.AcceptChanges();

                for(int i = 0; i < oPatRx.Rows.Count; i++)
                {
                    oDRow = oPatRx.Rows[i];

                    //oDRow["STATUS"].ToString().TrimEnd().Equals("U") ||

                    if(oDRow["STATUS"].ToString().TrimEnd().Equals("F")|| oDRow["STATUS"].ToString().TrimEnd().Equals("T") || oDRow["PICKEDUP"].ToString().TrimEnd().Equals("Y"))
                    {
                        oDRow.Delete();
                        continue;
                    }

                    if(sSpecialFilter.Equals("DATEF"))
                    {
                        if(MMSUtil.UtilFunc.IsDate(oDRow["DATEF"].ToString()))
                        {
                            DateTime oDateF=Convert.ToDateTime(oDRow["DATEF"]);

                            if(!(oLastFillDate.Date.Equals(oDateF.Date)))
                                oDRow.Delete();
                        }
                        else
                        {
                            oDRow.Delete();
                        }
                    }
                }

                oPatRx.AcceptChanges();
            }

            this.GetRxDetails(oPatRx);

            return oPatRx;
        }

        public DataTable GetLoginDetails(string dtFrom, string dtTo)
        {
            try
            {
                return this.oDBCont.GetLoginDetails(dtFrom, dtTo);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxLabel(string sRxNo, string sRefNo)
        {
            try
            {
                return this.oDBCont.GetRxLabel(sRxNo, sRefNo);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private bool IsRxTooEarly(DataRow[] oDataRows)
        {
            /*
             * This routine will determine if the RX being filled is too early.
             * If it is, it will return true, if it isnt it will return false.
             * */

            bool bRetVal=false;
            int iDaysSupply=0;
            try
            {
                string sLFillDate=oDataRows[oDataRows.Length - 1]["DATEF"].ToString();

                if(sLFillDate.Trim().Length == 0)
                    throw new Exception("Error In IsRxTooEarly The Fill Date In Last Prescription Is Blank");

                try
                {
                    iDaysSupply = Convert.ToInt32(oDataRows[oDataRows.Length - 1]["DAYS"].ToString());
                }
                catch(Exception ex)
                {
                    throw new Exception("Error In IsRxTooEarly Determining The Last Days Supply " + ex.Message);
                }

                //DateTime dtLastFill=Convert.ToDateTime(sLFillDate.Substring(4,2)+"/"+sLFillDate.Substring(6,2)+"/"+sLFillDate.Substring(0,4));
                DateTime dtLastFill=Convert.ToDateTime(sLFillDate);

                TimeSpan tsInterval=DateTime.Today - dtLastFill;

                if(tsInterval.Days < iDaysSupply)
                    if(((tsInterval.Days / iDaysSupply) * 100) < 80)
                        bRetVal = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error In IsRxTooEarly " + ex.Message);
            }
            return bRetVal;
        }

        public static string GetRxStatString(PharmBL.RxType oRxType)
        {
            switch(oRxType)
            {
                case PharmBL.RxType.NotFound:
                    return "Rx Not Found";

                case PharmBL.RxType.Found:
                    return "Rx Found";

                case PharmBL.RxType.RxTooSoon:
                    return "Rx Too Soon";

                case PharmBL.RxType.Over1YearOld:
                    return "Rx Over One Year Old";

                case PharmBL.RxType.CtrlDrgOver6Mnth:
                    return "Control Drug Rx Over 6 Months";

                case PharmBL.RxType.NarcoticNoRefAllowed:
                    return "Rx Narcotic No Refill Allowed";

                case PharmBL.RxType.NoRefLeftOnRx:
                    return "No Refill Left On Rx";

                case PharmBL.RxType.Over1YearFromDate:
                    return "Rx Over One Year From Date";

                case PharmBL.RxType.RefAlreadyInQueue:
                    return "Rx Already In Queue";

                case PharmBL.RxType.RxOk:
                    return "Rx Refillable";

                default:
                    return "Cannot Comprehend The Status";
            }
        }

        private string CheckControlStat(DataRow[] oDataRows)
        {
            /*
             * This function checks if the prescription matches the control
             * drug logic. If with the control logic it is not fillable than
             * it will return "C" , if it is not fillable and it is class 2
             * than it will return "N" else it will return "" which means
             * ControlStat is good to go.
             * */
            string sRetVal="";
            try
            {
                string sNdc=oDataRows[0]["NDC"].ToString().Trim();

                if(sNdc == "")
                    throw new Exception("Error In Class Rx CheckControlStat Cannot Check Control Stat Without NDC Number In Rx");

                DataTable oDgTable;
                DataTable oCnstTable;

                oDgTable = this.GetPhDrug(sNdc);

                if(oDgTable.Rows.Count == 0)
                    throw new Exception("Error In Check Control Stat Drug Not Found In Drug File");

                string sDgClass=oDgTable.Rows[0]["CLASS"].ToString().Trim();

                if(sDgClass == "2" || sDgClass == "3" || sDgClass == "4" || sDgClass == "5")
                {
                    string sField = "CLASS" + sDgClass + "REFD";

                    oCnstTable = this.oDBCont.GetConstant();

                    if(oCnstTable.Rows.Count > 0)
                    {
                        int iNumberOfDays=0;
                        try
                        {
                            iNumberOfDays = Convert.ToInt32(oCnstTable.Rows[0][sField].ToString());
                        }
                        catch(Exception ex)
                        {
                            throw new Exception("Dont Know How To Convert ClassxRefD Invalid Value In Field " + ex.Message);
                        }

                        string sOrdDate=oDataRows[0]["DATEO"].ToString().Trim();

                        if(sOrdDate == "")
                            throw new Exception("Error In CheckControlStat Order Date Undeterminable");

                        //DateTime dtOrdDate=Convert.ToDateTime(sOrdDate.Substring(4,2)+"/"+sOrdDate.Substring(6,2)+"/"+sOrdDate.Substring(0,4));
                        DateTime dtOrdDate=Convert.ToDateTime(sOrdDate);

                        TimeSpan tsInterval=DateTime.Today - dtOrdDate;

                        if(tsInterval.Days > iNumberOfDays)
                        {
                            if(sDgClass == "2")
                                sRetVal = "N";
                            else
                                sRetVal = "C";
                        }
                    }
                    else
                    {
                        throw new Exception("Error In Getting Constant Values Class Rx ");
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("Error In CheckControl " + ex.Message);
            }
            return sRetVal;
        }

        private bool IsRxOver1Year(DataRow[] oDataRows)
        {
            /*
             * This function will tell if the Rx is over 1 year.
             * return true if it is and false if it is not.
             * */

            bool bRetVal=false;
            try
            {
                string sOrdDate=oDataRows[0]["DATEO"].ToString();

                if(sOrdDate == "")
                    throw new Exception("Not a Valid Date Order To Work With In Class RX Routine Rx1Year ");

                //DateTime dtOrdDate=Convert.ToDateTime(sOrdDate.Substring(4,2)+"/"+sOrdDate.Substring(6,2)+"/"+sOrdDate.Substring(0,4));

                DateTime dtOrdDate=Convert.ToDateTime(sOrdDate);

                string sPatType=oDataRows[0]["PATTYPE"].ToString().Trim();
                DataTable oInsTable;

                oInsTable = this.oDBCont.GetInsInfo(sPatType);
                //bool bInsFound=oRxDB.GetInsInfo(sPatType,out oInsTable);

                if(oInsTable.Rows.Count == 0)
                    throw new Exception("Error In Rx Over 1 Year in Class Rx Insurance Not Found In Inscar");

                int iMdRefill=Convert.ToInt32(oInsTable.Rows[0]["MDREFILL"].ToString());

                TimeSpan tsInterval=DateTime.Today - dtOrdDate;
                if(tsInterval.Days > iMdRefill)
                    bRetVal = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error In Routine IsRxOver1Year in Class Rx " + ex.Message);
            }
            return bRetVal;
        }

        private bool IsRefillable(DataRow[] oDataRows)
        {
            /*
             * This routine determines if the rx is refillable, if it is
             * then it will return true, if it is not then it will return false.
             * * */
            bool bRetVal=false;
            double dQtyOrder=0;
            double dTotQtyConsumed=0;
            double dTotQtyOrdered=0;
            int iTRefills=0;

            if(oDataRows.Length == 0)
                throw new Exception("Error In IsRefillable, No Prescription To Check");

            try
            {
                try
                {
                    dQtyOrder = Convert.ToDouble(oDataRows[0]["QTY_ORD"].ToString());
                }
                catch { }

                try
                {
                    if(dQtyOrder <= 0)
                        dQtyOrder = Convert.ToDouble(oDataRows[0]["QUANT"].ToString());
                }
                catch(Exception ex)
                {
                    throw new Exception("No Qty Convertable In Routine IsRefillable in Class RX Wouldnt Know How To Proceed Further " + ex.Message);
                }

                foreach(DataRow oRxRow in oDataRows)
                {
                    try
                    {
                        dTotQtyConsumed += Convert.ToDouble(oRxRow["QUANT"].ToString());
                    }
                    catch { }
                }

                try
                {
                    /*
                     * I am getting trefills from the last prescription because
                     * when the user changes authorized refills it only takes
                     * effect in the last prescription and not all the refills
                     * */
                    iTRefills = Convert.ToInt32(oDataRows[oDataRows.Length - 1]["TREFILLS"].ToString());
                }
                catch { }

                dTotQtyOrdered = dQtyOrder * (iTRefills + 1);

                if((dTotQtyOrdered - dTotQtyConsumed) > 0)
                    bRetVal = true;
            }
            catch(Exception ex)
            {
                throw new Exception("Error In Routine Is Refillable Class Rx " + ex.Message);
            }
            return bRetVal;
        }

        public DataTable GetConstant()
        {
            try
            {
                return this.oDBCont.GetConstant();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCounseling(string sNdc, string sType)
        {
            try
            {
                return this.oDBCont.GetCounseling(sNdc, sType);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public RxType GetRxStat(DataRow[] oDataRows)
        {
            /*
             * This function will return back what type of prescription it is.
             * It will do that by the RxNo. It will get the last refill
             * and return the info. If bgetrxdata is true it will return
             * back the data of the prescription as well regardless of the
             * status.
             * */
            try
            {
                //	DataTable oRxTable;
                //DataTable oDataTable;

                //oRxTable=this.oDBCont.GetRxs(sRxNo,"-1");

                if(oDataRows.Length == 0)
                    return RxType.NotFound;

                //if the status is "F" then it is equivalent to saying
                //the prescription is not found.
                if(oDataRows[0]["STATUS"].ToString() == "F")
                    return RxType.NotFound;

                //if the patient is not found it is as saying
                //the prescription is not found.
                if(this.oDBCont.GetPatient(oDataRows[0]["PATIENTNO"].ToString()).Rows.Count == 0)
                    return RxType.NotFound;

                //This will determine if rx is in Que.
                if(this.oDBCont.GetRxQueue(oDataRows[0]["RXNO"].ToString()).Rows.Count > 0)
                    return RxType.RefAlreadyInQueue;

                //This will determine if rx is refillable or not.
                if(!(IsRefillable(oDataRows)))
                    return RxType.NoRefLeftOnRx;

                //this will determine if rx is too early.
                if(this.IsRxTooEarly(oDataRows))
                    return RxType.RxTooSoon;

                try
                {
                    //this will do the control check
                    string sControlCheck=this.CheckControlStat(oDataRows);
                    if(sControlCheck == "C")
                        return RxType.CtrlDrgOver6Mnth;
                    else if(sControlCheck == "N")
                        return RxType.NarcoticNoRefAllowed;
                }
                catch { }

                //this determines if rx is over 1 year
                if(this.IsRxOver1Year(oDataRows))
                    return RxType.Over1YearFromDate;
            }
            catch(Exception ex)
            {
                throw new Exception("Error In Determining Rx Status " + ex.Message);
            }
            return RxType.RxOk;
        }

        public DataTable GetPharmacyMessage()
        {
            try
            {
                return this.oDBCont.GetPharmacyMessage();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatInsurance(string sInsID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatInsurance(sInsID);
                return oTable;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void SaveMessages(string sPharMsg, string sInsMsg, string sPhmSigData, string sInsSig, string sPatientNo, long sTransNo)
        {
            try
            {
                this.oDBCont.SaveMessages(sPharMsg, sInsMsg, sPhmSigData, sInsSig, sPatientNo, sTransNo);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Added By Rohit Nair For PRIMEPOS-2372
        /// <summary>
        /// Returs the List of Delivery Addresses belonging to the Patient List Provided(comma seperated)
        /// </summary>
        /// <param name="commaSeparatedPatients">Comma seperated Patient Numbers</param>
        /// <returns></returns>
        public DataSet GetPatientsDeliveryAddr(string commaSeparatedPatients)
        {
            
            try
            {
                DataSet oDsResult = null;
                oDsResult = this.oDBCont.GetPatientsDeliveryAddr(commaSeparatedPatients);
                return oDsResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region PRIMEPOS-CONSENT SAJID DHUKKA PRIMEPOS-2871
        public DataTable PopulateConsentSource()
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.PopulateConsentSource();
                return oTable;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing PopulateConsentSource");
                throw ex;
            }
        }
        public DataTable PopulateConsentNameBasedOnID(string consentId)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.PopulateConsentNameBasedOnID(consentId);
                return oTable;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing PopulateConsentSource");
                throw ex;
            }
        }
        public DataTable GetActivePatientConsent(int patientNo, System.Collections.Generic.Dictionary<int, string> activeConsentList, out bool isConsentExpired, out bool isConsentHave)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetActivePatientConsent(patientNo, activeConsentList, out isConsentExpired, out isConsentHave);
                return oTable;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing GetActivePatientConsent");
                throw ex;
            }

        }
        public DataTable GetActiveConsentTypeById(int ConsentSourceID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetActiveConsentTypeById(ConsentSourceID);
                return oTable;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing GetActivePatientConsent");
                throw ex;
            }

        }
        public DataTable GetActiveConsentStatusById(int ConsentSourceID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetActiveConsentStatusById(ConsentSourceID);
                return oTable;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An Error Occured while executing GetActivePatientConsent");
                throw ex;
            }

        }
        public DataTable GetConsentRelationshipById(int ConsentSourceID)
        {
            try
            {
                return this.oDBCont.GetConsentRelationshipById(ConsentSourceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetConsentTextById(int ConsentSourceID)
        {
            try
            {
                return this.oDBCont.GetConsentTextById(ConsentSourceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Customer Engagement Details PRIMEPOS-2794 SAJID
        public DataSet GetPatMedAdherence(int patientNo)
        {
            try
            {
                DataSet oTable = null;
                oTable = this.oDBCont.GetPatMedAdherence(patientNo);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetAllPatInsurance(string patientNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetAllPatInsurance(patientNo);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PRIMEPOS-2442 ADDED BY ROHIT NAIR
        //Added By Rohit Nair 
        public DataTable GetConsentSourceByName(string consentSourceName)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetConsentSourceByName(consentSourceName);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetConsentSourceID(string consentSourceName)
        {
            try
            {

                return this.oDBCont.GetConsentSourceID(consentSourceName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int GetConsentTextID(int ConsentSourceID, int languageno)
        {
            try
            {
               
                return this.oDBCont.GetConsentTextID(ConsentSourceID, languageno);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetConsentTypeID(int ConsentSourceID, string typeCode)
        {
            try
            {

                return this.oDBCont.GetConsentTypeID(ConsentSourceID, typeCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetConsentStatusID(int ConsentSourceID, string StatusCode)
        {
            try
            {

                return this.oDBCont.GetConsentStatusID(ConsentSourceID, StatusCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetConsentRelationShipID(int ConsentSourceID, string RelationShipString)
        {
            try
            {

                return this.oDBCont.GetConsentRelationShipID(ConsentSourceID, RelationShipString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region PRIMEPOS-3276
        public bool isConsentActiveForPatient(int PatientNo, int ConsentSourceID)
        {
            try
            {
                isConsentActive = this.oDBCont.isConsentActiveForPatient(PatientNo, ConsentSourceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isConsentActive;
        }
        #endregion

        //PRIMEPOS-2761 change void to bool
        public bool SavePatientConsent(int PatientNo, int ConsentTextID, int ConsentTypeID, int ConsentStatusID, DateTime ConsentCaptureDate, DateTime ConsentEffectiveDate, DateTime ConsentEndDate, int RelationID, string SigneeName, byte[] SignatureData, int ConsentSourceID = 1)
        {
            try
            {

                isRxTxnSuccess = this.oDBCont.SavePatientConsent(PatientNo, ConsentTextID, ConsentTypeID, ConsentStatusID, ConsentCaptureDate, ConsentEffectiveDate, ConsentEndDate, RelationID, SigneeName, SignatureData, ConsentSourceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isRxTxnSuccess;
        }

        //PRIMEPOS-3192
        public long SaveAndGetIDPatientConsent(int PatientNo, int ConsentTextID, int ConsentTypeID, int ConsentStatusID, DateTime ConsentCaptureDate, DateTime ConsentEffectiveDate, DateTime ConsentEndDate, int RelationID, string SigneeName, byte[] SignatureData, int ConsentSourceID = 1)
        {
            long patientConsentID = 0;
            try
            {

                patientConsentID = this.oDBCont.SaveAndGetIDPatientConsent(PatientNo, ConsentTextID, ConsentTypeID, ConsentStatusID, ConsentCaptureDate, ConsentEffectiveDate, ConsentEndDate, RelationID, SigneeName, SignatureData, ConsentSourceID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return patientConsentID;
        }

        //PRIMEPOS-3120
        public int GetConsentValidityPeriod(int ConsentSourceID, int StatusID)
        {
            try
            {
                return this.oDBCont.GetConsentValidityPeriod(ConsentSourceID, StatusID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetLastPatientConsent(int PatientNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetLastPatientConsent(PatientNo);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetLastPatientConsent(int PatientNo,string ConsentSourceName)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetLastPatientConsent(PatientNo, ConsentSourceName);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PrimePOS-2448 Added BY Rohit Nair

        public DataTable GetIntakeBatchByBatchID(long BatchID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetIntakeBatchByBatchID(BatchID);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long GetBatchIDFromRxno(long Rxno, int Nrefill)
        {
            try
            {

                return this.oDBCont.GetBatchIDFromRxno(Rxno, Nrefill);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long GetBatchIDFromPatno(int PatientNo)
        {
            try
            {

                return this.oDBCont.GetBatchIDFromPatno(PatientNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetAllBatchesForPatient(int PatientNo)
        {
            try
            {
                string result = string.Empty;
                result = this.oDBCont.GetAllBatchesForPatient(PatientNo);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetIntakeQueueByBatchID(long BatchID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetIntakeQueueByBatchID(BatchID);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRXOFSInfo(long Rxno, int Nrefill)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetRXOFSInfo(Rxno, Nrefill);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAllRXOFSInfofromBatchID(long BatchID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetAllRXOFSInfofromBatchID(BatchID);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetRxsinBatch(long BatchID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetRxsinBatch(BatchID);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable GetBatchStatusfromView(string BatchID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetBatchStatusfromView(BatchID);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public DataTable GetCustomerAuthIDTypes()
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetCustomerAuthIDTypes();
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveRxPickupLog(DataTable rxListing, DataTable custInfo)
        {
            try
            {
                this.oDBCont.SaveRxPickupLog(rxListing, custInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet LoadPatientRxsByFilledDate(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return this.oDBCont.LoadPatientRxsByFilledDate(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetCompoundDrugNames(long rxNo, int refillNo)
        {
            try
            {
                return this.oDBCont.GetCompoundDrugNames(rxNo, refillNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet LoadPatientInvoiceByTxnDate(int patientNo, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return this.oDBCont.LoadPatientInvoiceByTxnDate(patientNo, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveQB_PatientCustomer(DataTable patientTbl)
        {
            try
            {
                this.oDBCont.SaveQB_PatientCustomer(patientTbl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveQB_RxItem(ref DataTable itemTbl)
        {
            try
            {
                this.oDBCont.SaveQB_RxItem(ref itemTbl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveQB_PatientInvoice(int QBRefNumber, decimal invoiceBalanceDue, DateTime invoiceDueDate, DataTable itemTbl)
        {
            try
            {
                this.oDBCont.SaveQB_PatientInvoice(QBRefNumber, invoiceBalanceDue, invoiceDueDate, itemTbl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet LoadFacilityRxsByFilledDate(DateTime FromDate, DateTime ToDate, string facilityFilter)
        {
            try
            {
                return this.oDBCont.LoadFacilityRxsByFilledDate(FromDate, ToDate, facilityFilter);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveQB_FacilityAccount(DataTable patientTbl)
        {
            try
            {
                this.oDBCont.SaveQB_FacilityAccount(patientTbl);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveQB_FacilityInvoice(int QBRefNumber, decimal invoiceBalanceDue, DateTime invoiceDueDate, DataTable itemTbl, string accountName, string facilityCode)
        {
            try
            {
                this.oDBCont.SaveQB_FacilityInvoice(QBRefNumber, invoiceBalanceDue, invoiceDueDate, itemTbl, accountName, facilityCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet LoadFacilityInvoiceByTxnDate(string FacilityCode, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return this.oDBCont.LoadFacilityInvoiceByTxnDate(FacilityCode, FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet LoadAttachedDocumentByScanDate(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return this.oDBCont.LoadAttachedDocumentByScanDate(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region PRIMEPOS-2587 29-Nov-2018 JY Added
        public DataTable GetHCAccountDetailsByPosTransId(long POSTRANSID)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetHCAccountDetailsByPosTransId(POSTRANSID);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public Int32 GetNumOfQBItemsByDates(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return this.oDBCont.GetNumOfQBItemsByDates(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 GetNumOfQBInvoicesByDates(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return this.oDBCont.GetNumOfQBInvoicesByDates(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable LoadQBItemsByCreationDate(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return this.oDBCont.LoadQBItemsByCreationDate(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable LoadQBInvoicesByCreationDate(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                return this.oDBCont.LoadQBInvoicesByCreationDate(FromDate, ToDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteFrom_QB_RxItem(string whereClause)
        {
            try
            {
                return this.oDBCont.DeleteFrom_QB_RxItem(whereClause);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteFrom_QB_PatientInvoice(string whereClause)
        {
            try
            {
                return this.oDBCont.DeleteFrom_QB_PatientInvoice(whereClause);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Batch Delivery - NileshJ - PRIMERX-7688 - 25-Sept-2019
        public DataTable GetBatchDeliveryDetails(string BatchNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetBatchDeliveryDetails(BatchNo);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBatchDeliveryPatient(string BatchNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetBatchDeliveryPatient(BatchNo);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetBatchDeliveryRx(string BatchNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetBatchDeliveryRx(BatchNo);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBatchDeliveryPaymentStatus(DataTable dtBatchDeliveryData)
        {
            try
            {
                this.oDBCont.UpdateBatchDeliveryPaymentStatus(dtBatchDeliveryData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateBatchDeliveryOrderPaymentStatus(DataTable dtBatchDeliveryData)
        {
            try
            {
                this.oDBCont.UpdateBatchDeliveryOrderPaymentStatus(dtBatchDeliveryData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBatchDeliveryDetailPaymentStatus(DataTable dtBatchDeliveryData)
        {
            try
            {
                this.oDBCont.UpdateBatchDeliveryDetailPaymentStatus(dtBatchDeliveryData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBatchDeliveryData(DateTime dFillDateFrom, DateTime dFillDateTo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetBatchDeliveryData(dFillDateFrom, dFillDateTo);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PRIMEPOS-2760 18-Nov-2019 JY Added
        public bool RxExistsInPrimeRxDb(string sRxNo, string sRefNo, string sPartialFillNo = "0")
        {
            try
            {
                return this.oDBCont.RxExistsInPrimeRxDb(sRxNo, sRefNo, sPartialFillNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PRIMEPOS-2761

        public DataSet FetchRxDetails(string RxNo, string Nrefill, string PartialFillNo = "0")
        {
            return this.oDBCont.FetchRxDetails(RxNo, Nrefill, PartialFillNo);
        }
        public bool RollbackRxUpdate(DataSet dsRxData)
        {
            return this.oDBCont.RollbackRxUpdate(dsRxData);
        }
        #endregion

        #region PRIMEESC-36 24-Jul-2020 JY Added
        public DataTable GetAutoUpdateServiceURL()
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetAutoUpdateServiceURL();
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region PRIMEPOS-3207
        public DataTable GetHyphenSettings()
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetHyphenSettings();
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatAllIns(string PatientNo)
        {
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatAllIns(PatientNo);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region PRIMEPOS-1637 26-May-2021 JY Added
        public DataTable GetDrugClass(string sRxNo, string sRefill)
        {
            DataTable oTable = null;
            try
            {
                oTable = this.oDBCont.GetDrugClass(sRxNo, sRefill);
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public bool ConnectedTo_ePrimeRx()
        {
            if (this.oDBCont.GetDBType() == DBController.DbType.WebServiceDB)
                return true;
            else
                return false;
        }

        public DataTable GetRxsWithStatus(string sRxNo, string sRefNo, String billStatus, int iPartialFillNo)
        {
            DataTable dt = null;
            try
            {
                if (this.oDBCont.GetDBType() == DBController.DbType.WebServiceDB)
                {
                    dt = this.oDBCont.GetRxDetails(sRxNo, sRefNo, billStatus, iPartialFillNo);
                }
                else if (this.oDBCont.GetDBType() == DBController.DbType.SqlDB)
                {
                    dt = this.oDBCont.GetRxs(sRxNo, sRefNo, billStatus);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public void GetPatient(string sPatNo, out DataSet ds)
        {
            ds = null;
            try
            {
                DataTable oTable = null;
                oTable = this.oDBCont.GetPatient(sPatNo);
                ds = new DataSet();
                if (oTable != null)
                {
                    DataTable dt = oTable.Copy();
                    ds.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveDetails_ePrimeRx(DataTable dtRxTransMissionData, DataTable dtSigTransData, DataTable dtRxData)
        {
            bool rtnCode = false;

            try
            {
                rtnCode = this.oDBCont.SaveDetails_ePrimeRx(dtRxTransMissionData, dtSigTransData, dtRxData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtnCode;
        }

        #region PRIMEPOS-3060 01-Mar-2022 JY Added
        public DataTable GetInvalidSign(string sFilter)
        {
            DataTable oTable = null;
            try
            {
                oTable = this.oDBCont.GetInvalidSign(sFilter);                
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return oTable;
        }

        public DataTable GetInvalidSign(string sFilter, int nTop)
        {
            DataTable oTable = null;
            try
            {
                oTable = this.oDBCont.GetInvalidSign(sFilter, nTop);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return oTable;
        }

        public void IndexReOrganizeAndRebuild()
        {
            this.oDBCont.IndexReOrganizeAndRebuild();
        }

        public void UpdateInsSigTrans(long TRANSNO, byte[] bBinarySign)
        {
            this.oDBCont.UpdateInsSigTrans(TRANSNO, bBinarySign);
        }
        #endregion

        #region PRIMEPOS-2651 07-Mar-2022 JY Added
        public Boolean IsDrugRefrigerated(string sNDC11)
        {
            return this.oDBCont.IsDrugRefrigerated(sNDC11);
        }
        #endregion

        #region PRIMEPOS-3085 05-Apr-2022 JY Added
        public Boolean UpdateInvalidSignForMultiplePatientsTrans(string TransData, string PatientNo, byte[] bBinarySign)
        {
            return this.oDBCont.UpdateInvalidSignForMultiplePatientsTrans(TransData, PatientNo, bBinarySign);
        }
        #endregion

        #region PRIMEPOS-3088 28-Apr-2022 JY Added
        public DataTable UpdateInsSigTrans(DataTable dt)
        {
            return this.oDBCont.UpdateInsSigTrans(dt);
        }
        #endregion

        #region PRIMEPOS-3091 06-May-2022 JY Added
        public DataTable GetPOSTransactionRxDetail(DataTable dt)
        {
            return this.oDBCont.GetPOSTransactionRxDetail(dt);
        }
        #endregion

        #region PRIMEPOS-3094 19-May-2022 JY Added
        public string UpdateMissingTransDet(string TransNo)
        {
            return this.oDBCont.UpdateMissingTransDet(TransNo);
        }

        public void UpdateBlankSignWithSamePatientsSign()
        {
            this.oDBCont.UpdateBlankSignWithSamePatientsSign();
        }
        #endregion

        #region PRIMEPOS-3157 28-Nov-2022 JY Added
        public DataTable GetDocSubCat(int CategoryId)
        {
            DataTable oTable = null;
            try
            {
                oTable = this.oDBCont.GetDocSubCat(CategoryId);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return oTable;
        }

        public DataTable GetDocuments(int PATIENTNO, int CategoryId, string SubCategoryIds)
        {
            DataTable oTable = null;
            try
            {
                oTable = this.oDBCont.GetDocuments(PATIENTNO, CategoryId, SubCategoryIds);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return oTable;
        }

        public int GetDocumentLocation()
        {
            int nDocumentLocation = 0;
            try
            {
                nDocumentLocation = this.oDBCont.GetDocumentLocation();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return nDocumentLocation;
        }

        public string GetDocumentPhysicalPath()
        {
            string strPath = string.Empty;
            try
            {
                strPath = this.oDBCont.GetDocumentPhysicalPath();
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return strPath;
        }

        public byte[] GetDocumentFromdb(string document)
        {
            byte[] doc = null;
            try
            {
                doc = this.oDBCont.GetDocumentFromdb(document);
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return doc;
        }
        #endregion

        #region PRIMEPOS-3192
        public DataTable GetPendingSignatureForAutoRefillConsent(string sPatientNo, string rxs)
        {
            DataTable dt = null;
            try
            {
                dt = this.oDBCont.GetPendingSignatureForAutoRefillConsent(sPatientNo, rxs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }
        public void SavePatientPrescriptionConsent(long PatientConsentID, long RxNo, string DrugNDC, int ConsentStatusID, int specificProductId)
        {
            try
            {
                this.oDBCont.SavePatientPrescriptionConsent(PatientConsentID, RxNo, DrugNDC, ConsentStatusID, specificProductId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePatientConsentSignature(long PatientConsentID, long PatientNo, byte[] SignatureData)
        {
            try
            {
                this.oDBCont.UpdatePatientConsentSignature(PatientConsentID, PatientNo, SignatureData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string checkPatientPrescriptionRecord(string RxNo)
        {
            try
            {
                string result = string.Empty;
                result = this.oDBCont.checkPatientPrescriptionRecord(RxNo);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public bool isPrescriptionConsentActive(int consentId, string strCode)//PRIMEPOS-3192N
        {
            bool rtnCode = false;

            try
            {
                rtnCode = this.oDBCont.isPrescriptionConsentActive(consentId, strCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtnCode;
        }

        public DataSet GetPhUsers(string strQuery)
        {
            DataSet ds = null;
            try
            {
                ds = this.oDBCont.GetPhUsers(strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataTable LoadPatientCounselHistory(int PatientNo)
        {
            DataTable dt = null;
            try
            {
                dt = this.oDBCont.LoadPatientCounselHistory(PatientNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public bool SavePatCounselHistoryToDB(DataTable dtHist, DataTable dtRXs)
        {
            bool rtn = false;

            try
            {
                rtn = this.oDBCont.SavePatCounselHistoryToDB(dtHist, dtRXs);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return rtn;
        }

        public string GetSettingPatientCounselingAudited()
        {
            string sValSetting = "N";
            string sSetting = GetPrimeRxSettingDetailValue(11, "PatientCounselingAudited");
            if(!string.IsNullOrWhiteSpace(sSetting))
            {
                if (sSetting.Trim().ToUpper().Substring(0, 1) == "Y")
                    sValSetting = "Y";
            }
            return sValSetting;
        }

        public string GetPrimeRxSettingDetailValue(int SettingID, string FieldName)
        {
            string sValSetting = string.Empty;

            try
            {
                sValSetting = this.oDBCont.GetPrimeRxSettingDetailValue(SettingID, FieldName);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sValSetting;
        }

        public DataTable GetPatCounselingRulesByState(string sPharmST)
        {
            DataTable dt = null;
            try
            {
                dt = this.oDBCont.GetPatCounselingRulesByState(sPharmST);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable GetLastCounselledPatientDrugInfo(int PatientNo, string sNDC)
        {
            DataTable dt = null;
            try
            {
                dt = this.oDBCont.GetLastCounselledPatientDrugInfo(PatientNo, sNDC);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public bool HasRXRefillDoseChanged(long RxNo, int RefillNo)
        {
            bool rtn = false;
            try
            {
                rtn = this.oDBCont.HasRXRefillDoseChanged(RxNo, RefillNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtn;
        }
        #region PRIMEPOS-3403
        public DataTable GetPayRecs(long lRXNO, int iRefNo)
        {
            return this.oDBCont.GetPayRecords(lRXNO, iRefNo);
        }
        public DataTable GetClaimPaymentView(long lRXNO, int iRefNo)
        {
            return this.oDBCont.GetClaimPaymentView(lRXNO, iRefNo);
        }
        public DataTable GetRxDiag(long lRXNO)
        {
            return this.oDBCont.GetRxDiag(lRXNO);
        }
        public DataTable GetRxExtra(long lRXNO, int iRefNo)
        {
            return this.oDBCont.GetRxExtra(lRXNO, iRefNo);
        }
        #endregion
    }
}