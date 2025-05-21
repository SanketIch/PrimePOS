
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemComboPricingData : DataSet 
	{

		private ItemComboPricingTable _ItemComboPricingTable;

		#region Constructors
		public ItemComboPricingData() 
		{
			this.InitClass();
		}

		#endregion

		public ItemComboPricingTable ItemComboPricing 
		{
			get 
			{
				return this._ItemComboPricingTable;
			}
			set 
			{
				this._ItemComboPricingTable = value;
			}
		}

		public override DataSet Clone() 
		{
			ItemComboPricingData cln = (ItemComboPricingData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization

		internal void InitVars() 
		{
			_ItemComboPricingTable = (ItemComboPricingTable)this.Tables[clsPOSDBConstants.ItemComboPricing_tbl];
			if (_ItemComboPricingTable != null) 
			{
				_ItemComboPricingTable.InitVars();
			}
		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.ItemComboPricing_tbl;
			this.Prefix = "";
			_ItemComboPricingTable = new ItemComboPricingTable();
			this.Tables.Add(this.ItemComboPricing);
		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.ItemComboPricing_tbl] != null) 
			{
				this.Tables.Add(new ItemComboPricingTable(ds.Tables[clsPOSDBConstants.ItemComboPricing_tbl]));
			}
            
			this.DataSetName = ds.DataSetName;
			this.Prefix = ds.Prefix;
			this.Namespace = ds.Namespace;
			this.Locale = ds.Locale;
			this.CaseSensitive = ds.CaseSensitive;
			this.EnforceConstraints = ds.EnforceConstraints;
			this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
			this.InitVars();
		}

		#endregion

		private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) 
		{
			if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) 
			{
				this.InitVars();
			}
		}


	}

}
