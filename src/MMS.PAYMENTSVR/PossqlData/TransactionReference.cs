using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PossqlData
{
    [Table("TransactionReference")]
    public partial class TransactionReference
    {
        [Column("id")]
        public int Id { set; get; }

        [Column("Processor")]
        public string Processor { set; get; }

        [Column("LastTransaction")]
        public string LastTransaction { set; get; }
    }
}
