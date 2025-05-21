using POS_Core.CommonData;
using POS_Core.DataAccess;
using POS_Core.ErrorLogging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace POS.BusinessTier
{
    public class ItemDescription : IDisposable
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        // A method for inserting and updating ItemMonitorCategoryDetail data.
        public void Persist(ItemDescriptionData updates)
        {

            try
            {
                using (ItemDescriptionSvr dao = new ItemDescriptionSvr())
                {
                    dao.Persist(updates);
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
                logger.Fatal(ex, "Persist(ItemDescriptionData updates)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
            }
        }
        #endregion

        #region Get Methods

        // Fills a DataSet with all ItemMonitorCategoryDetails based on a condition.
        public ItemDescriptionData PopulateList(string whereClause)
        {
            try
            {
                using (ItemDescriptionSvr dao = new ItemDescriptionSvr())
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
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
                return null;
            }
        }

        #endregion

        #region Validation Methods

        public ItemDescriptionData Populate(System.Int32 Id)
        {
            try
            {
                using (ItemDescriptionSvr dao = new ItemDescriptionSvr())
                {
                    return dao.Populate(Id);
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
                logger.Fatal(ex, "Populate(System.Int32 Id)");  //PRIMEPOS-2971 07-Jun-2021 JY Added
                //ErrorHandler.throwException(ex, "", "");    //PRIMEPOS-2971 07-Jun-2021 JY Commented as no need to log it in errorlog
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
