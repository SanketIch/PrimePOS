
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ItemComboPricingDetailData : DataSet 
	{

		private ItemComboPricingDetailTable _ItemComboPricingDetailTable;

		#region Constructors
		public ItemComboPricingDetailData() 
		{
			this.InitClass();
		}

		#endregion


		public ItemComboPricingDetailTable ItemComboPricingDetail 
		{
			get 
			{
				return this._ItemComboPricingDetailTable;
			}
			set 
			{
				this._ItemComboPricingDetailTable = value;
			}
		}

		public override DataSet Clone() 
		{
			ItemComboPricingDetailData cln = (ItemComboPricingDetailData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_ItemComboPricingDetailTable = (ItemComboPricingDetailTable)this.Tables[clsPOSDBConstants.ItemComboPricingDetail_tbl];
			if (_ItemComboPricingDetailTable != null) 
			{
				_ItemComboPricingDetailTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = clsPOSDBConstants.ItemComboPricingDetail_tbl;
			this.Prefix = "";


			_ItemComboPricingDetailTable = new ItemComboPricingDetailTable();
			this.Tables.Add(this.ItemComboPricingDetail);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.ItemComboPricingDetail_tbl] != null) 
			{
				this.Tables.Add(new ItemComboPricingDetailTable(ds.Tables[clsPOSDBConstants.ItemComboPricingDetail_tbl]));
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
