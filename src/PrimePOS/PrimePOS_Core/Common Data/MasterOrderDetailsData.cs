using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData
{
    public partial class MasterOrderDetailsData : DataSet
    {
        private OrderDetalis orderDetailsTable;
        private PODetailTable poDetailTable;
        private MasterOrderDetailsTable masterOrderDetailsTable;
		#region Constructors
        public MasterOrderDetailsData() 
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
        public MasterOrderDetailsTable MasterOrderDetailTable
        {
            get
            {
                return this.masterOrderDetailsTable;
            }
            set
            {
                this.masterOrderDetailsTable = value;
            }
        }
		public override DataSet Clone() 
		{
            MasterOrderDetailsData cln = (MasterOrderDetailsData)base.Clone();
			cln.InitVars();
			return cln;
		}
		#region Initialization

		internal void InitVars() 
		{

            masterOrderDetailsTable = (MasterOrderDetailsTable)this.Tables[clsPOSDBConstants.MasterOrderDetails_tbl];
            if (masterOrderDetailsTable != null) 
			{
                masterOrderDetailsTable.InitVars();
			}

		}

		private void InitClass() 
		{
            this.DataSetName = clsPOSDBConstants.MasterOrderDetails_tbl;
			this.Prefix = "";


            orderDetailsTable = new OrderDetalis();
            poDetailTable = new PODetailTable();
            masterOrderDetailsTable = new MasterOrderDetailsTable();
            this.Tables.Add(this.masterOrderDetailsTable);
            this.Tables.Add(this.orderDetailsTable);
            this.Tables.Add(this.poDetailTable);
		}

		private void InitClass(DataSet ds) 
		{

            if (ds.Tables[clsPOSDBConstants.OrderDetail_tbl] != null) 
			{
                this.Tables.Add(new MasterOrderDetailsTable(ds.Tables[clsPOSDBConstants.MasterOrderDetails_tbl]));
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
