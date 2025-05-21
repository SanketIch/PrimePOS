
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class WarningMessagesRow : DataRow 
	{
		private WarningMessagesTable table;

		internal WarningMessagesRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (WarningMessagesTable)this.Table;
		}
		#region Public Properties

		public System.Int32 WarningMessageID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.WarningMessageID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.WarningMessageID] = value; }
		}

		public System.String WarningMessage
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.WarningMessage];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.WarningMessage] = value; }
		}

        public System.Boolean IsActive
        {
            get
            {
                try
                {
                    return (System.Boolean)this[this.table.IsActive];
                }
                catch
                {
                    return false;
                }
            }
            set { this[this.table.IsActive] = value; }
        }

		#endregion 
	}
}
