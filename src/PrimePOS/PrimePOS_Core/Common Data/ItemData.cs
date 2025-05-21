// ----------------------------------------------------------------
// ----------------------------------------------------------------

namespace POS_Core.CommonData
{
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData.Tables;
	using System;
	using System.Data;
	using System.Runtime.Serialization;
	using System.Xml;

	public class ItemData : DataSet
	{
		private ItemTable _ItemTable;

		#region Constants

		/// <value>The constant used for Item table. </value>
		public const String Item_TABLE = clsPOSDBConstants.Item_tbl;

		#endregion Constants

		#region Constructors

		public ItemData()
		{
			this.InitClass();
			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}

		protected ItemData(SerializationInfo info, StreamingContext context)
		{
			string strSchema = (string)info.GetValue("XmlSchema", typeof(string));
			if (strSchema != null)
			{
				DataSet ds = new DataSet();
				ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
				this.InitClass(ds);
			}
			else
			{
				this.InitClass();
			}
			this.GetSerializationData(info, context);
			System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			this.Tables.CollectionChanged += schemaChangedHandler;
			this.Relations.CollectionChanged += schemaChangedHandler;
		}

		#endregion Constructors

		public ItemTable Item
		{
			get
			{
				return this._ItemTable;
			}
			set
			{
				this._ItemTable = value;
			}
		}

		public override DataSet Clone()
		{
			ItemData cln = (ItemData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization

		internal void InitVars()
		{
			_ItemTable = (ItemTable)this.Tables[Item_TABLE];
			if (_ItemTable != null)
			{
				_ItemTable.InitVars();
			}
		}

		private void InitClass()
		{
			this.DataSetName = "ItemData";
			this.Prefix = "";
			this.Locale = new System.Globalization.CultureInfo("en-US");
			this.CaseSensitive = false;
			this.EnforceConstraints = true;

			_ItemTable = new ItemTable();
			this.Tables.Add(this.Item);
		}

		private void InitClass(DataSet ds)
		{
			if (ds.Tables[Item_TABLE] != null)
			{
				this.Tables.Add(new ItemTable(ds.Tables[Item_TABLE]));
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

		#endregion Initialization

		private void SchemaChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
		{
			if ((e.Action == System.ComponentModel.CollectionChangeAction.Remove))
			{
				this.InitVars();
			}
		}

		public delegate void ItemRowChangeEventHandler(object sender, ItemRowChangeEvent e);
	}
}