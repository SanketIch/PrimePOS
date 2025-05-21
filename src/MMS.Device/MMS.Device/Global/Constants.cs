namespace MMS.Device.Global
{
    public static class Constants
    {
        #region Consent source

        public const string CONSENT_SOURCE_HEALTHIX = "Healthix";

        public const string CONSENT_SOURCE_AUTO_REFILL = "Auto Refill";//PRIMEPOS-3192

        #endregion

        #region Consent Relationship

        public const string CONSENT_RELATIONSHIP_SELF = "Self";
        public const string CONSENT_RELATIONSHIP_LEGAL = "Legal Representative";
        public const string CONSENT_RELATIONSHIP_BOTH = "Both";
        public const string CONSENT_RELATIONSHIP_OTHER = "Other";//PRIMEPOS-3146

        #endregion

        #region Consent Status 

        public const string CONSENT_STATUS_CODE_YES = "Y";
        public const string CONSENT_STATUS_CODE_NO = "N";
        public const string CONSENT_STATUS_CODE_EMERGENCY_ONLY = "E";

        #endregion

        #region Consent Type

        public const string CONSENT_TYPE_CODE_COMMUNITY = "C";
        public const string CONSENT_TYPE_CODE_FACILITY = "F";

        #endregion
        public const string CONSENT_TYPE_CODE_PRESCRIPTION_AUTO_REFILL = "S";//PRIMEPOS-3192N
    }
}
