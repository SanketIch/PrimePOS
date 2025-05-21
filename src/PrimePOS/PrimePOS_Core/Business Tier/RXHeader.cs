using MMS.Device;
using POS_Core.Resources;
//using POS.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core.BusinessRules
{
  
        public class RXHeader
        {
            public string PrivacyText;
            public string InsType;
            public string NOPPStatus;
            public bool isNOPPSignRequired = true;
            public string NOPPSignature;
            public string RXSignature;
            public string PatientNo;
            public string PatientName;
            public string PatientAddr;
            public string PatientState;
            public string CounselingRequest;
            private RXDetailList oRXDetailList;
            public byte[] bBinarySign;
            public byte[] NoppBinarySign;
            public string FamilyID; //Sprint-25 - PRIMEPOS-2322 01-Feb-2017 JY Added

            public List<PatientConsent> PatConsent { set; get; } //PRIMEPOS-CONSENT SAJID DHUKKA PRIMEPOS-2866
            public bool IsConsentRequired = false;
            public DataTable TblPatient {get; set;}
            public string sFamilyMember { get; set; }

        #region PrimePOS-2448 Added BY Rohit Nair
        public bool IsIntakeBatch;
            public long BatchID;
            #endregion

            public RXDetailList RXDetails
            {
                get {
                    if (oRXDetailList == null) {
                        oRXDetailList = new RXDetailList();
                    }
                    return oRXDetailList;
                }
            }
        }

        public class RXDetail
        {
            public long RXNo;   //PRIMEPOS-2515 17-Oct-2018 JY changed int to long
            public short RefillNo;
            public short PartialFillNo;
            public string DrugName;
            public string RxDate; //Addeded By Dharmendra SRT for displaying rxdate on hhp 30-09-08
                                  //Added By SRT(Ritesh Parekh)
                                  //Gets current value to show rx description stored in Preferances.
            public DataTable TblClaims { get; set; }
            public bool ShowRxDescription
            {
                get {
                    return (Configuration.CPOSSet.PrintRXDescription);
                }
            }
            //End Of Added By SRT(Ritesh Parekh)
        }

        public class RXHeaderList : System.Collections.Generic.List<RXHeader>
        {
            public RXHeader FindByPatient(string sPatientNo)
            {
                RXHeader retValue = null;
                foreach (RXHeader oHeader in this) {
                    if (oHeader.PatientNo == sPatientNo) {
                        retValue = oHeader;
                        break;
                    }
                }
                return retValue;
            }

            public RXDetailList FindRXDetailByRXNo(long RXNo)   //PRIMEPOS-2515 17-Oct-2018 JY changed int to long
        {
                RXDetailList retValue = null;
                foreach (RXHeader oDetail in this) {
                    retValue.AddRange(oDetail.RXDetails.FindByRXNo(RXNo, Configuration.convertNullToInt(oDetail.RXDetails[0].RefillNo.ToString()), Configuration.convertNullToInt(oDetail.RXDetails[0].PartialFillNo.ToString())));
                }
                return retValue;
            }
        }

        public class RXDetailList : System.Collections.Generic.List<RXDetail>
        {
            public RXDetail FindByRX(long RXNo, int nRefill) //PRIMEPOS-2515 17-Oct-2018 JY changed int to long
            {
                RXDetail retValue = null;
                foreach (RXDetail oDetail in this) {
                    if (oDetail.RXNo == RXNo && oDetail.RefillNo == nRefill) {
                        retValue = oDetail;
                        break;
                    }
                }
                return retValue;
            }

            public RXDetail FindByRXRefillNotInTrans(long RXNo, int nRefill)    //PRIMEPOS-2515 17-Oct-2018 JY changed int to long
            {
                RXDetail retValue = null;
                foreach (RXDetail oDetail in this) {
                    if (oDetail.RXNo == RXNo && oDetail.RefillNo == nRefill) {
                        retValue = oDetail;
                        break;
                    }
                }
                return retValue;
            }
            /// <summary>
            /// Modifeid by shitaljit : Added RefillNo is the parameter list as while searching RX item in transaction it is neccessery 
            /// To consider RefillNo specially in case we allow other Unpicked RX to be added to transaction while scanning other RX for same patient.
            /// </summary>
            /// <param name="RXNo"></param>
            /// <param name="RefillNo"></param>
            /// <returns></returns>
            public RXDetailList FindByRXNo(long RXNo, int RefillNo, int PartialFillNo)  //PRIMEPOS-2515 17-Oct-2018 JY changed int to long
            {
                RXDetailList retValue = new RXDetailList();
                foreach (RXDetail oDetail in this) {
                    if (oDetail.RXNo == RXNo && oDetail.RefillNo == RefillNo && oDetail.PartialFillNo == PartialFillNo) {
                        retValue.Add(oDetail);
                        break;
                    }
                }
                return retValue;
            }
        }




    }

