using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PossqlData
{
    [Table("MerchantConfig")]
    public partial class MerchantConfig
    {

        [Key,Column("Config_ID")]
        public int Config_ID { set; get; }

        [Column("User_ID")]
        public string User_ID { set; get; }

        [Column("Merchant")]
        public string Merchant { set; get; }

        [Column("Processor_ID")]
        public string Processor_ID { set; get; }

        [Column("Payment_Server")]
        public string Payment_Server { set; get; }

        [Column("Port_No")]
        public string Port_No { set; get; }

        [Column("Payment_Client")]
        public string Payment_Client { set; get; }

        [Column("Payment_ResultFile")]
        public string Payment_ResultFile { set; get; }


        [Column("Application_Name")]
        public string Application_Name { set; get; }

        [Column("XCClientUITitle")]
        public string XCClientUITitle { set; get; }

        [Column("LicenseID")]
        public string LicenseID { set; get; }

        [Column("SiteID")]
        public string SiteID { set; get; }

        [Column("DeviceID")]
        public string DeviceID { set; get; }

        [Column("URL")]
        public string URL { set; get; }

        [Column("VCBin")]
        public string VCBin { set; get; }

        [Column("MCBin")]
        public string MCBin { set; get; }

    }
}
