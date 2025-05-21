using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData
{
    public partial class OrderDetailsData : DataSet
    {
    	private OrderDetalis orderDetailsTable;
        private PODetailTable poDetailTable;

		#region Constructors
        public OrderDetailsData() 
		{
			this.InitClass();
		}
		#endregion
        public OrderDetalis OrderDetailsTable 
		{
			get 
			{
                return this.orderDetailsTable;
			}
			set 
			{
                this.orderDetailsTable = value;
			}
		}
        public PODetailTable PODetailTable
        {
            get
            {
                return this.poDetailTable;
            }
            set
            {
                this.poDetailTable = value;
            }
        }
		public override DataSet Clone() 
		{
            OrderDetailsData cln = (OrderDetailsData)base.Clone();
			cln.InitVars();
			return cln;
		}
		#region Initialization

		internal void InitVars() 
		{

            orderDetailsTable = (OrderDetalis)this.Tables[clsPOSDBConstants.OrderDetail_tbl];
            if (orderDetailsTable != null) 
			{
                orderDetailsTable.InitVars();
			}

		}

		private void InitClass() 
		{
            this.DataSetName = clsPOSDBConstants.OrderDetail_tbl;
			this.Prefix = "";


            orderDetailsTable = new OrderDetalis();
            poDetailTable = new PODetailTable();
            this.Tables.Add(this.orderDetailsTable);
            this.Tables.Add(this.poDetailTable);
		}

		private void InitClass(DataSet ds) 
		{

            if (ds.Tables[clsPOSDBConstants.OrderDetail_tbl] != null) 
			{
                this.Tables.Add(new OrderDetalis(ds.Tables[clsPOSDBConstants.OrderDetail_tbl]));
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
