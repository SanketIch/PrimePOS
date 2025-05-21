using POS_Core.CommonData.Rows;
using System;
using System.Data;

namespace POS_Core.CommonData.Tables
{
	public class ItemTaxTable : EnumerableDataTable
	{
		public DataColumn ID { get; private set; }

		public DataColumn EntityID { get; private set; }

		public DataColumn EntityType { get; private set; }

		public DataColumn TaxID { get; private set; }

		public ItemTaxRow this[int index]
		{
			get { return (ItemTaxRow) (Rows[index]); }
		}

		internal ItemTaxTable()
			: base(clsPOSDBConstants.ItemTaxTableName)
		{
			PrepareTable();
		}

		internal ItemTaxTable(DataTable table)
			: base(table.TableName)
		{
		}

		public void AddRow(ItemTaxRow row)
		{
			AddRow(row, false);
		}

		public void AddRow(ItemTaxRow row, bool preserveChanges)
		{
			if (!IsItemWithSameIdAlreadyPresent(row.ID))
			{
				Rows.Add(row);

				if (!preserveChanges)
					row.AcceptChanges();
			}
		}

		public virtual void MergeTable(DataTable dataTable)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				ItemTaxRow itemTaxRow = (ItemTaxRow) NewRow();

				if (row[clsPOSDBConstants.ItemTaxTable_IDColumnName] == DBNull.Value)
					itemTaxRow[clsPOSDBConstants.ItemTaxTable_IDColumnName] = DBNull.Value;
				else
					itemTaxRow[clsPOSDBConstants.ItemTaxTable_IDColumnName] =
						Convert.ToInt32(row[clsPOSDBConstants.ItemTaxTable_IDColumnName]);

				if (row[clsPOSDBConstants.ItemTaxTable_EntityIdColumnName] == DBNull.Value)
					itemTaxRow[clsPOSDBConstants.ItemTaxTable_EntityIdColumnName] = DBNull.Value;
				else
					itemTaxRow[clsPOSDBConstants.ItemTaxTable_EntityIdColumnName] =
						Convert.ToString(row[clsPOSDBConstants.ItemTaxTable_EntityIdColumnName]);

				if (row[clsPOSDBConstants.ItemTaxTable_EntityTypeColumnName] == DBNull.Value)
					itemTaxRow[clsPOSDBConstants.ItemTaxTable_EntityTypeColumnName] = DBNull.Value;
				else
					itemTaxRow[clsPOSDBConstants.ItemTaxTable_EntityTypeColumnName] =
						Convert.ToString(row[clsPOSDBConstants.ItemTaxTable_EntityTypeColumnName]);

				if (row[clsPOSDBConstants.ItemTaxTable_TaxIdColumnName] == DBNull.Value)
					itemTaxRow[clsPOSDBConstants.ItemTaxTable_TaxIdColumnName] = DBNull.Value;
				else
					itemTaxRow[clsPOSDBConstants.ItemTaxTable_TaxIdColumnName] =
						Convert.ToInt32(row[clsPOSDBConstants.ItemTaxTable_TaxIdColumnName]);

				AddRow(itemTaxRow);
			}
		}

		public override DataTable Clone()
		{
			ItemTaxTable itemTaxTable = (ItemTaxTable) base.Clone();
			itemTaxTable.InitializeColumns();
			return itemTaxTable;
		}

		public void InitializeColumns()
		{
			ID = Columns[clsPOSDBConstants.ItemTaxTable_IDColumnName];
			EntityID = Columns[clsPOSDBConstants.ItemTaxTable_EntityIdColumnName];
			EntityType = Columns[clsPOSDBConstants.ItemTaxTable_EntityTypeColumnName];
			TaxID = Columns[clsPOSDBConstants.ItemTaxTable_TaxIdColumnName];
		}

		private void PrepareTable()
		{
			ID = CreateDataColumn<Int32>(clsPOSDBConstants.ItemTaxTable_IDColumnName, false);
			EntityID = CreateDataColumn<string>(clsPOSDBConstants.ItemTaxTable_EntityIdColumnName, false);
			EntityType = CreateDataColumn<char>(clsPOSDBConstants.ItemTaxTable_EntityTypeColumnName, false);
			TaxID = CreateDataColumn<Int32>(clsPOSDBConstants.ItemTaxTable_TaxIdColumnName, false);

			PrimaryKey = new[] {ID};
		}

		private DataColumn CreateDataColumn<TType>(string columnName, bool allowDbNull)
		{
			var dataColumn = new DataColumn(columnName, typeof (TType), null, MappingType.Element);

			Columns.Add(dataColumn);
			dataColumn.AllowDBNull = allowDbNull;

			return dataColumn;
		}

		private bool IsItemWithSameIdAlreadyPresent(int itemID)
		{
			return GetRow<ItemTaxRow>(itemID) != null;
		}

		public void AddRow(int id, string entityId, char entityType, int taxId)
		{
			ItemTaxRow row = (ItemTaxRow) this.NewRow();
			row.ItemArray = new object[] {id, entityId, entityType, taxId};
			Rows.Add(row);
		}

		protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
		{
			return new ItemTaxRow(builder);
		}

		protected override Type GetRowType()
		{
			return typeof (ItemTaxRow);
		}
	}
}