using System.Globalization;
using POS_Core.CommonData;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData.Tables;
using POS_Core.ErrorLogging;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using POS_Core.Resources;
//using POS.Resources;
using NLog;

namespace POS_Core.DataAccess
{
	public class ItemTaxSvr : DatabaseServerBase
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		public override void Persist(DataSet dataset)
		{
			var connection = CreateConnection();
			connection.Open();

			IDbTransaction transaction = connection.BeginTransaction();

			try
			{
				Persist(dataset, transaction);
				transaction.Commit();
			}
			catch (POSExceptions)
			{
				throw;
			}

			catch (OtherExceptions)
			{
				throw;
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				logger.Fatal(ex, "Persist(DataSet dataset)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
			}
		}

		public override void Insert(DataSet dataset, IDbTransaction transaction)
		{
			ItemTaxTable addedTable = GetChanges<ItemTaxTable>(dataset, clsPOSDBConstants.ItemTaxTableName, DataRowState.Added);

			if (addedTable != null && addedTable.Rows.Count > 0)
			{
				foreach (ItemTaxRow row in addedTable.Rows)
				{
					IDbDataParameter[] parameters = GetParameters(row);
					string insertQuery = GetInsertQuery(parameters);

					try
					{
						DataHelper.ExecuteNonQuery(transaction, CommandType.Text, insertQuery, parameters);
					}
					catch (POSExceptions)
					{
						throw;
					}
					catch (OtherExceptions)
					{
						throw;
					}

					catch (Exception ex)
					{
						logger.Fatal(ex, "Insert(DataSet dataset, IDbTransaction transaction)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex, "", "");
					}
				}
				addedTable.AcceptChanges();
			}
		}

		public override void Delete(DataSet dataset, IDbTransaction transaction)
		{
			ItemTaxTable deletedTable = GetChanges<ItemTaxTable>(dataset, clsPOSDBConstants.ItemTaxTableName, DataRowState.Deleted);

			if (deletedTable != null && deletedTable.Rows.Count > 0)
			{
				MakeDeletedRowsAccessible(deletedTable);

				foreach (ItemTaxTable row in deletedTable.Rows)
				{
					IDbDataParameter[] parameters ={CreateParameter(clsPOSDBConstants.ItemTaxTable_IDColumnName, DbType.Int32, row.ID)};

					string deleteQuery = GetDeleteQuere(parameters);

					try
					{
						DataHelper.ExecuteNonQuery(transaction, CommandType.Text, deleteQuery, parameters);
					}
					catch (POSExceptions)
					{
						throw;
					}
					catch (OtherExceptions)
					{
						throw;
					}
					catch (Exception ex)
					{
						logger.Fatal(ex, "Delete(DataSet dataset, IDbTransaction transaction)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
						//ErrorHandler.throwException(ex, "", "");
					}
				}
			}
		}

		public override void Update(DataSet dataset, IDbTransaction transaction)
		{
			//Updated it not implemented in this class, as it is not required here.
		}

        public ItemTaxData Populate(string entityID, string entityType)
        {
	        return Populate(entityID, entityType, CreateConnection());
        }

		public ItemTaxData Populate(string taxID, IDbConnection connection)
		{
			var itemTaxData = new ItemTaxData();

			var selectQuery = "Select *" + " FROM " + clsPOSDBConstants.ItemTaxTableName + " WHERE " +
                              clsPOSDBConstants.ItemTaxTable_EntityIdColumnName + " = '" + taxID + "'";

            IDbDataParameter[] parameters = { CreateParameter(clsPOSDBConstants.ItemTaxTable_IDColumnName, DbType.Int32, taxID) };

			try
			{
				var dataset = DataHelper.ExecuteDataset(connection, CommandType.Text, selectQuery, parameters);
				itemTaxData.ItemTaxTable.MergeTable(dataset.Tables[0]);
			}
			catch (POSExceptions)
			{
				throw;
			}
			catch (OtherExceptions)
			{
				throw;
			}
			catch (Exception ex)
			{
				logger.Fatal(ex, "Populate(string taxID, IDbConnection connection)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
			}

			return itemTaxData;
		}

		private static ItemTaxData Populate(string entityID, string entityType, IDbConnection connection)
		{
			var itemTaxData = new ItemTaxData();

			var selectQuery = "Select *" + " FROM " + clsPOSDBConstants.ItemTaxTableName + " WHERE " +
			                  clsPOSDBConstants.ItemTaxTable_EntityIdColumnName + " = '" + entityID + "'" +
			                  " AND " + clsPOSDBConstants.ItemTaxTable_EntityTypeColumnName + " ='" + entityType + "'";

			try
			{
				var dataset = DataHelper.ExecuteDataset(connection, CommandType.Text, selectQuery);
				itemTaxData.ItemTaxTable.MergeTable(dataset.Tables[0]);
			}
			catch (POSExceptions)
			{
				throw;
			}
			catch (OtherExceptions)
			{
				throw;
			}
			catch (Exception ex)
			{
				logger.Fatal(ex, "Populate(string entityID, string entityType, IDbConnection connection)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
			}

			return itemTaxData;
		}

		public void DeleteAllTaxEntries(string entityID, char entityType)
		{
			var selectQuery = "DELETE " + " FROM " + clsPOSDBConstants.ItemTaxTableName + " WHERE " +
							  clsPOSDBConstants.ItemTaxTable_EntityIdColumnName + " = '" + entityID + "'" +
							  " AND " + clsPOSDBConstants.ItemTaxTable_EntityTypeColumnName + " ='" + entityType + "'";

			try
			{
				DataHelper.ExecuteScalar(CreateConnection(), CommandType.Text, selectQuery);
			}
			catch (POSExceptions)
			{
				throw;
			}
			catch (OtherExceptions)
			{
				throw;
			}
			catch (Exception ex)
			{
				logger.Fatal(ex, "DeleteAllTaxEntries(string entityID, char entityType)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
			}
		}

		/// <summary>
		/// Create a connectionusing DataFactory and also prepare it with the connection string.
		/// </summary>
		/// <returns>Database connection using a connection string in Configuration.</returns>
		private static IDbConnection CreateConnection()
		{
			IDbConnection conn = DataFactory.CreateConnection();

			conn.ConnectionString = Configuration.ConnectionString;

			return conn;
		}

		/// <summary>
		/// Gets the insert Query.
		/// </summary>
		/// <param name="parameters">The parameters to include in query.</param>
		/// <returns>The Insert query.</returns>
		private static string GetInsertQuery(IDbDataParameter[] parameters)
		{
			string insertQuery = "INSERT INTO " + clsPOSDBConstants.ItemTaxTableName + " ( ";

			foreach (var parameter in parameters)
			{
				insertQuery += parameter.SourceColumn + ",";
			}

			insertQuery = TrimEndComma(insertQuery);
			insertQuery += " ) VALUES ( ";

			foreach (var parameter in parameters)
			{
				insertQuery += parameter.ParameterName + ",";
			}

			insertQuery = TrimEndComma(insertQuery);
			insertQuery += " )";

			return insertQuery;
		}

		/// <summary>
		/// Trims the Comma(,) present in the end of the string.
		/// </summary>
		/// <param name="insertQuery"></param>
		/// <returns></returns>
		private static string TrimEndComma(string insertQuery)
		{
			return insertQuery.TrimEnd(new[] { ',' });
		}

		/// <summary>
		/// Gets the Delete Query.
		/// </summary>
		/// <param name="parameters">The parameters to include in query.</param>
		/// <returns>The Delete query.</returns>
		private static string GetDeleteQuere(IEnumerable<IDbDataParameter> parameters)
		{
			string deleteQuere = "DELETE FROM " + clsPOSDBConstants.ItemTaxTableName + " WHERE ";
			
			foreach (var parameter in parameters)
			{
				deleteQuere += parameter.SourceColumn + " = " + parameter.ParameterName;
			}
			
			return deleteQuere;
		}

		/// <summary>
		/// Rejects the changes in table so that the deleted rows can be made accessible.
		/// </summary>
		/// <param name="deletedTable">DataTable whose deleted rows are to be made accessible.</param>
		private static void MakeDeletedRowsAccessible(DataTable deletedTable)
		{
			deletedTable.RejectChanges();
		}

		/// <summary>
		/// Gets the array of ItemTax table paramters.
		/// </summary>
		/// <param name="row">The row for which to get the paramters.</param>
		/// <returns>Array of paramters.</returns>
		private static IDbDataParameter[] GetParameters(ItemTaxRow row)
		{
			IDbDataParameter[] sqlParams = DataFactory.CreateParameterArray(3);

			//sqlParams[0] = CreateParameter(clsPOSDBConstants.ItemTaxTable_IDColumnName, DbType.Int32, row.ID);

			sqlParams[0] = CreateParameter(clsPOSDBConstants.ItemTaxTable_EntityIdColumnName, DbType.String,
				(object)row.EntityID ?? DBNull.Value);

			sqlParams[1] = CreateParameter(clsPOSDBConstants.ItemTaxTable_EntityTypeColumnName, DbType.String,
				row.EntityType);

			sqlParams[2] = CreateParameter(clsPOSDBConstants.ItemTaxTable_TaxIdColumnName, DbType.Int32,
				row.TaxID);

			return sqlParams;
		}

		/// <summary>
		/// Create a parameter.
		/// </summary>
		/// <param name="columnName">Name of the column</param>
		/// <param name="dbType">Db Type.</param>
		/// <param name="value">Value of the paramter.</param>
		/// <returns>The DB parameter.</returns>
		private static IDbDataParameter CreateParameter(string columnName, DbType dbType, object value)
		{
			return DataFactory.CreateParameter("@" + columnName, dbType, columnName, value);
		}
	}
}