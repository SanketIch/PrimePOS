using POS_Core.CommonData.Tables;
using System;
using System.Data;

namespace POS_Core.CommonData.Rows
{
	public class ItemTaxRow : DataRow
	{
		private readonly ItemTaxTable _table;

		public Int32 ID
		{
			get { return GetValue<Int32>(_table.ID); }
			set { this[_table.ID] = value; }
		}

		public string EntityID
		{
			get { return GetValue<string>(_table.EntityID) ?? string.Empty; }
			set { this[_table.EntityID] = value; }
		}

		public char EntityType
		{
			get { return GetValue<char>(_table.EntityType); }
			set { this[_table.EntityType] = value; }
		}

		public Int32 TaxID
		{
			get { return GetValue<Int32>(_table.TaxID); }
			set { this[_table.TaxID] = value; }
		}

		protected internal ItemTaxRow(DataRowBuilder dataRowBuilder)
			: base(dataRowBuilder)
		{
			_table = (ItemTaxTable)Table;
		}

		private T GetValue<T>(DataColumn dataColumn)
		{
			try
			{
				return (T)this[dataColumn];
			}
			catch (Exception)
			{
				return default(T);
			}
		}
	}
}