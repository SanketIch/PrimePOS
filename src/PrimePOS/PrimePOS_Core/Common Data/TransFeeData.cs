namespace POS_Core.CommonData
{
	using POS_Core.CommonData.Rows;
	using POS_Core.CommonData.Tables;
	using System;
	using System.Data;
	using System.Runtime.Serialization;
	using System.Xml;

	public class TransFeeData : DataSet
    {
		private TransFeeTable _TransFeeTable;

		#region Constructors
		public TransFeeData()
		{
			this.InitClass();
			//System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
			//this.Tables.CollectionChanged += schemaChangedHandler;
			//this.Relations.CollectionChanged += schemaChangedHandler;
		}

		//protected TransFeeData(SerializationInfo info, StreamingContext context)
		//{
		//	string strSchema = (string)info.GetValue("XmlSchema", typeof(string));
		//	if (strSchema != null)
		//	{
		//		DataSet ds = new DataSet();
		//		ds.ReadXmlSchema(new XmlTextReader(new System.IO.StringReader(strSchema)));
		//		this.InitClass(ds);
		//	}
		//	else
		//	{
		//		this.InitClass();
		//	}
		//	this.GetSerializationData(info, context);
		//	System.ComponentModel.CollectionChangeEventHandler schemaChangedHandler = new System.ComponentModel.CollectionChangeEventHandler(this.SchemaChanged);
		//	this.Tables.CollectionChanged += schemaChangedHandler;
		//	this.Relations.CollectionChanged += schemaChangedHandler;
		//}
		#endregion Constructors

		public TransFeeTable TransFee
		{
			get
			{
				return this._TransFeeTable;
			}
			set
			{
				this._TransFeeTable = value;
			}
		}

		public override DataSet Clone()
		{
			TransFeeData cln = (TransFeeData)base.Clone();
			cln.InitVars();
			return cln;
		}

		#region Initialization
		internal void InitVars()
		{
			_TransFeeTable = (TransFeeTable)this.Tables[clsPOSDBConstants.TransFee_tbl];
			if (_TransFeeTable != null)
			{
				_TransFeeTable.InitVars();
			}
		}

		private void InitClass()
		{
			this.DataSetName = clsPOSDBConstants.TransFee_tbl;
			this.Prefix = "";
			this.Locale = new System.Globalization.CultureInfo("en-US");
			this.CaseSensitive = false;
			this.EnforceConstraints = true;

			_TransFeeTable = new TransFeeTable();
			this.Tables.Add(this.TransFee);
		}

		private void InitClass(DataSet ds)
		{
			if (ds.Tables[clsPOSDBConstants.TransFee_tbl] != null)
			{
				this.Tables.Add(new TransFeeTable(ds.Tables[clsPOSDBConstants.TransFee_tbl]));
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

		public delegate void TransFeeRowChangeEventHandler(object sender, TransFeeRowChangeEvent e);
	}
}
