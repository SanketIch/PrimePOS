using System;
using System.Configuration;
using System.Data;
using NLog; //PRIMEPOS-Issue 15-Aug-2019 JY Added

namespace PharmData
{
    /// <summary>
    /// This class is responsible for determining if to use PharmFPDb class
    /// to access data or to use any other class.
    /// </summary>
    internal class DBController
    {
        ILogger logger = LogManager.GetCurrentClassLogger();    //PRIMEPOS-Issue 15-Aug-2019 JY Added
        public enum DbType
        {
            FoxProDB,
            SqlDB,
            WebServiceDB
        };

        private DbType eDbType;
        private PharmFpDB oPharmFpDB = null;
        private PharmSqlDB oPharmSqlDB = null;
        private PharmWebService oPharmWebSer = null;

        public DBController()
        {
            //
            // TODO: Add constructor logic here
            //
            SetDbSetProp();
        }

        public DbType GetDBType()
        {
            return this.eDbType;
        }

        public DataTable GetDoctor(string sDocNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetDoctor(sDocNo);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetDoctor(sDocNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFacilityDoctors(string sFacID)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetFacilityDoctors(sFacID);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetDoctor(string fname, string lname)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetDoctor(fname, lname);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetMaxAccessionNo()
        {
            int accno = 0;
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    accno = this.oPharmSqlDB.GetMaxAccessNo();
                }
            }
            catch (Exception ex)
            {
                accno = 0;
            }
            return accno.ToString();
        }

        public DataTable GetUsers(string logintype, string lastname, string firstname)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetUsers(logintype, lastname, firstname);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertLoginDetails(DataTable oTable)
        {
            /*
             * This function inserts entry into the login table
             */
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                return (this.oPharmSqlDB.insertLoginDetails(oTable));
            }
            else
                return null;
        }

