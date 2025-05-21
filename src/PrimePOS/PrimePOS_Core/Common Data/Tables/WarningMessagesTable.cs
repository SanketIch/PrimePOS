
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class WarningMessagesTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colWarningMessageID;
		private DataColumn colWarningMessage;
		private DataColumn colIsActive;
		

		#region Constructors 
		internal WarningMessagesTable() : base(clsPOSDBConstants.WarningMessages_tbl) { this.InitClass(); }
		internal WarningMessagesTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public WarningMessagesRow this[int index] 
		{
			get 
			{
				return ((WarningMessagesRow)(this.Rows[index]));
			}
		}

		public DataColumn WarningMessageID 
		{
			get 
			{
				return this.colWarningMessageID;
			}
		}

		public DataColumn WarningMessage
		{
			get 
			{
				return this.colWarningMessage;
			}
		}

		public DataColumn IsActive 
		{
			get 
			{
				return this.colIsActive;
			}
		}

		#endregion //Properties

        #region Add and Get Methods 

		public  void AddRow(WarningMessagesRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(WarningMessagesRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.WarningMessageID) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}

		public WarningMessagesRow AddRow( System.Int32 WarningMessageID
										, System.String WarningMessage
										, bool  IsActive ) 
		{
			WarningMessagesRow row = (WarningMessagesRow)this.NewRow();
			row.WarningMessageID=WarningMessageID;
			row.WarningMessage=WarningMessage;
			row.IsActive=IsActive;
		
			this.Rows.Add(row);
			return row;
		}

		public WarningMessagesRow GetRowByID(System.Int32 WarningMessageID) 
		{
			return (WarningMessagesRow)this.Rows.Find(new object[] {WarningMessageID});
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			WarningMessagesRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (WarningMessagesRow)this.NewRow();

				if (dr[clsPOSDBConstants.WarningMessages_Fld_WarningMessageID] == DBNull.Value) 
					row[clsPOSDBConstants.WarningMessages_Fld_WarningMessageID] = DBNull.Value;
				else
					row[clsPOSDBConstants.WarningMessages_Fld_WarningMessageID] = Convert.ToInt32((dr[clsPOSDBConstants.WarningMessages_Fld_WarningMessageID].ToString()=="")?"0":dr[0].ToString());

				if (dr[clsPOSDBConstants.WarningMessages_Fld_WarningMessage] == DBNull.Value) 
					row[clsPOSDBConstants.WarningMessages_Fld_WarningMessage] = DBNull.Value;
				else
					row[clsPOSDBConstants.WarningMessages_Fld_WarningMessage] = Convert.ToString(dr[clsPOSDBConstants.WarningMessages_Fld_WarningMessage].ToString());

				string strField=clsPOSDBConstants.WarningMessages_Fld_IsActive;
				if (dr[strField] == DBNull.Value) 
					row[strField] = false;
				else
					row[strField] = Convert.ToBoolean(dr[strField].ToString());
				
				this.AddRow(row);
			}
		}
		
		#endregion 

		public override DataTable Clone() 
		{
			WarningMessagesTable cln = (WarningMessagesTable)base.Clone();
			cln.InitVars();
			return cln;
		}

		protected override DataTable CreateInstance() 
		{
			return new WarningMessagesTable();
		}

		internal void InitVars() 
		{
			this.colWarningMessageID = this.Columns[clsPOSDBConstants.WarningMessages_Fld_WarningMessageID];
			this.colWarningMessage = this.Columns[clsPOSDBConstants.WarningMessages_Fld_WarningMessage];
			this.colIsActive= this.Columns[clsPOSDBConstants.WarningMessages_Fld_IsActive];
		}

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colWarningMessageID = new DataColumn(clsPOSDBConstants.WarningMessages_Fld_WarningMessageID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colWarningMessageID);
			this.colWarningMessageID.AllowDBNull = true;

			this.colWarningMessage = new DataColumn(clsPOSDBConstants.WarningMessages_Fld_WarningMessage, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colWarningMessage);
			this.colWarningMessage.AllowDBNull = true;

			this.colIsActive= new DataColumn(clsPOSDBConstants.WarningMessages_Fld_IsActive,typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colIsActive);
			this.colIsActive.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] {this.colWarningMessageID};
		}

		public  WarningMessagesRow NewWarningMessagesRow() 
		{
			return (WarningMessagesRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new WarningMessagesRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(WarningMessagesRow);
		}
	}
}
