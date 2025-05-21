using POS_Core.ErrorLogging;
using System;
using System.Data;
using NLog;

namespace POS_Core.DataAccess
{
	public abstract class DatabaseServerBase : IDisposable
	{
		private static ILogger logger = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Persistes the changes.
		/// </summary>
		/// <param name="dataset">The Dataset containing changes.</param>
		public abstract void Persist(DataSet dataset);

		/// <summary>
		/// Inserts the Dataset into table.
		/// </summary>
		/// <param name="dataset">The Dataset to be inserted.</param>
		/// <param name="transaction">The transaction.</param>
		public abstract void Insert(DataSet dataset, IDbTransaction transaction);

		/// <summary>
		/// Updates the Dataset into table.
		/// </summary>
		/// <param name="dataset">The Dataset to be Updated.</param>
		/// <param name="transaction">The transaction.</param>
		public abstract void Update(DataSet dataset, IDbTransaction transaction);

		/// <summary>
		/// Deletes the from table.
		/// </summary>
		/// <param name="dataset">The Dataset contaning the rows to be deleted..</param>
		/// <param name="transaction">The transaction.</param>
		public abstract void Delete(DataSet dataset, IDbTransaction transaction);

		/// <summary>
		/// Persists the changes.
		/// </summary>
		/// <param name="dataset">The DataSet containing the changes.</param>
		/// <param name="transaction">The transaction.</param>
		public virtual void Persist(DataSet dataset, IDbTransaction transaction)
		{
			try
			{
				Delete(dataset, transaction);
				Insert(dataset, transaction);
				Update(dataset, transaction);

				dataset.AcceptChanges();
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
				logger.Fatal(ex, "Persist(DataSet dataset, IDbTransaction transaction)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
				//ErrorHandler.throwException(ex, "", "");
			}
		}

		/// <summary>
		/// Gets the changed DataTable from the Dataset.
		/// </summary>
		/// <typeparam name="TTable">DataTable type</typeparam>
		/// <param name="dataset">The Dataset</param>
		/// <param name="tableName">The Table Name</param>
		/// <param name="dataRowState">The DatRowState.</param>
		/// <returns></returns>
		protected static TTable GetChanges<TTable>(DataSet dataset, string tableName, DataRowState dataRowState) where TTable : DataTable
		{
			return (TTable)dataset.Tables[tableName].GetChanges(dataRowState);
		}

		public virtual void Dispose()
		{
			
		}
	}
}