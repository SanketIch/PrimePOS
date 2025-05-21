// ----------------------------------------------------------------
//Added By Shitaljit(QuicSolv) 0n 5 oct 2011
// ----------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using System.Xml;
using System.Runtime.Serialization;

namespace POS_Core.CommonData
{
    public class NotesData : DataSet
    {
        private NotesTable _NotesTable;
        #region Constants
        /// <value>The constant used for Notes table. </value>
        public const String Notes_TABLE = clsPOSDBConstants.Notes_tbl;
        #endregion

        #region Constructors
		public NotesData() {
			this.InitClass();
			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}

        protected NotesData(SerializationInfo info, StreamingContext context)
        {
			string strSchema = (string)info.GetValue("XmlSchema", typeof(string));
			if (strSchema != null) {
				DataSet ds = new DataSet();
				ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
				this.InitClass(ds);
			}
			else {
				this.InitClass();
			}
			this.GetSerializationData(info, context);
			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}

		#endregion

        #region Constructor
        public NotesTable Notes
        {
            get
            {
                return this._NotesTable;
            }
            set
            {
                this._NotesTable = value;
            }
        }

        public override DataSet Clone()
        {
            NotesData cln = (NotesData)base.Clone();
            cln.InitVars();
            return cln;
        }

        internal void InitVars() {

		_NotesTable = (NotesTable)this.Tables[Notes_TABLE];
			if (_NotesTable != null) {
			_NotesTable.InitVars();
			}

		}

		private void InitClass() {
			this.DataSetName = "NotesData";
			this.Prefix = "";
			this.Locale = new System.Globalization.CultureInfo("en-US");
			this.CaseSensitive = false;
			this.EnforceConstraints = true;


			_NotesTable = new NotesTable();
			this.Tables.Add(this.Notes);

		}

		private void InitClass(DataSet ds) {

			if (ds.Tables[Notes_TABLE] != null) {
				this.Tables.Add(new NotesTable(ds.Tables[Notes_TABLE]));
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


    public delegate void NotesRowChangeEventHandler(object sender, NotesRowChangeEvent e);
    }
}
