
   namespace POS_Core.CommonData.Rows {
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class InvTransTypeRow : DataRow {
		private InvTransTypeTable table;

		internal InvTransTypeRow(DataRowBuilder rb) : base(rb) 
		{
			this.table = (InvTransTypeTable)this.Table;
		}
		#region Public Properties

		public System.Int32 ID
		{
			get { 
				try { 
					return (System.Int32)this[this.table.ID];
				}
					catch{ 
						return 0 ; 
				}
			} 
			set { this[this.table.ID] = value; }
		}

		public System.String TypeName
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.TypeName];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.TypeName] = value; }
		}

		public System.String UserID
		{
			get 
			{ 
				try 
				{ 
					return (System.String)this[this.table.UserID];
				}
				catch
				{ 
					return System.String.Empty ; 
				}
			} 
			set { this[this.table.UserID] = value; }
		}

		public System.Int16 TransType
		{
			get { 
				try { 
					return (System.Int16)this[this.table.TransType];
				}
					catch{ 
						return 0; 
				}
			} 
			set { this[this.table.TransType] = value; }
		}

		#endregion 
	}
}
