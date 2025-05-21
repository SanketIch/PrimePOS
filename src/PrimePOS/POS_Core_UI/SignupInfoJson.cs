using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
namespace POS_Core_UI
{
    //PRIMEPOS-2875
    public partial class SignupInfoJson
    {
            [JsonProperty("client_auth_key")]
            public string ClientAuthKey { get; set; }

            [JsonProperty("partner_reference_id")]
            public string PartnerReferenceId { get; set; }

            [JsonProperty("shop")]
            public SignupShop SignupShop { get; set; }

            [JsonProperty("owner")]
            public signupOwner signupOwner { get; set; }

            [JsonProperty("utm_campaign")]
            public string UtmCampaign { get; set; }
        }

        public partial class signupOwner
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }

            [JsonProperty("phone_number")]
            public string PhoneNumber { get; set; }
        }

        public partial class SignupShop
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("location")]
            public Location Location { get; set; }

            [JsonProperty("opening_hours")]
            public OpeningHours OpeningHours { get; set; }

            [JsonProperty("phone_number")]
            public string PhoneNumber { get; set; }
        }

        public partial class SignupLocation
        {
            [JsonProperty("lat")]
            public double Lat { get; set; }

            [JsonProperty("lon")]
            public double Lon { get; set; }

            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }
        }

        public partial class SignupOpeningHours
        {
            [JsonProperty("mon")]
            public SignupMinutes Mon { get; set; }

            [JsonProperty("tue")]
            public SignupMinutes Tue { get; set; }

            [JsonProperty("wed")]
            public SignupMinutes Wed { get; set; }

            [JsonProperty("thu")]
            public SignupMinutes Thu { get; set; }

            [JsonProperty("fri")]
            public SignupMinutes Fri { get; set; }

            [JsonProperty("sat")]
            public SignupMinutes Sat { get; set; }

            [JsonProperty("sun")]
            public SignupMinutes Sun { get; set; }
        }

        public partial class SignupMinutes
    {
            [JsonProperty("opening_mins")]
            
            public long OpeningMins { get; set; }

            [JsonProperty("closing_mins")]
            
            public long ClosingMins { get; set; }
        }
    }


