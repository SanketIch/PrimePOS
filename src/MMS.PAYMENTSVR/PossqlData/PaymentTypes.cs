using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PossqlData
{
    [Table("PayType")]
    public class PaymentTypes
    {
        [Key]
        [Column("PayTypeID")]
        public string PayTypeID { set; get; }

        [Column("PayTypeDesc")]
        public string PayTypeDesc { set; get; }

        [Column("UserID")]
        public string UserID { set; get; }
    }
}
