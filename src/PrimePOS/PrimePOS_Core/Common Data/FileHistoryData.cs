using System;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;

namespace POS_Core.CommonData
{
   public class FileHistoryData : DataSet
    {
       
        private FileHistoryTable  _clsFileHistoryTable;


		#region Constructors
		//     Constructor for VendorData.

       public FileHistoryData() 
		{
			this.InitClass();
		}

		#endregion

		public override DataSet Clone() 
		{
            FileHistoryData cln = (FileHistoryData)base.Clone();
			cln.InitVars();
			return cln;
		}


		public  FileHistoryTable FileHistoryTable 
		{
			get 
			{
				return this._clsFileHistoryTable;
			}
			set 
			{
				this._clsFileHistoryTable = value;
			}
		}

		#region Initialization

		internal void InitVars() 
		{

            _clsFileHistoryTable = (FileHistoryTable)this.Tables[clsPOSDBConstants.FileHistory_tbl];
		
            if (_clsFileHistoryTable != null) 
			{
				_clsFileHistoryTable.InitVars();
			}
		}

		private void InitClass() 
		{
			this.DataSetName = "FileHistoryData";
			this.Prefix = "";

            _clsFileHistoryTable = new FileHistoryTable();
		   this.Tables.Add(this.FileHistoryTable);
		}

		private void InitClass(DataSet ds) 
		{

			if (ds.Tables[clsPOSDBConstants.FileHistory_tbl] != null) 
			{
                this.Tables.Add(new FileHistoryTable(ds.Tables[clsPOSDBConstants.FileHistory_tbl]));
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
