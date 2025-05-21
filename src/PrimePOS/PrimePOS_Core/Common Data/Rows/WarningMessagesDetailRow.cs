
namespace POS_Core.CommonData.Rows 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class WarningMessagesDetailRow : DataRow 
	{
		private WarningMessagesDetailTable table;

		internal WarningMessagesDetailRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (WarningMessagesDetailTable)this.Table;
		}
		#region Public Properties

		public System.Int32 WarningMessagesDetailID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.WarningMessagesDetailID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.WarningMessagesDetailID] = value; }
		}

		public System.Int32 WarningMessagesID
		{
			get 
			{ 
				try 
				{ 
					return (System.Int32)this[this.table.WarningMessagesID];
				}
				catch
				{ 
					return 0 ; 
				}
			} 
			set { this[this.table.WarningMessagesID] = value; }
		}

		public System.String RefObjectID
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.RefObjectID];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.RefObjectID] = value; }
		}

		public System.String RefObjectDescription
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.RefObjectDescription];
				}
				catch
				{ 
					return ""; 
				}
			} 
			set { this[this.table.RefObjectDescription] = value; }
		}

		public System.String RefObjectType
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.RefObjectType];
				}
				catch
				{ 
					return "I"; 
				}
			} 
			set { this[this.table.RefObjectType] = value; }
		}
		#endregion 
	}
}
