
namespace POS_Core.CommonData 
{
	using System;
	using System.Data;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class POSTransPaymentData : DataSet 
	{

		private POSTransPaymentTable _POSTransPaymentTable;
        private decimal PayCash=0;

		#region Constructors
		public POSTransPaymentData() 
		{
			this.InitClass();
		}

		#endregion


		public POSTransPaymentTable POSTransPayment 
		{
			get 
			{
				return this._POSTransPaymentTable;
			}
			set 
			{
				this._POSTransPaymentTable = value;
			}
		}

		public override DataSet Clone() 
		{
			POSTransPaymentData cln = (POSTransPaymentData)base.Clone();
			cln.InitVars();
			return cln;
		}


		#region Initialization

		internal void InitVars() 
		{

			_POSTransPaymentTable = (POSTransPaymentTable)this.Tables[clsPOSDBConstants.POSTransPayment_tbl];
			if (_POSTransPaymentTable != null) 
			{
				_POSTransPaymentTable.InitVars();
			}

		}

		private void InitClass() 
		{
			this.DataSetName = "POSTransPaymentData";
			this.Prefix = "";


			_POSTransPaymentTable = new POSTransPaymentTable();
			this.Tables.Add(this.POSTransPayment);

		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.POSTransPayment_tbl] != null) 
			{
				this.Tables.Add(new POSTransPaymentTable(ds.Tables[clsPOSDBConstants.POSTransPayment_tbl]));
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
        public Decimal PaytypeCash // This is for Cash Count Total amount to payment screen cash Textbox
        {
            get
            {
                return PayCash;
            }
            set
            {
                PayCash = value;
            }
        }


	}

}
