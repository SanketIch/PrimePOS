using System.Collections;
using System.Data;

namespace POS_Core.CommonData.Tables
{
	public class EnumerableDataTable : DataTable, IEnumerable
	{
		protected EnumerableDataTable(string tableName) : base(tableName)
		{
		}

		public IEnumerator GetEnumerator()
		{
			return Rows.GetEnumerator();
		}

		public TDataRow GetRow<TDataRow>(int itemID) where TDataRow : DataRow
		{
			return (TDataRow) Rows.Find(new object[] {itemID});
		}
	}
}