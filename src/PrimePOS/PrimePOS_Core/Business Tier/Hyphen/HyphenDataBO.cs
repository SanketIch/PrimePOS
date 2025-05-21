namespace POS_Core.Business_Tier.Hyphen
{
    public class HyphenMMSDataRequestBO
    {
        public HyphenMMSConnInfo connInfo { get; set; }
        public HyphenDataRequestBO hyphenData { get; set; }
    }
    public class HyphenMMSConnInfo
    {
        //public string ClientID { get; set; }
        //public string ClientSecret { get; set; }
        //public string VendorBaseUrl { get; set; }
        //public string AuthEndpoint { get; set; }
        //public string AlertEndpoint { get; set; }
        //public string UIEndpoint { get; set; }
        public string AzureFunctionUrl { get; set; }
        public string AzureFunctionKeyCode { get; set; }
        public string appId { get; set; }
        public string source { get; set; }
        public string AppVersion { get; set; } //PRIEMPOS-3491
    }
    public class HyphenDataRequestBO
    {
        public HyphenMemberInfo memberInfo { get; set; }
        public HyphenPharmacyInfo pharmacyInfo { get; set; }
        public HyphenUserInfo userInfo { get; set; }
        public HyphenMedicationInfo medicationInfo { get; set; }
        public HyphenPrescriberInfo prescriber { get; set; }

        /*
        {
          "memberInfo": {
            "memberId": "abc123456789",       // Required string
            "firstName": "ADA",               // Required string
            "lastName": "Joseph",             // Required string
            "dob": "01/01/1950",              // Required string
            "phoneNumber": "5555555555",      // Required string
            "gender" : "M",                   // Required string. Enum class M, F, or U.
            "email": "ajoseph@gmail.com",     // Nullable string
            "address" : {
              "addressLine1" : "my_address",  // Required string
              "addressLine2" : null,          // Nullable string
              "city" : "Hartford",            // Required string
              "state" : "CT",                 // Required string
              "zip" : "11111"                 // Required string
            },
            "insurance": {
              "name": "Healthfirst Inc.",     // Required string
              "bin": "123",                   // Required string
              "pcn": "456"                    // Required string
            }
          },
          "pharmacyInfo": {
            "name": "ABC Pharmacy",           // Required string
            "npi": "1234567"                  // Required string
          },
          "userInfo": {
            "name": "John Smith",             // Required string
            "userType": "T",                  // Required string. Enum class T or P.
            "credentials": null,              // Nullable string
            "npi": "1234567",                 // Nullable string
            "userId": "JS"                    // Required string
          }
        }

                */
    }
    public class HyphenUserInfo
    {
        public string name { get; set; }
        public string userType { get; set; }
        public string credentials { get; set; }
        public string npi { get; set; }
        public string userId { get; set; }
    }
    public class HyphenPharmacyInfo
    {
        public string name { get; set; }
        public string npi { get; set; }
    }
    public class HyphenMemberInfo
    {
        /*
         "memberInfo": {
        "memberId": "abc123456789",       // Required string
        "firstName": "ADA",               // Required string
        "lastName": "Joseph",             // Required string
        "dob": "1950-01-01",              // Required string
        "phoneNumber": "555-5555-5555",   // Required string
        "gender" : "male",                // Required string
        "email": "ajoseph@gmail.com",     // Required string
        "address" : {
          "addressLine1" : "my_address",  // Required string
          "addressLine2" : null,          // Nullable string
          "city" : "Hartford",            // Required string
          "state" : "CT",                 // Required string
          "zip" : "11111"                 // Required string
        },
        "insurance": {
          "name": "Healthfirst Inc."     // Required string
        }
      }
         */
        public string memberId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string dob { get; set; }
        public string phoneNumber { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public HyphenMemberAddress address { get; set; }
        public HyphenMemberInsurance insurance { get; set; }
    }
    public class HyphenMemberAddress
    {
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
    }
    public class HyphenMedicationInfo
    {
        public string name { get; set; }
        public string ndcCode { get; set; }
        public string quantityDispensed { get; set; }
        public string rejectionCode { get; set; }

    }
    public class HyphenMemberInsurance
    {
        public string name { get; set; }
        public string bin { get; set; }
        public string pcn { get; set; }
        public string groupid { get; set; }
    }
    public class HyphenPrescriberInfo
    {

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string credentials { get; set; }
        public string npi { get; set; }
    }
    public class HyphenAlertResponseBO
    {
        public string HyphenTraceID { get; set; }
        public bool showAlert { get; set; }
        public string launchURL { get; set; }
    }
    public class HyphenAuthResponseBO
    {
        public string HyphenTraceID { get; set; }
        public string accessToken { get; set; }
        public int expiresIn { get; set; }
        public string sessionId { get; set; }
    }

    public enum enumHyphenAppId
    {
        PrimeRX,
        PrimePOS
    }

    public enum enumHyphenSource
    {
        PatientHistory,
        RxFill,
        PrimePOS
    }

    public class InsuranceSetBO 
    {
        public string bin { get; set; }
        public string pcn { get; set; }
        public string groupid { get; set; }
    }
}
