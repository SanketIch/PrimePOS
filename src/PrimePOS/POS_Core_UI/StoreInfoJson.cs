using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_Core_UI
{
    //PRIMEPOS-2875
    public class StoreInfoJson
    {
        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("shop")]
        public Shop Shop { get; set; }

        [JsonProperty("partner_reference_id")]
        public string PID { get; set; }

        [JsonProperty("client_auth_key")]
        public string MMSKey { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        [JsonProperty("utm_campaign")]
        public string Campcode { get; set; }

        [JsonProperty("opening_hours")]
        public OpeningHours openhrs { get; set; }
    }
    public class Owner
    {
        [JsonProperty("name")]
        public string OwnerName { get; set; }

        [JsonProperty("email")]
        public string OwnerEmail { get; set; }

        [JsonProperty("phone_numbe")]
        public string OwnerPhNum { get; set; }

    }

    public  class Shop
    {
        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("tz")]
        public string Tz { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("opening_hours")]
        public OpeningHours OpeningHours { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("partner_reference_id")]
        public string PartnerId { get; set; }
    }

    public  class Location
    {
        [JsonProperty("lat")]
        public double Lat { get; set; }

        [JsonProperty("lon")]
        public double Lon { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }
    }

    public  class OpeningHours
    {
        [JsonProperty("mon")]
        public Minutes Mon { get; set; }
        [JsonProperty("tue")]
        public Minutes Tue { get; set; }
        [JsonProperty("wed")]
        public Minutes Wed { get; set; }
        [JsonProperty("thu")]
        public Minutes Thu { get; set; }

        [JsonProperty("fri")]
        public Minutes Fri { get; set; }

        [JsonProperty("sat")]
        public Minutes Sat { get; set; }

        [JsonProperty("sun")]
        public Minutes Sun { get; set; }
    }

    public  class Minutes
    {
        [JsonProperty("opening_mins")]
        public string OpeningMins { get; set; }

        [JsonProperty("closing_mins")]
        public string ClosingMins { get; set; }
    }

    public  class Status
    {
        [JsonProperty("code")]
        public string Code { get; set; }
    }
}
