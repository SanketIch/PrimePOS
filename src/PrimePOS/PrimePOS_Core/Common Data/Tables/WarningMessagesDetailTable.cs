
namespace POS_Core.CommonData.Tables 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class WarningMessagesDetailTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colWarningMessagesDetailID;
		private DataColumn colWarningMessagesID;
		private DataColumn colRefObjectID;
		private DataColumn colRefObjectDescription;
		private DataColumn colRefObjectType;
		

		#region Constructors 
		internal WarningMessagesDetailTable() : base(clsPOSDBConstants.WarningMessagesDetail_tbl) { this.InitClass(); }
		internal WarningMessagesDetailTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties
		
        public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public WarningMessagesDetailRow this[int index] 
		{
			get 
			{
				return ((WarningMessagesDetailRow)(this.Rows[index]));
			}
		}


		public DataColumn WarningMessagesDetailID 
		{
			get 
			{
				return this.colWarningMessagesDetailID;
			}
		}

		public DataColumn WarningMessagesID 
		{
			get 
			{
				return this.colWarningMessagesID;
			}
		}

		public DataColumn RefObjectType
		{
			get 
			{
				return this.colRefObjectType;
			}
		}

		public DataColumn RefObjectID
		{
			get 
			{
				return this.colRefObjectID;
			}
		}

		public DataColumn RefObjectDescription
		{
			get 
			{
				return this.colRefObjectDescription;
			}
		}

		#endregion //Properties

		#region Add and Get Methods 

		public  void AddRow(WarningMessagesDetailRow row) 
		{
			AddRow(row, false);
		}
		
		public  void AddRow(WarningMessagesDetailRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.WarningMessagesDetailID) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}
		
		public WarningMessagesDetailRow GetRowByID(System.Int32 WarningMessagesDetailID) 
		{
			return (WarningMessagesDetailRow)this.Rows.Find(new object[] {WarningMessagesDetailID});
		}


		public WarningMessagesDetailRow AddRow(System.Int32 WarningMessagesDetailID 
										,System.Int32 WarningMessagesID 
										, System.String  RefObjectID
										,System.String RefObjectType
                                        , System.String RefObjectDescription) 
		{
			WarningMessagesDetailRow row = (WarningMessagesDetailRow)this.NewRow();
			row.ItemArray = new object[] {WarningMessagesDetailID,WarningMessagesID,RefObjectType, RefObjectID,RefObjectDescription};

			this.Rows.Add(row);
			return row;
		}

		
        public WarningMessagesDetailRow AddRow() 
		{
			WarningMessagesDetailRow row = (WarningMessagesDetailRow)this.NewRow();
			//row.ItemArray = new object[] {0,0,0,0,0, "", "",0};
			row.ItemArray = new object[] {0,0,"", "",""};
			this.Rows.Add(row);
			return row;
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			WarningMessagesDetailRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (WarningMessagesDetailRow)this.NewRow();
				
				if (dr[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID] == DBNull.Value) 
					row[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID] = DBNull.Value;
				else
					row[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID] = Convert.ToInt32((dr[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID].ToString()=="")?"0":dr[0].ToString());
				
				if (dr[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID] == DBNull.Value) 
					row[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID] = DBNull.Value;
				else
					row[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID] = Convert.ToString((dr[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID].ToString()=="")? "":dr[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID].ToString());
				
				if (dr[clsPOSDBConstants.Item_Fld_Description] == DBNull.Value) 
					row[clsPOSDBConstants.Item_Fld_Description] = DBNull.Value;
				else
					row[clsPOSDBConstants.Item_Fld_Description] = dr[clsPOSDBConstants.Item_Fld_Description].ToString();

				if (dr[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID] == DBNull.Value) 
					row[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID] = DBNull.Value;
				else
					row[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID] = Convert.ToInt32((dr[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID].ToString()=="")?"0":dr[0].ToString());

				if (dr[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType] == DBNull.Value) 
					row[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType] = DBNull.Value;
				else
					row[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType] = Convert.ToString(dr[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType].ToString());

				this.AddRow(row);
			}
		}
		
		#endregion 
		
        public override DataTable Clone() 
		{
			WarningMessagesDetailTable cln = (WarningMessagesDetailTable)base.Clone();
			cln.InitVars();
			return cln;
		}

		protected override DataTable CreateInstance() 
		{
			return new WarningMessagesDetailTable();
		}

		internal void InitVars() 
		{
			this.colRefObjectID= this.Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID];
			this.colRefObjectDescription= this.Columns[clsPOSDBConstants.Item_Fld_Description];
			this.colWarningMessagesID= this.Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID];
			this.colWarningMessagesDetailID= this.Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID];
			this.colRefObjectType= this.Columns[clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType];
		}

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colWarningMessagesDetailID= new DataColumn(clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageDetailID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colWarningMessagesDetailID);
			this.colWarningMessagesDetailID.AllowDBNull = true;

			this.colWarningMessagesID = new DataColumn(clsPOSDBConstants.WarningMessagesDetail_Fld_WarningMessageID, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colWarningMessagesID);
			this.colWarningMessagesID.AllowDBNull = true;

            this.colRefObjectType = new DataColumn(clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectType, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colRefObjectType);
			this.colRefObjectType.AllowDBNull = true;

			this.colRefObjectID= new DataColumn(clsPOSDBConstants.WarningMessagesDetail_Fld_RefObjectID, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colRefObjectID);
			this.colRefObjectID.AllowDBNull = true;

			this.colRefObjectDescription= new DataColumn(clsPOSDBConstants.Item_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colRefObjectDescription);
			this.colRefObjectDescription.AllowDBNull = true;

			this.PrimaryKey = new DataColumn[] {this.colWarningMessagesDetailID};
		}

		public  WarningMessagesDetailRow NewWarningMessagesDetailRow() 
		{
            int DetailID=0;
            for(int i=0;i<this.Rows.Count;i++)
            {
                if (this[i].WarningMessagesDetailID>DetailID)
                {
                    DetailID=this[i].WarningMessagesDetailID;
                }
            }

            DetailID++;

			WarningMessagesDetailRow oRow =(WarningMessagesDetailRow)this.NewRow();
            oRow.WarningMessagesDetailID = DetailID;
            return oRow;

		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new WarningMessagesDetailRow(builder);
		}
	}
}