        public DataTable GetDrugCategory()
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    //	oTable=this.oPharmFpDB.GetDrugCategory(sCateg);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetDrugCategory();
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDrugOrderedByDrugCatDS(string sFromDate, string sToDate, string sFacility, string sDrugCategory, string sPrescriber, string sPatient)
        {
            try
            {
                DataTable dt = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                    dt = this.oPharmSqlDB.GetDrugOrderedByDrugCatDS(sFromDate, sToDate, sFacility, sDrugCategory, sPrescriber, sPatient);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataTable GetPhDrug(string sNdc)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetPhDrug(sNdc);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetDrug(sNdc);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPhDrugName(string sName)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetPhDrugName(sName);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public string CheckDDIDUP(string sDrgNdc, string sTxrCode, long lPatNo, int iDDIDays, out DataTable oDtInteracts, out bool bFoundDupDrugs, out string sDupDrugs)
        //{
        //    try
        //    {
        //        if(this.eDbType == DBController.DbType.FoxProDB)
        //        {
        //            return this.oPharmFpDB.CheckDDIDUP(sDrgNdc, sTxrCode, lPatNo, iDDIDays, out oDtInteracts, out bFoundDupDrugs, out sDupDrugs);
        //        }
        //        else
        //        {
        //            return this.oPharmSqlDB.CheckDDIDUP(sDrgNdc, sTxrCode, lPatNo, iDDIDays, out oDtInteracts, out bFoundDupDrugs, out sDupDrugs);
        //        }
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
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    return this.oPharmFpDB.CheckAllergy(strTxrCode, strNDC, strPatAllergy, out strRetInfo);
                }
                else
                {
                    return this.oPharmSqlDB.CheckAllergy(strTxrCode, strNDC, strPatAllergy, out strRetInfo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebDrug(string sNdc)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetWebDrug(sNdc);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetDrug(sNdc);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebDrugName(string sName)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetWebDrugName(sName);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFacility(string sFacCode)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetFacility(sFacCode);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetFacility(sFacCode);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFacility()
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetFacility();
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPhInfo()
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetPhInfo();
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPhInfo();
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetPhInfo();
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPhUser(string sPhInit)
        {
            try
            {
                DataTable oTable = null;

                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetPhUser(sPhInit);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPhUser(sPhInit);
                }

                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetContsByDate(DateTime FromDate, DateTime ToDate, string sPharmId, string sProcess)
        {
            DataTable oTable = null;

            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetContsByDate(FromDate, ToDate, sPharmId, sProcess);
                }
                else
                {
                    oTable = this.oPharmSqlDB.GetContsByDate(FromDate, ToDate, sPharmId, sProcess);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oTable;
        }

        public DataTable GetConstant()
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetConstant();
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetConstant();
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebLoginByUserName(string sUserName, string sPassword)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetWebLoginByUserName(sUserName, sPassword);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetWebLoginByUserName(sUserName, sPassword);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebLoginByUserName(string sUserName)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetWebLoginByUserName(sUserName);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebLoginByID(string sLoginId)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetWebLoginByID(sLoginId);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetWebLoginByType(string sLoginType)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetWebLoginByLoginType(sLoginType);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFaq(string sFaqNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetFaq(sFaqNo);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetFaq(sFaqNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDocPat(string sFacNo, string sDocNo, string sPatFname, string sPatLname)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetDocPat(sDocNo, sFacNo, sPatFname, sPatLname);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetDocPat(sDocNo, sFacNo, sPatFname, sPatLname);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxLabel(string sRxNo, string sRefNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    //oTable=this.oPharmFpDB.GetPatHistory(sPatNo,sDocNo,odtStartDate,odtEndDate,bIncludeDiscontinued);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.getRxLabel(sRxNo, sRefNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatHistory(string sPatNo, string sDocNo, System.DateTime odtStartDate, System.DateTime odtEndDate, bool bIncludeDiscontinued)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetPatHistory(sPatNo, sDocNo, odtStartDate, odtEndDate, bIncludeDiscontinued);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatHistory(sPatNo, sDocNo, odtStartDate, odtEndDate, bIncludeDiscontinued);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatHistory(string sDocNo, System.DateTime odtStartDate, System.DateTime odtEndDate, bool bIncludeDiscontinued)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    //oTable=this.oPharmFpDB.GetPatHistory(sDocNo,odtStartDate,odtEndDate,bIncludeDiscontinued);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatHistory(sDocNo, odtStartDate, odtEndDate, bIncludeDiscontinued);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatHistory(string sDocNo, System.DateTime odtStartDate, System.DateTime odtEndDate, string rxno1, string rxno2, bool bIncludeDiscontinued)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    //oTable=this.oPharmFpDB.GetPatHistory(sDocNo,odtStartDate,odtEndDate,rxno1,rxno2,bIncludeDiscontinued);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatHistory(sDocNo, odtStartDate, odtEndDate, rxno1, rxno2, bIncludeDiscontinued);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetUser(string username)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetUser(username);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatient(string sPatNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetPatient(sPatNo);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatient(sPatNo);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetPatient(sPatNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Sprint-25 - PRIMEPOS-2322 01-Feb-2017 JY Added logic to get all patients w.r.t. patients FamilyId
        public DataTable GetPatientByFamilyID(int iFamilyID)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatientByFamilyID(iFamilyID);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetPatientByFamilyID(iFamilyID);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// This function Get the Patient Notes that is mark with POPUP
        /// </summary>
        /// Author: Manoj Kumar 1/28/2013
        /// <param name="sPatNo"></param>
        /// <returns>DataTable</returns>
        public DataTable GetPatientNotes(string sPatNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatientNotes(sPatNo);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetPatientNotes(sPatNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This function Get the RX Notes that is mark with POPUP
        /// </summary>
        /// Author: Manoj Kumar 1/28/2013
        /// <param name="sRxNo"></param>
        /// <returns>DataTable</returns>
        public DataTable GetRxNotes(string sRxNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRxNotes(sRxNo);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetRxNotes(sRxNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatientPayPref(string sPatNo, string sPaytype)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatientPayPref(sPatNo, sPaytype);
                }
                return oTable;
            }
            catch (Exception ex)
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatientPayPrefByAccNo(sAccNo, sPayType);
                }
                return oTable;
            }
            catch (Exception ex)
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatientPayPrefByPatientNo(sPatientNo, sPayType);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatientByAccNo(sAccNo);
                }
                return oTable;
            }
            catch (Exception ex)
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatientByRxNo(sRxNo);
                }
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
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = null;
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPrivackAck(sPatNo);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetPrivackAck(sPatNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPrivackAckAndConsentInfo(string sPatNo, int consentType)
        {
            try
            {
                DataSet oDS = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oDS = null;
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oDS = this.oPharmSqlDB.GetPrivackAckAndConsentInfo(sPatNo, consentType);
                }
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
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oDS = null;
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oDS = this.oPharmSqlDB.GetConsentText(consentSource);
                }
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
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oDS = null;
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oDS = this.oPharmSqlDB.GetConsentReferenceData();
                }
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
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetPatientByName(lname, fname, bRemoveWebLogin);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDoctorsByName(string lname, string fname, bool bRemoveWebLogin)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetDoctorsByName(lname, fname, bRemoveWebLogin);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetFacilityByName(string sName, bool bRemoveWebLogin)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetFacilityByName(sName, bRemoveWebLogin);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetLoginDetails(string dtFrom, string dtTo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetLoginDetails(dtFrom, dtTo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertFAQ(DataTable oTable)
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.InsertFAQ(oTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertNewRxOrd(DataTable oTable)
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    return this.oPharmFpDB.InsertNewRxOrd(oTable);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.InsertNewRxOrd(oTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";
        }

        public void InsertContact(DataTable oTable)
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.InsertContact(oTable);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.InsertContact(oTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertRxPickUpLog(DataTable oTable)
        {
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.InsertRxPickUpLog(oTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxQueue(string sRxNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetRxQueue(sRxNo);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRxQueue(sRxNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetInsInfo(string sIns)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetInsInfo(sIns);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetInsInfo(sIns);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertAccess(DataTable oTable)
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.InsertAccess(oTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAccess(DataTable oTable)
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.UpdateAccess(oTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string InsertAccess1(DataTable oTable)
        {
            /*
             * i am duplicating the function to get the ses_no for the time being
             */
            string res = null;
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    res = this.oPharmFpDB.InsertAccess1(oTable);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    res = this.oPharmSqlDB.InsertAccess(oTable);
                }
            }
            catch (Exception ex)
            {
                res = null;
            }
            return res;
        }

        public bool checkRandomPassword(string username, string password)
        {
            return this.oPharmSqlDB.checkRandomPassword(username, password);
        }

        public DataTable GetAccess(string sUserName, string sLogoutDt)
        {
            DataTable oTable = new DataTable();

            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                    oTable = this.oPharmFpDB.GetAccess(sUserName, sLogoutDt);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oTable;
        }

        public string UpdateLogin(string username, string password)
        {
            /*
             * this function updates the login table with the new random generated password
             */
            return this.oPharmSqlDB.UpdateLogin(username, password);
        }

        public string InsertLogin(DataTable oTable)
        {
            /*
             * This function inserts entry into the login table
             */
            string res = "Success";
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.InsertLogin(oTable);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.UpdateLogin(oTable);
                }
            }
            catch (Exception ex)
            {
                res = null;
            }
            return res;
        }

        public void InsertAdvImg(DataTable oTable)
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.InsertAdvImg(oTable);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Hit()
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.Hit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertRxQue(DataTable oTable)
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.InsertRxQue(oTable);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                    this.oPharmSqlDB.InsertRxQue(oTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Added By Manoj 9/18/2012 - This is to mark the CopayPaid in Pharmsql.
        public void MarkCopayPaid(string sRxNo, string sRefNo, char val, string sPartialFillNo = "0")
        {
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.MarkCopayPaid(sRxNo, sRefNo, val);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    this.oPharmWebSer.MarkCopayPaid(sRxNo, sRefNo, sPartialFillNo, val);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DiscontinueRx(DataTable dtDCRxs)
        {
            string res = "Success";
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    res = this.oPharmSqlDB.DiscontinueRx(dtDCRxs);
                }
            }
            catch (Exception ex)
            {
                res = ex.Message;
            }
            return res;
        }

        public DataTable GetWebSet(string sSetType)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetWebSet(sSetType);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetWebSet(sSetType);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCategory(string sCateg)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetCategory(sCateg);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetCounseling(string sNdc, string sType)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetCounseling(sNdc, sType);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetCounselling(sNdc, sType);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetAdvImg(string sAdvImg)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetAdvImg(sAdvImg);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetAdvImg(sAdvImg);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxsByPatient(string sPatNo, string sBillStatus)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetRxsByPatient(sPatNo);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRxsByPatient(sPatNo, sBillStatus);
                }
                else
                {
                }

                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private double CalcPrice(DataRow oRow)
        {
            double dAmount = this.GetProtectiveDouble(oRow["AMOUNT"].ToString());

            double dPfee = this.GetProtectiveDouble(oRow["PFEE"].ToString());

            double dOthAmt = this.GetProtectiveDouble(oRow["OTHAMT"].ToString());

            double dOthFee = this.GetProtectiveDouble(oRow["OTHFEE"].ToString());

            double dSTax = this.GetProtectiveDouble(oRow["STAX"].ToString());

            double dDiscount = this.GetProtectiveDouble(oRow["DISCOUNT"].ToString());

            return ((dAmount + dPfee + dOthAmt + dOthFee + dSTax) - dDiscount);
        }

        private double GetProtectiveDouble(string sValue)
        {
            double dReturn = 0;
            try
            {
                dReturn = Convert.ToDouble(sValue);
            }
            catch { }
            return dReturn;
        }

        private double CalcCopay(DataRow oRow)
        {
            double dReturn = 0;

            if (oRow["BILLTYPE"].ToString().TrimEnd() == "C" || oRow["PATTYPE"].ToString().TrimEnd() == "C")
            {
                double dAmount = this.GetProtectiveDouble(oRow["AMOUNT"].ToString());
                double dPFee = this.GetProtectiveDouble(oRow["PFEE"].ToString());
                double dDiscount = this.GetProtectiveDouble(oRow["DISCOUNT"].ToString());
                dReturn = (dAmount + dPFee) - dDiscount;
            }
            else
            {
                double dCopay = this.GetProtectiveDouble(oRow["COPAY"].ToString());
                if (dCopay > 0)
                    dReturn = dCopay;
            }

            return dReturn;
        }

        public string GetLastRefill(string sRxNo)
        {
            string sNoRefill = string.Empty;
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                sNoRefill = this.oPharmSqlDB.GetLastRefill(sRxNo);
            }
            else if (this.eDbType == DBController.DbType.WebServiceDB)
            {
                sNoRefill = this.oPharmWebSer.GetLastRefill(sRxNo);
            }
            return sNoRefill;
        }

        public void MarkDelivery(string sRxNo, string sRefNo, string sDelivery, string sPickedUp, System.DateTime PickUpDate, string sPickUpPOS, out string sError, bool isBatchDelivery = false, string sPartialFillNo = "0")// PRIMERX-7688 - Added isBatchDelivery - NileshJ - 23-Sept-2019
        {
            sError = "";
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.MarkDelivery(sRxNo, sRefNo, sDelivery, sPickedUp, PickUpDate, out sError);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.MarkDelivery(sRxNo, sRefNo, sDelivery, sPickedUp, PickUpDate, sPickUpPOS, isBatchDelivery); // PRIMERX-7688 - NileshJ - Added isBatchDelivery - 23-Sept-2019
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    this.oPharmWebSer.MarkDelivery(sRxNo, sRefNo, sPartialFillNo, sDelivery, sPickedUp, PickUpDate, sPickUpPOS, out sError, isBatchDelivery);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DoesRxExist(string sRxNo, string sRefNo)
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    return this.oPharmFpDB.DoesRxExist(sRxNo, sRefNo);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.DoesRxExist(sRxNo, sRefNo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public void SaveTransDet(long lTransNo, long lRxNo, int iRefNo, string sPartialFillNo = "0")
        {
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                this.oPharmSqlDB.SaveTransDet(lTransNo, lRxNo, iRefNo);
            }
            else if (this.eDbType == DBController.DbType.WebServiceDB)
            {
                this.oPharmWebSer.SaveTransDet(lTransNo, lRxNo, iRefNo, sPartialFillNo);
            }
        }

        public void SavePrivacyAck(long lPatNo, System.DateTime dtSigned, string sPatAccept, string sPrivacyText, string sSignature, string sSigType)
        {
            this.oPharmSqlDB.SavePrivacyAck(lPatNo, dtSigned, sPatAccept, sPrivacyText, sSignature, sSigType);
        }

        public void SavePrivacyAck(long lPatNo, System.DateTime dtSigned, string sPatAccept, string sPrivacyText, string sSignature, string sSigType, byte[] bBinarySign)
        {
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                this.oPharmSqlDB.SavePrivacyAck(lPatNo, dtSigned, sPatAccept, sPrivacyText, sSignature, sSigType, bBinarySign);
            }
            else if (this.eDbType == DBController.DbType.WebServiceDB)
            {
                this.oPharmWebSer.SavePrivacyAck(lPatNo, dtSigned, sPatAccept, sPrivacyText, sSignature, sSigType, bBinarySign);
            }
        }

        public DataTable GetExtPatByDate(DateTime FromDate, DateTime ToDate, string sPharmId, string sProcess)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = oPharmFpDB.GetExtPatByDate(FromDate, ToDate, sPharmId, sProcess);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertExtPat(DataTable oTable)
        {
            try
            {
                if (this.eDbType == DBController.DbType.FoxProDB)
                    this.oPharmFpDB.InsertExtPat(oTable);
                else if (this.eDbType == DBController.DbType.SqlDB)
                    this.oPharmSqlDB.InsertExtPat(oTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SavePatientAck(long lPatNo, string sAck, DateTime dtAckDate)
        {
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.SavePatientAck(lPatNo, sAck, dtAckDate);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    this.oPharmWebSer.SavePatientAck(lPatNo, sAck, dtAckDate);
                }
                else if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB.SavePatientAck(lPatNo, sAck, dtAckDate);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public long SaveInsSigTrans(System.DateTime dtTransDate, long lPatNo, string sInsType, string sTransData, string sSignature, string sCounselingReq, string sSigType)
        {
            return this.oPharmSqlDB.SaveInsSigTrans(dtTransDate, lPatNo, sInsType, sTransData, sSignature, sCounselingReq, sSigType);
        }

        public long SaveInsSigTrans(System.DateTime dtTransDate, long lPatNo, string sInsType, string sTransData, string sSignature, string sCounselingReq, string sSigType, byte[] bBinarySign)
        {
            long rtn = 0;
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                rtn = this.oPharmSqlDB.SaveInsSigTrans(dtTransDate, lPatNo, sInsType, sTransData, sSignature, sCounselingReq, sSigType, bBinarySign);
            }
            else if (this.eDbType == DBController.DbType.WebServiceDB)
            {
                rtn = this.oPharmWebSer.SaveInsSigTrans(dtTransDate, lPatNo, sInsType, sTransData, sSignature, sCounselingReq, sSigType, bBinarySign);
            }
            return rtn;
        }

        public string GetBillStatus(string sRxNo, string sRefNo)
        {
            return this.oPharmSqlDB.GetBillStatus(sRxNo, sRefNo);
        }

        public DataTable GetRxsAllFields(string sRxNo, string sRefNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetRxsALLFields(sRxNo, sRefNo);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRxs(sRxNo, sRefNo, "");
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetLastRefill(string sRxNo, string sRefNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetLastRefill(sRxNo, sRefNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxs(string sRxNo, string sRefNo, string sBillStatus)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetRxs(sRxNo, sRefNo);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRxs(sRxNo, sRefNo, sBillStatus);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetRxs(sRxNo, sRefNo, sBillStatus);
                }
                return oTable;
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
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = null;
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, sBillStatus, cType);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, sBillStatus, cType);
                }
                return oTable;
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
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = null; //this.oPharmFpDB.GetRxs(sRxNo,sRefNo);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, sBillStatus, IsBatchDelivery);  //NileshJ - PRIMERX - 7688 - Added IsBatchDelivery - 23-Sept-2019
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetRxs(sPatientNo, dFillDateFrom, dFillDateTo, sBillStatus, IsBatchDelivery);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxsByDate(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    oTable = this.oPharmFpDB.GetRxsByDate(FromDate, ToDate);
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRxsByDate(FromDate, ToDate);
                }
                else
                {
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SetDbSetProp()
        {
            /*
             * This function will determine which database class to use
             * by looking at the application configuration file
             * and instantiate that class it will also set the property dbtype
             * which will be used throughout to determine which db to goto.
             * */
            try
            {
                string sWebUrl = "";
                string sWebToken = "";
                AppSettingsReader oAppSetRead = new AppSettingsReader();

                //this.eDbType = (DbType)oAppSetRead.GetValue("DBTYPE",typeof(int));

                string sDbType = (string)oAppSetRead.GetValue("DBTYPEPHARM", typeof(string));

                if (sDbType.Equals("SQL") || sDbType.Equals("1"))
                {
                    this.eDbType = DbType.SqlDB;
                }
                else if (sDbType.Equals("FOXPRO") || sDbType.Equals("0"))
                {
                    this.eDbType = DbType.FoxProDB;
                }
                else if (sDbType.Equals("WebServiceDB", StringComparison.OrdinalIgnoreCase) || sDbType.Equals("2"))
                {
                    this.eDbType = DbType.WebServiceDB;
                    sWebUrl = (string)oAppSetRead.GetValue("ePrimeRxURL", typeof(string));
                    sWebToken = (string)oAppSetRead.GetValue("ePrimeRxToken", typeof(string));
                }

                string sPath = "";
                string sFdbPath = "";
                if (this.eDbType == DBController.DbType.FoxProDB)
                {
                    this.oPharmFpDB = new PharmFpDB();
                    System.Type oStrType = System.Type.GetType("System.String");
                    sPath = oAppSetRead.GetValue("FilePath", oStrType).ToString();
                    sFdbPath = oAppSetRead.GetValue("FDBFilePath", oStrType).ToString();

                    if (sPath.Substring(sPath.Length - 1, 1) != "\\")
                        sPath += "\\";

                    if (sFdbPath.Substring(sFdbPath.Length - 1, 1) != "\\")
                        sFdbPath += "\\";

                    oPharmFpDB.Access = sPath + oAppSetRead.GetValue("AccessFile", oStrType).ToString();
                    oPharmFpDB.Claims = sPath + oAppSetRead.GetValue("ClaimsFile", oStrType).ToString();
                    oPharmFpDB.Constant = sPath + oAppSetRead.GetValue("ConstantFile", oStrType).ToString();
                    oPharmFpDB.Df0001 = sPath + oAppSetRead.GetValue("DF0001File", oStrType).ToString();
                    oPharmFpDB.Drug = sPath + oAppSetRead.GetValue("DrugFile", oStrType).ToString();
                    oPharmFpDB.Facility = sPath + oAppSetRead.GetValue("FacilityFile", oStrType).ToString();
                    oPharmFpDB.FAQ = sPath + oAppSetRead.GetValue("FAQFile", oStrType).ToString();
                    oPharmFpDB.InsFile = sPath + oAppSetRead.GetValue("InsFile", oStrType).ToString();
                    oPharmFpDB.NewRxOrd = sPath + oAppSetRead.GetValue("NewRxOrdFile", oStrType).ToString();
                    oPharmFpDB.Patient = sPath + oAppSetRead.GetValue("PatientFile", oStrType).ToString();
                    oPharmFpDB.PatPres = sPath + oAppSetRead.GetValue("PatPresFile", oStrType).ToString();
                    oPharmFpDB.PhUser = sPath + oAppSetRead.GetValue("PhUserFile", oStrType).ToString();
                    oPharmFpDB.Prescrib = sPath + oAppSetRead.GetValue("PrescribFile", oStrType).ToString();
                    oPharmFpDB.RxRefQue = sPath + oAppSetRead.GetValue("RxRefQueFile", oStrType).ToString();
                    oPharmFpDB.WebContact = sPath + oAppSetRead.GetValue("WebContactFile", oStrType).ToString();
                    oPharmFpDB.WebDrugs = sPath + oAppSetRead.GetValue("WebDrugsFile", oStrType).ToString();
                    oPharmFpDB.WebLogin = sPath + oAppSetRead.GetValue("WebLoginFile", oStrType).ToString();
                    oPharmFpDB.WebSet = sPath + oAppSetRead.GetValue("WebSetFile", oStrType).ToString();
                    oPharmFpDB.RxSig = sPath + oAppSetRead.GetValue("RxSigFile", oStrType).ToString();
                    oPharmFpDB.AdvImg = sPath + oAppSetRead.GetValue("AdvImgFile", oStrType).ToString();
                    oPharmFpDB.WebInfo = sPath + oAppSetRead.GetValue("WebInfo", oStrType).ToString();
                    oPharmFpDB.ExtrnPat = sPath + oAppSetRead.GetValue("ExtrnPat", oStrType).ToString();

                    oPharmFpDB.PcLong = sFdbPath + oAppSetRead.GetValue("PcLong", oStrType).ToString();
                    oPharmFpDB.PcShort = sFdbPath + oAppSetRead.GetValue("PcShort", oStrType).ToString();

                    this.oPharmSqlDB = new PharmSqlDB();
                }
                else if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB = new PharmSqlDB();
                    try
                    {
                        //logger.Trace("SetDbSetProp() Connection string - " + this.oPharmSqlDB.db.ConnectString);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
                        logger.Trace("SetDbSetProp() Connection string - " + this.oPharmSqlDB.db.DBServer + " - " + this.oPharmSqlDB.db.Catalog);  //PRIMEPOS-Issue 16-Aug-2019 JY Added
                    }
                    catch (Exception Ex)
                    {
                        //logger.Fatal(Ex, "SetDbSetProp() - Connection string Error - ");
                    }
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    this.oPharmWebSer = new PharmWebService(sWebUrl, sWebToken);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPharmacyMessage()
        {
            try
            {
                DataTable pharMsg = null;
                try
                {
                    if (this.eDbType == DBController.DbType.SqlDB)
                    {
                        pharMsg = this.oPharmSqlDB.GetPharmacyMessage();
                    }
                }
                catch (Exception ex)
                {
                }
                return pharMsg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPatInsurance(string sInsID)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatInsurance(sInsID);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SaveMessages(string sPharMsg, string sInsMsg, string sPhmSigData, string sInsSig, string sPatientNo, long sTransNo)
        {
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.SaveMessages(sPharMsg, sInsMsg, sPhmSigData, sInsSig, sPatientNo, sTransNo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Added By Rohit Nair For PRIMEPOS-2372
        public DataSet GetPatientsDeliveryAddr(string commaSeparatedPatients)
        {
            try
            {
                DataSet odsResult = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    odsResult = this.oPharmSqlDB.GetPatientsDeliveryAddr(commaSeparatedPatients);


                }
                return odsResult;
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.PopulateConsentSource();
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.PopulateConsentSource();
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable PopulateConsentNameBasedOnID(string consentId)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.PopulateConsentNameBasedOnID(consentId);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.PopulateConsentNameBasedOnID(consentId);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetActivePatientConsent(int PatientNo, System.Collections.Generic.Dictionary<int, string> activeConsentList, out bool isConsentExpired, out bool isConsentHave)
        {
            try
            {
                DataTable oTable = null;
                isConsentExpired = false;
                isConsentHave = false;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetActivePatientConsent(PatientNo, activeConsentList, out isConsentExpired, out isConsentHave);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetActivePatientConsent(PatientNo, activeConsentList, out isConsentExpired, out isConsentHave);
                }

                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetActiveConsentTypeById(int ConsentSourceID)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetActiveConsentTypeById(ConsentSourceID);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetActiveConsentTypeById(ConsentSourceID);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetActiveConsentStatusById(int ConsentSourceID)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetActiveConsentStatusById(ConsentSourceID);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetActiveConsentStatusById(ConsentSourceID);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetConsentRelationshipById(int ConsentSourceID)
        {
            try
            {

                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetConsentRelationshipById(ConsentSourceID);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetConsentRelationshipById(ConsentSourceID);
                }
                else
                {
                    return new DataTable();
                }

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

                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetConsentTextById(ConsentSourceID);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetConsentTextById(ConsentSourceID);
                }
                else
                {
                    return new DataTable();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Customer Engagement Details PRIMEPOS-2794 SAJID DHUKKA
        public DataSet GetPatMedAdherence(int patientNo)
        {
            try
            {
                DataSet oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatMedAdherence(patientNo);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetPatMedAdherence(patientNo);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetAllPatInsurance(patientNo);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetAllPatInsurance(patientNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PRIMEPOS-2442 ADDED BY ROHIT NAIR
        public DataTable GetConsentSourceByName(string consentSourceName)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetConsentSourceByName(consentSourceName);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetConsentSourceByName(consentSourceName);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetConsentSourceID(consentSourceName);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetConsentSourceID(consentSourceName);
                }
                else
                {
                    return -1;
                }
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

                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetConsentTextID(ConsentSourceID, languageno);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetConsentTextID(ConsentSourceID, languageno);
                }
                else
                {
                    return -1;
                }

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

                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetConsentTypeID(ConsentSourceID, typeCode);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetConsentTypeID(ConsentSourceID, typeCode);
                }
                else
                {
                    return -1;
                }

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

                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetConsentStatusID(ConsentSourceID, StatusCode);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetConsentStatusID(ConsentSourceID, StatusCode);
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //PRIMEPOS-3120
        public int GetConsentValidityPeriod(int ConsentSourceID, int StatusID)
        {
            try
            {

                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetConsentValidityPeriod(ConsentSourceID, StatusID);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetConsentValidityPeriod(ConsentSourceID, StatusID);
                }
                else
                {
                    return -1;
                }

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

                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetConsentRelationShipID(ConsentSourceID, RelationShipString);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetConsentRelationShipID(ConsentSourceID, RelationShipString);
                }
                else
                {
                    return -1;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region PRIMEPOS-3276
        public bool isConsentActiveForPatient(int PatientNo, int ConsentSourceID)
        {
            bool isActiveConsent = false;
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    isActiveConsent = this.oPharmSqlDB.isConsentActiveForPatient(PatientNo, ConsentSourceID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isActiveConsent;
        }
        #endregion

        //PRIMEPOS-2761 - void to bool
        public bool SavePatientConsent(int PatientNo, int ConsentTextID, int ConsentTypeID, int ConsentStatusID, DateTime ConsentCaptureDate, DateTime ConsentEffectiveDate, DateTime ConsentEndDate, int RelationID, string SigneeName, byte[] SignatureData, int ConsentSourceID)
        {
            bool isRxTxSuccess = false;
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    isRxTxSuccess = this.oPharmSqlDB.SavePatientConsent(PatientNo, ConsentTextID, ConsentTypeID, ConsentStatusID, ConsentCaptureDate, ConsentEffectiveDate, ConsentEndDate, RelationID, SigneeName, SignatureData, ConsentSourceID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isRxTxSuccess;
        }

        //PRIMEPOS-3192

        public long SaveAndGetIDPatientConsent(int PatientNo, int ConsentTextID, int ConsentTypeID, int ConsentStatusID, DateTime ConsentCaptureDate, DateTime ConsentEffectiveDate, DateTime ConsentEndDate, int RelationID, string SigneeName, byte[] SignatureData, int ConsentSourceID)
        {
            long patientConsentID = 0;
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    patientConsentID = this.oPharmSqlDB.SaveAndGetIDPatientConsent(PatientNo, ConsentTextID, ConsentTypeID, ConsentStatusID, ConsentCaptureDate, ConsentEffectiveDate, ConsentEndDate, RelationID, SigneeName, SignatureData, ConsentSourceID);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return patientConsentID;
        }

        public DataTable GetLastPatientConsent(int PatientNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetLastPatientConsent(PatientNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetLastPatientConsent(int PatientNo, string ConsentSourceName)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetLastPatientConsent(PatientNo, ConsentSourceName);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetIntakeBatchByBatchID(BatchID);
                }

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

                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetBatchIDFromRxno(Rxno, Nrefill);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    return this.oPharmWebSer.GetBatchIDFromRxno(Rxno, Nrefill);
                }
                else
                {
                    return -1;
                }

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

                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    return this.oPharmSqlDB.GetBatchIDFromPatno(PatientNo);
                }
                else
                {
                    return -1;
                }

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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    result = this.oPharmSqlDB.GetAllBatchesForPatient(PatientNo);
                }

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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetIntakeQueueByBatchID(BatchID);
                }

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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRXOFSInfo(Rxno, Nrefill);
                }

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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetAllRXOFSInfofromBatchID(BatchID);
                }

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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetRxsinBatch(BatchID);
                }

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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetBatchStatusfromView(BatchID);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetBatchStatusfromView(BatchID);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetCustomerAuthIDTypes();
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.SaveRxPickupLog(rxListing, custInfo);
                }
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
                DataSet dset = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    dset = this.oPharmSqlDB.LoadPatientRxsByFilledDate(FromDate, ToDate);
                }

                return dset;
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
                string compDesc = string.Empty;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    compDesc = this.oPharmSqlDB.GetCompoundDrugNames(rxNo, refillNo);
                }

                return compDesc;
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
                DataSet dset = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    dset = this.oPharmSqlDB.LoadPatientInvoiceByTxnDate(patientNo, FromDate, ToDate);
                }

                return dset;
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.SaveQB_PatientCustomer(patientTbl);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.SaveQB_RxItem(ref itemTbl);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.SaveQB_PatientInvoice(QBRefNumber, invoiceBalanceDue, invoiceDueDate, itemTbl);
                }
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
                DataSet dset = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    dset = this.oPharmSqlDB.LoadFacilityRxsByFilledDate(FromDate, ToDate, facilityFilter);
                }

                return dset;
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.SaveQB_FacilityAccount(patientTbl);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    this.oPharmSqlDB.SaveQB_FacilityInvoice(QBRefNumber, invoiceBalanceDue, invoiceDueDate, itemTbl, accountName, facilityCode);
                }
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
                DataSet dset = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    dset = this.oPharmSqlDB.LoadAttachedDocumentByScanDate(FromDate, ToDate);
                }

                return dset;
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
                DataSet dset = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    dset = this.oPharmSqlDB.LoadFacilityInvoiceByTxnDate(FacilityCode, FromDate, ToDate);
                }

                return dset;
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetHCAccountDetailsByPosTransId(POSTRANSID);
                }

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
            int numOfItems = 0;
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    numOfItems = this.oPharmSqlDB.GetNumOfQBItemsByDates(FromDate, ToDate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return numOfItems;
        }

        public Int32 GetNumOfQBInvoicesByDates(DateTime FromDate, DateTime ToDate)
        {
            int numOfInvoices = 0;
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    numOfInvoices = this.oPharmSqlDB.GetNumOfQBInvoicesByDates(FromDate, ToDate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return numOfInvoices;
        }

        public DataTable LoadQBItemsByCreationDate(DateTime FromDate, DateTime ToDate)
        {
            DataTable dt = null;
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    dt = this.oPharmSqlDB.LoadQBItemsByCreationDate(FromDate, ToDate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public DataTable LoadQBInvoicesByCreationDate(DateTime FromDate, DateTime ToDate)
        {
            DataTable dt = null;
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    dt = this.oPharmSqlDB.LoadQBInvoicesByCreationDate(FromDate, ToDate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        public bool DeleteFrom_QB_RxItem(string whereClause)
        {
            bool rtn = false;

            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    rtn = this.oPharmSqlDB.DeleteFrom_QB_RxItem(whereClause);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtn;
        }

        public bool DeleteFrom_QB_PatientInvoice(string whereClause)
        {
            bool rtn = false;

            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    rtn = this.oPharmSqlDB.DeleteFrom_QB_PatientInvoice(whereClause);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtn;
        }

        #region Batch Delivery - NileshJ - PRIMERX-7688 - 23-Sept-2019
        public DataTable GetBatchDeliveryDetails(string batchNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetBatchDeliveryDetails(batchNo);
                }

                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBatchDeliveryPatient(string batchNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetBatchDeliveryPatient(batchNo);
                }

                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetBatchDeliveryRx(string batchNo)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetBatchDeliveryRx(batchNo);
                }

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
                this.oPharmSqlDB.UpdateBatchDeliveryPaymentStatus(dtBatchDeliveryData);
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
                this.oPharmSqlDB.UpdateBatchDeliveryOrderPaymentStatus(dtBatchDeliveryData);
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
                this.oPharmSqlDB.UpdateBatchDeliveryDetailPaymentStatus(dtBatchDeliveryData);
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetBatchDeliveryData(dFillDateFrom, dFillDateTo);
                }

                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PRIMEPOS-2760 18-Nov-2019 JY Added
        public bool RxExistsInPrimeRxDb(string sRxNo, string sRefNo, string sPartialFillNo)
        {
            bool bStatus = false;
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    bStatus = this.oPharmSqlDB.RxExistsInPrimeRxDb(sRxNo, sRefNo);
                }
                else if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    bStatus = true;  //this.oPharmWebSer.RxExistsIn_ePrimeRxDb(sRxNo, sRefNo, sPartialFillNo);
                }
                return bStatus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PRIMEPOS-2761

        public DataSet FetchRxDetails(string RxNo, string Nrefill, string PartialFillNo)
        {
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                return this.oPharmSqlDB.FetchRxDetails(RxNo, Nrefill);
            }
            else if (this.eDbType == DBController.DbType.WebServiceDB)
            {
                return this.oPharmWebSer.FetchRxDetails(RxNo, Nrefill, PartialFillNo);
            }
            else
                return null;
        }
        public bool RollbackRxUpdate(DataSet dsRxData)
        {
            return this.oPharmSqlDB.RollbackRxUpdate(dsRxData);
        }
        #endregion

        #region PRIMEESC-36 24-Jul-2020 JY Added
        public DataTable GetAutoUpdateServiceURL()
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetAutoUpdateServiceURL();
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetHyphenSettings();
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetPatAllIns(PatientNo);
                }
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
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetDrugClass(sRxNo, sRefill);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public DataTable GetRxDetails(string sRxNo, string sRefNo, string sBillStatus, int iPartialFillNo)
        {
            try
            {
                DataTable oTable = null;

                if (this.eDbType == DBController.DbType.WebServiceDB)
                {
                    oTable = this.oPharmWebSer.GetRxDetails(sRxNo, sRefNo, sBillStatus, iPartialFillNo);
                }
                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetRxForDelivery(string sRxNo, out string sStatus, string RefillNo, string PartialFillNo)
        {
            sStatus = "false";
            DataTable dt = new DataTable();
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                dt = this.oPharmSqlDB.GetRxForDelivery(sRxNo, out sStatus, RefillNo);
            }
            else if (this.eDbType == DBController.DbType.WebServiceDB)
            {
                dt = this.oPharmWebSer.GetRxForDelivery(sRxNo, out sStatus, RefillNo, PartialFillNo);
            }

            return dt;
        }

        public int GetLastPartialFillNo(string sRxNo, string sRefNo)
        {
            int iPartialFillNo = 0;

            try
            {
                if (this.eDbType == DbType.WebServiceDB)
                {
                    iPartialFillNo = this.oPharmWebSer.GetLastPartialFillNo(sRxNo, sRefNo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return iPartialFillNo;
        }

        public bool SaveDetails_ePrimeRx(DataTable dtRxTransMissionData, DataTable dtSigTransData, DataTable dtRxData)
        {
            bool rtnCode = false;

            try
            {
                if (this.eDbType == DbType.WebServiceDB)
                {
                    rtnCode = this.oPharmWebSer.SaveDetails_ePrimeRx(dtRxTransMissionData, dtSigTransData, dtRxData);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetInvalidSign(sFilter);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetInvalidSign(sFilter, nTop);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return oTable;
        }

        public void IndexReOrganizeAndRebuild()
        {
            this.oPharmSqlDB.IndexReOrganizeAndRebuild();
        }

        public void UpdateInsSigTrans(long TRANSNO, byte[] bBinarySign)
        {
            this.oPharmSqlDB.UpdateInsSigTrans(TRANSNO, bBinarySign);
        }
        #endregion

        #region PRIMEPOS-2651 07-Mar-2022 JY Added
        public Boolean IsDrugRefrigerated(string sNDC11)
        {
            try
            {
                if (this.eDbType == DbType.SqlDB)
                    return this.oPharmSqlDB.IsDrugRefrigerated(sNDC11);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        #endregion

        #region PRIMEPOS-3085 05-Apr-2022 JY Added
        public Boolean UpdateInvalidSignForMultiplePatientsTrans(string TransData, string PatientNo, byte[] bBinarySign)
        {
            Boolean rtn = false;
            try
            {
                if (this.eDbType == DbType.SqlDB)
                    return this.oPharmSqlDB.UpdateInvalidSignForMultiplePatientsTrans(TransData, PatientNo, bBinarySign);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtn;
        }
        #endregion

        #region PRIMEPOS-3088 28-Apr-2022 JY Added
        public DataTable UpdateInsSigTrans(DataTable dt)
        {
            DataTable tbl = null;
            try
            {
                if (this.eDbType == DbType.SqlDB)
                    return this.oPharmSqlDB.UpdateInsSigTrans(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tbl;
        }
        #endregion

        #region PRIMEPOS-3091 06-May-2022 JY Added
        public DataTable GetPOSTransactionRxDetail(DataTable dt)
        {
            DataTable tbl = null;
            try
            {
                if (this.eDbType == DbType.SqlDB)
                    return this.oPharmSqlDB.GetPOSTransactionRxDetail(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tbl;
        }
        #endregion

        #region PRIMEPOS-3094 19-May-2022 JY Added
        public string UpdateMissingTransDet(string TransNo)
        {
            string rtnStr = string.Empty;
            try
            {
                if (this.eDbType == DbType.SqlDB)
                    return this.oPharmSqlDB.UpdateMissingTransDet(TransNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rtnStr;
        }

        public void UpdateBlankSignWithSamePatientsSign()
        {
            try
            {
                if (this.eDbType == DbType.SqlDB)
                    this.oPharmSqlDB.UpdateBlankSignWithSamePatientsSign();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region PRIMEPOS-3157 28-Nov-2022 JY Added
        public DataTable GetDocSubCat(int CategoryId)
        {
            DataTable oTable = null;
            try
            {
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetDocSubCat(CategoryId);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    oTable = this.oPharmSqlDB.GetDocuments(PATIENTNO, CategoryId, SubCategoryIds);
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    nDocumentLocation = this.oPharmSqlDB.GetDocumentLocation();
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    strPath = this.oPharmSqlDB.GetDocumentPhysicalPath();
                }
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
                if (this.eDbType == DBController.DbType.SqlDB)
                {
                    doc = this.oPharmSqlDB.GetDocumentFromdb(document);
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            return doc;
        }
        #endregion

        //PRIMEPOS-3192
        public DataTable GetPendingSignatureForAutoRefillConsent(string sPatientNo, string rxs)
        {
            try
            {
                DataTable oTable = null;
                if (this.eDbType == DbType.SqlDB)
                    oTable = this.oPharmSqlDB.GetPendingSignatureForAutoRefillConsent(sPatientNo, rxs);

                return oTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //PRIMEPOS-3192
        public void SavePatientPrescriptionConsent(long PatientConsentID, long RxNo, string DrugNDC, int ConsentStatusID, int specificProductId)
        {
            try
            {
                if (this.eDbType == DbType.SqlDB)
                    this.oPharmSqlDB.SavePatientPrescriptionConsent(PatientConsentID, RxNo, DrugNDC, ConsentStatusID, specificProductId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //PRIMEPOS-3192
        public void UpdatePatientConsentSignature(long PatientConsentID, long PatientNo, byte[] SignatureData)
        {
            try
            {
                if (this.eDbType == DbType.SqlDB)
                    this.oPharmSqlDB.UpdatePatientConsentSignature(PatientConsentID, PatientNo, SignatureData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //PRIMEPOS-3192
        public string checkPatientPrescriptionRecord(string RxNo)
        {
            try
            {
                string result = string.Empty;
                if (this.eDbType == DbType.SqlDB)
                    result = this.oPharmSqlDB.checkPatientPrescriptionRecord(RxNo);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool isPrescriptionConsentActive(int consentId, string strCode)//PRIMEPOS-3192N
        {
            bool rtnCode = false;

            try
            {
                if (this.eDbType == DbType.SqlDB)
                    rtnCode = this.oPharmSqlDB.isPrescriptionConsentActive(consentId, strCode);
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
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                ds = this.oPharmSqlDB.GetPhUsers(strQuery);
            }
            return ds;
        }

        public DataTable LoadPatientCounselHistory(int PatientNo)
        {
            DataTable dt = null;
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                dt = this.oPharmSqlDB.LoadPatientCounselHistory(PatientNo);
            }
            return dt;
        }

        public bool SavePatCounselHistoryToDB(DataTable dtHist, DataTable dtRXs)
        {
            bool rtn = false;
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                rtn = this.oPharmSqlDB.SavePatCounselHistoryToDB(dtHist, dtRXs);
            }
            return rtn;
        }

        public string GetPrimeRxSettingDetailValue(int SettingID, string FieldName)
        {
            string sValSetting = string.Empty;

            if (this.eDbType == DBController.DbType.SqlDB)
            {
                sValSetting = this.oPharmSqlDB.GetPrimeRxSettingDetailValue(SettingID, FieldName);
            }
            return sValSetting;
        }

        public DataTable GetPatCounselingRulesByState(string sPharmST)
        {
            DataTable dt = null;
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                dt = this.oPharmSqlDB.GetPatCounselingRulesByState(sPharmST);
            }
            return dt;
        }

        public DataTable GetLastCounselledPatientDrugInfo(int PatientNo, string sNDC)
        {
            DataTable dt = null;
            if (this.eDbType == DBController.DbType.SqlDB)
            {
                dt = this.oPharmSqlDB.GetLastCounselledPatientDrugInfo(PatientNo, sNDC);
            }
            return dt;
        }

        public bool HasRXRefillDoseChanged(long RxNo, int RefillNo)
        {
            bool rtn=false;

            if (this.eDbType == DBController.DbType.SqlDB)
            {
                rtn = this.oPharmSqlDB.HasRXRefillDoseChanged(RxNo, RefillNo);
            }
            return rtn;
        }
        #region PRIMEPOS-3403
        public DataTable GetPayRecords(long lRXNO, int iRefNo)
        {
            return this.oPharmSqlDB.GetRxPayRec(lRXNO, iRefNo);
        }
        public DataTable GetClaimPaymentView(long lRXNO, int iRefNo)
        {
            return this.oPharmSqlDB.GetClaimPaymentView(lRXNO, iRefNo);
        }
        public DataTable GetRxDiag(long lRXNO)
        {
            return this.oPharmSqlDB.GetRxDiag(lRXNO);
        }
        public DataTable GetRxExtra(long lRXNO, int iRefNo)
        {
            return this.oPharmSqlDB.GetRxExtra(lRXNO, iRefNo);
        }
        #endregion
    }
}