
namespace POS_Core.CommonData.Tables 
{
    using System;
    using System.Data;
    using POS_Core.CommonData.Tables;
    using POS_Core.CommonData.Rows;
    using Resources;

    //using POS.Resources;

    public class ItemComboPricingTable : DataTable, System.Collections.IEnumerable 
	{

		private DataColumn colId;
		private DataColumn colDescription;
		private DataColumn colForceGroupPricing;
		private DataColumn colComboItemPrice;
        private DataColumn colMinComboItems;
        private DataColumn colIsActive;
        private DataColumn colMaxComboItems; //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added 

        #region Constructors 
        internal ItemComboPricingTable() : base(clsPOSDBConstants.ItemComboPricing_tbl) { this.InitClass(); }
		internal ItemComboPricingTable(DataTable table) : base(table.TableName) {}
		#endregion

		#region Properties

		public int Count 
		{
			get 
			{
				return this.Rows.Count;
			}
		}

		public ItemComboPricingRow this[int index] 
		{
			get 
			{
				return ((ItemComboPricingRow)(this.Rows[index]));
			}
		}

		public DataColumn Id 
		{
			get 
			{
				return this.colId;
			}
		}

		public DataColumn Description
		{
			get 
			{
				return this.colDescription;
			}
		}

		public DataColumn ForceGroupPricing 
		{
			get 
			{
				return this.colForceGroupPricing;
			}
		}

		public DataColumn ComboItemPrice
		{
			get 
			{
				return this.colComboItemPrice;
			}
		}

        public DataColumn MinComboItems
        {
            get
            {
                return this.colMinComboItems;
            }
        }

        public DataColumn IsActive
        {
            get
            {
                return this.colIsActive;
            }
        }

        #region Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added
        public DataColumn MaxComboItems
        {
            get
            {
                return this.colMaxComboItems;
            }
        }
        #endregion

        #endregion //Properties

        #region Add and Get Methods 

        public void AddRow(ItemComboPricingRow row) 
		{
			AddRow(row, false);
		}

		public  void AddRow(ItemComboPricingRow row, bool preserveChanges) 
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
		
        public ItemComboPricingRow AddRow( System.Int32 id
										, System.String description
										, System.Boolean forceGroupPricing
                                        , System.Decimal comboItemPrice
                                        , System.Int32 MinComboItems
                                        , Boolean IsActive) 
		{
			ItemComboPricingRow row = (ItemComboPricingRow)this.NewRow();
			row.Id=id;
			row.Description=description;
			row.ForceGroupPricing=forceGroupPricing;
            row.ComboItemPrice = comboItemPrice;
            row.MinComboItems = MinComboItems;
            row.IsActive = IsActive;
			this.Rows.Add(row);
			return row;
		}

		public ItemComboPricingRow GetRowByID(System.Int32 Id) 
		{
			return (ItemComboPricingRow)this.Rows.Find(new object[] {Id});
		}

		public  void MergeTable(DataTable dt) 
		{ 
      
			ItemComboPricingRow row;
			foreach(DataRow dr in dt.Rows) 
			{
				row = (ItemComboPricingRow)this.NewRow();

				row[clsPOSDBConstants.ItemComboPricing_Fld_Id] = Convert.ToInt32((dr[clsPOSDBConstants.ItemComboPricing_Fld_Id].ToString()=="")?"0":dr[0].ToString());
                row[clsPOSDBConstants.ItemComboPricing_Fld_Description] = Convert.ToString(dr[clsPOSDBConstants.ItemComboPricing_Fld_Description].ToString());
                row[clsPOSDBConstants.ItemComboPricing_Fld_ForceGroupPricing] = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.ItemComboPricing_Fld_ForceGroupPricing].ToString());
                row[clsPOSDBConstants.ItemComboPricing_Fld_ComboItemPrice] = Configuration.convertNullToDecimal(dr[clsPOSDBConstants.ItemComboPricing_Fld_ComboItemPrice].ToString());
                row[clsPOSDBConstants.ItemComboPricing_Fld_MinComboItems] = Configuration.convertNullToInt(dr[clsPOSDBConstants.ItemComboPricing_Fld_MinComboItems].ToString());
                row[clsPOSDBConstants.ItemComboPricing_Fld_IsActive] = Configuration.convertNullToBoolean(dr[clsPOSDBConstants.ItemComboPricing_Fld_IsActive].ToString());
                row[clsPOSDBConstants.ItemComboPricing_Fld_MaxComboItems] = Configuration.convertNullToInt(dr[clsPOSDBConstants.ItemComboPricing_Fld_MaxComboItems].ToString());  //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added

