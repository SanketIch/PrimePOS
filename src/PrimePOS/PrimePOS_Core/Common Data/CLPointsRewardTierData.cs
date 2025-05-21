
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class CLPointsRewardTierData : DataSet 
	{

		private CLPointsRewardTierTable _CLPointsRewardTierTable;

		#region Constructors
		public CLPointsRewardTierData() 
		{
			this.InitClass();
		}

		#endregion


		public CLPointsRewardTierTable CLPointsRewardTier 
		{
			get 
			{
				return this._CLPointsRewardTierTable;
			}
			set 
			{
				this._CLPointsRewardTierTable = value;
			}
		}

		public override DataSet Clone() 
		{
			CLPointsRewardTierData cln = (CLPointsRewardTierData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_CLPointsRewardTierTable = (CLPointsRewardTierTable)this.Tables[clsPOSDBConstants.CLPointsRewardTier_tbl];
			if (_CLPointsRewardTierTable != null) 
			{
				_CLPointsRewardTierTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = "CLPointsRewardTierData";
			this.Prefix = "";


			_CLPointsRewardTierTable = new CLPointsRewardTierTable();
			this.Tables.Add(this.CLPointsRewardTier);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.CLPointsRewardTier_tbl] != null) 
			{
				this.Tables.Add(new CLPointsRewardTierTable(ds.Tables[clsPOSDBConstants.CLPointsRewardTier_tbl]));
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
