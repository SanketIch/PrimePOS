
namespace POS_Core.CommonData.Rows 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    //using POS.Resources;

    public class CLPointsRewardTierRow : DataRow 
	{
        private CLPointsRewardTierTable table;

		internal CLPointsRewardTierRow(DataRowBuilder rb) : base(rb) 
		{
            this.table = (CLPointsRewardTierTable)this.Table;
		}
		#region Public Properties

		public System.Int32 ID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.ID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.ID] = value; }
		}

		public System.String Description
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.Description];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.Description] = value; }
		}

		public System.Int32 Points
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.Points];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.Points] = value; }
		}

		public System.Decimal Discount
		{
			get 
			{ 
				try 
				{ 
					return Configuration.convertNullToDecimal(this[this.table.Discount]);
				}
				catch
				{ 
					return 0; 
				}
			} 
			set { this[this.table.Discount] = value; }
		}

        public System.Int32 RewardPeriod
        {
            get
            {
                try
                {
                    return (System.Int32)this[this.table.RewardPeriod];
                }
                catch
                {
                    return 0;
                }
            }
            set { this[this.table.RewardPeriod] = value; }
        }


		#endregion 
	}
}
