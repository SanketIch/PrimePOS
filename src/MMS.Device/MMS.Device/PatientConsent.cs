using MMS.Device.Global;
using System;

namespace MMS.Device
{
    public class PatientConsent
    {
        #region Consent Source
        public int ConsentSourceID { set; get; }

        public string ConsentSourceName { set; get; }
        #endregion

        #region Consent Text ID
        public int ConsentTextID { set; get; }
        #endregion

        #region Consent Type
        public int ConsentTypeID { set; get; }

        public string ConsentTypeCode { set; get; }
        #endregion

        #region Consent Status
        public int ConsentStatusID { set; get; }

        public string ConsentStatusCode { set; get; }
        #endregion

        public DateTime ConsentCaptureDate { set; get; }

        public DateTime ConsentEffectiveDate { set; get; }

        public DateTime ConsentEndDate { set; get; }

        #region Patient Relationship
        public int PatConsentRelationID { set; get; }

        public string PatConsentRelationShipDescription { set; get; }
        #endregion

        public string SigneeName { set; get; }

        public byte[] SignatureData { set; get; }

        public bool IsConsentSkip { set; get; }

        public PatientConsent()
        {
            //ConsentEndDate = DateTime.MaxValue;
            IsConsentSkip = false;
        }



        public static string GetRelationshipCodeForHealthix(int userinput)
        {
            string result = string.Empty;
            switch (userinput)
            {
                case 1:
                    result = Constants.CONSENT_RELATIONSHIP_SELF;
                    break;
                case 2:
                    result = Constants.CONSENT_RELATIONSHIP_LEGAL;
                    break;
                case 3:
                    result = Constants.CONSENT_RELATIONSHIP_BOTH;
                    break;
                case 4:
                    result = Constants.CONSENT_RELATIONSHIP_OTHER;//PRIMEPOS-3146
                    break;
                default:
                    break;

            }
            return result;
        }

        public static string GetStatusCodeForHealthix(int userinput)
        {
            string result = string.Empty;
            switch (userinput)
            {
                case 1:
                    result = Constants.CONSENT_STATUS_CODE_YES;
                    break;
                case 2:
                    result = Constants.CONSENT_STATUS_CODE_EMERGENCY_ONLY;
                    break;
                case 3:
                    result = Constants.CONSENT_STATUS_CODE_NO;
                    break;
                default:
                    break;
            }
            return result;
        }


        public static string GetConsentTypeCodeForHealthix(int userinput)
        {
            string result = string.Empty;
            switch (userinput)
            {
                case 1:
                    result = Constants.CONSENT_TYPE_CODE_COMMUNITY;
                    break;
                case 2:
                    result = Constants.CONSENT_TYPE_CODE_FACILITY;
                    break;
                default:
                    break;
            }
            return result;
        }


    }
}
