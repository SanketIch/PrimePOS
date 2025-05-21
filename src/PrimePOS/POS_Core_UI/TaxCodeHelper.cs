using System;
using POS_Core.BusinessRules;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.DataAccess;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace POS_Core_UI
{
	public static class TaxCodeHelper
	{
		public static DataTable GetTaxCodeDataTable()
		{
			using (var taxCodes = new TaxCodes())
			{
				//Added By Arvind 2664
				var taxCodesData = taxCodes.PopulateList(" WHERE Active = 1");

				DataTable dtTaxCodes = new DataTable();
				dtTaxCodes.Columns.Add(clsPOSDBConstants.TaxCodes_Fld_TaxCode, typeof(String));
				dtTaxCodes.Columns.Add(clsPOSDBConstants.TaxCodes_Fld_Description, typeof(String));

				foreach (TaxCodesRow oRow in taxCodesData.TaxCodes.Rows)
				{
					DataRow dr = dtTaxCodes.NewRow();
					dr[0] = oRow.TaxID;
					dr[1] = oRow.TaxCode + " (" + oRow.Description +"-" + oRow.Amount + " %)";
					dtTaxCodes.Rows.Add(dr);
				}

				return dtTaxCodes;
			}
		}

		public static ItemTaxData FetchDeparmentTaxInfo(int departmentId)
		{
			return GetItemTaxData(departmentId.ToString(CultureInfo.InvariantCulture), EntityType.Department);
		}

		public static ItemTaxData FetchItemTaxInfo(string itemId)
		{
			return GetItemTaxData(itemId, EntityType.Item);
		}

		public static void PersistItemTaxCodes(IEnumerable<int> selectedTaxCodes, string id)
		{
			PersistTaxCodes(selectedTaxCodes, id, GetEntityType(EntityType.Item));
		}

		public static void PersistDepartmentTaxCodes(IEnumerable<int> selectedTaxCodes, string id)
		{
			PersistTaxCodes(selectedTaxCodes, id, GetEntityType(EntityType.Department));
		}

		private static void PersistTaxCodes(IEnumerable<int> selectedTaxCodes, string id, char entityType)
		{
			DeletePreviousEntries(id, entityType);

			int rowCount = 1;
			using (ItemTaxData itemTaxData = new ItemTaxData())
			{
				foreach (int selectedTax in selectedTaxCodes)
				{
					itemTaxData.ItemTaxTable.AddRow(rowCount++, id, entityType, selectedTax);
				}

				using (ItemTaxSvr itemTaxSvr = new ItemTaxSvr())
				{
					itemTaxSvr.Persist(itemTaxData);
				}
			}
		}

		private static void DeletePreviousEntries(string id, char entityType)
		{
			using (ItemTaxSvr itemTaxSvr = new ItemTaxSvr())
			{
				itemTaxSvr.DeleteAllTaxEntries(id, entityType);
			}
		}

		private static char GetEntityType(EntityType entityType)
		{
			return entityType == EntityType.Item ? 'I' : 'D';
		}

		private static ItemTaxData GetItemTaxData(string departmentId, EntityType entityType)
		{
			using (ItemTax itemTax = new ItemTax())
			{
				return itemTax.Populate(departmentId, entityType);
			}
		}

		internal static string GetTrimmedTaxCodes(Infragistics.Win.CheckedValueListItemsCollection checkedValueListItemsCollection)
		{
			string trimmedTaxCodes = string.Empty;

			foreach (var item in checkedValueListItemsCollection)
			{
				trimmedTaxCodes += " " + item.DisplayText.Substring(0, item.DisplayText.IndexOf('(') - 1) + ",";
			}

			trimmedTaxCodes = trimmedTaxCodes.TrimEnd(new[] { ',' });
			trimmedTaxCodes = trimmedTaxCodes.Trim();

			return trimmedTaxCodes;
		}

		public static DataSet GetDataSetWithStringTaxTypeColumns(DataSet dataSet)
		{
			DataTable dataTable = dataSet.Tables[0];

			dataTable.Columns.Add("Tax Type", typeof(String));

			foreach (DataRow oRow in dataTable.Rows)
			{
				var taxtypeDataValue = oRow[clsPOSDBConstants.TaxCodes_Fld_TaxType];

				if (taxtypeDataValue == DBNull.Value || IsTaxTypeValuesOutOfrange((int)taxtypeDataValue))
					continue;

				oRow["Tax Type"] = ((TaxTypes)((int)taxtypeDataValue)).ToString();
			}

			dataTable.Columns.Remove(dataTable.Columns[clsPOSDBConstants.TaxCodes_Fld_TaxType]);
			return dataSet;
		}

		/// <summary>
		/// Checks if the tax type db value is out of range as, we just have six typs of tax types so this will check for the
		/// values less than 1 and greater than 7.
		/// the range start from 1 because first x type has a value of 1 and increses by one for each next tax type till 7.
		/// </summary>
		/// <param name="taxtypeDataValue"></param>
		/// <returns></returns>
		private static bool IsTaxTypeValuesOutOfrange(int taxtypeDataValue)
		{
			return taxtypeDataValue <= 0 || taxtypeDataValue > 7;
		}
	}
}