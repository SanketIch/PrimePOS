// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData {
	using System;
	using System.Data;
	using System.Xml;
	using System.Runtime.Serialization;
	using POS_Core.CommonData.Tables;
	using POS_Core.CommonData.Rows;

	public class ColorSchemeForViewPOSTransData : DataSet {

        private ColorSchemeForViewPOSTransTable _ColorSchemeForViewPOSTransTable;

		#region Constructors

		public ColorSchemeForViewPOSTransData() {
			this.InitClass();

			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}
		#endregion


        public ColorSchemeForViewPOSTransTable ColorSchemeForViewPOSTrans
        {
			get {
				return this._ColorSchemeForViewPOSTransTable;
			}
			set {
				this._ColorSchemeForViewPOSTransTable = value;
			}
		}

		/// <summary>
		///     override DataSet Clone() methods.
		///     <remarks>Copies the structure of the ColorSchemeForViewPOSTransData, including all DataTable schemas, relations, and constraints. Does not copy any data.</remarks> 
		/// </summary>
		public override DataSet Clone() {
			ColorSchemeForViewPOSTransData cln = (ColorSchemeForViewPOSTransData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization

		internal void InitVars() {

            _ColorSchemeForViewPOSTransTable = (ColorSchemeForViewPOSTransTable)this.Tables[clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl];
			if (_ColorSchemeForViewPOSTransTable != null) {
			_ColorSchemeForViewPOSTransTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = "ColorSchemeForViewPOSTransData";
			this.Prefix = "";
			this.Locale = new System.Globalization.CultureInfo("en-US");
			this.CaseSensitive = false;
			this.EnforceConstraints = true;


			_ColorSchemeForViewPOSTransTable = new ColorSchemeForViewPOSTransTable();
			this.Tables.Add(this.ColorSchemeForViewPOSTrans);

		}

		private void InitClass(DataSet ds) {

            if (ds.Tables[clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl] != null)
            {
				this.Tables.Add(new ColorSchemeForViewPOSTransTable(ds.Tables[clsPOSDBConstants.ColorSchemeForViewPOSTrans_tbl]));
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


        public delegate void ColorSchemeForViewPOSTransRowChangeEventHandler(object sender, ColorSchemeForViewPOSTransRowChangeEvent e);
  }

}
