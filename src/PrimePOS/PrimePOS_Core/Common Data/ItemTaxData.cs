using POS_Core.CommonData.Tables;
using System.ComponentModel;
using System.Data;
using System.Runtime.Serialization;
using System.Xml;

namespace POS_Core.CommonData
{
	public class ItemTaxData : DataSet
	{
		public ItemTaxData()
		{
			InitClass();

			AddEventHandler();
		}

		private void AddEventHandler()
		{
			CollectionChangeEventHandler schemaChangedHandler = SchemaChanged;

			Tables.CollectionChanged += schemaChangedHandler;
			Relations.CollectionChanged += schemaChangedHandler;
		}

		protected ItemTaxData(SerializationInfo info, StreamingContext context)
		{
			string strSchema = (string)info.GetValue("XmlSchema", typeof(string));

			if (strSchema != null)
			{
				DataSet ds = new DataSet();
				ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
				InitClass(ds);
			}
			else
			{
				InitClass();
			}
			
			GetSerializationData(info, context);

			AddEventHandler();
		}

		public ItemTaxTable ItemTaxTable { get; set; }

		public override DataSet Clone()
		{
			ItemTaxData cln = (ItemTaxData)base.Clone();
			cln.InitVars();
			return cln;
		}

		internal void InitVars()
		{
			ItemTaxTable = (ItemTaxTable)Tables[clsPOSDBConstants.ItemTaxTableName];

			if (ItemTaxTable != null)
				ItemTaxTable.InitializeColumns();
		}

		private void InitClass()
		{
			DataSetName = "ItemTaxData";
			Prefix = "";
			Locale = new System.Globalization.CultureInfo("en-US");
			CaseSensitive = false;
			EnforceConstraints = true;

			ItemTaxTable = new ItemTaxTable();
			Tables.Add(ItemTaxTable);
		}

		private void InitClass(DataSet ds)
		{
			if (ds.Tables[clsPOSDBConstants.ItemTaxTableName] != null)
				Tables.Add(new ItemTaxTable(ds.Tables[clsPOSDBConstants.ItemTaxTableName]));

			DataSetName = ds.DataSetName;
			Prefix = ds.Prefix;
			Namespace = ds.Namespace;
			Locale = ds.Locale;
			CaseSensitive = ds.CaseSensitive;
			EnforceConstraints = ds.EnforceConstraints;
			
			Merge(ds, false, MissingSchemaAction.Add);
			InitVars();
		}

		private void SchemaChanged(object sender, CollectionChangeEventArgs e)
		{
			if ((e.Action == CollectionChangeAction.Remove))
			{
				InitVars();
			}
		}
	}
}