using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using POS_Core.CommonData.Tables;

namespace POS_Core.CommonData
{
    public partial class MessageLogData : DataSet
    {
    
        private MessageLogTable _clsMessageLogTable;
		#region Constructors
		//     Constructor for VendorData.

        public MessageLogData() 
		{
			this.InitClass();
		}
		#endregion
		public override DataSet Clone() 
		{
            MessageLogData cln = (MessageLogData)base.Clone();
			cln.InitVars();
			return cln;
		}
        public MessageLogTable MessageLog
		{
			get 
			{
				return this._clsMessageLogTable;
			}
			set 
			{
                this._clsMessageLogTable = value;
			}
		}
		#region Initialization
		internal void InitVars() 
		{
            _clsMessageLogTable = (MessageLogTable)this.Tables[clsPOSDBConstants.MessageLog_tbl];
            if (_clsMessageLogTable != null) 
			{
                _clsMessageLogTable.InitVars();
			}
		}
		private void InitClass() 
		{
			this.DataSetName = "MessageLogData";
			this.Prefix = "";

			_clsMessageLogTable = new MessageLogTable();
			this.Tables.Add(this.MessageLog);
		}
		private void InitClass(DataSet ds) 
		{
			if (ds.Tables[clsPOSDBConstants.MessageLog_tbl] != null) 
			{
                this.Tables.Add(new MessageLogTable(ds.Tables[clsPOSDBConstants.MessageLog_tbl]));
			}
			this.DataSetName = ds.DataSetName;
			this.CaseSensitive = ds.CaseSensitive;
			this.EnforceConstraints = ds.EnforceConstraints;
			this.Merge(ds, false, System.Data.MissingSchemaAction.Add);
			this.InitVars();
		}
		#endregion
    }
}
