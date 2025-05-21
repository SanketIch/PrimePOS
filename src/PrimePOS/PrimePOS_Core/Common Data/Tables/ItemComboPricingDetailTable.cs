
namespace POS_Core.CommonData.Tables 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    //using POS.Resources;
    using System.Collections.Generic;
    using Resources;

    public class ItemComboPricingDetailTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colId;
		private DataColumn colItemComboPricingId;
		private DataColumn colQTY;
		private DataColumn colSalePrice;
		private DataColumn colItemID;
		private DataColumn colItemDescription;

		#region Constructors 
		internal ItemComboPricingDetailTable() : base(clsPOSDBConstants.ItemComboPricingDetail_tbl) { this.InitClass(); }
		internal ItemComboPricingDetailTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties
		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public ItemComboPricingDetailRow this[int index] 
		{
			get 
			{
				return ((ItemComboPricingDetailRow)(this.Rows[index]));
			}
		}


		public DataColumn Id 
		{
			get 
			{
				return this.colId;
			}
		}

		public DataColumn ItemComboPricingId 
		{
			get 
			{
				return this.colItemComboPricingId;
			}
		}

		public DataColumn QTY
		{
			get 
			{
				return this.colQTY;
			}
		}
		
		public DataColumn SalePrice
		{
			get 
			{
				return this.colSalePrice;
			}
		}

		public DataColumn ItemID
		{
			get 
			{
				return this.colItemID;
			}
		}

		public DataColumn ItemDescription
		{
			get 
			{
				return this.colItemDescription;
			}
		}

		#endregion //Properties

		#region Add and Get Methods 

		public  void AddRow(ItemComboPricingDetailRow row) 
		{
			AddRow(row, false);
		}
		
		public  void AddRow(ItemComboPricingDetailRow row, bool preserveChanges) 
		{
			if(this.GetRowByID(row.Id) == null) 
			{
				this.Rows.Add(row);
				if(!preserveChanges) 
				{
					row.AcceptChanges();
				}
			}
		}
		
		public ItemComboPricingDetailRow GetRowByID(System.Int32 Id) 
		{
			return (ItemComboPricingDetailRow)this.Rows.Find(new object[] {Id});
		}


		public ItemComboPricingDetailRow AddRow(System.Int32 Id 
										,System.Int32 ItemComboPricingId 
										,System.Int32 QTY
										,System.Decimal SalePrice
										, System.String  ItemID) 
		{
			ItemComboPricingDetailRow row = (ItemComboPricingDetailRow)this.NewRow();
			row.ItemArray = new object[] {Id,ItemComboPricingId,QTY,SalePrice, ItemID};

			this.Rows.Add(row);
			return row;
		}

		public ItemComboPricingDetailRow AddRow() 
		{
			ItemComboPricingDetailRow row = (ItemComboPricingDetailRow)this.NewRow();
			row.ItemArray = new object[] {0,0,0,0, ""};

            this.Rows.Add(row);
			return row;
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			ItemComboPricingDetailRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (ItemComboPricingDetailRow)this.NewRow();
				
				row[clsPOSDBConstants.ItemComboPricingDetail_Fld_Id] = Convert.ToInt32(dr[0].ToString());
				row[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID] = dr[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID].ToString();
				row[clsPOSDBConstants.Item_Fld_Description] = dr[clsPOSDBConstants.Item_Fld_Description].ToString();
                row[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId] = Configuration.convertNullToInt(dr[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId].ToString());
				row[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY] = Configuration.convertNullToInt(dr[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY].ToString());
				row[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice] = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice].ToString());

				this.AddRow(row);
			}
		}
		
		#endregion
 
		public override DataTable Clone() 
		{
			ItemComboPricingDetailTable cln = (ItemComboPricingDetailTable)base.Clone();
			cln.InitVars();
			return cln;
		}

		protected override DataTable CreateInstance() 
		{
			return new ItemComboPricingDetailTable();
		}

		internal void InitVars() 
		{
			this.colSalePrice= this.Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice];
			this.colQTY= this.Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY];
			this.colItemID= this.Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID];
			this.colItemDescription= this.Columns[clsPOSDBConstants.Item_Fld_Description];
			this.colItemComboPricingId= this.Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId];
			this.colId= this.Columns[clsPOSDBConstants.ItemComboPricingDetail_Fld_Id];			
		}

		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colId= new DataColumn(clsPOSDBConstants.ItemComboPricingDetail_Fld_Id, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colId);
			this.colId.AllowDBNull = true;
            this.colId.AutoIncrement = true;

			this.colItemComboPricingId = new DataColumn(clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemComboPricingId, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colItemComboPricingId);
			this.colItemComboPricingId.AllowDBNull = true;

			this.colQTY= new DataColumn(clsPOSDBConstants.ItemComboPricingDetail_Fld_QTY, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colQTY);
			this.colQTY.AllowDBNull = true;

			this.colSalePrice = new DataColumn(clsPOSDBConstants.ItemComboPricingDetail_Fld_SalePrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colSalePrice);
			this.colSalePrice.AllowDBNull = true;

			this.colItemID= new DataColumn(clsPOSDBConstants.ItemComboPricingDetail_Fld_ItemID, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colItemID);
			this.colItemID.AllowDBNull = true;

			this.colItemDescription= new DataColumn(clsPOSDBConstants.Item_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colItemDescription);
			this.colItemDescription.AllowDBNull = true;

            this.PrimaryKey = new DataColumn[] { this.colId };
		}

		public  ItemComboPricingDetailRow NewItemComboPricingDetailRow() 
		{
			return (ItemComboPricingDetailRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new ItemComboPricingDetailRow(builder);
		}

        #region Sprint-26 - PRIMEPOS-1857 19-Jul-2017 JY Added
        public IEnumerable<ItemComboPricingDetailRow> AsEnumerable()
        {
            foreach (ItemComboPricingDetailRow row in this.Rows)
            {
                yield return row;
            }
        }
        #endregion
    }
}