                this.AddRow(row);
			}
		}
		
		#endregion 
		
        public override DataTable Clone() 
		{
			ItemComboPricingTable cln = (ItemComboPricingTable)base.Clone();
			cln.InitVars();
			return cln;
		}
		protected override DataTable CreateInstance() 
		{
			return new ItemComboPricingTable();
		}

		internal void InitVars() 
		{
			this.colId = this.Columns[clsPOSDBConstants.ItemComboPricing_Fld_Id];
			this.colDescription = this.Columns[clsPOSDBConstants.ItemComboPricing_Fld_Description];
            this.colComboItemPrice = this.Columns[clsPOSDBConstants.ItemComboPricing_Fld_ComboItemPrice];
			this.colForceGroupPricing= this.Columns[clsPOSDBConstants.ItemComboPricing_Fld_ForceGroupPricing];
            this.colMinComboItems= this.Columns[clsPOSDBConstants.ItemComboPricing_Fld_MinComboItems];
            this.colIsActive = this.Columns[clsPOSDBConstants.ItemComboPricing_Fld_IsActive];
            this.colMaxComboItems = this.Columns[clsPOSDBConstants.ItemComboPricing_Fld_MaxComboItems];   //Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added
        }
		public System.Collections.IEnumerator GetEnumerator() 
		{
			return this.Rows.GetEnumerator();
		}

		private void InitClass() 
		{
			this.colId = new DataColumn(clsPOSDBConstants.ItemComboPricing_Fld_Id, typeof(System.Int32), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colId);
			this.colId.AllowDBNull = true;

			this.colDescription = new DataColumn(clsPOSDBConstants.ItemComboPricing_Fld_Description, typeof(System.String), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colDescription);
			this.colDescription.AllowDBNull = true;

			this.colForceGroupPricing= new DataColumn(clsPOSDBConstants.ItemComboPricing_Fld_ForceGroupPricing,typeof(System.Boolean), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colForceGroupPricing);
			this.colForceGroupPricing.AllowDBNull = true;

            this.colComboItemPrice = new DataColumn(clsPOSDBConstants.ItemComboPricing_Fld_ComboItemPrice, typeof(System.Decimal), null, System.Data.MappingType.Element);
			this.Columns.Add(this.colComboItemPrice);
			this.colComboItemPrice.AllowDBNull = true;

            this.colMinComboItems = new DataColumn(clsPOSDBConstants.ItemComboPricing_Fld_MinComboItems, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMinComboItems);
            this.colMinComboItems.AllowDBNull = true;

            this.colIsActive= new DataColumn(clsPOSDBConstants.ItemComboPricing_Fld_IsActive, typeof(System.Boolean), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colIsActive);
            this.colIsActive.AllowDBNull = false;

            #region Sprint-26 - PRIMEPOS-1857 17-Jul-2017 JY Added
            this.colMaxComboItems = new DataColumn(clsPOSDBConstants.ItemComboPricing_Fld_MaxComboItems, typeof(System.Int32), null, System.Data.MappingType.Element);
            this.Columns.Add(this.colMaxComboItems);
            this.colMaxComboItems.AllowDBNull = true;
            #endregion

            this.PrimaryKey = new DataColumn[] {this.colId};
		}
		
        public  ItemComboPricingRow NewItemComboPricingRow() 
		{
			return (ItemComboPricingRow)this.NewRow();
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder) 
		{
			return new ItemComboPricingRow(builder);
		}

		protected override System.Type GetRowType() 
		{
			return typeof(ItemComboPricingRow);
		}
	}
}
