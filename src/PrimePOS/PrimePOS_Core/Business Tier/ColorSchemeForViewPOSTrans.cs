using System;
using System.Data;
using POS_Core.DataAccess;
using POS_Core.CommonData.Tables;
using POS_Core.CommonData.Rows;
using POS_Core.CommonData;
using POS_Core.ErrorLogging;
using NLog;

namespace POS_Core.BusinessRules
{
    public class ColorSchemeForViewPOSTrans
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        #region Persist Methods
        /// <summary>
        /// A method for inserting and updating POS Trans Color Scheme data. 
        /// </summary>
        /// <param name="updates">It is POS Trans Color Scheme type dataset class. It contains all information of POS Trans Color Scheme.</param>       
        public void Persist(ColorSchemeForViewPOSTransData updates)
        {
            try
            {
                using (ColorSchemeForViewPOSTransSvr dao = new ColorSchemeForViewPOSTransSvr())
                {
                    dao.Persist(updates);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Persist(ColorSchemeForViewPOSTransData updates)");
                throw (ex);
            }
        }
        #endregion

        #region Get Methods
        /// <summary>
        /// Fills a POS Trans Color Scheme type DataSet with all POS Trans Color Scheme based on a Function Key.
        /// </summary>
        /// <param name="FunKey">Function key combination.</param>
        /// <returns>ColorSchemeForViewPOSTransData type dataset</returns>
        public ColorSchemeForViewPOSTransData Populate(Int32 Id)
        {
            try
            {
                using (ColorSchemeForViewPOSTransSvr dao = new ColorSchemeForViewPOSTransSvr())
                {
                    return dao.Populate(Id);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Populate(Int32 Id)");
                throw (ex);
            }
        }
        /// <summary>
        /// Get collection of function keys on the bases of SQL WHERE CLUASE
        /// </summary>
        /// <param name="whereClause">Where clause description.</param>
        /// <returns>POS Trans Color Scheme type dataset.</returns>
        public ColorSchemeForViewPOSTransData PopulateList(string whereClause)
        {
            try
            {
                using (ColorSchemeForViewPOSTransSvr dao = new ColorSchemeForViewPOSTransSvr())
                {
                    return dao.PopulateList(whereClause);
                }
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "PopulateList(string whereClause)");
                throw (ex);
            }
        }

        #endregion


        #region Validation Methods
        

        #endregion
        /// <summary>
        /// Free all resources of POS Trans Color Scheme object.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
