using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
//using POS.Resources;
using Resources;
using NLog;

namespace POS_Core.BusinessRules
{
    public class Notes : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        /// <summary>
        /// Insert or update Notes data.
        /// </summary>
        /// <param name="updates">Notes type dataset.</param>
        public void Persist(NotesData updates)
        {

            try
            {
                IDbTransaction tx;
                IDbConnection oConn = DataFactory.CreateConnection(POS_Core.Resources.Configuration.ConnectionString);
                tx = oConn.BeginTransaction();
                Persist(updates, tx);
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(NotesData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }

        /// <summary>
        /// Insert or update data by checking user priviliges. 
        /// This insertion or updation takes place under Transactions.
        /// </summary>
        /// <param name="updates">Notes type dataset.</param>
        /// <param name="tx">This insertion or updation will be the part of this transaction.</param>
        public void Persist(NotesData updates, IDbTransaction tx)
        {
            try
            {
                checkIsValidData(updates);
                using (NotesSvr dao = new NotesSvr())
                {
                    dao.Persist(updates, tx);
                    tx.Commit();
                }
            }
            catch (POSExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                tx.Rollback();
                throw (ex);
            }

            catch (Exception ex)
            {
                tx.Rollback();
                logger.Fatal(ex, "Persist(NotesData updates, IDbTransaction tx)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
            }
        }
        #endregion

         #region Validation Methods

        /// <summary>
        /// Validate a Notes. This would be the place to put field validations.
        /// 
        /// </summary>
        /// <param name="updates"></param>
        public void checkIsValidData(NotesData updates)
        {
            NotesTable table;

            NotesRow oRow;

            oRow = (NotesRow)updates.Tables[0].Rows[0];

            table = (NotesTable)updates.Tables[0].GetChanges(DataRowState.Added);

            if (table == null)
                table = (NotesTable)updates.Tables[0].GetChanges(DataRowState.Modified);
            else if ((NotesTable)updates.Tables[0].GetChanges(DataRowState.Modified) != null)
                table.MergeTable((NotesTable)updates.Tables[0].GetChanges(DataRowState.Modified));

            if (table == null)
                table = (NotesTable)updates.Tables[0].GetChanges(DataRowState.Unchanged);
            else if ((NotesTable)updates.Tables[0].GetChanges(DataRowState.Unchanged) != null)
                table.MergeTable((NotesTable)updates.Tables[0].GetChanges(DataRowState.Unchanged));

            if (table == null) return;

           
        }

        #endregion

        #region Get Methods
        /// <summary>
        /// Fills a DataSet with all Notess based on a condition.
        /// </summary>
        /// <param name="whereClause">SQL WHERE CLUASE type condition.</param>
        /// <returns>returns Typed DataSet (NotesData).</returns>
        /// 
        public NotesData PopulateList(string whereClause)
        {
            try
            {
                using (NotesSvr dao = new NotesSvr())
                {
                    return dao.PopulateList(whereClause);
                }
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        public DataSet PopulateList(string  _Table,string whereClause)
        {
            try
            {
                using (NotesSvr dao = new NotesSvr())
                {
                    return dao.PopulateList( _Table ,whereClause);
                }
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string  _Table,string whereClause)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        /// <summary>
        /// Fills a DataSet with all Notess based on a condition.
        /// </summary>
        /// <param name="whereClause">SQL WHERE CLUASE type condition.</param>
        /// <returns>returns Typed DataSet (NotesData).</returns>
        public NotesData Populate(int NoteId)
        {
            try
            {
                using (NotesSvr dao = new NotesSvr())
                {
                    return dao.Populate(NoteId);
                }
            }
            catch (POSExceptions ex)
            {
                throw (ex);
            }

            catch (OtherExceptions ex)
            {
                throw (ex);
            }

            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(int NoteId)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");
                return null;
            }
        }
        #endregion
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
