using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PossqlData
{
    [DbConfigurationType(typeof(PossqlConfig))]
    public partial class Possql:DbContext
    {
        public Possql():base(DBManager.CreateConnection())
        {

        }
        public virtual DbSet<CCTransmission_Log> CCTransmission_Logs { set; get; }

        public virtual DbSet<MerchantConfig> MerchantConfigs { set; get; }

        public virtual DbSet<TransactionReference> TransactionReferences { set; get; }

        public virtual DbSet<PaymentTypes> PayTypes { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

    }
}
