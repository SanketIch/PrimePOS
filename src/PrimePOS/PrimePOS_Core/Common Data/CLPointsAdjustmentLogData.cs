
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class CLPointsAdjustmentLogData : DataSet 
	{

		private CLPointsAdjustmentLogTable _CLPointsAdjustmentLogTable;

		#region Constructors
		public CLPointsAdjustmentLogData() 
		{
			this.InitClass();
		}

		#endregion


		public CLPointsAdjustmentLogTable CLPointsAdjustmentLog 
		{
			get 
			{
				return this._CLPointsAdjustmentLogTable;
			}
			set 
			{
				this._CLPointsAdjustmentLogTable = value;
			}
		}

		public override DataSet Clone() 
		{
			CLPointsAdjustmentLogData cln = (CLPointsAdjustmentLogData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_CLPointsAdjustmentLogTable = (CLPointsAdjustmentLogTable)this.Tables[clsPOSDBConstants.CLPointsAdjustmentLog_tbl];
			if (_CLPointsAdjustmentLogTable != null) 
			{
				_CLPointsAdjustmentLogTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = "CLPointsAdjustmentLogData";
			this.Prefix = "";


			_CLPointsAdjustmentLogTable = new CLPointsAdjustmentLogTable();
			this.Tables.Add(this.CLPointsAdjustmentLog);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.CLPointsAdjustmentLog_tbl] != null) 
			{
				this.Tables.Add(new CLPointsAdjustmentLogTable(ds.Tables[clsPOSDBConstants.CLPointsAdjustmentLog_tbl]));
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
