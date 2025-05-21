// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using System.Runtime.Serialization;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class SubDepartmentData : DataSet {

		private SubDepartmentTable _SubDepartmentTable;

		#region Constructors
		
        public SubDepartmentData() {
			this.InitClass();
			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}

		#endregion


		public SubDepartmentTable SubDepartment {
			get {
				return this._SubDepartmentTable;
			}
			set {
				this._SubDepartmentTable = value;
			}
		}

		/// <summary>
		///     override DataSet Clone() methods.
		///     <remarks>Copies the structure of the SubDepartmentData, including all DataTable schemas, relations, and constraints. Does not copy any data.</remarks> 
		/// </summary>
		public override DataSet Clone() {
			SubDepartmentData cln = (SubDepartmentData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization

		internal void InitVars() {

		_SubDepartmentTable = (SubDepartmentTable)this.Tables[clsPOSDBConstants.SubDepartment_tbl];
			if (_SubDepartmentTable != null) {
			_SubDepartmentTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = "SubDepartmentData";
			this.Prefix = "";
			this.Locale = new System.Globalization.CultureInfo("en-US");
			this.CaseSensitive = false;
			this.EnforceConstraints = true;


			_SubDepartmentTable = new SubDepartmentTable();
			this.Tables.Add(this.SubDepartment);

		}

		private void InitClass(DataSet ds) {
			
			if (ds.Tables[clsPOSDBConstants.SubDepartment_tbl] != null) {
				this.Tables.Add(new SubDepartmentTable(ds.Tables[clsPOSDBConstants.SubDepartment_tbl]));
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

    private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e) {
      if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove)) {
        this.InitVars();
      }
    }

    public delegate void SubDepartmentRowChangeEventHandler(object sender, SubDepartmentRowChangeEvent e);

    }

}
